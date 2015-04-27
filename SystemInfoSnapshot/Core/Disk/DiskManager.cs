using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        public Dictionary<uint, DiskItem> Disks { get; private set; }

        /// <summary>
        /// Gets the disks count
        /// </summary>
        public int Count { get { return Disks.Count;  } }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public DiskManager()
        {
            Disks = GetAll();
        }
        #endregion

        #region Methods

        public void Update()
        {
            Disks = GetAll();
        }
        #endregion

        #region Static Methods

        public static Dictionary<uint, DiskItem> GetAll()
        {
            var drives = new Dictionary<uint, DiskItem>();

            if (!SystemHelper.IsWindows)
                return drives;

            //var props = typeof (DiskItem).GetProperties();

            // Win32_DiskDrive
            var wdSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");

            // extract model and interface information
            foreach (ManagementObject drive in wdSearcher.Get())
            {
                var hdd = new DiskItem();

                /*foreach (var property in drive.Properties)
                {
                    Console.Write(property.Name + ": ");
                    Console.WriteLine(property.Value);
                }*/

                foreach (var prop in hdd.GetType().GetProperties())
                {
                    try
                    {
                        prop.SetValue(hdd, drive[prop.Name]);
                    }
                    catch (Exception)
                    {
                        //prop.SetValue(drives[iDriveIndex], null);
                    }

                }
                hdd.Model = string.IsNullOrEmpty(hdd.Model) ? "None" : hdd.Model.Trim();
                hdd.SerialNumber = string.IsNullOrEmpty(hdd.SerialNumber) ? "None" : hdd.SerialNumber.Trim();

                drives.Add(hdd.Index, hdd);
            } 


            // Win32_PhysicalMedia
            /*var pmsearcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");

            // retrieve hdd serial number
            //iDriveIndex = 0;
            foreach (ManagementObject drive in pmsearcher.Get())
            {
                // because all physical media will be returned we need to exit
                // after the hard drives serial info is extracted
                //if (iDriveIndex >= drives.Count)
                //    break;

                foreach (var property in drive.Properties)
                {
                    Console.Write(property.Name + ": ");
                    Console.WriteLine(property.Value);
                }

                
                foreach (var prop in drives[iDriveIndex].GetType().GetProperties())
                {
                    Console.WriteLine(prop.Name);
                    try
                    {
                        prop.SetValue(drives[iDriveIndex], drive[prop.Name]);
                    }
                    catch (Exception)
                    {
                        //prop.SetValue(drives[iDriveIndex], null);
                    }

                }

                
                //drives[iDriveIndex].SerialNumber = drive["SerialNumber"] == null ? "None" : drive["SerialNumber"].ToString().Trim();
                //iDriveIndex++;
            }*/
            // get wmi access to hdd 
            var searcher = new ManagementObjectSearcher("Select * from Win32_DiskDrive")
            {
                Scope = new ManagementScope(@"\root\wmi"),
                Query = new ObjectQuery("Select * from MSStorageDriver_FailurePredictStatus")
            };
            uint iDriveIndex = 0;

            try
            {
                // check if SMART reports the drive is failing

                foreach (ManagementObject drive in searcher.Get())
                {
                    drives[iDriveIndex].IsOK = (bool)drive.Properties["PredictFailure"].Value == false;
                    iDriveIndex++;
                }
            }
            catch (Exception)
            {
                // ignored
            }


            try
            {
                // retrive attribute flags, value worste and vendor data information
                searcher.Query = new ObjectQuery("Select * from MSStorageDriver_FailurePredictData");
                iDriveIndex = 0;
                foreach (ManagementObject data in searcher.Get())
                {
                    byte[] bytes = (Byte[]) data.Properties["VendorSpecific"].Value;
                    for (int i = 0; i < 30; ++i)
                    {
                        try
                        {
                            int id = bytes[i*12 + 2];

                            int flags = bytes[i*12 + 4];
                                // least significant status byte, +3 most significant byte, but not used so ignored.
                            //bool advisory = (flags & 0x1) == 0x0;
                            bool failureImminent = (flags & 0x1) == 0x1;
                            //bool onlineDataCollection = (flags & 0x2) == 0x2;

                            int value = bytes[i*12 + 5];
                            int worst = bytes[i*12 + 6];
                            int vendordata = BitConverter.ToInt32(bytes, i*12 + 7);
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
            }
            catch (Exception)
            {
                // ignored
            }

            try
            {
                // retreive threshold values foreach attribute
                searcher.Query = new ObjectQuery("Select * from MSStorageDriver_FailurePredictThresholds");
                iDriveIndex = 0;
                foreach (ManagementObject data in searcher.Get())
                {
                    Byte[] bytes = (Byte[]) data.Properties["VendorSpecific"].Value;
                    for (int i = 0; i < 30; ++i)
                    {
                        try
                        {

                            int id = bytes[i*12 + 2];
                            int thresh = bytes[i*12 + 3];
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
            }
            catch (Exception)
            {
                // ignored
            }


            return drives;
        }
        #endregion

        #region Overrides

        public IEnumerator<DiskItem> GetEnumerator()
        {
            return Disks.Values.GetEnumerator();
        }

        /// <summary>
        /// String representation of this class
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var device in Disks)
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
                return Disks[(uint)index];
            }

            set
            {
                Disks[(uint)index] = value;
            }
        }

        /// <summary>
        /// Indexers 
        /// </summary>
        /// <param name="index">index</param>
        /// <returns></returns>
        public DiskItem this[uint index]    // Indexer declaration
        {
            get
            {
                return Disks[index];
            }

            set
            {
                Disks[index] = value;
            }
        }

        public DiskItem this[string name]    // Indexer declaration
        {
            get
            {
                var result = Disks.FirstOrDefault(item => item.Value.Equals(name));
                if (ReferenceEquals(result.Value, null))
                {
                    result = Disks.FirstOrDefault(item => item.Value.Model.Equals(name));
                }
                return result.Value;
            }
        }

        #endregion
    }
}
