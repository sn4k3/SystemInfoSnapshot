using System;
using System.Collections.Generic;
using System.Management;

namespace SystemInfoSnapshot
{
    public static class Devices
    {
        public class PnPDeviceInfo
        {
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

            public PnPDeviceInfo()
            {
            }

            public PnPDeviceInfo(string deviceID, string pnpDeviceID, string description)
            {
                DeviceID = deviceID;
                PnpDeviceID = pnpDeviceID;
                Description = description;
            }
        }

        public static List<PnPDeviceInfo> GetPnPDevices()
        {
            var devices = new List<PnPDeviceInfo>();

            ManagementObjectCollection collection;
            using (var searcher = new ManagementObjectSearcher(@"Select * From Win32_PnPEntity"))
                collection = searcher.Get();

            foreach (var device in collection)
            {
                var usb = new PnPDeviceInfo();
                foreach (var prop in  usb.GetType().GetProperties())
                {
                    try
                    {
                        prop.SetValue(usb, device.GetPropertyValue(prop.Name));
                    }
                    catch (Exception)
                    {
                        prop.SetValue(usb, null);
                    }
                    
                }
                devices.Add(usb);
            }

            collection.Dispose();
            return devices;
        }
    }
}
