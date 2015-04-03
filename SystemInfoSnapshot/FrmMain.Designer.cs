namespace SystemInfoSnapshot
{
    partial class FrmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.btnOpenReport = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.bgWorker = new System.ComponentModel.BackgroundWorker();
            this.pbProgress = new System.Windows.Forms.ProgressBar();
            this.btnRebuildReport = new System.Windows.Forms.Button();
            this.lbLead = new System.Windows.Forms.Label();
            this.lbFilename = new System.Windows.Forms.Label();
            this.lbLocationT = new System.Windows.Forms.Label();
            this.lbFilenameT = new System.Windows.Forms.Label();
            this.lbLocation = new System.Windows.Forms.Label();
            this.btnOpenReport2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnOpenReport
            // 
            this.btnOpenReport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenReport.Enabled = false;
            this.btnOpenReport.Image = global::SystemInfoSnapshot.Properties.Resources.open;
            this.btnOpenReport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOpenReport.Location = new System.Drawing.Point(13, 182);
            this.btnOpenReport.Margin = new System.Windows.Forms.Padding(4);
            this.btnOpenReport.Name = "btnOpenReport";
            this.btnOpenReport.Size = new System.Drawing.Size(373, 33);
            this.btnOpenReport.TabIndex = 0;
            this.btnOpenReport.Text = "Open generated report in browser";
            this.btnOpenReport.UseVisualStyleBackColor = true;
            this.btnOpenReport.Click += new System.EventHandler(this.ButtonClick);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.Enabled = false;
            this.btnExit.Image = global::SystemInfoSnapshot.Properties.Resources.exit;
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExit.Location = new System.Drawing.Point(394, 182);
            this.btnExit.Margin = new System.Windows.Forms.Padding(4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(94, 33);
            this.btnExit.TabIndex = 1;
            this.btnExit.Text = "Close";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.ButtonClick);
            // 
            // bgWorker
            // 
            this.bgWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWorker_DoWork);
            this.bgWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgWorker_RunWorkerCompleted);
            // 
            // pbProgress
            // 
            this.pbProgress.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pbProgress.Location = new System.Drawing.Point(0, 222);
            this.pbProgress.MarqueeAnimationSpeed = 10;
            this.pbProgress.Name = "pbProgress";
            this.pbProgress.Size = new System.Drawing.Size(502, 23);
            this.pbProgress.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.pbProgress.TabIndex = 2;
            // 
            // btnRebuildReport
            // 
            this.btnRebuildReport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRebuildReport.Enabled = false;
            this.btnRebuildReport.Image = global::SystemInfoSnapshot.Properties.Resources.refresh;
            this.btnRebuildReport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRebuildReport.Location = new System.Drawing.Point(13, 141);
            this.btnRebuildReport.Margin = new System.Windows.Forms.Padding(4);
            this.btnRebuildReport.Name = "btnRebuildReport";
            this.btnRebuildReport.Size = new System.Drawing.Size(475, 33);
            this.btnRebuildReport.TabIndex = 3;
            this.btnRebuildReport.Text = "Rebuild report";
            this.btnRebuildReport.UseVisualStyleBackColor = true;
            this.btnRebuildReport.Click += new System.EventHandler(this.ButtonClick);
            // 
            // lbLead
            // 
            this.lbLead.Location = new System.Drawing.Point(12, 9);
            this.lbLead.Name = "lbLead";
            this.lbLead.Size = new System.Drawing.Size(476, 73);
            this.lbLead.TabIndex = 4;
            this.lbLead.Text = resources.GetString("lbLead.Text");
            // 
            // lbFilename
            // 
            this.lbFilename.AutoSize = true;
            this.lbFilename.Location = new System.Drawing.Point(85, 91);
            this.lbFilename.Name = "lbFilename";
            this.lbFilename.Size = new System.Drawing.Size(17, 16);
            this.lbFilename.TabIndex = 5;
            this.lbFilename.Text = "...";
            // 
            // lbLocationT
            // 
            this.lbLocationT.AutoSize = true;
            this.lbLocationT.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbLocationT.Location = new System.Drawing.Point(12, 111);
            this.lbLocationT.Name = "lbLocationT";
            this.lbLocationT.Size = new System.Drawing.Size(71, 16);
            this.lbLocationT.TabIndex = 6;
            this.lbLocationT.Text = "Location:";
            // 
            // lbFilenameT
            // 
            this.lbFilenameT.AutoSize = true;
            this.lbFilenameT.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbFilenameT.Location = new System.Drawing.Point(12, 91);
            this.lbFilenameT.Name = "lbFilenameT";
            this.lbFilenameT.Size = new System.Drawing.Size(76, 16);
            this.lbFilenameT.TabIndex = 7;
            this.lbFilenameT.Text = "Filename:";
            // 
            // lbLocation
            // 
            this.lbLocation.AutoSize = true;
            this.lbLocation.Location = new System.Drawing.Point(85, 111);
            this.lbLocation.Name = "lbLocation";
            this.lbLocation.Size = new System.Drawing.Size(59, 16);
            this.lbLocation.TabIndex = 8;
            this.lbLocation.Text = "Desktop";
            // 
            // btnOpenReport2
            // 
            this.btnOpenReport2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenReport2.Enabled = false;
            this.btnOpenReport2.Image = global::SystemInfoSnapshot.Properties.Resources.open;
            this.btnOpenReport2.Location = new System.Drawing.Point(460, 87);
            this.btnOpenReport2.Margin = new System.Windows.Forms.Padding(4);
            this.btnOpenReport2.Name = "btnOpenReport2";
            this.btnOpenReport2.Size = new System.Drawing.Size(28, 24);
            this.btnOpenReport2.TabIndex = 9;
            this.btnOpenReport2.UseVisualStyleBackColor = true;
            this.btnOpenReport2.Click += new System.EventHandler(this.ButtonClick);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(502, 245);
            this.Controls.Add(this.btnOpenReport2);
            this.Controls.Add(this.lbLocation);
            this.Controls.Add(this.lbFilenameT);
            this.Controls.Add(this.lbLocationT);
            this.Controls.Add(this.lbFilename);
            this.Controls.Add(this.lbLead);
            this.Controls.Add(this.btnRebuildReport);
            this.Controls.Add(this.pbProgress);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnOpenReport);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "FrmMain";
            this.Text = "System Info Snapshot";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOpenReport;
        private System.Windows.Forms.Button btnExit;
        private System.ComponentModel.BackgroundWorker bgWorker;
        private System.Windows.Forms.ProgressBar pbProgress;
        private System.Windows.Forms.Button btnRebuildReport;
        private System.Windows.Forms.Label lbLead;
        private System.Windows.Forms.Label lbFilename;
        private System.Windows.Forms.Label lbLocationT;
        private System.Windows.Forms.Label lbFilenameT;
        private System.Windows.Forms.Label lbLocation;
        private System.Windows.Forms.Button btnOpenReport2;
    }
}

