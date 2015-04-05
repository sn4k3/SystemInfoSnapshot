/*
 * SystemInfoSnapshot
 * Author: Tiago Conceição
 * 
 * http://systeminfosnapshot.com/
 * https://github.com/sn4k3/SystemInfoSnapshot
 */
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;

namespace SystemInfoSnapshot
{
    public class ProcessHelper
    {
        public const string ExecutablePath = "ExecutablePath";
        public const string StartTime = "CreationDate";
        public const string Caption = "Caption ";

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
        public static ManagementObject GetProcessInfo(Process process, bool cache = true)
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

        /// <summary>
        /// Open a process
        /// </summary>
        /// <param name="filename">Filename to open</param>
        /// <param name="arguments">Arguments to pass</param>
        public static void Open(string filename, string arguments = null)
        {
            using (var process = new Process())
            {
                process.StartInfo.FileName = filename;
                process.StartInfo.Arguments = arguments;
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
            using (var process = new Process())
            {
                process.StartInfo.FileName = "explorer.exe";
                process.StartInfo.Arguments = string.Format("/select,\"{0}\"", path);
                process.Start();
                process.Close();
            }
        }
    }
}
