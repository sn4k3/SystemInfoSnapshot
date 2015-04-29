/*
 * SystemInfoSnapshot
 * Author: Tiago Conceição
 * 
 * http://systeminfosnapshot.com/
 * https://github.com/sn4k3/SystemInfoSnapshot
 */

using System.Collections.Generic;
using System.Net;
using Fclp;

namespace SystemInfoSnapshot
{
    /// <summary>
    /// Represent the application passed arguments.
    /// </summary>
    public sealed class ApplicationArguments
    {
        #region Singleton
        /// <summary>
        /// Gets the singleton instance of this class
        /// </summary>
        public static ApplicationArguments Instance
        {
            get { return CmdParser.Object; }
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets the parameter parser
        /// </summary>
        public static FluentCommandLineParser<ApplicationArguments> CmdParser = new FluentCommandLineParser<ApplicationArguments> { IsCaseSensitive = false };

        /// <summary>
        /// Gets if program will lunch without the GUI. (Terminal only)
        /// </summary>
        public bool NoGUI
        {
            get
            {
                return Silent || Null;
            }
        }

        /// <summary>
        /// Gets if Null mode as passed as argument.
        /// </summary>
        public bool Null { get; set; }

        /// <summary>
        /// Gets if Silent mode as passed as argument.
        /// </summary>
        public bool Silent { get; set; }

        /// <summary>
        /// Gets if OpenReport mode as passed as argument.
        /// </summary>
        public bool OpenReport { get; set; }

        /// <summary>
        /// Gets if the reports will be generated under a single thread.
        /// </summary>
        public int MaxDegreeOfParallelism { get; set; }

        /// <summary>
        /// Gets the full or partial filename to save the report
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// Gets the listen server args
        /// </summary>
        public List<string> ListenServerArgs { get; set; }

        /// <summary>
        /// Gets if the web server is active or not
        /// </summary>
        public bool IsListenServer { get; set; }

        /// <summary>
        /// Gets the web server <see cref="IPEndPoint"/>
        /// </summary>
        public IPEndPoint ListenServerIPEndPoint { get; set; }

        /// <summary>
        /// Gets the update interval
        /// </summary>
        public uint ListenServerUpdateInterval { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor. Auto initalize arguments.
        /// </summary>
        public ApplicationArguments()
        {
            ListenServerArgs = new List<string>();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Fix some paramters and define limits.
        /// </summary>
        public void Fix()
        {
            // Set to -1 if 0 or lower
            if (MaxDegreeOfParallelism <= 0)
            {
                MaxDegreeOfParallelism = -1;
            }

            if (string.IsNullOrWhiteSpace(Filename))
            {
                Filename = null;
            }

            ListenServerIPEndPoint = new IPEndPoint(IPAddress.Any, Core.Web.WebServer.DefaultPort);
            if (ListenServerArgs.Count > 0)
            {
                IsListenServer = true;
                OpenReport = false;
                if (Silent)
                {
                    Silent = false;
                    Null = true;
                }

                var args = ListenServerArgs[0].Split(':');
                IPAddress address;
                if (args[0].Equals("localhost") || args[0].Equals("loopback"))
                {
                    ListenServerIPEndPoint.Address = IPAddress.Loopback;
                }
                else if (args[0].Equals("*") || args[0].Equals("0:0:0:0") || args[0].Equals("any"))
                {
                    // ListenServerIPEndPoint.Address = IPAddress.Any;
                }
                else if (IPAddress.TryParse(args[0], out address))
                {
                    ListenServerIPEndPoint.Address = address;
                }

                if (args.Length >= 2)
                {
                    ushort port;
                    if (ushort.TryParse(args[1], out port))
                    {
                        if (port > 0)
                        {
                            ListenServerIPEndPoint.Port = port;
                        }
                    }
                }

                uint updateinterval = 10;
                if (ListenServerArgs.Count > 1)
                {
                    if (uint.TryParse(ListenServerArgs[1], out updateinterval))
                    {
                        if (updateinterval < 1)
                            updateinterval = 5;
                    }
                    else
                    {
                        updateinterval = 10;
                    }
                }
                ListenServerUpdateInterval = updateinterval;
            }
        }
        #endregion

        #region Static Methods
        public static ICommandLineParserResult Init(string[] args)
        {
            CmdParser.SetupHelp("?", "help").Callback(SystemHelper.DisplayMessage);
            CmdParser.Setup(arg => arg.Null).
                As('n', "null").
                SetDefault(false).
                WithDescription("Only generate the report without showing or doing anything. Good for scripts or tasks.");

            CmdParser.Setup(arg => arg.Silent).
                As('s', "silent").
                SetDefault(false).
                WithDescription("Run program without showing the GUI. After the report is generated that will be shown on explorer after completion.");

            CmdParser.Setup(arg => arg.OpenReport).
                As('o', "open-report").
                SetDefault(false).
                WithDescription("After the report is generated that will be opened automatically in the default browser.");

            CmdParser.Setup(arg => arg.MaxDegreeOfParallelism).
                As('t', "max-tasks").
                SetDefault(-1).
                WithDescription("Sets the maximum number of concurrent tasks enabled to generate the reports. If it is -1, there is no limit on the number of concurrently running operations (Default). " +
                                "If it is 1 it will run in a single thread, best used with single core CPUs or for debuging.");

            CmdParser.Setup(arg => arg.Filename).
                As('f', "target").
                SetDefault(null).
                WithDescription("Set the path and/or filename for the generated report file. If the passed path is a directory the default filename will be appended to it. Absolute or relative paths are allowed.");

            CmdParser.Setup(arg => arg.ListenServerArgs).
                As("listen-server").
                //SetDefault(null).
                WithDescription("Create a local web server and display live information about this machine on the web. Syntax: <<ip:port> OR <ip> OR <*:port>> [update-interval]");

            var result = CmdParser.Parse(args);
            
            return result;

        }
        #endregion
    }
}
