using System;
using System.Linq;
using System.Windows.Forms;

namespace SystemInfoSnapshot
{
    static class Program
    {

        public static HTMLTemplate htmlTemplate;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.AssemblyResolve += EmbeddedAssembly.OnResolveAssembly;

            var args = Environment.GetCommandLineArgs().ToList();
            htmlTemplate = new HTMLTemplate();

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

        public static void WriteTemplate()
        {
            htmlTemplate.WriteTitle(SystemInfo.GetTitleHTML());
            htmlTemplate.WriteSystemInfo(SystemInfo.GetSystemInfoHTML());
            htmlTemplate.WriteProcesses(SystemInfo.GetProcessesHTML());
            htmlTemplate.WriteServices(SystemInfo.GetServicesHTML());
            htmlTemplate.WriteStartup(SystemInfo.GetStartupHTML());
            htmlTemplate.WritePrograms(SystemInfo.GetProgramsHTML());
            htmlTemplate.WriteToFile();
        }
    }
}
