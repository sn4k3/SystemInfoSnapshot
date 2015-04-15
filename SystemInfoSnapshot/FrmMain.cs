/*
 * SystemInfoSnapshot
 * Author: Tiago Conceição
 * 
 * http://systeminfosnapshot.com/
 * https://github.com/sn4k3/SystemInfoSnapshot
 */

using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace SystemInfoSnapshot
{
    public partial class FrmMain : Form
    {
        #region Properties
        /// <summary>
        /// Button list to be disabled and enabled.
        /// </summary>
        private readonly Button[] Buttons;

        /// <summary>
        /// Gets the datetime when report generation process started
        /// </summary>
        public DateTime StartDateTime { get; private set; }

        /// <summary>
        /// Gets the datetime when report generation process ended
        /// </summary>
        public DateTime EndDateTime { get; private set; }
        #endregion

        #region Constructor
        public FrmMain()
        {
            InitializeComponent();
            Buttons = new[]
            {
                btnRebuildReport,
                btnOpenReport,
                btnOpenReport2,
                btnOpenFolder,
                btnOpenFolder2,
                btnExit
            };
            tmClock.Tick += (sender, args) =>
            {
                lbStatus.Text = string.Format("Generating the report. Please wait... {0:0.##}s", Math.Ceiling((DateTime.Now - StartDateTime).TotalSeconds));
            };
        }
        #endregion

        #region Overrides

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            StartWorker();
        }

        #endregion

        #region Events

        private void ButtonClick(object sender, EventArgs e)
        {
            if (sender == btnWebsite)
            {
                SystemHelper.OpenLink(Program.Website);
                return;
            }
            if (sender == btnRebuildReport)
            {
                ButtonsSetEnabled(false);
                StartWorker();
                return;
            }
            if (sender == btnOpenReport || sender == btnOpenReport2)
            {
                Program.HtmlTemplate.OpenInDefaultBrowser();
                return;
            }
            if (sender == btnOpenFolder || sender == btnOpenFolder2)
            {
                Program.HtmlTemplate.ShowInExplorer();

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
            EndDateTime = DateTime.Now;
            tmClock.Stop();
            lbStatus.Text = string.Format("Report completed in {0:0.##}s", (EndDateTime - StartDateTime).TotalSeconds);
        }

        #endregion

        #region Methods

        private void StartWorker()
        {
            if (bgWorker.IsBusy) return;
            StartDateTime = DateTime.Now;
            lbStatus.Text = @"Generating the report. Please wait...";
            tmClock.Start();
            bgWorker.RunWorkerAsync();
        }

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

