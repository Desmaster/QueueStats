using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using Newtonsoft.Json;

using RiotSharp;
using src.api;

namespace src.patch {

    class PatchClient {

        private String homePath;
        private String currentPath;
        private String patchPath;

        private Patcher patcher;

        private List<PatchNode> nodes = new List<PatchNode>();
        public List<PatchNode> patchableNodes = new List<PatchNode>();

        public PatchClient(Patcher patcher) {
            String key = Properties.Settings.Default.api_key;
            var riotAPI = RiotApi.GetInstance(key, false);
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

            var staticAPI = StaticRiotApi.GetInstance(key);
            
            ChampionListStatic champions;
            if(File.Exists(currentPath + @"\champions\list.json")) {
                champions = JsonConvert.DeserializeObject<ChampionListStatic>(File.ReadAllText(currentPath + @"\champions\list.json"));
            } else {
                champions = staticAPI.GetChampions(Region.euw, ChampionData.all);
                var json = JsonConvert.SerializeObject(champions);
                var champPath = currentPath + @"\champions";
                if(!Directory.Exists(champPath)) {
                    Directory.CreateDirectory(champPath);
                }
                File.WriteAllText(champPath + @"\list.json", json);
            }
            
            foreach(KeyValuePair<String, ChampionStatic> pair in champions.Champions) {
                ChampionPatchNode node = new ChampionPatchNode(pair.Value, staticAPI);
                nodes.Add(node);
            }

            ItemListStatic items;
            if(File.Exists(currentPath + @"\items\list.json")) {
                items = JsonConvert.DeserializeObject<ItemListStatic>(File.ReadAllText(currentPath + @"\items\list.json"));
            } else {
                items = staticAPI.GetItems(Region.euw, ItemData.all);
                var json = JsonConvert.SerializeObject(items);
                var itemPath = currentPath + @"\items";
                if(!Directory.Exists(itemPath)) {
                    Directory.CreateDirectory(itemPath);
                }
                File.WriteAllText(itemPath + @"\list.json", json);
            }
            foreach(KeyValuePair<int, ItemStatic> pair in items.Items) {
                ItemPatchNode node = new ItemPatchNode(pair.Value, staticAPI);
                nodes.Add(node);
            }

        }

        private bool isPatched() {
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
            return patched;
        }

        public bool shouldPatch() {
            bool patched = false;

            patched = isPatched();

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

        public void patch() {
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
                patchableNodes[i].patch(currentPath);
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
            del = new ObjectDelegate(patcher.stopSpinning);
            del.Invoke(null);
            patcher.setStatusInvoked("Patching Finished");
            Console.WriteLine("Patching Finished");
            Console.WriteLine("Downloaded " + patchableNodes.Count + " files");
        }

    }
}
