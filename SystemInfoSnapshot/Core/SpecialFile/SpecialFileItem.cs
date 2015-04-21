/*
 * SystemInfoSnapshot
 * Author: Tiago Conceição
 * 
 * http://systeminfosnapshot.com/
 * https://github.com/sn4k3/SystemInfoSnapshot
 */

using System.IO;
using System.Text;

namespace SystemInfoSnapshot.Core.SpecialFile
{
    /// <summary>
    /// Represents a install program in the system.
    /// </summary>
    public sealed class SpecialFileItem
    {
        #region Properties
        /// <summary>
        /// Gets the file name
        /// </summary>
        public string Filename { get; private set; }

        /// <summary>
        /// Gets the file full path
        /// </summary>
        public string FullPath { get; private set; }

        /// <summary>
        /// Gets the file content
        /// </summary>
        public StringBuilder Content { get; private set; }
        #endregion

        #region Construtor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="path"></param>
        public SpecialFileItem(string path)
        {
            Filename = Path.GetFileName(path);
            FullPath = path;
            Content = new StringBuilder();
        }

        #endregion

        #region Overrides
        private bool Equals(SpecialFileItem other)
        {
            return string.Equals(FullPath, other.FullPath);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj is string)
                return FullPath.Equals(obj.ToString());
            return obj is SpecialFileItem && Equals((SpecialFileItem)obj);
        }

        public override int GetHashCode()
        {
            return (FullPath != null ? FullPath.GetHashCode() : 0);
        }
        #endregion
    }
}
