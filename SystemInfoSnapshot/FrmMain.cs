using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace SystemInfoSnapshot
{
    public partial class FrmMain : Form
    {
        private readonly Button[] Buttons;
        public FrmMain()
        {
            InitializeComponent();
            Buttons = new[]
            {
                btnRebuildReport,
                btnOpenReport,
                btnOpenReport2,
                btnExit
            };
        }

        #region Overrides

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            bgWorker.RunWorkerAsync();
        }

        #endregion

        #region Events
        private void ButtonClick(object sender, EventArgs e)
        {
            if (sender == btnRebuildReport)
            {
                ButtonsSetEnabled(false);
                if (!bgWorker.IsBusy)
                    bgWorker.RunWorkerAsync();
                return;
            }
            if (sender == btnOpenReport || sender == btnOpenReport2)
            {
                if (!string.IsNullOrEmpty(Program.HtmlTemplate.LastSaveFilePath))
                {
                    using (Process process = new Process())
                    {
                        process.StartInfo.FileName = Program.HtmlTemplate.LastSaveFilePath;
                        process.Start();
                        process.Close();
                    }

                    return;
                }

                return;
            }

            if (sender == btnExit)
            {
                Application.Exit();
                return;
            }
        }

        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(500);
            Program.WriteTemplate();
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ButtonsSetEnabled(true);
            var location = Path.GetDirectoryName(Program.HtmlTemplate.LastSaveFilePath);
            if (string.IsNullOrEmpty(location))
            {
                location = Application.StartupPath;
            }
            lbFilename.Text = Path.GetFileName(Program.HtmlTemplate.LastSaveFilePath);
            //lbLocation.Text = location;
        }

        #endregion

        #region Methods

        private void ButtonsSetEnabled(bool enabled = true)
        {
            foreach (var button in Buttons)
            {
                button.Enabled = enabled;
            }
            if (enabled)
            {
                pbProgress.Style = ProgressBarStyle.Blocks;
                pbProgress.Enabled = false;
            }
            else
            {
                pbProgress.Style = ProgressBarStyle.Marquee;
                pbProgress.Enabled = true;
                lbFilename.Text = @"...";
                lbLocation.Text = @"Desktop";
            }
        }
        #endregion
    }
}
