/*
 * SystemInfoSnapshot
 * Author: Tiago Conceição
 * 
 * http://systeminfosnapshot.com/
 * https://github.com/sn4k3/SystemInfoSnapshot
 */

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
                return Silent && Null;
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
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor. Auto initalize arguments.
        /// </summary>
        public ApplicationArguments()
        {
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
        }
        #endregion

        #region Static methods
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

            var result = CmdParser.Parse(args);
            
            return result;

        }
        #endregion
    }
}
