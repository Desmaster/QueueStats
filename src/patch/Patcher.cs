using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using Newtonsoft.Json;

using src.api;
using src.api.champion.list;

namespace src.patch {
    public partial class Patcher : Form {

        private PatchClient client;
        private Thread patchThread;

        public Patcher() {
            InitializeComponent();
            client = new PatchClient(this);
            patchThread = new Thread(new ThreadStart(client.patch));
            if(client.shouldPatch()) {
                btnPatch.Enabled = true;
            }
        }

        private delegate void ObjectDelegate(object obj);

        public void setProgress(object obj) {
            if(InvokeRequired) {
                ObjectDelegate method = new ObjectDelegate(setProgress);
                Invoke(method, obj);
                return;
            }
            int value = (int)obj;
            pbPatch.Value = value;
        }

        public void setFilesRemaining(object obj) {
            if(InvokeRequired) {
                ObjectDelegate method = new ObjectDelegate(setFilesRemaining);
                Invoke(method, obj);
                return;
            }
            int value = (int)obj;
            lblFilesRemaining.Text = value.ToString();
        }

        public void setStatus(object obj) {
            if(InvokeRequired) {
                ObjectDelegate method = new ObjectDelegate(setStatus);
                Invoke(method, obj);
                return;
            }
            String value = (String)obj;
            ttslblCurrentFile.Text = value;
        }

        private void btnPatch_Click(object sender, EventArgs e) {
            btnPatch.Enabled = false;
            patchThread.Start();
        }

        private void Patcher_Load(object sender, EventArgs e) {
        }

    }
}
