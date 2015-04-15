/*
 * SystemInfoSnapshot
 * Author: Tiago Conceição
 * 
 * http://systeminfosnapshot.com/
 * https://github.com/sn4k3/SystemInfoSnapshot
 */

using System;
using System.Windows.Forms;
using SystemInfoSnapshot.Reports;

namespace SystemInfoSnapshot
{
    static class Program
    {
        public const string Website = "http://systeminfosnapshot.com";
        public static HtmlTemplate HtmlTemplate;
        public static Report[] Reports;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.AssemblyResolve += EmbeddedAssembly.OnResolveAssembly;
            Reports = Report.GetReports();

            // Null or Silent mode, skip GUI.
            if (ApplicationArguments.Instance.Null || ApplicationArguments.Instance.Silent)
            {
                WriteTemplate();
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain());
        }

        /// <summary>
        /// Write all info into template and output the html file.
        /// </summary>
        public static void WriteTemplate()
        {
            HtmlTemplate = Report.GenerateReports(Reports, true, ApplicationArguments.Instance.UseSingleThread);

            if (ApplicationArguments.Instance.Silent)
            {
                HtmlTemplate.ShowInExplorer();
            }

            if (ApplicationArguments.Instance.OpenReport)
            {
                HtmlTemplate.OpenInDefaultBrowser();
            }
        }
    }
}
