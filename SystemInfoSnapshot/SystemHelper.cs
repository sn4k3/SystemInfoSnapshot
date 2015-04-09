using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace SystemInfoSnapshot
{
    /// <summary>
    /// System Helper Utilities
    /// </summary>
    public sealed class SystemHelper {
        /// <summary>
        /// Get program files X86 path, windows only
        /// </summary>
        /// <returns>Program files X86 path</returns>
        public static string GetProgramFilesX86Path()
        {
            if (!IsWindows())
            {
                return string.Empty;
            }
            return Environment.GetFolderPath(Environment.Is64BitOperatingSystem ? Environment.SpecialFolder.ProgramFilesX86 : Environment.SpecialFolder.ProgramFiles);
        }

        /// <summary>
        /// Is windows OS
        /// </summary>
        /// <returns>True if Windows, otherwise false</returns>
        public static bool IsWindows()
        {
            return !string.IsNullOrEmpty(Environment.GetFolderPath(Environment.SpecialFolder.Programs));
        }

        /// <summary>
        /// Is UNIX OS
        /// </summary>
        /// <returns>True if Unix, otherwise false</returns>
        public static bool IsUnix()
        {
                var p = (int)Environment.OSVersion.Platform;
                return (p == 4) || (p == 6) || (p == 128);
        }

       
        /// <summary>
        /// Open website link
        /// </summary>
        /// <param name="address">URL address</param>
        public static void OpenLink(string address)
        {
            try
            {
                if (!IsUnix())
                {
                    // Use Microsoft's way of opening sites
                    using(Process.Start(address))
                    {}
                }
                else
                {
                    // We're on Unix, try gnome-open (used by GNOME), then open
                    // (used my MacOS), then Firefox or Konqueror browsers (our last
                    // hope).
                    string cmdline = string.Format("xdg-open {0} || gnome-open {0} || open {0} || " +
                        "chromium-browser {0} || mozilla-firefox {0} || firefox {0} || konqueror {0}", address);
                    using (TextWriter textWriter = new StreamWriter("tempopenlink.sh"))
                    {
                        textWriter.WriteLine(cmdline);
                        textWriter.WriteLine("rm -f tempopenlink.sh");
                        textWriter.Close();
                    }
                    using (Process proc = new Process())
                    {
                        proc.StartInfo.FileName = "sh";
                        proc.StartInfo.Arguments = "tempopenlink.sh";
                        proc.Start();
                        proc.Close();
                    }
                }
            }
            catch (Exception)
            {
                // We don't want any surprises
            }
        }
    }
}