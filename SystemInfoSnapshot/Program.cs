using System;
using System.Linq;
using System.Windows.Forms;

namespace SystemInfoSnapshot
{
    static class Program
    {

        public static HTMLTemplate HtmlTemplate;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.AssemblyResolve += EmbeddedAssembly.OnResolveAssembly;

            var args = Environment.GetCommandLineArgs().ToList();
            HtmlTemplate = new HTMLTemplate();

            // Silent mode, skip GUI.
            if (args.Contains("/s") || args.Contains("-s") || args.Contains("--silent"))
            {
                WriteTemplate();
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FrmMain());
            }
            
        }

        /// <summary>
        /// Write all info into template and output the html file.
        /// </summary>
        public static void WriteTemplate()
        {
            HtmlTemplate.WriteTitle(SystemInfo.GetTitleHtml());
            HtmlTemplate.WriteSystemInfo(SystemInfo.GetSystemInfoHTML());
            HtmlTemplate.WriteProcesses(SystemInfo.GetProcessesHtml());
            HtmlTemplate.WriteServices(SystemInfo.GetServicesHtml());
            HtmlTemplate.WriteStartup(SystemInfo.GetStartupHtml());
            HtmlTemplate.WritePrograms(SystemInfo.GetProgramsHtml());
            HtmlTemplate.WriteToFile();
        }
    }
}
