using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Web.Administration;

namespace Setup_Installer.Class.Common
{
    public class IISConfiguration
    {
        public static void ProlexSettings(string installationPath)
        {
            try
            {
                ServerManager iisManager = new ServerManager();
                Site site = iisManager.Sites[@"prolexnet"];
                iisManager.Sites.Remove(site);
                iisManager.CommitChanges();
            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }

            try
            {
                var installationSiteFolder = Path.Combine(installationPath, "ProlexNet Server", "www");
                ServerManager iisManager = new ServerManager();
                iisManager.Sites.Add("prolexnet", "http", "*:18520:", installationSiteFolder);
                iisManager.CommitChanges();
            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }

            string appcmdVersion = Path.Combine(Environment.ExpandEnvironmentVariables("%windir%"), "system32", "inetsrv", "appcmd.exe");
            if (Environment.Is64BitOperatingSystem && !Environment.Is64BitProcess)
            {
                appcmdVersion = Path.Combine(Environment.ExpandEnvironmentVariables("%windir%"), "sysnative", "inetsrv", "appcmd.exe");
            }

            Process process = new Process();
            try
            {
                var appcmdArgsDeletion = "delete apppool /name:prolexnet";
                process.StartInfo.FileName = appcmdVersion;
                process.StartInfo.Arguments = appcmdArgsDeletion;
                process.StartInfo.CreateNoWindow = true;

                process.Start();
                process.WaitForExit();
            }
            catch
            {

            }

            var appcmdArgs = "add apppool /name:prolexnet /managedRuntimeVersion:v4.0";
            process.StartInfo.FileName = appcmdVersion;
            process.StartInfo.Arguments = appcmdArgs;
            process.StartInfo.CreateNoWindow = true;

            process.Start();
            process.WaitForExit();

            appcmdArgs = $"set site /site.name:prolexnet /[path='/'].applicationPool:prolexnet";
            process.StartInfo.FileName = appcmdVersion;
            process.StartInfo.Arguments = appcmdArgs;
            process.StartInfo.CreateNoWindow = true;

            process.Start();
            process.WaitForExit();
            return;
        }
    }
}
