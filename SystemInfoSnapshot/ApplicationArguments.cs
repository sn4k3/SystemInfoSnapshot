/*
 * SystemInfoSnapshot
 * Author: Tiago Conceição
 * 
 * http://systeminfosnapshot.com/
 * https://github.com/sn4k3/SystemInfoSnapshot
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace SystemInfoSnapshot
{
    /// <summary>
    /// Represent the application passed arguments.
    /// </summary>
    public sealed class ApplicationArguments
    {
        #region Singleton
        /// <summary>
        /// A instance of this class
        /// </summary>
        private static ApplicationArguments _instance;

        /// <summary>
        /// Gets the singleton instance of this class
        /// </summary>
        public static ApplicationArguments Instance
        {
            get { return _instance ?? (_instance = new ApplicationArguments());  }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets if Null mode as passed as argument.
        /// </summary>
        public bool Null { get; private set; }

        /// <summary>
        /// Gets if Silent mode as passed as argument.
        /// </summary>
        public bool Silent { get; private set; }

        /// <summary>
        /// Gets if OpenReport mode as passed as argument.
        /// </summary>
        public bool OpenReport { get; private set; }

        /// <summary>
        /// Gets if the reports will be generated under a single thread.
        /// </summary>
        public bool UseSingleThread { get; private set; }
        #endregion

        #region Arguments Variable
        /// <summary>
        /// Variable - Arguments list
        /// </summary>
        public readonly Dictionary<string, string[]>  Arguments = new Dictionary<string, string[]>
        {
            {"Null",            new []{"-n", "/n", "--null"}},
            {"Silent",          new []{"-s", "/s", "--silent"}},
            {"OpenReport",      new []{"-o", "/o", "--open-report"}},
            {"UseSingleThread", new []{"-st", "/st", "--single-thread"}}
        };
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor. Auto initalize arguments.
        /// </summary>
        private ApplicationArguments()
        {
            var args = Environment.GetCommandLineArgs().ToList();

            foreach (var argument in Arguments)
            {
                foreach (var s in argument.Value)
                {
                    if(!args.Contains(s))
                        continue;

                    var type = GetType();
                    var prop = type.GetProperty(argument.Key);
                    prop.SetValue(this, true);
                }
            }
        }
        #endregion
    }
}
