/*
 * SystemInfoSnapshot
 * Author: Tiago Conceição
 * 
 * http://systeminfosnapshot.com/
 * https://github.com/sn4k3/SystemInfoSnapshot
 */

namespace SystemInfoSnapshot.Core.Disk
{
    public sealed class DiskSmart
    {
        #region Properties
        /// <summary>
        /// Check if this Smart is valid and have data to show
        /// </summary>
        public bool HasData
        {
            get
            {
                return Current != 0 || Worst != 0 || Threshold != 0 || Raw != 0;
            }
        }

        /// <summary>
        /// Gets the attribute name for smart
        /// </summary>
        public string AttributeName { get; private set; }

        /// <summary>
        /// Gets or sets the current value  in smart
        /// </summary>
        public int Current { get; set; }

        /// <summary>
        /// Gets or sets the worst value in smart
        /// </summary>
        public int Worst { get; set; }

        /// <summary>
        /// Gets or sets the threshold value in smart
        /// </summary>
        public int Threshold { get; set; }

        /// <summary>
        /// Gets or sets the raw data  in smart
        /// </summary>
        public int Raw { get; set; }

        /// <summary>
        /// Gets or sets if smart report is ok
        /// </summary>
        public bool IsOK { get; set; }

        /// <summary>
        /// Gets if lower raw value is better
        /// </summary>
        public bool IsLowerRawValueBetter { get; private set; }

        /// <summary>
        /// Gets if higher raw value is better
        /// </summary>
        public bool IsHigherRawValueBetter { get { return !IsHigherRawValueBetter; } }

        /// <summary>
        /// Gets if this attribute is a potential indicator of imminent electromechanical failure
        /// </summary>
        public bool IsCritical { get; private set; }
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="attributeNameName"></param>
        /// <param name="lowerRawValueBetter"></param>
        /// <param name="isCritical"></param>
        public DiskSmart(string attributeNameName, bool lowerRawValueBetter = true, bool isCritical = false)
        {
            AttributeName = attributeNameName;
            IsLowerRawValueBetter = lowerRawValueBetter;
            IsCritical = isCritical;
        }
        #endregion

        #region Overrides
        /// <summary>
        /// String representation of this class
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{5}[{6}] {0}:\t{1}\t{2}\t{3}\t{4}", 
                AttributeName, 
                Current, 
                Worst,
                Threshold, 
                Raw, 
                IsCritical ? "*" : string.Empty, 
                IsOK ? "OK" : "Fail"
                );
        }
        #endregion
    }
}
