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
    }
}
