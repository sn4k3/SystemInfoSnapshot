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
        /// Gets or sets the HDD index
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets if the HDD is ok
        /// </summary>
        public bool IsOK { get; set; }

        /// <summary>
        /// Gets or sets the HDD Model
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Gets or sets the HDD type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the HDD serial number
        /// </summary>
        public string Serial { get; set; }

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
                Serial,
                Type
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
            return string.Equals(Serial, other.Serial);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is DiskItem && Equals((DiskItem) obj);
        }

        public override int GetHashCode()
        {
            return (Serial != null ? Serial.GetHashCode() : 0);
        }

        #endregion
    }
}
