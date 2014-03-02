using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using Newtonsoft.Json;

using src.api;
using src.api.champion.list;

namespace src.patch {
    class PatchClient {

        private String homePath;
        private String currentPath;
        private String patchPath;

        private Patcher patcher;

        private List<PatchNode> nodes = new List<PatchNode>();
        public List<PatchNode> patchableNodes = new List<PatchNode>();

        public PatchClient(Patcher patcher) {
            this.patcher = patcher;
            homePath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\QueueStats\";
            currentPath = homePath + API.getVersion() + @"\";
            patchPath = homePath + "patch.config";
            Console.WriteLine("Home Path: " + homePath);
            Console.WriteLine("Current Path: " + currentPath);
            Console.WriteLine("Patch Path: " + patchPath);
            if(!Directory.Exists(homePath)) {
                Directory.CreateDirectory(homePath);
            }
            string championlist = API.loadServerFile("champions.json");
            ChampionKeyPair[] list = JsonConvert.DeserializeObject<ChampionKeyPair[]>(championlist);
            for(int i = 0; i < list.Length; i++) {
                ChampionPatchNode node = new ChampionPatchNode(list[i].Key, list[i].Name, API.STATIC_CHAMPION_ID);
                nodes.Add(node);
            }

        }

        public bool shouldPatch() {
            bool patched = false;
            if(File.Exists(patchPath)) {
                string lastPatchVersion = "";
                bool patchedLastTime = false;
                try {
                    StreamReader reader = File.OpenText(patchPath);
                    while(!reader.EndOfStream) {
                        string line = reader.ReadLine();
                        if(line.StartsWith("version:")) {
                            lastPatchVersion = line.Substring(8);
                        } else if(line.StartsWith("patched:")) {
                            patchedLastTime = Boolean.Parse(line.Substring(8));
                        }
                    }
                    reader.Close();
                    if(lastPatchVersion == API.getVersion()) {
                        patched = patchedLastTime;
                    } else {
                        patched = false;
                    }
                } catch(Exception e) {
                    Console.WriteLine("The file could not be read:");
                    Console.WriteLine(e.Message);
                }
            } else {
                patched = false;
            }

            for(int i = 0; i < nodes.Count; i++) {
                if(!nodes[i].patched(currentPath))
                    patched = false;
            }

            if(!patched) {
                for(int i = 0; i < nodes.Count; i++) {
                    PatchNode node = nodes[i];
                    if(!node.patched(currentPath)) {
                        patchableNodes.Add(node);
                    }
                }
            }
            return !patched;
        }

        private delegate void ObjectDelegate(object obj);

        public async void patch() {
            if(!Directory.Exists(currentPath)) {
                Directory.CreateDirectory(currentPath);
            }
            ObjectDelegate del;
            int elements = patchableNodes.Count;
            del = new ObjectDelegate(patcher.setFilesRemainingInvoked);
            del.Invoke(elements);
            double valuePerElement = 1.0 / elements * 100;
            double progress = 0;
            for(int i = 0; i < patchableNodes.Count; i++) {
                await patchableNodes[i].patch(currentPath);
                patcher.setStatusInvoked("Currently Downloading: " + patchableNodes[i].name + ".json");
                progress += valuePerElement;

                del = new ObjectDelegate(patcher.setProgressInvoked);
                del.Invoke((int)progress);

                del = new ObjectDelegate(patcher.setFilesRemainingInvoked);
                del.Invoke(elements - i - 1);
            }
            if(File.Exists(patchPath)) {
                File.Delete(patchPath);
            }
            StreamWriter strm = File.CreateText(patchPath);
            strm.WriteLine("version:" + API.getVersion());
            strm.WriteLine("patched:true");
            strm.Close();
            patcher.setStatusInvoked("Patching Finished");
            Console.WriteLine("Patching Finished");
            Console.WriteLine("Downloaded " + patchableNodes.Count + " files");
        }

    }
}
