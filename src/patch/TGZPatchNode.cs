using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

using Ionic.Zip;

namespace src.patch {

    class TGZPatchNode : PatchNode {

        private PatchClient patchClient;
        private String url, path, imgPath, name;

        public TGZPatchNode(PatchClient patchClient, String url, String path, String name) : base(null) {
            this.patchClient = patchClient;
            this.url = url;
            this.path = path;
            imgPath = path + @"img\";
            this.name = name;
        }

        public override bool patched(string path) {
            bool patched = true;
            if (File.Exists(path + name)) {
                using (ZipFile zip = ZipFile.Read(path + name)) {
                    foreach (ZipEntry e in zip) {
                        String loc = e.FileName;
                        if (loc == "")
                            continue;
                        if (e.IsDirectory && !Directory.Exists(imgPath + loc)) {
                            patched = false;
                        } else if (!e.IsDirectory) {
                            if (!File.Exists(imgPath + loc)) {
                                patched = false;
                            }
                        }
                    }
                }
            } else {
                patched = false;
            }
            return patched;
        }

        public override async Task patch(string path) {
            if (!File.Exists(path + name)) {
                using (var client = new WebClient()) {
                    client.DownloadFileCompleted += client_DownloadFileCompleted;
                    client.DownloadProgressChanged += client_DownloadProgressChanged;
                    patchClient.status("Downloading: " + name);
                    await Task.Run(() => {
                        patchClient.status("Downloading: " + name);
                        client.DownloadFileAsync(new Uri(url + name), path + name);
                    });
                    patchClient.status("Downloading: " + name);
                }
            } else {
                extract();
            }
        }

        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e) {
            patchClient.DownloadProgressChanged(sender, e);
        }

        void client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e) {
            patchClient.DownloadFileCompleted(sender, e, "Downloaded " + name);
            extract();
        }

        private void extract() {
            using (ZipFile zip = ZipFile.Read(path + name)) {
                int total = zip.Entries.Count;
                double pointPerFile = 100.0/total;
                double progress = 0;
                foreach (ZipEntry entry in zip) {
                    String loc = entry.FileName;
                    patchClient.status("Currently extracting: " + loc);
                    if (loc == "")
                        continue;
                    entry.Extract(imgPath);
                    progress += pointPerFile;
                    patchClient.progress((int) progress);
                }
            }
            patchClient.status("Finished patching and extracting.");
        }


    }

}
