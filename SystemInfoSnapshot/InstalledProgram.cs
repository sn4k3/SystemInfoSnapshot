using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace SystemInfoSnapshot
{
    public sealed class InstalledProgram
    {
        public string Name { get; private set; }
        public string Version { get; private set; }
        public string Publisher { get; private set; }
        public string InstallDate { get; private set; }

        public InstalledProgram(string name, string version, string publisher, string installdate)
        {
            Name = name;
            Version = version;
            Publisher = publisher;
            InstallDate = installdate;
        }

        private bool Equals(InstalledProgram other)
        {
            return string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is InstalledProgram && Equals((InstalledProgram) obj);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }

        public static List<InstalledProgram> GetInstalledPrograms()
        {
            var programs = new Dictionary<RegistryKey, string[]>
            {
               {Registry.LocalMachine, new []
               {
                   "SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall",
                   "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall",
                   
               }},
               {Registry.CurrentUser, new [] 
               {
                   "SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall",
                   "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall",
               }},
            };

            var result = new List<InstalledProgram>();


            foreach (var program in programs)
            {
                foreach (var runKey in program.Value)
                {
                    using (var key = program.Key.OpenSubKey(runKey))
                    {
                        if (key == null) continue;

                        foreach (string subkeyName in key.GetSubKeyNames())
                        {
                            using (RegistryKey subkey = key.OpenSubKey(subkeyName))
                            {
                                var name = (string)subkey.GetValue("DisplayName");
                                var version = (string)subkey.GetValue("DisplayVersion");
                                var publisher = (string)subkey.GetValue("Publisher");
                                var installDate = (string)subkey.GetValue("InstallDate");
                                //var releaseType = (string)subkey.GetValue("ReleaseType");
                                //var unistallString = (string)subkey.GetValue("UninstallString");
                                var systemComponent = subkey.GetValue("SystemComponent");
                                //var parentName = (string)subkey.GetValue("ParentDisplayName");

                                
                                if(!(!string.IsNullOrEmpty(name)
                                //&& string.IsNullOrEmpty(releaseType)
                                //&& string.IsNullOrEmpty(parentName)
                                && (systemComponent == null)))
                                    continue;

                               

                                if (!string.IsNullOrEmpty(installDate) && installDate.Length == 8)
                                {
                                    installDate = string.Format("{0}-{1}-{2}", installDate.Substring(0, 4), installDate.Substring(4, 2), installDate.Substring(6, 2));
                                }

                                var iprogram = new InstalledProgram(name, version, publisher, installDate);
                                if (!result.Contains(iprogram))
                                    result.Add(iprogram);
                            }
                        }
                    }
                }
            }

            result.Sort((c1, c2) => String.Compare(c1.Name, c2.Name, StringComparison.Ordinal));

            return result;
        }

    }
}
