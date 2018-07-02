using Microsoft.Win32;

namespace ProlexNetUpdater.Library.Common
{
    public class Registry
    {
        public static string InstallationSubFolder { get; set; }
        public static string ServicePath { get; set; }

        public static void LoadPath()
        {
            var registryUninstallPath = Constants.RegistryUninstallPath;
            var applicationGuid = Constants.ApplicationGuid;

            using (RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(registryUninstallPath, true))
            {
                if (key != null)
                {
                    RegistryKey child = key.OpenSubKey(applicationGuid);
                    if (child != null)
                    {
                        var rootPath = child.GetValue("RootLocation");
                        var servicePath = child.GetValue("ServicePath");
                        InstallationSubFolder = rootPath.ToString();
                        ServicePath = servicePath.ToString();
                    }
                }
            }
        }
    }
}