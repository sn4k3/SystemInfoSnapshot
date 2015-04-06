/*
 * SystemInfoSnapshot
 * Author: Tiago Conceição
 * 
 * http://systeminfosnapshot.com/
 * https://github.com/sn4k3/SystemInfoSnapshot
 */
using System;
using System.Linq;
using System.Windows.Forms;

namespace SystemInfoSnapshot
{
    static class Program
    {
        public const string Website = "http://systeminfosnapshot.com";
        public static HTMLTemplate HtmlTemplate;
        public static ApplicationArguments ApplicationArguments;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.AssemblyResolve += EmbeddedAssembly.OnResolveAssembly;
            ApplicationArguments = new ApplicationArguments();

            var args = Environment.GetCommandLineArgs().ToList();
            HtmlTemplate = new HTMLTemplate();

            // Null or Silent mode, skip GUI.
            if (ApplicationArguments.Null || ApplicationArguments.Silent)
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
            HtmlTemplate.WriteTitle(SystemInfo.GetTitleHtml());
            HtmlTemplate.WriteSystemInfo(SystemInfo.GetSystemInfoHTML());
            HtmlTemplate.WriteNetworkDevices(SystemInfo.GetNetworkDevicesHtml());
            HtmlTemplate.WritePnPDevices(SystemInfo.GetPnPDevicesHtml());
            HtmlTemplate.WriteProcesses(SystemInfo.GetProcessesHtml());
            HtmlTemplate.WriteServices(SystemInfo.GetServicesHtml());
            HtmlTemplate.WriteStartup(SystemInfo.GetStartupHtml());
            HtmlTemplate.WritePrograms(SystemInfo.GetProgramsHtml());
            HtmlTemplate.WriteToFile();

            if (ApplicationArguments.Silent)
            {
                HtmlTemplate.ShowInExplorer();
            }

            if (ApplicationArguments.OpenReport)
            {
                HtmlTemplate.OpenInDefaultBrowser();
            }
        }
    }
}
