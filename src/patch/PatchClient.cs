using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
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

            TGZPatchNode images = new TGZPatchNode(this, "http://tdegroot.nl/api/qstats/", currentPath, "dragontail-" + API.getVersion() + ".zip");
            nodes.Add(images);
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
            int patchCode = 0;
            for(int i = 0; i < patchableNodes.Count; i++) {
                patchCode = patchableNodes[i].patch(currentPath);
            }
            if(File.Exists(patchPath)) {
                File.Delete(patchPath);
            }

            if (patchCode == 1) {
                del = new ObjectDelegate(patcher.stopSpinning);
                del.Invoke(null);
                patcher.setStatusInvoked("Patching Finished");
                Console.WriteLine("Patching Finished");
                Console.WriteLine("Downloaded " + patchableNodes.Count + " files");
            }
            
            StreamWriter strm = File.CreateText(patchPath);
            strm.WriteLine("version:" + API.getVersion());
            strm.WriteLine("patched:true");
            strm.Close();
            
        }

        public void status(String status) {
            patcher.setStatusInvoked(status);
        }

        public void progress(int percentage) {
            var del = new ObjectDelegate(patcher.setProgressInvoked);
            del.Invoke(percentage);
        }

        public void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e) {
            var del = new ObjectDelegate(patcher.setProgressInvoked);
            del.Invoke(e.ProgressPercentage);
        }

        public void DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e) {
            ObjectDelegate del;
            del = new ObjectDelegate(patcher.stopSpinning);
            del.Invoke(null);
            patcher.setStatusInvoked("Patching Finished");
            Console.WriteLine("Patching Finished");
            Console.WriteLine("Downloaded " + patchableNodes.Count + " files");
        }

    }
}
