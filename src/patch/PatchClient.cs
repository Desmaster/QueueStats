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
        private List<PatchNode> patchableNodes = new List<PatchNode>();

        public PatchClient(SettingsView settingsView) {
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

            nodes.Add(new ImagePatchNode(this, currentPath + @"img\profile", "/profile/series_win.png"));
            nodes.Add(new ImagePatchNode(this, currentPath + @"img\profile", "/profile/series_lose.png"));
            nodes.Add(new ImagePatchNode(this, currentPath + @"img\profile", "/profile/series_none.png"));
            nodes.Add(new ImagePatchNode(this, currentPath + @"img\profile", "/profile/league_hot.png"));
            nodes.Add(new ImagePatchNode(this, currentPath + @"img\profile", "/profile/league_new.png"));
            nodes.Add(new TGZPatchNode(this, "http://tdegroot.nl/api/qstats/", Core.getInstance().getCurrentPath(),
                                        "dragontail-" + API.getVersion() + ".zip"));

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
                    }
                } catch(Exception e) {
                    Console.WriteLine("The file could not be read:");
                    Console.WriteLine(e.Message);
                }
            }
            return patched;
        }

        public bool shouldPatch() {
            bool patched = isPatched();

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

            for(int i = 0; i < patchableNodes.Count; i++) {
               await patchableNodes[i].patch(currentPath);
            }

            if(File.Exists(patchPath)) {
                File.Delete(patchPath);
            }
            
            StreamWriter sw = File.CreateText(patchPath);
            sw.WriteLine("version:" + API.getVersion());
            sw.WriteLine("patched:true");
            sw.Close();
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
