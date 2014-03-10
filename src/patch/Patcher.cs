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
                Visible = true;
                Activate();
                Focus();
                lblFilesRemaining.Text = ("" + client.patchableNodes.Count);
            }
        }

        private delegate void ObjectDelegate(object obj);

        public void setProgressInvoked(object obj) {
            if(InvokeRequired) {
                ObjectDelegate method = new ObjectDelegate(setProgressInvoked);
                Invoke(method, obj);
                return;
            }
            int value = (int)obj;
            pbPatch.Value = value;
        }

        public void setFilesRemainingInvoked(object obj) {
            if(InvokeRequired) {
                ObjectDelegate method = new ObjectDelegate(setFilesRemainingInvoked);
                Invoke(method, obj);
                return;
            }
            int value = (int)obj;
            lblFilesRemaining.Text = value.ToString();
        }

        public void setStatusInvoked(object obj) {
            if(InvokeRequired) {
                ObjectDelegate method = new ObjectDelegate(setStatusInvoked);
                Invoke(method, obj);
                return;
            }
            String value = (String)obj;
            ttslblCurrentFile.Text = value;
        }

        public void stopSpinning(object obj) {
            if(InvokeRequired) {
                ObjectDelegate method = new ObjectDelegate(stopSpinning);
                Invoke(method, obj);
                return;
            }
            pictureBox1.Visible = false;
        }

        private void btnPatch_Click(object sender, EventArgs e) {
            btnPatch.Enabled = false;
            pictureBox1.Visible = true;
            patchThread.Start();
        }

        private void Patcher_Load(object sender, EventArgs e) {
        }

    }
}
