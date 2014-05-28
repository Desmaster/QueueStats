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
using src.views;

namespace src.patch {

    class PatchClient {

        private String homePath;
        private String currentPath;
        private String patchPath;

        private SettingsView settingsView;

        private List<PatchNode> nodes = new List<PatchNode>();
        public List<PatchNode> patchableNodes = new List<PatchNode>();

        public PatchClient(SettingsView settingsView) {
            String key = Properties.Settings.Default.api_key_1;
            var riotAPI = RiotApi.GetInstance(key, false);
            this.settingsView = settingsView;
            homePath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\QueueStats\";
            currentPath = homePath + API.getVersion() + @"\";
            patchPath = homePath + "patch.config";
            Console.WriteLine("Home Path: " + homePath);
            Console.WriteLine("Current Path: " + currentPath);
            Console.WriteLine("Patch Path: " + patchPath);

            if(!Directory.Exists(homePath)) {
                Directory.CreateDirectory(homePath);
            }

            ImagePatchNode profileWin = new ImagePatchNode(this, currentPath + @"img\profile", "/profile/series_win.png");
            ImagePatchNode profileLose = new ImagePatchNode(this, currentPath + @"img\profile", "/profile/series_lose.png");
            ImagePatchNode profileNone = new ImagePatchNode(this, currentPath + @"img\profile", "/profile/series_none.png");
            ImagePatchNode profileHot = new ImagePatchNode(this, currentPath + @"img\profile", "/profile/league_hot.png");
            ImagePatchNode profileNew = new ImagePatchNode(this, currentPath + @"img\profile", "/profile/league_new.png");

            nodes.Add(profileWin);
            nodes.Add(profileLose);
            nodes.Add(profileNone);
            nodes.Add(profileHot);
            nodes.Add(profileNew);

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

        public async Task patch() {
            if(!Directory.Exists(currentPath)) {
                Directory.CreateDirectory(currentPath);
            }
            int elements = patchableNodes.Count;
            double valuePerElement = 1.0 / elements * 100;
            double progress = 0;
            for(int i = 0; i < patchableNodes.Count; i++) {
               await patchableNodes[i].patch(currentPath);
            }
            if(File.Exists(patchPath)) {
                File.Delete(patchPath);
            }
            
            StreamWriter strm = File.CreateText(patchPath);
            strm.WriteLine("version:" + API.getVersion());
            strm.WriteLine("patched:true");
            strm.Close();
            
        }

        public void status(String status) {
            settingsView.setStatus(status);
        }

        public void progress(int percentage) {
            settingsView.setProgress(percentage);
        }

        public void completion() {
            status("Finished patching and extracting.");
            progress(100);
            settingsView.completion();
        }

        public void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e) {
            progress(e.ProgressPercentage);
        }

        public void DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e, String message) {
            status(message);
        }

    }
}
