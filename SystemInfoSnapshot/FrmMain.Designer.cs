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
            this.btnOpenFolder2 = new System.Windows.Forms.Button();
            this.btnOpenFolder = new System.Windows.Forms.Button();
            this.lbStatus = new System.Windows.Forms.Label();
            this.lbStatusT = new System.Windows.Forms.Label();
            this.btnWebsite = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnOpenReport
            // 
            this.btnOpenReport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenReport.Enabled = false;
            this.btnOpenReport.Image = global::SystemInfoSnapshot.Properties.Resources.open;
            this.btnOpenReport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOpenReport.Location = new System.Drawing.Point(13, 283);
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
            this.btnExit.Location = new System.Drawing.Point(394, 283);
            this.btnExit.Margin = new System.Windows.Forms.Padding(4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(95, 65);
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
            this.pbProgress.Location = new System.Drawing.Point(0, 355);
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
            this.btnRebuildReport.Location = new System.Drawing.Point(13, 242);
            this.btnRebuildReport.Margin = new System.Windows.Forms.Padding(4);
            this.btnRebuildReport.Name = "btnRebuildReport";
            this.btnRebuildReport.Size = new System.Drawing.Size(476, 33);
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
            this.lbFilename.Location = new System.Drawing.Point(83, 145);
            this.lbFilename.Name = "lbFilename";
            this.lbFilename.Size = new System.Drawing.Size(17, 16);
            this.lbFilename.TabIndex = 5;
            this.lbFilename.Text = "...";
            // 
            // lbLocationT
            // 
            this.lbLocationT.AutoSize = true;
            this.lbLocationT.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbLocationT.Location = new System.Drawing.Point(10, 169);
            this.lbLocationT.Name = "lbLocationT";
            this.lbLocationT.Size = new System.Drawing.Size(71, 16);
            this.lbLocationT.TabIndex = 6;
            this.lbLocationT.Text = "Location:";
            // 
            // lbFilenameT
            // 
            this.lbFilenameT.AutoSize = true;
            this.lbFilenameT.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbFilenameT.Location = new System.Drawing.Point(10, 145);
            this.lbFilenameT.Name = "lbFilenameT";
            this.lbFilenameT.Size = new System.Drawing.Size(76, 16);
            this.lbFilenameT.TabIndex = 7;
            this.lbFilenameT.Text = "Filename:";
            // 
            // lbLocation
            // 
            this.lbLocation.AutoSize = true;
            this.lbLocation.Location = new System.Drawing.Point(83, 169);
            this.lbLocation.Name = "lbLocation";
            this.lbLocation.Size = new System.Drawing.Size(59, 16);
            this.lbLocation.TabIndex = 8;
            this.lbLocation.Text = "Desktop";
            // 
            // btnOpenReport2
            // 
            this.btnOpenReport2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenReport2.Enabled = false;
            this.btnOpenReport2.Image = global::SystemInfoSnapshot.Properties.Resources.open;
            this.btnOpenReport2.Location = new System.Drawing.Point(458, 141);
            this.btnOpenReport2.Margin = new System.Windows.Forms.Padding(4);
            this.btnOpenReport2.Name = "btnOpenReport2";
            this.btnOpenReport2.Size = new System.Drawing.Size(28, 24);
            this.btnOpenReport2.TabIndex = 9;
            this.btnOpenReport2.UseVisualStyleBackColor = true;
            this.btnOpenReport2.Click += new System.EventHandler(this.ButtonClick);
            // 
            // btnOpenFolder2
            // 
            this.btnOpenFolder2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenFolder2.Enabled = false;
            this.btnOpenFolder2.Image = global::SystemInfoSnapshot.Properties.Resources.folder_open;
            this.btnOpenFolder2.Location = new System.Drawing.Point(458, 165);
            this.btnOpenFolder2.Margin = new System.Windows.Forms.Padding(4);
            this.btnOpenFolder2.Name = "btnOpenFolder2";
            this.btnOpenFolder2.Size = new System.Drawing.Size(28, 24);
            this.btnOpenFolder2.TabIndex = 10;
            this.btnOpenFolder2.UseVisualStyleBackColor = true;
            this.btnOpenFolder2.Click += new System.EventHandler(this.ButtonClick);
            // 
            // btnOpenFolder
            // 
            this.btnOpenFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenFolder.Enabled = false;
            this.btnOpenFolder.Image = global::SystemInfoSnapshot.Properties.Resources.folder_open;
            this.btnOpenFolder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOpenFolder.Location = new System.Drawing.Point(13, 315);
            this.btnOpenFolder.Margin = new System.Windows.Forms.Padding(4);
            this.btnOpenFolder.Name = "btnOpenFolder";
            this.btnOpenFolder.Size = new System.Drawing.Size(373, 33);
            this.btnOpenFolder.TabIndex = 11;
            this.btnOpenFolder.Text = "Show generated report in explorer";
            this.btnOpenFolder.UseVisualStyleBackColor = true;
            this.btnOpenFolder.Click += new System.EventHandler(this.ButtonClick);
            // 
            // lbStatus
            // 
            this.lbStatus.AutoSize = true;
            this.lbStatus.Location = new System.Drawing.Point(83, 194);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(29, 16);
            this.lbStatus.TabIndex = 13;
            this.lbStatus.Text = "???";
            // 
            // lbStatusT
            // 
            this.lbStatusT.AutoSize = true;
            this.lbStatusT.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbStatusT.Location = new System.Drawing.Point(10, 194);
            this.lbStatusT.Name = "lbStatusT";
            this.lbStatusT.Size = new System.Drawing.Size(55, 16);
            this.lbStatusT.TabIndex = 12;
            this.lbStatusT.Text = "Status:";
            // 
            // btnWebsite
            // 
            this.btnWebsite.Image = global::SystemInfoSnapshot.Properties.Resources.web;
            this.btnWebsite.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnWebsite.Location = new System.Drawing.Point(12, 86);
            this.btnWebsite.Margin = new System.Windows.Forms.Padding(4);
            this.btnWebsite.Name = "btnWebsite";
            this.btnWebsite.Size = new System.Drawing.Size(476, 33);
            this.btnWebsite.TabIndex = 14;
            this.btnWebsite.Text = "Open website: systeminfosnapshot.com";
            this.btnWebsite.UseVisualStyleBackColor = true;
            this.btnWebsite.Click += new System.EventHandler(this.ButtonClick);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(502, 378);
            this.Controls.Add(this.btnWebsite);
            this.Controls.Add(this.lbStatus);
            this.Controls.Add(this.lbStatusT);
            this.Controls.Add(this.btnOpenFolder);
            this.Controls.Add(this.btnOpenFolder2);
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
        private System.Windows.Forms.Button btnOpenFolder2;
        private System.Windows.Forms.Button btnOpenFolder;
        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.Label lbStatusT;
        private System.Windows.Forms.Button btnWebsite;
    }
}

