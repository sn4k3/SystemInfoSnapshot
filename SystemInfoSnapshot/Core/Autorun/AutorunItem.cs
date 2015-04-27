/*
 * SystemInfoSnapshot
 * Author: Tiago Conceição
 * 
 * http://systeminfosnapshot.com/
 * https://github.com/sn4k3/SystemInfoSnapshot
 */

using System;

namespace SystemInfoSnapshot.Core.Autorun
{
    /// <summary>
    /// Represents a install program in the system.
    /// </summary>
    public sealed class AutorunItem
    {
        #region Properties
        //Time,Entry Location,Entry,Enabled,Category,Profile,Description,Publisher,Image Path,Version,Launch String

        public DateTime Time { get; set; }
        public string EntryLocation { get; set; }
        public string Entry { get; set; }
        public bool Enabled { get; set; }
        public string Category { get; set; }
        public string Profile { get; set; }
        public string Description { get; set; }
        public string Publisher { get; set; }
        public string ImagePath { get; set; }
        public string Version { get; set; }
        public string LunchString { get; set; }

        public bool IsValidFile { get; set; }
        #endregion

        #region Construtor
        /// <summary>
        /// Constructor
        /// </summary>
        public AutorunItem()
        {
            IsValidFile = true;
        }

        #endregion

        #region Overrides
        private bool Equals(AutorunItem other)
        {
            return string.Equals(Entry, other.Entry);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj is string)
                return Entry.Equals(obj.ToString());
            return obj is AutorunItem && Equals((AutorunItem)obj);
        }

        public override int GetHashCode()
        {
            return (Entry != null ? Entry.GetHashCode() : 0);
        }
        #endregion
    }
}
