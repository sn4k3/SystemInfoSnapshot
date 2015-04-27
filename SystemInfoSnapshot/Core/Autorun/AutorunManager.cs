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
using SystemInfoSnapshot.Properties;

namespace SystemInfoSnapshot.Core.Autorun
{
    /// <summary>
    /// Sysinternals Autoruns v13.2 - Autostart program viewer
    /// Copyright (C) 2002-2015 Mark Russinovich
    /// Sysinternals - www.sysinternals.com
    /// Autorunsc shows programs configured to autostart during boot.
    /// Usage: autorunsc [-a <*|bdeghiklmoprsw>] [-c|-ct] [-h] [-m] [-s] [-u] [-vt] [[-z<systemroot> <userprofile>] | [user]]]
    /// -a   Autostart entry selection:
    /// *    All.
    /// b    Boot execute.
    /// d    Appinit DLLs.
    /// e    Explorer addons.
    /// g    Sidebar gadgets (Vista and higher)
    /// h    Image hijacks.
    /// i    Internet Explorer addons.
    /// k    Known DLLs.
    /// l    Logon startups (this is the default).
    /// m    WMI entries.
    /// n    Winsock protocol and network providers.
    /// o    Codecs.
    /// p    Printer monitor DLLs.
    /// r    LSA security providers.
    /// s    Autostart services and non-disabled drivers.
    /// t    Scheduled tasks.
    /// w    Winlogon entries.
    /// -c     Print output as CSV.
    /// -ct    Print output as tab-delimited values.
    /// -h     Show file hashes.
    /// -m     Hide Microsoft entries (signed entries if used with -v).
    /// -s     Verify digital signatures.
    /// -t     Show timestamps in normalized UTC (YYYYMMDD-hhmmss).
    /// -u     If VirusTotal check is enabled, show files that are unknown by VirusTotal or have non-zero detection, otherwise show only unsigned files.
    /// -x     Print output as XML.
    /// -v[rs] Query VirusTotal (www.virustotal.com) for malware based on file hash.
    /// Add 'r' to open reports for files with non-zero detection. Files 
    /// reported as not previously scanned will be uploaded to VirusTotal
    /// if the 's' option is specified. Note scan results may not be
    /// available for five or more minutes.
    /// -vt    Before using VirusTotal features, you must accept  VirusTotal terms of service. See:
    /// https://www.virustotal.com/en/about/terms-of-service/
    /// 
    /// If you haven't accepted the terms and you omit this option, you will be interactively prompted.
    /// -z     Specifies the offline Windows system to scan. 
    /// user   Specifies the name of the user account for which autorun items will be shown. Specify '*' to scan all user profiles.
    /// </summary>
    public sealed class AutorunManager : IEnumerable<AutorunItem>, IDisposable
    {
        #region Properties
        /// <summary>
        /// Gets the autoruns
        /// </summary>
        public List<AutorunItem> Autoruns { get; private set; }

        /// <summary>
        /// Gets the path for the executable file autorunsc.exe
        /// </summary>
        public static string ExecutableFile { get; private set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="getAll">True if you want populate this class with the predefined files, otherwise false to make it blank.</param>
        public AutorunManager(bool getAll = true)
        {
            Autoruns = getAll ? GetAuroruns() : new List<AutorunItem>();
        }
        #endregion

        #region Methods
        public void Update()
        {
            Autoruns = GetAuroruns();
        }

        public Dictionary<string, List<AutorunItem>> GetAsDictionary()
        {
            if (Autoruns.Count == 0)
                Update();

            var dict = new Dictionary<string, List<AutorunItem>>();
            foreach (var autorunEntry in Autoruns)
            {
                if (!dict.ContainsKey(autorunEntry.Category))
                {
                    dict.Add(autorunEntry.Category, new List<AutorunItem>());
                }

                dict[autorunEntry.Category].Add(autorunEntry);
            }
            
            return dict;
        }

        public void Clear()
        {
            if (!string.IsNullOrEmpty(ExecutableFile)) return;

            try
            {
                File.Delete(ExecutableFile);
                ExecutableFile = null;
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void Dispose()
        {
            Clear();
        }
        #endregion

        #region Static Methods

        /// <summary>
        /// Gets a list of special files on this machine.
        /// </summary>
        /// <returns></returns>
        public static List<AutorunItem> GetAuroruns()
        {
            var result = new List<AutorunItem>();
            if (!SystemHelper.IsWindows)
                return result;

            try
            {
                if (string.IsNullOrEmpty(ExecutableFile))
                {
                    ExecutableFile = Path.Combine(Path.GetTempPath(), "autorunsc.exe");
                    if (!File.Exists(ExecutableFile))
                        File.WriteAllBytes(ExecutableFile, Resources.autorunsc);
                }
                using (var proc = new System.Diagnostics.Process())
                {
                    proc.StartInfo.FileName = ExecutableFile;
                    proc.StartInfo.Arguments = "-a * -m -c -accepteula";
                    proc.StartInfo.CreateNoWindow = true;
                    proc.StartInfo.UseShellExecute = false;
                    proc.StartInfo.RedirectStandardOutput = true;
                    proc.Start();
                    int i = 0;
                    while (!proc.StandardOutput.EndOfStream)
                    {
                        i++;
                        string line = proc.StandardOutput.ReadLine();
                        if (i == 1 || string.IsNullOrEmpty(line))
                            continue;

                        var args = line.Split(',');
                        if (args.Length != 11)
                            continue;
                        for (var index = 0; index < args.Length; index++)
                        {
                            args[index] = args[index].Replace("\"", string.Empty);
                        }

                        byte argc = 0;
                        var entry = new AutorunItem();
                        if (!string.IsNullOrEmpty(args[argc]))
                        {
                            DateTime datetime;
                            DateTime.TryParse(args[argc], out datetime);
                            entry.Time = datetime;
                        }

                        argc++;
                        entry.EntryLocation = args[argc];

                        argc++;
                        entry.Entry = args[argc];

                        argc++;
                        entry.Enabled = args[argc].Equals("enabled");

                        argc++;
                        entry.Category = args[argc];

                        argc++;
                        entry.Profile = args[argc];

                        argc++;
                        entry.Description = args[argc];

                        argc++;
                        entry.Publisher = args[argc];

                        argc++;
                        entry.ImagePath = args[argc];

                        argc++;
                        entry.Version = args[argc];

                        argc++;
                        entry.LunchString = args[argc];

                        if (entry.ImagePath.StartsWith("File not found:"))
                        {
                            entry.IsValidFile = false;
                        }
                        if (entry.ImagePath.EndsWith("SystemInfoSnapshot.sys"))
                        {
                            i--;
                            continue;
                        }

                        result.Add(entry);
                    }

                    proc.Close();
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return result;
        }

        #endregion

        #region Overrides
        public IEnumerator<AutorunItem> GetEnumerator()
        {
            return Autoruns.GetEnumerator();
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
        public AutorunItem this[int index]    // Indexer declaration
        {
            get
            {
                return Autoruns[index];
            }

            set
            {
                Autoruns[index] = value;
            }
        }

        /// <summary>
        /// Indexers 
        /// </summary>
        /// <param name="name">name</param>
        /// <returns></returns>
        public AutorunItem this[string name]    // Indexer declaration
        {
            get
            { return Autoruns.FirstOrDefault(item => item.Equals(name)); }
        }
        #endregion
    }
}
