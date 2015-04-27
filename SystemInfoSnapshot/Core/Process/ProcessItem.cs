/*
 * SystemInfoSnapshot
 * Author: Tiago Conceição
 * 
 * http://systeminfosnapshot.com/
 * https://github.com/sn4k3/SystemInfoSnapshot
 */

using System;
using System.Diagnostics;

namespace SystemInfoSnapshot.Core.Process
{
    /// <summary>
    /// Represents a install program in the system.
    /// </summary>
    public sealed class ProcessItem
    {
        #region Properties

        /// <summary>
        /// Gets the associated process
        /// </summary>
        public System.Diagnostics.Process Process { get; private set; }

        /// <summary>
        /// Gets the process id
        /// </summary>
        public int Id
        {
            get { return Process.Id; }
        }

        /// <summary>
        /// Gets the process name
        /// </summary>
        public string ProcessName
        {
            get
            {
                try
                {
                    return Process.ProcessName;
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the process executable path
        /// </summary>
        public string ExecutablePath
        {
            get
            {
                try // Try if process is executed under 32bit and system is 64bits we need to handle permission exceptions
                {
                    return Process.MainModule.FileName;
                }
                catch (Exception)
                {
                    if (SystemHelper.IsWindows)
                    {
                        var processinfo = ProcessManager.GetProcessInfo(Process);
                        if (ReferenceEquals(processinfo, null)) return null;
                        if (!ReferenceEquals(processinfo["ExecutablePath"], null))
                            return processinfo["ExecutablePath"].ToString();
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the process start time
        /// </summary>
        public DateTime StartTime
        {
            get
            {
                try
                {
                    return Process.StartTime;
                }
                catch (Exception)
                {
                    return DateTime.Now;
                }
            }
        }

        /// <summary>
        /// Gets the process threads
        /// </summary>
        public ProcessThreadCollection Threads
        {
            get { return Process.Threads; }
        }

        /// <summary>
        /// Gets the process PeakWorkingSet64
        /// </summary>
        public long PeakWorkingSet64
        {
            get
            {
                try
                {
                    return Process.PeakWorkingSet64;
                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Gets the process WorkingSet64
        /// </summary>
        public long WorkingSet64
        {
            get
            {
                try
                {
                    return Process.WorkingSet64;
                }
                catch
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// Gets the process up time
        /// </summary>
        public TimeSpan UpTime { get { return DateTime.Now.Subtract(StartTime); } }

        /// <summary>
        /// Gets the process up time string
        /// </summary>
        public string UpTimeString { get { return UpTime.ToString(@"d\.hh\:mm\:ss"); } }


        #endregion

        #region Construtor
        /// <summary>
        /// Constructor
        /// </summary>
        public ProcessItem(System.Diagnostics.Process process)
        {
            Process = process;
        }

        #endregion
    }
}
