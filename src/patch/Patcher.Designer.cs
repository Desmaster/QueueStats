namespace src.patch {
    partial class Patcher {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Patcher));
            this.pbPatch = new System.Windows.Forms.ProgressBar();
            this.btnPatch = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblFilesRemaining = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ttslblCurrentFile = new System.Windows.Forms.ToolStripStatusLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pbPatch
            // 
            this.pbPatch.ForeColor = System.Drawing.Color.OliveDrab;
            this.pbPatch.Location = new System.Drawing.Point(12, 115);
            this.pbPatch.Name = "pbPatch";
            this.pbPatch.Size = new System.Drawing.Size(268, 23);
            this.pbPatch.TabIndex = 0;
            // 
            // btnPatch
            // 
            this.btnPatch.Enabled = false;
            this.btnPatch.Location = new System.Drawing.Point(100, 186);
            this.btnPatch.Name = "btnPatch";
            this.btnPatch.Size = new System.Drawing.Size(75, 23);
            this.btnPatch.TabIndex = 1;
            this.btnPatch.Text = "Patch";
            this.btnPatch.UseVisualStyleBackColor = true;
            this.btnPatch.Click += new System.EventHandler(this.btnPatch_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 96);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Files remaining:";
            // 
            // lblFilesRemaining
            // 
            this.lblFilesRemaining.AutoSize = true;
            this.lblFilesRemaining.Location = new System.Drawing.Point(99, 96);
            this.lblFilesRemaining.Name = "lblFilesRemaining";
            this.lblFilesRemaining.Size = new System.Drawing.Size(13, 13);
            this.lblFilesRemaining.TabIndex = 3;
            this.lblFilesRemaining.Text = "0";
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.White;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ttslblCurrentFile});
            this.statusStrip1.Location = new System.Drawing.Point(0, 251);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(292, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ttslblCurrentFile
            // 
            this.ttslblCurrentFile.Name = "ttslblCurrentFile";
            this.ttslblCurrentFile.Size = new System.Drawing.Size(0, 17);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Gray;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 172);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(64, 64);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            // 
            // Patcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.lblFilesRemaining);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnPatch);
            this.Controls.Add(this.pbPatch);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Patcher";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Patcher";
            this.Load += new System.EventHandler(this.Patcher_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar pbPatch;
        private System.Windows.Forms.Button btnPatch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblFilesRemaining;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel ttslblCurrentFile;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}