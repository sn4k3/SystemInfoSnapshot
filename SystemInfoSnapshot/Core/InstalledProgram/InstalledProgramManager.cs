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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.Win32;

namespace SystemInfoSnapshot.Core.InstalledProgram
{
    public sealed class InstalledProgramManager : IEnumerable<InstalledProgramItem>
    {
        #region Properties
        public List<InstalledProgramItem> InstalledPrograms { get; private set; }
        #endregion

        #region Constructor
        public InstalledProgramManager(bool getPrograms = true)
        {
            InstalledPrograms = getPrograms ? GetInstalledPrograms() : new List<InstalledProgramItem>();
        }
        #endregion

        #region Static Methods

        /// <summary>
        /// Gets a list of programs installed on this machine.
        /// </summary>
        /// <returns></returns>
        public static List<InstalledProgramItem> GetInstalledPrograms()
        {
            switch (SystemHelper.SystemOS)
            {
                    case SystemOS.Windows:
                        return GetInstalledProgramsWindows();
                    case SystemOS.MacOSX:
                        return GetInstalledProgramsMacOSX();
                    case SystemOS.Unix:
                        return GetInstalledProgramsUnix();
                    default:
                        return new List<InstalledProgramItem>();
            }
        }

        /// <summary>
        /// Gets a list of programs installed on this machine.
        /// </summary>
        /// <returns></returns>
        public static List<InstalledProgramItem> GetInstalledProgramsWindows()
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

            var result = new List<InstalledProgramItem>();


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
                                    var val = subkey.GetValue("DisplayName");
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

                                    var iprogram = new InstalledProgramItem(name, version, publisher, installDate);
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

            result.Sort((c1, c2) => string.Compare(c1.Name, c2.Name, StringComparison.Ordinal));

            return result;
        }

        /// <summary>
        /// Gets a list of programs installed on this machine.
        /// </summary>
        /// <returns></returns>
        public static List<InstalledProgramItem> GetInstalledProgramsUnix()
        {
            var result = new List<InstalledProgramItem>();

            var shellCommands = new Dictionary<string, string>
            {
                {"dpkg", "-l"},                 // Debian
                {"yum", "list installed"},      // CentOS, RedHat
                {"rpm", "-qa"},                 // CentOS, RedHat
                {"pkg", "info"},                // FreeBSD >= 10.X
                {"pkg_info", null},             // FreeBSD < 10.X
                {"pacman", "-Q"},               // Arch
                {"slapt-get", "--installed"},   // Slackware
                {"equery", "list"},             // Gentoo
                {"eix", "-I"},                  // Gentoo
                {"compgen", "-c"}               // Others
            };

            foreach (var shellCommand in shellCommands)
            {
                try
                {
                    using (var proc = new Process())
                    {
                        proc.StartInfo.FileName = shellCommand.Key;
                        proc.StartInfo.Arguments = shellCommand.Value;
                        proc.StartInfo.CreateNoWindow = true;
                        proc.StartInfo.UseShellExecute = false;
                        proc.StartInfo.RedirectStandardOutput = true;
                        proc.Start();
                        var i = 0;
                        while (!proc.StandardOutput.EndOfStream)
                        {
                            i++;
                            string line = proc.StandardOutput.ReadLine();
                            if (string.IsNullOrEmpty(line))
                                continue;

                            if (shellCommand.Key.Equals("dpkg")) // Debian, ignore no ii lines
                            {
                                if(!line.StartsWith("ii"))
                                    continue;
                            }

                            line = Regex.Replace(line, @"\s+", " ");
                            //MessageBox.Show(line);

                            var splitLine = line.Split(new []{' '}, 5);
                            var count = 0;

                            // Arch, pacman, freeBSD, openBSD    
                            if (splitLine.Length == 2)
                            {
                                InstalledProgramItem item;
                                if (shellCommand.Key.StartsWith("pkg")) // FreeBSD
                                    item = new InstalledProgramItem(splitLine[count++], null, null, null, splitLine[count++]); // (Name and description only)
                                else // Arch
                                    item = new InstalledProgramItem(splitLine[count++], splitLine[count++]); // (Name and version only)
                                result.Add(item);
                                continue;
                            }

                            // CentOS / RedHat (Name, version and arch)
                            if (shellCommand.Key.Equals("yum"))
                            {
                                if (splitLine.Length == 3)
                                {
                                    var item = new InstalledProgramItem(splitLine[count++],
                                        splitLine[count++], splitLine[count++]);
                                    result.Add(item);
                                }
                                continue;
                            }

                          
                            // dpkg, debian (ii, Name, version, arch, description)
                            if (splitLine.Length == 5)
                            {
                                count++;
                                var item = new InstalledProgramItem(splitLine[count++],
                                    splitLine[count++] + " " + splitLine[count++], null, null, splitLine[count++]);
                                result.Add(item);

                                continue;
                            }

                            // compgen and others, simple list with just one string (Name only)
                            if (splitLine.Length == 1)
                            {

                                var item = new InstalledProgramItem(splitLine[count++]);
                                result.Add(item);
                                continue;
                            }
                        }
                        if(result.Count > 0)
                            break;
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            

            return result;
        }

        /// <summary>
        /// Gets a list of programs installed on this machine.
        /// </summary>
        /// <returns></returns>
        public static List<InstalledProgramItem> GetInstalledProgramsMacOSX()
        {
            var result = new List<InstalledProgramItem>();
            try
            {
                const string folder = "/Applications/";
                foreach (var file in Directory.EnumerateDirectories(folder, "*.app", SearchOption.AllDirectories))
                {
                    result.Add(new InstalledProgramItem(file.Remove(file.Length - 4).Remove(0, folder.Length))); // Remove /Applications/ and .app from filename
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


            return result;
        }

        #endregion

        #region Overrides
        public IEnumerator<InstalledProgramItem> GetEnumerator()
        {
            return InstalledPrograms.GetEnumerator();
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
        public InstalledProgramItem this[int index]    // Indexer declaration
        {
            get
            {
                return InstalledPrograms[index];
            }

            set
            {
                InstalledPrograms[index] = value;
            }
        }

        /// <summary>
        /// Indexers 
        /// </summary>
        /// <param name="name">name</param>
        /// <returns></returns>
        public InstalledProgramItem this[string name]    // Indexer declaration
        {
            get
            { return InstalledPrograms.FirstOrDefault(item => item.Equals(name)); }
        }
        #endregion
    }
}
