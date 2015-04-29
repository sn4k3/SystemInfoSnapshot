/*
 * SystemInfoSnapshot
 * Author: Tiago Conceição
 * 
 * http://systeminfosnapshot.com/
 * https://github.com/sn4k3/SystemInfoSnapshot
 */

using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemInfoSnapshot.Core.Web;
using SystemInfoSnapshot.GUI;
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
                return;
            }
            if (resultArgs.HelpCalled)
            {
                // Quit
                return;
            }
            ApplicationArguments.Instance.Fix();

            if (ApplicationArguments.Instance.IsListenServer)
            {
                try
                {
                    if (!WebServer.Instance.Start(ApplicationArguments.Instance.ListenServerIPEndPoint))
                    {
                        SystemHelper.DisplayMessage("Unable to start the server with the selected configuration. Maybe port is on use? Please try again with other options.");
                        return;
                    }
                }
                catch
                {
                    SystemHelper.DisplayMessage("Unable to start the server with the selected configuration. Maybe port is on use? Please try again with other options.");
                    return;
                }
                
            }

            // Null or Silent mode, skip GUI.
            if (ApplicationArguments.Instance.NoGUI)
            {
                WriteTemplate();
                if (ApplicationArguments.Instance.IsListenServer)
                {
                    Task.Factory.StartNew(() =>
                    {
                        while (true)
                        {
                            Thread.Sleep((int)ApplicationArguments.Instance.ListenServerUpdateInterval*1000);
                            WriteTemplate();
                        }
                        
                    });
                    Console.WriteLine("Server is now listen on: " + ApplicationArguments.Instance.ListenServerIPEndPoint);
                    WebServer.Instance.BlockAndWait();
                }
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
            HtmlTemplate = Report.GenerateReports(Reports, !ApplicationArguments.Instance.IsListenServer, ApplicationArguments.Instance.Filename, ApplicationArguments.Instance.MaxDegreeOfParallelism);

            if (!ApplicationArguments.Instance.IsListenServer)
            {
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
        #endregion
    }
}
