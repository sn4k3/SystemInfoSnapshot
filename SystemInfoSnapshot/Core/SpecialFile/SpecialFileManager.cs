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
using Microsoft.Win32;

namespace SystemInfoSnapshot.Core.SpecialFile
{
    public sealed class SpecialFileManager : IEnumerable<SpecialFileItem>
    {
        #region Properties
        /// <summary>
        /// Gets the special files
        /// </summary>
        public List<SpecialFileItem> SpecialFiles { get; private set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="getAll">True if you want populate this class with the predefined files, otherwise false to make it blank.</param>
        public SpecialFileManager(bool getAll = true)
        {
            SpecialFiles = getAll ? GetSpecialFiles() : new List<SpecialFileItem>();
        }
        #endregion

        #region Methods

        /// <summary>
        /// Gets a list of special files on this machine.
        /// </summary>
        /// <returns></returns>
        public static List<SpecialFileItem> GetSpecialFiles()
        {
            var specialFiles = new Dictionary<SystemOS, string[]>
            {
               {SystemOS.Windows, new []
               {
                   Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "drivers\\etc\\hosts"),
                   
               }},
               {SystemOS.Unix, new [] 
               {
                   "/etc/hosts",
               }},
               {SystemOS.MacOSX, new [] 
               {
                   "/private/etc/hosts",
                   "/etc/hosts",
               }},
            };

            var result = new List<SpecialFileItem>();

            if (!specialFiles.ContainsKey(SystemHelper.SystemOS))
                return result;

            foreach (var file in specialFiles[SystemHelper.SystemOS])
            {
                if (!File.Exists(file)) continue;

                try
                {
                    using (TextReader tr = new StreamReader(file))
                    {
                        var item = new SpecialFileItem(file);
                        string line;
                        // Read and display lines from the file until the end of 
                        // the file is reached.
                        while ((line = tr.ReadLine()) != null)
                        {
                            item.Content.AppendLine(line.Trim());
                        }
                        result.Add(item);
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine(file);
                    // ignored
                }
            }
            
            //result.Sort((c1, c2) => string.Compare(c1.Filename, c2.Filename, StringComparison.Ordinal));

            return result;
        }

        #endregion

        #region Overrides
        public IEnumerator<SpecialFileItem> GetEnumerator()
        {
            return SpecialFiles.GetEnumerator();
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
        public SpecialFileItem this[int index]    // Indexer declaration
        {
            get
            {
                return SpecialFiles[index];
            }

            set
            {
                SpecialFiles[index] = value;
            }
        }

        /// <summary>
        /// Indexers 
        /// </summary>
        /// <param name="name">name</param>
        /// <returns></returns>
        public SpecialFileItem this[string name]    // Indexer declaration
        {
            get
            { return SpecialFiles.FirstOrDefault(item => item.Equals(name)); }
        }
        #endregion
    }
}
