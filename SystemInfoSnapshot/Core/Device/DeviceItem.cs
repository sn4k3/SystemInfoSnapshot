/*
 * SystemInfoSnapshot
 * Author: Tiago Conceição
 * 
 * http://systeminfosnapshot.com/
 * https://github.com/sn4k3/SystemInfoSnapshot
 */

namespace SystemInfoSnapshot.Core.Device
{
    /// <summary>
    /// Represents a device in the system.
    /// </summary>
    public sealed class DeviceItem
    {
        #region Properties
        // PnPDeviceInfo

        //public ushort Availability { get; private set; }
        //public string Caption { get; private set; }

        //public string ClassGuid { get; private set; }
        //public string[] CompatibleID { get; private set; }
        //public uint ConfigManagerErrorCode { get; private set; }
        //public bool ConfigManagerUserConfig { get; private set; }
        //public string CreationClassName { get; private set; }
        /// <summary>
        /// Gets the description
        /// </summary>
        public string Description { get; private set; }
        /// <summary>
        /// Gets the Device ID
        /// </summary>
        public string DeviceID { get; private set; }
        //public bool ErrorCleared { get; private set; }
        //public bool ErrorDescription { get; private set; }
        //public string[] HardwareID { get; private set; }
        //public DateTime InstallDate { get; private set; }
        //public uint LastErrorCode { get; private set; }
        public string Manufacturer { get; private set; }
        //public string PNPClass { get; private set; }
        /// <summary>
        /// Gets the PnP Device Id
        /// </summary>
        public string PnpDeviceID { get; private set; }
        //public ushort[] PowerManagementCapabilities { get; private set; }
        //public bool PowerManagementSupported { get; private set; }
        //public bool Present { get; private set; }
        //public string Service { get; private set; }
        //public string Status { get; private set; }
        //public ushort StatusInfo { get; private set; }
        //public string SystemCreationClassName { get; private set; }
        //public string SystemName { get; private set; }
        #endregion

        #region Construtor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="path"></param>
        public DeviceItem()
        {
        }

        public DeviceItem(string deviceID, string pnpDeviceID, string description)
        {
            DeviceID = deviceID;
            PnpDeviceID = pnpDeviceID;
            Description = description;
        }

        #endregion

        #region Overrides
        private bool Equals(DeviceItem other)
        {
            return string.Equals(DeviceID, other.DeviceID);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj is string)
                return DeviceID.Equals(obj.ToString());
            return obj is DeviceItem && Equals((DeviceItem)obj);
        }

        public override int GetHashCode()
        {
            return (DeviceID != null ? DeviceID.GetHashCode() : 0);
        }
        #endregion
    }
}
