/*
 * SystemInfoSnapshot
 * Author: Tiago Conceição
 * 
 * http://systeminfosnapshot.com/
 * https://github.com/sn4k3/SystemInfoSnapshot
 */
using System;
using System.Collections.Generic;
using Microsoft.Win32;

namespace SystemInfoSnapshot
{
    /// <summary>
    /// Represents a install program in the system.
    /// </summary>
    public sealed class InstalledProgram
    {
        #region Properties
        /// <summary>
        /// Program display name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Program display version.
        /// </summary>
        public string Version { get; private set; }

        /// <summary>
        /// Program Publisher.
        /// </summary>
        public string Publisher { get; private set; }

        /// <summary>
        /// Program installed dated.
        /// </summary>
        public string InstallDate { get; private set; }
        #endregion

        #region Construtor
        public InstalledProgram(string name, string version, string publisher, string installdate)
        {
            Name = name;
            Version = version;
            Publisher = publisher;
            InstallDate = installdate;
        }
        #endregion

        #region Overrides
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
        #endregion

        #region Methods
        /// <summary>
        /// Gets a list of programs installed on this machine.
        /// </summary>
        /// <returns></returns>
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
                                try
                                {
                                    object val;
                                    val = subkey.GetValue("DisplayName");
                                    var name = val == null ? string.Empty : val.ToString();

                                    val = subkey.GetValue("DisplayVersion");
                                    var version = val == null ? string.Empty : val.ToString();

                                    val = subkey.GetValue("Publisher");
                                    var publisher = val == null ? string.Empty : val.ToString();

                                    val = subkey.GetValue("InstallDate");
                                    var installDate = val == null ? string.Empty : val.ToString();

                                    //var releaseType = (string)subkey.GetValue("ReleaseType");
                                    //var unistallString = (string)subkey.GetValue("UninstallString");
                                    var systemComponent = subkey.GetValue("SystemComponent");
                                    //var parentName = (string)subkey.GetValue("ParentDisplayName");


                                    if (!(!string.IsNullOrEmpty(name)
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
                                catch (Exception)
                                {
                                    // ignored
                                }
                            }
                        }
                    }
                }
            }

            result.Sort((c1, c2) => String.Compare(c1.Name, c2.Name, StringComparison.Ordinal));

            return result;
        }
        #endregion
    }
}
