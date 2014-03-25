using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RiotSharp;

namespace src.patch {
    class ImagePatchNode : PatchNode {

        private String webName, localName, folder;
        private PatchClient patchClient;

        public ImagePatchNode(PatchClient patchClient, String folder, String path) : base(null) {
            this.patchClient = patchClient;
            webName = path;
            localName = path.Replace("/", @"\");
            this.folder = folder;
        }

        public override bool patched(string path) {
            String imgPath = path + @"\img" + localName;
            if (File.Exists(imgPath)) {
                return true;
            }
            return false;
        }

        public override int patch(string path) {
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            using (var client = new WebClient()) {
                client.DownloadProgressChanged += client_DownloadProgressChanged;
                client.DownloadFileCompleted += client_DownloadFileCompleted;
                Task.Run(() => client.DownloadFileAsync(new Uri("http://tdegroot.nl/api/qstats/img" + webName), path + @"\img" + webName.Replace("/", @"\")));
            }
            return 2;
        }

        void client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e) {
            patchClient.DownloadFileCompleted(sender, e);
        }

        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e) {
            patchClient.DownloadProgressChanged(sender, e);
        }
    }
}
