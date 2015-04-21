/*
 * SystemInfoSnapshot
 * Author: Tiago Conceição
 * 
 * http://systeminfosnapshot.com/
 * https://github.com/sn4k3/SystemInfoSnapshot
 */

namespace SystemInfoSnapshot.Core.InstalledProgram
{
    /// <summary>
    /// Represents a install program in the system.
    /// </summary>
    public sealed class InstalledProgramItem
    {
        #region Properties
        /// <summary>
        /// Program display name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Program display version.
        /// </summary>
        public string Version { get; private set; }

        /// <summary>
        /// Program Publisher.
        /// </summary>
        public string Publisher { get; private set; }

        /// <summary>
        /// Program installed dated.
        /// </summary>
        public string InstallDate { get; private set; }

        /// <summary>
        /// Program description, what it does
        /// </summary>
        public string Description { get; private set; }
        #endregion

        #region Construtor
        public InstalledProgramItem(string name, string version = null, string publisher = null, string installdate = null, string description = null)
        {
            Name = name;
            Version = version;
            Publisher = publisher;
            InstallDate = installdate;
            Description = description;
        }
        #endregion

        #region Overrides
        private bool Equals(InstalledProgramItem other)
        {
            return string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj is string)
                return Name.Equals(obj.ToString());
            return obj is InstalledProgramItem && Equals((InstalledProgramItem)obj);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }
        #endregion
    }
}
