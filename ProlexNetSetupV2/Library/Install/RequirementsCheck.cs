using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProlexNetSetupV2.Library
{
    internal class RequirementsCheck
    {
        public static bool Firebird()
        {
            try
            {
                var firebird = Directory.GetFiles(@"C:\Program Files\Firebird", "unins*.exe", SearchOption.AllDirectories).FirstOrDefault();
                if (firebird == null)
                {
                    firebird = Directory.GetFiles(@"C:\Program Files (x86)\Firebird", "unins*.exe", SearchOption.AllDirectories).FirstOrDefault();
                    if (firebird == null)
                        return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool DotNet()
        {
            try
            {
                const string subkey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";
                using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(subkey))
                {
                    if (ndpKey != null && ndpKey.GetValue("Release") != null)
                    {
                        var releaseKey = Convert.ToInt32(ndpKey.GetValue("Release"));
                        if (releaseKey >= 393295)
                            return false;
                    }
                    return true;
                }
            }
            catch
            {
                return true;
            }
        }

        public static bool VisualC2013x86()
        {
            try
            {
                const string subkey = @"SOFTWARE\Classes\Installer\Dependencies\{f65db027-aff3-4070-886a-0d87064aabb1}\";
                using (RegistryKey redistKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(subkey))
                {
                    if (redistKey != null && redistKey.GetValue("Version") != null)
                    {
                        var versionKey = redistKey.GetValue("Version").ToString();
                        if (versionKey == "12.0.30501.0")
                            return false;
                    }
                    return true;
                }
            }
            catch
            {
                return true;
            }
        }

        public static bool VisualC2013x64()
        {
            try
            {
                const string subkey = @"SOFTWARE\Classes\Installer\Dependencies\{050d4fc8-5d48-4b8f-8972-47c82c46020f}\";
                using (RegistryKey redistKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(subkey))
                {
                    if (redistKey != null && redistKey.GetValue("Version") != null)
                    {
                        var versionKey = redistKey.GetValue("Version").ToString();
                        if (versionKey == "12.0.30501.0")
                            return false;
                    }
                    return true;
                }
            }
            catch
            {
                return true;
            }
        }
    }
}