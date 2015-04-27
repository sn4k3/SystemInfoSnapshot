using SystemInfoSnapshot.Core.Autorun;
using SystemInfoSnapshot.Core.Device;
using SystemInfoSnapshot.Core.Hardware;
using SystemInfoSnapshot.Core.InstalledProgram;
using SystemInfoSnapshot.Core.Malware;
using SystemInfoSnapshot.Core.Process;
using SystemInfoSnapshot.Core.SpecialFile;

namespace SystemInfoSnapshot.Core
{
    public static class Managers
    {
        #region Autoruns
        private static AutorunManager _autorunManager;
        /// <summary>
        /// Gets the Process Manager
        /// </summary>
        public static AutorunManager AutorunManager
        {
            get { return _autorunManager ?? (_autorunManager = new AutorunManager(false)); }
        }
        #endregion

        #region Devices
        private static DeviceManager _deviceManager;
        /// <summary>
        /// Gets the Process Manager
        /// </summary>
        public static DeviceManager DeviceManager
        {
            get { return _deviceManager ?? (_deviceManager = new DeviceManager(false)); }
        }
        #endregion

        #region Hardware
        private static HardwareManager _hardwareManager;
        /// <summary>
        /// Gets the Process Manager
        /// </summary>
        public static HardwareManager HardwareManager
        {
            get { return _hardwareManager ?? (_hardwareManager = new HardwareManager()); }
        }
        #endregion

        #region InstalledPrograms
        private static InstalledProgramManager _installedProgramManager;
        /// <summary>
        /// Gets the InstalledProgram Manager
        /// </summary>
        public static InstalledProgramManager InstalledProgramManager
        {
            get { return _installedProgramManager ?? (_installedProgramManager = new InstalledProgramManager(false)); }
        }
        #endregion

        #region Malware
        /// <summary>
        /// Gets the Malware Manager
        /// </summary>
        public static MalwareManager MalwareManager
        {
            get { return MalwareManager.Instance; }
        }
        #endregion

        #region Processes
        private static ProcessManager _processManager;
        /// <summary>
        /// Gets the Process Manager
        /// </summary>
        public static ProcessManager ProcessManager
        {
            get { return _processManager ?? (_processManager = new ProcessManager(false)); }
        }
        #endregion

        #region SpecialFiles
        private static SpecialFileManager _specialFileManager;
        /// <summary>
        /// Gets the SpecialFiles Manager
        /// </summary>
        public static SpecialFileManager SpecialFileManager
        {
            get { return _specialFileManager ?? (_specialFileManager = new SpecialFileManager()); }
        }
        #endregion
    }
}
