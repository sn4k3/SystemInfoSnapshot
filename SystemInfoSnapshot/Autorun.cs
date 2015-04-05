/*
 * SystemInfoSnapshot
 * Author: Tiago Conceição
 * 
 * http://systeminfosnapshot.com/
 * https://github.com/sn4k3/SystemInfoSnapshot
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Win32;

namespace SystemInfoSnapshot
{
    /// <summary>
    /// Represent a autorun program.
    /// </summary>
    public class Autorun
    {
        /// <summary>
        /// Short name for program entry location.
        /// </summary>
        public string Key { get; private set; }

        /// <summary>
        /// Program name.
        /// </summary>
        public string Program { get; private set; }

        /// <summary>
        /// Program location.
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// Autorun entry location.
        /// </summary>
        public string Location { get; private set; }



        public Autorun(string key, string program, string path, string location = null)
        {
            Key = key;
            Program = program;
            Path = path;
            Location = location;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj is string)
            {
                return Program.Equals(obj.ToString());
            }
            if (obj is Autorun)
            {
                return Program.Equals(((Autorun)obj).Program);
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return (Program != null ? Program.GetHashCode() : 0);
        }

        /// <summary>
        /// Gets all autorun programs.
        /// </summary>
        /// <returns>A List with all found programs.</returns>
        public static List<Autorun> GetAutoruns()
        {
            var autoruns = new Dictionary<RegistryKey, KeyValuePair<string, string>[]>
            {
                //@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall"
                //@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall"
               {Registry.LocalMachine, new []
               {
                   new KeyValuePair<string, string>("HKLM:Run",             "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run"), 
                   new KeyValuePair<string, string>("HKLM:RunOnce",         "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\RunOnce"), 
                   new KeyValuePair<string, string>("HKLM:RunServices",     "Software\\Microsoft\\Windows\\CurrentVersion\\RunServices"), 
                   new KeyValuePair<string, string>("HKLM:RunServicesOnce", "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\RunServicesOnce"), 
                   new KeyValuePair<string, string>("HKLM:WinLogon",        "Software\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon\\Userinit"), 
                   new KeyValuePair<string, string>("HKLM:ExplorerRun",     "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer\\Run"), 
                   
                   new KeyValuePair<string, string>("HKLM:Run",             "SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Run"), 
                   new KeyValuePair<string, string>("HKLM:Run",             "SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\RunOnce"), 
               }},
               {Registry.CurrentUser, new []{
                   new KeyValuePair<string, string>("HKCU:Run",             "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run"), 
                   new KeyValuePair<string, string>("HKCU:RunOnce",         "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\RunOnce"), 
                   new KeyValuePair<string, string>("HKCU:RunServices",     "Software\\Microsoft\\Windows\\CurrentVersion\\RunServices"), 
                   new KeyValuePair<string, string>("HKCU:RunServicesOnce", "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\RunServicesOnce"), 
                   new KeyValuePair<string, string>("HKCU:WinLogon",        "Software\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon\\Userinit"), 
                   new KeyValuePair<string, string>("HKCU:ExplorerRun",     "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\Explorer\\Run"), 
                   
                   new KeyValuePair<string, string>("HKCU:Run",             "SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Run"), 
                   new KeyValuePair<string, string>("HKCU:Run",             "SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\RunOnce"), 
               }},
            };

            var result = new List<Autorun>();


            foreach (var autorun in autoruns)
            {
                foreach (var runKey in autorun.Value)
                {
                    using (var startupKey = autorun.Key.OpenSubKey(runKey.Value))
                    {
                        if (startupKey == null) continue;
                        var valueNames = startupKey.GetValueNames();

                        result.AddRange(from valueName in valueNames 
                            where !string.IsNullOrEmpty(valueName) 
                            where startupKey.GetValueKind(valueName) == RegistryValueKind.String
                            where !result.Contains(new Autorun(string.Empty, valueName, string.Empty))
                            select new Autorun(runKey.Key, valueName, startupKey.GetValue(valueName).ToString(), runKey.Value));
                    }
                }
            }

            var paths = new Dictionary<string, string>
            {
                {"CommonStartup", Environment.GetFolderPath(Environment.SpecialFolder.CommonStartup)},
                {"Startup", Environment.GetFolderPath(Environment.SpecialFolder.Startup)},
            };

            result.AddRange(
                from path in paths where Directory.Exists(path.Value) 
                from file in Directory.EnumerateFiles(path.Value) 
                where !file.EndsWith(".ini") 
                select new Autorun(path.Key, System.IO.Path.GetFileName(file), file, path.Value));

            return result;
        }
    }
}
