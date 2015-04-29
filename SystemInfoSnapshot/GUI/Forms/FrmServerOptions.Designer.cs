namespace SystemInfoSnapshot.GUI.Forms
{
    partial class FrmServerOptions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmServerOptions));
            this.cbIPAddress = new System.Windows.Forms.ComboBox();
            this.lbIP = new System.Windows.Forms.Label();
            this.lbTwoDot = new System.Windows.Forms.Label();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.lbPort = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnStartServer = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.tbUpdateInterval = new System.Windows.Forms.TextBox();
            this.lbUpdateInterval = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cbIPAddress
            // 
            this.cbIPAddress.Location = new System.Drawing.Point(15, 79);
            this.cbIPAddress.Name = "cbIPAddress";
            this.cbIPAddress.Size = new System.Drawing.Size(121, 21);
            this.cbIPAddress.TabIndex = 0;
            // 
            // lbIP
            // 
            this.lbIP.AutoSize = true;
            this.lbIP.Location = new System.Drawing.Point(12, 63);
            this.lbIP.Name = "lbIP";
            this.lbIP.Size = new System.Drawing.Size(97, 13);
            this.lbIP.TabIndex = 1;
            this.lbIP.Text = "IP Address to listen";
            // 
            // lbTwoDot
            // 
            this.lbTwoDot.AutoSize = true;
            this.lbTwoDot.Location = new System.Drawing.Point(142, 84);
            this.lbTwoDot.Name = "lbTwoDot";
            this.lbTwoDot.Size = new System.Drawing.Size(10, 13);
            this.lbTwoDot.TabIndex = 2;
            this.lbTwoDot.Text = ":";
            // 
            // tbPort
            // 
            this.tbPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPort.Location = new System.Drawing.Point(158, 80);
            this.tbPort.MaxLength = 7;
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(59, 20);
            this.tbPort.TabIndex = 3;
            this.tbPort.Text = "8080";
            // 
            // lbPort
            // 
            this.lbPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbPort.AutoSize = true;
            this.lbPort.Location = new System.Drawing.Point(155, 64);
            this.lbPort.Name = "lbPort";
            this.lbPort.Size = new System.Drawing.Size(26, 13);
            this.lbPort.TabIndex = 4;
            this.lbPort.Text = "Port";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(205, 54);
            this.label1.TabIndex = 5;
            this.label1.Text = "Create a local web server and display live information about this machine on the " +
    "internet.";
            // 
            // btnStartServer
            // 
            this.btnStartServer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnStartServer.Image = global::SystemInfoSnapshot.Properties.Resources.cloud;
            this.btnStartServer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStartServer.Location = new System.Drawing.Point(12, 136);
            this.btnStartServer.Name = "btnStartServer";
            this.btnStartServer.Size = new System.Drawing.Size(124, 23);
            this.btnStartServer.TabIndex = 6;
            this.btnStartServer.Text = "Start Server";
            this.btnStartServer.UseVisualStyleBackColor = true;
            this.btnStartServer.Click += new System.EventHandler(this.ButtonClick);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(142, 136);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.ButtonClick);
            // 
            // tbUpdateInterval
            // 
            this.tbUpdateInterval.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbUpdateInterval.Location = new System.Drawing.Point(158, 106);
            this.tbUpdateInterval.MaxLength = 7;
            this.tbUpdateInterval.Name = "tbUpdateInterval";
            this.tbUpdateInterval.Size = new System.Drawing.Size(59, 20);
            this.tbUpdateInterval.TabIndex = 8;
            this.tbUpdateInterval.Text = "10";
            // 
            // lbUpdateInterval
            // 
            this.lbUpdateInterval.AutoSize = true;
            this.lbUpdateInterval.Location = new System.Drawing.Point(12, 109);
            this.lbUpdateInterval.Name = "lbUpdateInterval";
            this.lbUpdateInterval.Size = new System.Drawing.Size(134, 13);
            this.lbUpdateInterval.TabIndex = 9;
            this.lbUpdateInterval.Text = "Reports update interval: (s)";
            // 
            // FrmServerOptions
            // 
            this.AcceptButton = this.btnStartServer;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(229, 171);
            this.Controls.Add(this.lbUpdateInterval);
            this.Controls.Add(this.tbUpdateInterval);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnStartServer);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbPort);
            this.Controls.Add(this.tbPort);
            this.Controls.Add(this.lbTwoDot);
            this.Controls.Add(this.lbIP);
            this.Controls.Add(this.cbIPAddress);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmServerOptions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Listen server configuration";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbIPAddress;
        private System.Windows.Forms.Label lbIP;
        private System.Windows.Forms.Label lbTwoDot;
        private System.Windows.Forms.TextBox tbPort;
        private System.Windows.Forms.Label lbPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnStartServer;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox tbUpdateInterval;
        private System.Windows.Forms.Label lbUpdateInterval;
    }
}