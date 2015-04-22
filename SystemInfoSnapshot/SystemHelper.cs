using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SystemInfoSnapshot
{
    #region SystemOS Enum
    /// <summary>
    /// Operating systems enum
    /// </summary>
    public enum SystemOS : byte
    {
        Windows,
        Unix,
        MacOSX,
        Other
    }

    public enum LinuxDistribution : byte
    {
        Debian = 0,
        RHEL,
        FreeBSD,
        Gentoo,
        Arch,
        Cygwin,
        Slackware,
        Other
    }
    #endregion

    /// <summary>
    /// System Helper Utilities
    /// </summary>
    public static class SystemHelper
    {
        #region PInvokes
        [DllImport("libc")]
        static extern int uname(IntPtr buf);
        #endregion

        #region Variables and Properties
        private static SystemOS? _systemOS;

        /// <summary>
        /// Get the current operating system wich program is running in
        /// </summary>
        public static SystemOS SystemOS
        {
            get
            {
                if (_systemOS.HasValue)
                    return _systemOS.Value;

                if (IsRunningOnWindows())
                    _systemOS = SystemOS.Windows;
                else if (IsRunningOnMac())
                    _systemOS = SystemOS.MacOSX;
                else if (Environment.OSVersion.Platform == PlatformID.Unix)
                    _systemOS = SystemOS.Unix;

                else
                    _systemOS = SystemOS.Other;

                return _systemOS.Value;
            }
            //private set { _systemOS = value; }
        }

        /// <summary>
        /// Is windows OS
        /// </summary>
        /// <returns>True if Windows, otherwise false</returns>
        public static bool IsWindows { get { return SystemOS == SystemOS.Windows; } }

        /// <summary>
        /// Is UNIX OS
        /// </summary>
        /// <returns>True if Unix, otherwise false</returns>
        public static bool IsUnix { get { return SystemOS == SystemOS.Unix; } }

        /// <summary>
        /// Is MacOSX
        /// </summary>
        /// <returns>True if MacOSX, otherwise false</returns>
        public static bool IsMacOSX { get { return SystemOS == SystemOS.MacOSX; } }
        #endregion

        #region Static Methods
        /// <summary>
        /// Get program files X86 path, windows only
        /// </summary>
        /// <returns>Program files X86 path</returns>
        public static string GetProgramFilesX86Path()
        {
            return !IsWindows ? string.Empty : Environment.GetFolderPath(Environment.Is64BitOperatingSystem ? Environment.SpecialFolder.ProgramFilesX86 : Environment.SpecialFolder.ProgramFiles);
        }

        /// <summary>
        /// Is windows OS
        /// </summary>
        /// <returns>True if Windows, otherwise false</returns>
        public static bool IsRunningOnWindows()
        {
            return Path.DirectorySeparatorChar == '\\' || Environment.OSVersion.Platform == PlatformID.Win32NT;
        }

        /// <summary>
        /// Is UNIX OS
        /// </summary>
        /// <returns>True if Unix, otherwise false</returns>
        public static bool IsRunningOnUnix()
        {
                var p = (int)Environment.OSVersion.Platform;
                return (p == 4) || (p == 6) || (p == 128);
        }

        /// <summary>
        /// Gets if running system is the Mac OSX system.
        /// </summary>
        /// <returns>True if is Mac OSX sytem, otherwise false.</returns>
        public static bool IsRunningOnMac()
        {
            IntPtr buf = IntPtr.Zero;
            try
            {
                buf = Marshal.AllocHGlobal(8192);
                // This is a hacktastic way of getting sysname from uname ()
                if (uname(buf) == 0)
                {
                    string os = Marshal.PtrToStringAnsi(buf);
                    if (os == "Darwin")
                        return true;
                }
            }
            catch
            {
                // ignored
            }
            finally
            {
                if (buf != IntPtr.Zero)
                    Marshal.FreeHGlobal(buf);
            }
            return false;
        }

        /// <summary>
        /// Gets if this program is running under mono runtime.
        /// </summary>
        /// <returns>True if running under mono, otherwise false.</returns>
        public static bool IsRunningOnMono()
        {
            return Type.GetType("Mono.Runtime") != null;
        }

        /// <summary>
        /// Open and select a file in explorer.
        /// </summary>
        /// <param name="path">Path to open.</param>
        /// <param name="selectFile">True to select file on explorer, otherwise false.</param>
        public static void OpenExplorer(string path, bool selectFile = false)
        {
            try
            {
                if (IsWindows)
                {
                    // Use Microsoft's way of opening sites
                    using (var process = new Process())
                    {
                        process.StartInfo.FileName = "explorer.exe";
                        process.StartInfo.Arguments = selectFile ? string.Format("/select,\"{0}\"", path) : path;
                        process.Start();
                        process.Close();
                    }
                }
                else
                {
                    if (selectFile) // Linux don't support select file, or maybe i don't know how...
                    {
                        path = Path.GetDirectoryName(path);
                    }
                    // We're on Unix, try gnome-open (used by GNOME), then open
                    // (used my MacOS), then Firefox or Konqueror browsers (our last
                    // hope).
                    string cmdline = string.Format("xdg-open {0} || gnome-open {0} || open {0}", path);
                    using (TextWriter textWriter = new StreamWriter("tempopenlink.sh"))
                    {
                        textWriter.WriteLine(cmdline);
                        textWriter.WriteLine("rm -f tempopenlink.sh");
                        textWriter.Close();
                    }
                    using (var proc = new Process())
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

       
        /// <summary>
        /// Open website link
        /// </summary>
        /// <param name="address">URL address</param>
        public static void OpenLink(string address)
        {
            try
            {
                if (IsWindows)
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

        public static void DisplayMessage(string text)
        {
            //Debug.WriteLine(text);
            Console.WriteLine(text);
            if (IsWindows || IsMacOSX)
            {
                MessageBox.Show(text);
            }
        }
        #endregion
    }
}