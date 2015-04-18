using System;
using System.Collections.Generic;
using System.Text;

namespace SystemInfoSnapshot.Core.Disk
{
    /// <summary>
    /// Represents a HDD 
    /// </summary>
    public sealed class DiskItem
    {
        #region Properties
        /// <summary>
        /// Gets or sets if the HDD is ok
        /// </summary>
        public bool IsOK { get; set; }

        

        #region Win32_DiskDrive
        /// <summary>
        /// Physical drive number of the given drive. 
        /// This property is filled by the GetDriveMapInfo method. 
        /// A value of 0xFF indicates that the given drive does not map to a physical drive.
        /// </summary>
        public uint Index { get; set; }

        /// <summary>
        /// Number of bytes in each sector for the physical disk drive.
        /// </summary>
        public uint BytesPerSector { get; set; }

        /// <summary>
        /// Description of the object. 
        /// This property is inherited from CIM_ManagedSystemElement.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Unique identifier of the disk drive with other devices on the system. 
        /// This property is inherited from CIM_LogicalDevice.
        /// </summary>
        public string DeviceID { get; set; }

        /// <summary>
        /// Revision for the disk drive firmware that is assigned by the manufacturer.
        /// </summary>
        public string FirmwareRevision { get; set; }

        /// <summary>
        /// Revision for the disk drive firmware that is assigned by the manufacturer.
        /// </summary>
        public string Manufacturer { get; set; }

        /// <summary>
        /// If True, the media for a disk drive is loaded, which means that the device has a readable file system and is accessible. For fixed disk drives, this property will always be TRUE.
        /// </summary>
        public bool MediaLoaded { get; set; }

        /// <summary>
        /// Type of media used or accessed by this device.
        /// Starting with Windows Vista, possible values are:
        /// External hard disk media
        /// Removable media other than floppy
        /// Fixed hard disk media
        /// Format is unknown
        /// </summary>
        public string MediaType { get; set; }

        /// <summary>
        /// Interface type of physical disk drive.
        /// The values are:
        /// SCSI
        /// HDC
        /// IDE
        /// USB
        /// 1394
        /// </summary>
        public string InterfaceType { get; set; }

        /// <summary>
        /// Manufacturer's model number of the disk drive.
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Name of the disk drive manufacturer.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// If True, the media access device needs cleaning. Whether manual or automatic cleaning is possible is indicated in the Capabilities property. 
        /// This property is inherited from CIM_MediaAccessDevice.
        /// </summary>
        public bool NeedsCleaning { get; set; }

        /// <summary>
        /// Maximum number of media which can be supported or inserted (when the media access device supports multiple individual media). 
        /// This property is inherited from CIM_MediaAccessDevice.
        /// </summary>
        public uint NumberOfMediaSupported { get; set; }

        /// <summary>
        /// Number of partitions on this physical disk drive that are recognized by the operating system.
        /// </summary>
        public uint Partitions { get; set; }

        /// <summary>
        /// Windows Plug and Play device identifier of the logical device. 
        /// This property is inherited from CIM_LogicalDevice.
        /// </summary>
        public string PNPDeviceID { get; set; }

        /// <summary>
        /// SCSI bus number of the disk drive.
        /// </summary>
        public uint SCSIBus { get; set; }

        /// <summary>
        /// SCSI logical unit number (LUN) of the disk drive.
        /// </summary>
        public ushort SCSILogicalUnit { get; set; }

        /// <summary>
        /// SCSI port number of the disk drive.
        /// </summary>
        public ushort SCSIPort { get; set; }

        /// <summary>
        /// SCSI identifier number of the disk drive.
        /// </summary>
        public ushort SCSITargetId { get; set; }

        /// <summary>
        /// Number of sectors in each track for this physical disk drive.
        /// </summary>
        public uint SectorsPerTrack { get; set; }

        /// <summary>
        /// Number allocated by the manufacturer to identify the physical media.
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// Disk identification. This property can be used to identify a shared resource.
        /// </summary>
        public uint Signature { get; set; }

        /// <summary>
        /// Size of the disk drive. 
        /// It is calculated by multiplying the total number of cylinders, tracks in each cylinder, sectors in each track, and bytes in each sector.
        /// </summary>
        public ulong Size { get; set; }

        /// <summary>
        /// Current status of the object. Various operational and nonoperational statuses can be defined. Operational statuses include: "OK", "Degraded", and "Pred Fail" (an element, such as a SMART-enabled hard disk drive, may be functioning properly but predicting a failure in the near future). Nonoperational statuses include: "Error", "Starting", "Stopping", and "Service". The latter, "Service", could apply during mirror-resilvering of a disk, reload of a user permissions list, or other administrative work. Not all such work is online, yet the managed element is neither "OK" nor in one of the other states. This property is inherited from CIM_ManagedSystemElement.
        /// Values are:
        /// "OK"
        /// "Error"
        /// "Degraded"
        /// "Unknown"
        /// "Pred Fail"
        /// "Starting"
        /// "Stopping"
        /// "Service"
        /// "Stressed"
        /// "NonRecover"
        /// "No Contact"
        /// "Lost Comm"
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Total number of cylinders on the physical disk drive. 
        /// Note: the value for this property is obtained through extended functions of BIOS interrupt 13h. 
        /// The value may be inaccurate if the drive uses a translation scheme to support high-capacity disk sizes. Consult the manufacturer for accurate drive specifications.
        /// </summary>
        public ulong TotalCylinders { get; set; }

        /// <summary>
        /// Total number of heads on the disk drive. 
        /// Note: the value for this property is obtained through extended functions of BIOS interrupt 13h. The value may be inaccurate if the drive uses a translation scheme to support high-capacity disk sizes. Consult the manufacturer for accurate drive specifications.
        /// </summary>
        public uint TotalHeads { get; set; }

        /// <summary>
        /// Total number of sectors on the physical disk drive. 
        /// Note: the value for this property is obtained through extended functions of BIOS interrupt 13h. 
        /// The value may be inaccurate if the drive uses a translation scheme to support high-capacity disk sizes. Consult the manufacturer for accurate drive specifications.
        /// </summary>
        public uint TotalSectors { get; set; }

        /// <summary>
        /// Total number of tracks on the physical disk drive. 
        /// Note: the value for this property is obtained through extended functions of BIOS interrupt 13h. 
        /// The value may be inaccurate if the drive uses a translation scheme to support high-capacity disk sizes. Consult the manufacturer for accurate drive specifications.
        /// </summary>
        public ulong TotalTracks { get; set; }

        /// <summary>
        /// Number of tracks in each cylinder on the physical disk drive. 
        /// Note: the value for this property is obtained through extended functions of BIOS interrupt 13h. 
        /// The value may be inaccurate if the drive uses a translation scheme to support high-capacity disk sizes. Consult the manufacturer for accurate drive specifications.
        /// </summary>
        public uint TracksPerCylinder { get; set; }
        #endregion

        #region Win32_PhysicalMedia
        /*
        /// <summary>
        /// Gets or sets the HDD serial number, Manufacturer-allocated number used to identify the physical media.
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// Short, one-line textual description of the object.
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// Textual description of the object.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// When the object was installed. This property does not require a value to indicate that the object is installed.
        /// </summary>
        public DateTime InstallDate { get; set; }

        /// <summary>
        /// Label by which the object is known. When subclassed, the Name property can be overridden to be a Key property.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Current status of the object. 
        /// Various operational and nonoperational statuses can be defined. 
        /// Operational statuses are "OK", "Degraded", and "Pred Fail". 
        /// "Pred Fail" indicates that an element may function properly at the present, but predicts a failure in the near future. For example, a SMART-enabled hard disk drive. Nonoperational statuses can also be specified. These are, "Error", "Starting", "Stopping," and "Service". The "Service" status applies to administrative work, such as mirror-resilvering of a disk or reload of a user permissions list. Not all such work is online, yet the managed element is neither "OK" nor in one of the other states.
        /// "OK"
        /// "Error"
        /// "Degraded"
        /// "Unknown"
        /// "Pred Fail"
        /// "Starting"
        /// "Stopping"
        /// "Service"
        /// "Stressed"
        /// "NonRecover"
        /// "No Contact"
        /// "Lost Comm"
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Name of the class or subclass used in the creation of an instance. 
        /// When used with other key properties of this class, CreationClassName allows all instances of this class and its subclasses to be uniquely identified.
        /// </summary>
        public string CreationClassName { get; set; }

        /// <summary>
        /// Name of the organization responsible for producing the physical element. 
        /// This can be the entity from whom the element is purchased, but this is not necessarily the case as this information is contained in the Vendor property.
        /// </summary>
        public string Manufacturer { get; set; }

        /// <summary>
        /// Stock keeping unit number for this physical element.
        /// </summary>
        public string SKU { get; set; }

        /// <summary>
        /// Version of the physical element.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Part number assigned by the manufacturer of the physical element.
        /// </summary>
        public string PartNumber { get; set; }

        /// <summary>
        /// Additional data, beyond asset tag information, that can be used to identify a physical element. One example is bar code data associated with an element that also has an asset tag. Note that if only bar code data is available, is unique, and it can be used as an element key, this property is NULL and the bar code data used is the class key in the Tag property.
        /// </summary>
        public string OtherIdentifyingInfo { get; set; }

        /// <summary>
        /// If TRUE the physical element is powered on.
        /// </summary>
        public bool PoweredOn { get; set; }

        /// <summary>
        /// If TRUE, the physical component is designed to be taken in and out of the physical container in which it is normally found, without impairing the function of the overall packaging. A component can still be removable if the power must be "off" to perform the removal. If power can be "on" and the component removed, the element is removable and can be hot-swapped.
        /// </summary>
        public bool Removable { get; set; }

        /// <summary>
        /// If TRUE, this physical media component can be replaced with a physically different one. For example, some computer systems allow the main processor chip to be upgraded to one of a higher clock rating. In this case, the processor is said to be replaceable. All removable components are inherently replaceable.
        /// </summary>
        public bool Replaceable { get; set; }

        /// <summary>
        /// If TRUE, this physical media component can be replaced with a physically different but equivalent one while the containing package has the power applied. For example, a fan component may be designed to be hot-swapped. 
        /// All components that can be hot-swapped are inherently removable and replaceable.
        /// </summary>
        public bool HotSwappable { get; set; }
         
        /// <summary>
        /// Number of bytes that can be read from or written to this physical media component. 
        /// This property does not apply to "Hard Copy" or cleaner media. 
        /// Data compression should not be assumed as it would increase the value of this property. 
        /// For tapes, it should be assumed that no filemarks or blank space areas are recorded on the media.
        /// </summary>
        public ulong Capacity { get; set; }

        /// <summary>
        /// The type of the media, as an enumerated integer. The MediaDescription property provides a more explicit definition of the media type.
        /// </summary>
        public ushort MediaType { get; set; }

        /// <summary>
        /// Additional detail related to the MediaType property. 
        /// For example, if MediaType has the value 3 (QIC Cartridge) the MediaDescription property can indicate whether the tape is wide or quarter inch.
        /// </summary>
        public string MediaDescription { get; set; }

        /// <summary>
        /// If TRUE, the media is currently write protected by some kind of physical mechanism, such as a protect tab on a floppy disk.
        /// </summary>
        public bool WriteProtectOn { get; set; }

        /// <summary>
        /// If TRUE, the physical media is used for cleaning purposes and not data storage.
        /// </summary>
        public bool CleanerMedia { get; set; }
         */
        #endregion

        /// <summary>
        /// Gets the HDD Smart attributes
        /// </summary>
        public Dictionary<int, DiskSmart> Attributes { get; private set; }
        #endregion
         
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public DiskItem()
        {
            // http://en.wikipedia.org/wiki/S.M.A.R.T.
            Attributes = new Dictionary<int, DiskSmart> {
                // ID              // Name          // Lower value is better?   // Is critical?
                {0x00, new DiskSmart("Invalid")},
                {0x01, new DiskSmart("Raw read error rate",                 true,   false)},
                {0x02, new DiskSmart("Throughput performance",              false,  false)},
                {0x03, new DiskSmart("Spinup time",                         true,   false)},
                {0x04, new DiskSmart("Start/Stop count",                    true,   false)},
                {0x05, new DiskSmart("Reallocated sector count",            true,   true)},
                {0x06, new DiskSmart("Read channel margin",                 true,   false)},
                {0x07, new DiskSmart("Seek error rate",                     true,   false)},
                {0x08, new DiskSmart("Seek timer performance",              false,  false)},
                {0x09, new DiskSmart("Power-on hours count",                true,   false)},
                {0x0A, new DiskSmart("Spinup retry count",                  false,  true)},
                {0x0B, new DiskSmart("Calibration retry count",             true,   false)},
                {0x0C, new DiskSmart("Power cycle count",                   true,   false)},
                {0x0D, new DiskSmart("Soft read error rate",                true,   false)},
                {0xB8, new DiskSmart("End-to-End error",                    true,   true)},
                {0xBE, new DiskSmart("Airflow Temperature",                 false,  false)},
                {0xBF, new DiskSmart("G-sense error rate",                  true,   false)},
                {0xC0, new DiskSmart("Power-off retract count",             true,   false)},
                {0xC1, new DiskSmart("Load/Unload cycle count",             true,   false)},
                {0xC2, new DiskSmart("HDD temperature",                     true,   false)},
                {0xC3, new DiskSmart("Hardware ECC recovered",              true,   false)},
                {0xC4, new DiskSmart("Reallocation count",                  true,   true)},
                {0xC5, new DiskSmart("Current pending sector count",        true,   true)},
                {0xC6, new DiskSmart("Offline scan uncorrectable count",    true,   true)},
                {0xC7, new DiskSmart("UDMA CRC error rate",                 true,   false)},
                {0xC8, new DiskSmart("Write error rate",                    true,   false)},
                {0xC9, new DiskSmart("Soft read error rate",                true,   true)},
                {0xCA, new DiskSmart("Data Address Mark errors",            true,   false)},
                {0xCB, new DiskSmart("Run out cancel",                      true,   false)},
                {0xCC, new DiskSmart("Soft ECC correction",                 true,   false)},
                {0xCD, new DiskSmart("Thermal asperity rate (TAR)",         true,   false)},
                {0xCE, new DiskSmart("Flying height",                       true,   false)},
                {0xCF, new DiskSmart("Spin high current",                   true,   false)},
                {0xD0, new DiskSmart("Spin buzz",                           true,   false)},
                {0xD1, new DiskSmart("Offline seek performance",            false,  false)},
                {0xDC, new DiskSmart("Disk shift",                          true,   false)},
                {0xDD, new DiskSmart("G-sense error rate",                  true,   false)},
                {0xDE, new DiskSmart("Loaded hours",                        true,   false)},
                {0xDF, new DiskSmart("Load/unload retry count",             true,   false)},
                {0xE0, new DiskSmart("Load friction",                       true,   false)},
                {0xE1, new DiskSmart("Load/Unload cycle count",             true,   false)},
                {0xE2, new DiskSmart("Load-in time",                        true,   false)},
                {0xE3, new DiskSmart("Torque amplification count",          true,   false)},
                {0xE4, new DiskSmart("Power-off retract count",             true,   false)},
                {0xE6, new DiskSmart("GMR head amplitude",                  true,   false)},
                {0xE7, new DiskSmart("Temperature",                         true,   false)},
                {0xF0, new DiskSmart("Head flying hours",                   true,   false)},
                {0xFA, new DiskSmart("Read error retry rate",               true,   false)},
                /* slot in any new codes you find in here */
            };
        }

        #endregion

        #region Overrides
        /// <summary>
        /// String representation of this class
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var sb = new StringBuilder(string.Format("#{0}: [{1}] {2} - {3} - {4}\n", 
                Index,
                IsOK ? "OK" : "Fail",
                Model,
                SerialNumber,
                InterfaceType
                ));

            foreach (var attribute in Attributes)
            {
                if(!attribute.Value.HasData)
                    continue;
                sb.AppendLine(attribute.Value.ToString());
            }

            return sb.ToString();
        }

        private bool Equals(DiskItem other)
        {
            return string.Equals(SerialNumber, other.SerialNumber);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is DiskItem && Equals((DiskItem) obj);
        }

        public bool Equals(string name)
        {
            return SerialNumber.Equals(name);
        }

        public override int GetHashCode()
        {
            return (SerialNumber != null ? SerialNumber.GetHashCode() : 0);
        }

        #endregion
    }
}
