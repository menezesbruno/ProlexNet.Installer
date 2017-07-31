using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace ProlexNetSetup.Class.Install
{
    public class InstallationDetect
    {
        public static bool Firebird()
        {
            // Desinstala o Firebird
            try
            {
                var firebirdUninstaller = Directory.GetFiles(@"C:\Program Files\Firebird", "unins*.exe", SearchOption.AllDirectories).FirstOrDefault();

                if (firebirdUninstaller == null)
                {
                    firebirdUninstaller = Directory.GetFiles(@"C:\Program Files (x86)\Firebird", "unins*.exe", SearchOption.AllDirectories).FirstOrDefault();
                    if (firebirdUninstaller == null)
                        return false;
                    else
                        return true;
                }
                else
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
                    int releaseKey = 0;
                    if (ndpKey != null && ndpKey.GetValue("Release") != null)
                    {
                        releaseKey = Convert.ToInt32(ndpKey.GetValue("Release"));
                    }
                    else
                    {
                        return true;
                    }
                    if (releaseKey >= 393295)
                        return false;

                    else
                        return true;
                }
            }
            catch
            {
                return true;
            }
        }
        
        public static bool VCRedist_X86()
        {
            try
            {
                const string subkey = @"SOFTWARE\Classes\Installer\Dependencies\{f65db027-aff3-4070-886a-0d87064aabb1}\";
                using (RegistryKey redistKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(subkey))
                {
                    string versionKey = "";
                    if (redistKey != null && redistKey.GetValue("Version") != null)
                    {
                        versionKey = redistKey.GetValue("Version").ToString();
                    }
                    else
                    {
                        return true;
                    }
                    if (versionKey == "12.0.30501.0")
                        return false;

                    else
                        return true;
                }
            }
            catch
            {
                return true;
            }
        }

        public static bool VCRedist_X64()
        {
            try
            {
                const string subkey = @"SOFTWARE\Classes\Installer\Dependencies\{050d4fc8-5d48-4b8f-8972-47c82c46020f}\";
                using (RegistryKey redistKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(subkey))
                {
                    string versionKey = "";
                    if (redistKey != null && redistKey.GetValue("Version") != null)
                    {
                        versionKey = redistKey.GetValue("Version").ToString();
                    }
                    else
                    {
                        return true;
                    }
                    if (versionKey == "12.0.30501.0")
                        return false;

                    else
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
