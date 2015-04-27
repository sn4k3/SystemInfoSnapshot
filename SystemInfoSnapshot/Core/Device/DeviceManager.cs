/*
 * SystemInfoSnapshot
 * Author: Tiago Conceição
 * 
 * http://systeminfosnapshot.com/
 * https://github.com/sn4k3/SystemInfoSnapshot
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace SystemInfoSnapshot.Core.Device
{
    public sealed class DeviceManager : IEnumerable<DeviceItem>
    {
        #region Properties
        /// <summary>
        /// Gets the special files
        /// </summary>
        public List<DeviceItem> Devices { get; private set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="getAll">True if you want populate this class with the predefined files, otherwise false to make it blank.</param>
        public DeviceManager(bool getAll = true)
        {
            Devices = getAll ? GetDevices() : new List<DeviceItem>();
        }
        #endregion

        #region Methods

        /// <summary>
        /// Update all devices
        /// </summary>
        public void Update()
        {
            Devices = GetDevices();
        }

        /// <summary>
        /// Gets a list of special files on this machine.
        /// </summary>
        /// <returns></returns>
        public static List<DeviceItem> GetDevices()
        {
            var result = new List<DeviceItem>();

            if (SystemHelper.IsWindows)
            {
                ManagementObjectCollection collection;
                using (var searcher = new ManagementObjectSearcher(@"Select * From Win32_PnPEntity"))
                    collection = searcher.Get();

                foreach (var device in collection)
                {
                    var item = new DeviceItem();
                    foreach (var prop in item.GetType().GetProperties())
                    {
                        try
                        {
                            prop.SetValue(item, device.GetPropertyValue(prop.Name));
                        }
                        catch (Exception)
                        {
                            prop.SetValue(item, null);
                        }

                    }
                    result.Add(item);
                }

                collection.Dispose();
            }

            return result;
        }

        #endregion

        #region Overrides
        public IEnumerator<DeviceItem> GetEnumerator()
        {
            return Devices.GetEnumerator();
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
        public DeviceItem this[int index]    // Indexer declaration
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

        /// <summary>
        /// Indexers 
        /// </summary>
        /// <param name="name">name</param>
        /// <returns></returns>
        public DeviceItem this[string name]    // Indexer declaration
        {
            get
            { return Devices.FirstOrDefault(item => item.Equals(name)); }
        }
        #endregion
    }
}
