using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using SystemInfoSnapshot.Properties;

namespace SystemInfoSnapshot
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
    public sealed class Autoruns : IDisposable
    {
        public sealed class AutorunEntry
        {
            //Time,Entry Location,Entry,Enabled,Category,Profile,Description,Publisher,Image Path,Version,Launch String

            public DateTime Time { get; set; }
            public string EntryLocation { get; set; }
            public string Entry { get; set; }
            public bool Enabled { get; set; }
            public string Category { get; set; }
            public string Profile { get; set; }
            public string Description { get; set; }
            public string Publisher { get; set; }
            public string ImagePath { get; set; }
            public string Version { get; set; }
            public string LunchString { get; set; }

            public bool IsValidFile { get; set; }

            public AutorunEntry()
            {
                IsValidFile = true;
            }
        }
        public string ExecutableFile { get; private set; }
        public List<AutorunEntry> AutorunEntries { get; private set; }
        public Autoruns()
        {
            AutorunEntries = new List<AutorunEntry>();
        }

        public void BuildEntries()
        {
            try
            {
                if (string.IsNullOrEmpty(ExecutableFile))
                {
                    ExecutableFile = Path.Combine(Path.GetTempPath(), "autorunsc.exe");
                    File.WriteAllBytes(ExecutableFile, Resources.autorunsc);
                }
                using (var proc = new Process())
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
                        AutorunEntry entry = new AutorunEntry();
                        if (!string.IsNullOrEmpty(args[argc]))
                        {
                            DateTime result;
                            DateTime.TryParse(args[argc], out result);
                            entry.Time = result;
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

                        AutorunEntries.Add(entry);
                    }

                    proc.Close();
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public Dictionary<string, List<AutorunEntry>> GetAsDictionary()
        {
            if(AutorunEntries.Count == 0)
                BuildEntries();

            var dict = new Dictionary<string, List<AutorunEntry>>();
            foreach (var autorunEntry in AutorunEntries)
            {
                if (!dict.ContainsKey(autorunEntry.Category))
                {
                    dict.Add(autorunEntry.Category, new List<AutorunEntry>());
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
    }
}
