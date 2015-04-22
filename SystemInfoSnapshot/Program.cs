/*
 * SystemInfoSnapshot
 * Author: Tiago Conceição
 * 
 * http://systeminfosnapshot.com/
 * https://github.com/sn4k3/SystemInfoSnapshot
 */

using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using SystemInfoSnapshot.Reports;

namespace SystemInfoSnapshot
{
    static class Program
    {
        #region Properties
        /// <summary>
        /// Gets the project website
        /// </summary>
        public const string Website = "http://systeminfosnapshot.com";
        public static HtmlTemplate HtmlTemplate { get; private set; }
        public static Report[] Reports { get; private set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        static Program()
        {
            AppDomain.CurrentDomain.AssemblyResolve += EmbeddedAssembly.OnResolveAssembly;
        }
        #endregion

        #region Bootsrap
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Skip this method and do all work on a secondary method for .NET able to register embbeded assemblies correctly
            MainCore();
        }

        /// <summary>
        /// 
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        static void MainCore()
        {
            Reports = Report.GetReports();
            var resultArgs = ApplicationArguments.Init(Environment.GetCommandLineArgs());
            if (resultArgs.HasErrors)
            {
                SystemHelper.DisplayMessage(resultArgs.ErrorText);
            }
            if (resultArgs.HelpCalled)
            {
                // Quit
                return;
            }
            ApplicationArguments.Instance.Fix();

            // Null or Silent mode, skip GUI.
            if (ApplicationArguments.Instance.NoGUI)
            {
                WriteTemplate();
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain());
        }
        #endregion

        #region Static Methods
        /// <summary>
        /// Write all info into template and output the html file.
        /// </summary>
        public static void WriteTemplate()
        {
            HtmlTemplate = Report.GenerateReports(Reports, true, ApplicationArguments.Instance.Filename, ApplicationArguments.Instance.MaxDegreeOfParallelism);

            if (ApplicationArguments.Instance.Silent)
            {
                HtmlTemplate.ShowInExplorer();
            }

            if (ApplicationArguments.Instance.OpenReport)
            {
                HtmlTemplate.OpenInDefaultBrowser();
            }
        }
        #endregion
    }
}
