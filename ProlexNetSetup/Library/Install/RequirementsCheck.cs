using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.IO;

namespace ProlexNetSetup.Library
{
    public class RequirementsCheck
    {
        public static bool VcRedistX86()
        {
            try
            {
                var subkey = @"SOFTWARE\Classes\Installer\Dependencies\,,x86,14.0,bundle\";
                using (RegistryKey redistKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(subkey))
                {
                    if (redistKey != null && redistKey.GetValue("Version") != null)
                    {
                        var versionKey = redistKey.GetValue("Version").ToString();
                        if (versionKey == "14.14.26429.4")
                            return false;
                    }
                }
                return true;
            }
            catch
            {
                return true;
            }
        }

        public static bool VcRedistX64()
        {
            try
            {
                var subkey = @"SOFTWARE\Classes\Installer\Dependencies\,,amd64,14.0,bundle\";
                using (RegistryKey redistKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(subkey))
                {
                    if (redistKey != null && redistKey.GetValue("Version") != null)
                    {
                        var versionKey = redistKey.GetValue("Version").ToString();
                        if (versionKey == "14.14.26429.4")
                            return false;
                    }
                }
                return true;
            }
            catch
            {
                return true;
            }
        }

        public static bool NetCore()
        {
            var keys = new List<object>
            {
                "{f8114565-9043-4981-a0b1-d08a963b2397}",
                "{15272d26-bc64-40d6-846b-c2501b858fc2}"
            };

            try
            {
                foreach (var item in keys)
                {
                    var subkey = $@"SOFTWARE\Classes\Installer\Dependencies\{item}\";
                    using (RegistryKey redistKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(subkey))
                    {
                        if (redistKey != null && redistKey.GetValue("Version") != null)
                        {
                            var versionKey = redistKey.GetValue("Version").ToString();
                            if (versionKey == "2.1.1.26606")
                                return false;
                        }
                    }
                }
                return true;
            }
            catch
            {
                return true;
            }
        }

        public static bool SQLServer()
        {
            try
            {
                var subkey = @"SOFTWARE\Classes\Applications\sqlwb.exe";
                using (RegistryKey redistKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(subkey))
                {
                    if (redistKey != null)
                        return false;
                }
                return true;
            }
            catch
            {
                return true;
            }
        }

        public static bool SQLServerStudio()
        {
            try
            {
                var subkey = @"SOFTWARE\Classes\Installer\Dependencies\{a4f19bdb-56d9-4fe3-8139-f4b0ffe2b9f7}\";
                using (RegistryKey redistKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(subkey))
                {
                    if (redistKey != null && redistKey.GetValue("Version") != null)
                    {
                        var versionKey = redistKey.GetValue("Version").ToString();
                        if (versionKey == "14.0.17254.0")
                            return false;
                    }
                }
                return true;
            }
            catch
            {
                return true;
            }
        }
    }
}