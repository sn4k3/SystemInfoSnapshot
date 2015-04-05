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
        /// Variable - Arguments list
        /// </summary>
        public readonly Dictionary<string, string[]>  Arguments = new Dictionary<string, string[]>
        {
            {"Null",        new []{"-n", "/n", "--null"}},
            {"Silent",      new []{"-s", "/s", "--silent"}},
            {"OpenReport",  new []{"-o", "/o", "--open-report"}}
        };

        /// <summary>
        /// Constructor. Auto initalize arguments.
        /// </summary>
        public ApplicationArguments()
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
    }
}
