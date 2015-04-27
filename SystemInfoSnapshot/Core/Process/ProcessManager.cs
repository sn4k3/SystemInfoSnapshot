/*
 * SystemInfoSnapshot
 * Author: Tiago Conceição
 * 
 * http://systeminfosnapshot.com/
 * https://github.com/sn4k3/SystemInfoSnapshot
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;

namespace SystemInfoSnapshot.Core.Process
{
    public sealed class ProcessManager : IEnumerable<ProcessItem>
    {
        #region Properties
        /// <summary>
        /// Gets the special files
        /// </summary>
        public List<ProcessItem> Processes { get; private set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="getAll">True if you want populate this class with all the processes on the machine, otherwise false to make it blank.</param>
        public ProcessManager(bool getAll = true)
        {
            Processes = getAll ? GetProcesses() : new List<ProcessItem>();
        }
        #endregion

        #region Methods

        /// <summary>
        /// Update all the processes
        /// </summary>
        public void Update()
        {
            Processes = GetProcesses();
        }

        #endregion

        #region Static Methods
        /// <summary>
        /// Gets a list of processes on this machine.
        /// </summary>
        /// <returns></returns>
        public static List<ProcessItem> GetProcesses()
        {
            return System.Diagnostics.Process.GetProcesses().Select(process => new ProcessItem(process)).OrderBy(item => -item.WorkingSet64).ToList();
        }

        /// <summary>
        /// Open a process
        /// </summary>
        /// <param name="filename">Filename to open</param>
        /// <param name="arguments">Arguments to pass</param>
        public static void Open(string filename, string arguments = null)
        {
            using (var process = new System.Diagnostics.Process())
            {
                process.StartInfo.FileName = filename;
                if (arguments != null) process.StartInfo.Arguments = arguments;
                process.Start();
                process.Close();
            }
        }

        /// <summary>
        /// Show and select a file in explorer
        /// </summary>
        /// <param name="path">Path to the file.</param>
        public static void ShowInExplorer(string path)
        {
            OpenExplorer(path, true);
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
                if (SystemHelper.IsWindows)
                {
                    // Use Microsoft's way of opening sites
                    using (var process = new System.Diagnostics.Process())
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
                    using (var proc = new System.Diagnostics.Process())
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
                if (SystemHelper.IsWindows)
                {
                    // Use Microsoft's way of opening sites
                    using (System.Diagnostics.Process.Start(address))
                    { }
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
                    using (System.Diagnostics.Process proc = new System.Diagnostics.Process())
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
        /// Cache for process info.
        /// </summary>
        private static readonly List<ManagementObject> ManagementObjects = new List<ManagementObject>();


        /// <summary>
        /// Get all avaliable process information from WMI.
        /// </summary>
        /// <param name="process">Process instance.</param>
        /// <param name="cache">True if you want to cache all results to latter reuse, otherwise false.</param>
        /// <returns><see cref="ManagementObject"/> object.</returns>
        public static ManagementObject GetProcessInfo(System.Diagnostics.Process process, bool cache = true)
        {
            string query;
            ManagementObjectSearcher searcher;
            if (!cache)
            {
                query = "SELECT * FROM Win32_Process where ProcessId='" + process.Id + "'";
                searcher = new ManagementObjectSearcher(query);
                return searcher.Get().Cast<ManagementObject>().FirstOrDefault();
            }

            if (ManagementObjects.Count == 0)
            {
                query = "SELECT * FROM Win32_Process";
                searcher = new ManagementObjectSearcher(query);
                foreach (var item in from ManagementObject item in searcher.Get() select item)
                {
                    ManagementObjects.Add(item);
                }
            }

            return (from managementObject in ManagementObjects let id = managementObject["ProcessID"] where id.ToString() == process.Id.ToString() select managementObject).FirstOrDefault();
        }

        /// <summary>
        /// Clears all process info from cache and force to rebuil cache on next <see cref="GetProcessInfo"/> call.
        /// </summary>
        public static void ClearProcessInfoCache()
        {
            ManagementObjects.Clear();
        }
        #endregion

        #region Overrides
        public IEnumerator<ProcessItem> GetEnumerator()
        {
            return Processes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region Indexers
        /// <summary>
        /// Indexers 
        /// </summary>
        /// <param name="index">index</param>
        /// <returns></returns>
        public ProcessItem this[int index]    // Indexer declaration
        {
            get
            {
                return Processes[index];
            }

            set
            {
                Processes[index] = value;
            }
        }

        #endregion
    }
}
