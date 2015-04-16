using System;
using System.Collections;
using System.Collections.Generic;
using System.Management;
using System.Text;

namespace SystemInfoSnapshot.Core.Disk
{
    public sealed class DiskManager : IEnumerable<DiskItem>
    {
        #region Properties
        /// <summary>
        /// Gets the avaliable devices list
        /// </summary>
        public List<DiskItem> Devices { get; private set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public DiskManager()
        {
            Devices = GetAll();
        }
        #endregion

        #region Static Methods

        public static List<DiskItem> GetAll()
        {
            var drives = new List<DiskItem>();

            // Win32_DiskDrive
            var wdSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");

            // extract model and interface information
            foreach (ManagementObject drive in wdSearcher.Get())
            {
                var hdd = new DiskItem
                {
                    Model = drive["Model"].ToString().Trim(),
                    Type = drive["InterfaceType"].ToString().Trim()
                };
                drives.Add(hdd);
            }

            // Win32_PhysicalMedia
            var pmsearcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");

            // retrieve hdd serial number
            var iDriveIndex = 0;
            foreach (ManagementObject drive in pmsearcher.Get())
            {
                // because all physical media will be returned we need to exit
                // after the hard drives serial info is extracted
                if (iDriveIndex >= drives.Count)
                    break;

                drives[iDriveIndex].Serial = drive["SerialNumber"] == null ? "None" : drive["SerialNumber"].ToString().Trim();
                iDriveIndex++;
            }

            // get wmi access to hdd 
            var searcher = new ManagementObjectSearcher("Select * from Win32_DiskDrive")
            {
                Scope = new ManagementScope(@"\root\wmi"),
                Query = new ObjectQuery("Select * from MSStorageDriver_FailurePredictStatus")
            };

            // check if SMART reports the drive is failing
            iDriveIndex = 0;
            foreach (ManagementObject drive in searcher.Get())
            {
                drives[iDriveIndex].IsOK = (bool)drive.Properties["PredictFailure"].Value == false;
                iDriveIndex++;
            }

            // retrive attribute flags, value worste and vendor data information
            searcher.Query = new ObjectQuery("Select * from MSStorageDriver_FailurePredictData");
            iDriveIndex = 0;
            foreach (ManagementObject data in searcher.Get())
            {
                byte[] bytes = (Byte[])data.Properties["VendorSpecific"].Value;
                for (int i = 0; i < 30; ++i)
                {
                    try
                    {
                        int id = bytes[i * 12 + 2];

                        int flags = bytes[i * 12 + 4]; // least significant status byte, +3 most significant byte, but not used so ignored.
                        //bool advisory = (flags & 0x1) == 0x0;
                        bool failureImminent = (flags & 0x1) == 0x1;
                        //bool onlineDataCollection = (flags & 0x2) == 0x2;

                        int value = bytes[i * 12 + 5];
                        int worst = bytes[i * 12 + 6];
                        int vendordata = BitConverter.ToInt32(bytes, i * 12 + 7);
                        if (id == 0) continue;

                        var attr = drives[iDriveIndex].Attributes[id];
                        attr.Current = value;
                        attr.Worst = worst;
                        attr.Raw = vendordata;
                        attr.IsOK = failureImminent == false;
                    }
                    catch
                    {
                        // given key does not exist in attribute collection (attribute not in the dictionary of attributes)
                    }
                }
                iDriveIndex++;
            }

            // retreive threshold values foreach attribute
            searcher.Query = new ObjectQuery("Select * from MSStorageDriver_FailurePredictThresholds");
            iDriveIndex = 0;
            foreach (ManagementObject data in searcher.Get())
            {
                Byte[] bytes = (Byte[])data.Properties["VendorSpecific"].Value;
                for (int i = 0; i < 30; ++i)
                {
                    try
                    {

                        int id = bytes[i * 12 + 2];
                        int thresh = bytes[i * 12 + 3];
                        if (id == 0) continue;

                        var attr = drives[iDriveIndex].Attributes[id];
                        attr.Threshold = thresh;
                    }
                    catch
                    {
                        // given key does not exist in attribute collection (attribute not in the dictionary of attributes)
                    }
                }

                iDriveIndex++;
            }


            return drives;
        }
        #endregion

        #region Overrides

        public IEnumerator<DiskItem> GetEnumerator()
        {
            return Devices.GetEnumerator();
        }

        /// <summary>
        /// String representation of this class
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var device in Devices)
            {
                sb.AppendLine(device.ToString());
            }

            return sb.ToString();
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
        public DiskItem this[int index]    // Indexer declaration
        {
            get
            {
                return Devices[index];
            }

            set
            {
                Devices[index] = value;
            }
        }

        #endregion
    }
}
