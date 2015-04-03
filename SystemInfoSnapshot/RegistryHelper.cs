using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace SystemInfoSnapshot
{
    public static class RegistryHelper
    {
        

        

        public static void GetPrograms()
        {
            ManagementObjectSearcher s = new ManagementObjectSearcher("SELECT * FROM Win32_Product");
        }

        public static bool ExistsInRemoteSubKey(string p_machineName, RegistryHive p_hive, string p_subKeyName, string p_attributeName, string p_name)
        {
            using (RegistryKey regHive = RegistryKey.OpenRemoteBaseKey(p_hive, p_machineName))
            {
                using (RegistryKey regKey = regHive.OpenSubKey(p_subKeyName))
                {
                    if (regKey != null)
                    {
                        foreach (string kn in regKey.GetSubKeyNames())
                        {
                            RegistryKey subkey;
                            using (subkey = regKey.OpenSubKey(kn))
                            {
                                var displayName = subkey.GetValue(p_attributeName) as string;
                                if (p_name.Equals(displayName, StringComparison.OrdinalIgnoreCase)) // key found!
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}
