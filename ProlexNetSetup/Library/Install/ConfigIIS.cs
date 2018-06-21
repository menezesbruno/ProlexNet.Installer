using Microsoft.Web.Administration;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace ProlexNetSetup.Library
{
    internal class ConfigIIS
    {
        public static async Task ProlexNetSettingsAsync(string installationPath, string serverPort)
        {
            var installationSitePath = Path.Combine(installationPath, "ProlexNet Server");
            int sitePort = Convert.ToInt32(serverPort);

            try
            {
                var regiisVersion = Path.Combine(Environment.ExpandEnvironmentVariables("%windir%"), "Microsoft.NET", "Framework", "v4.0.30319", "aspnet_regiis.exe");
                if (Environment.Is64BitOperatingSystem && !Environment.Is64BitProcess)
                    regiisVersion = Path.Combine(Environment.ExpandEnvironmentVariables("%windir%"), "Microsoft.NET", "Framework64", "v4.0.30319", "aspnet_regiis.exe");

                Process process = new Process();
                var regiisArgs = "-i";
                process.StartInfo.FileName = regiisVersion;
                process.StartInfo.Arguments = regiisArgs;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"{nameof(ConfigIIS)}:{nameof(ProlexNetSettingsAsync)}:{ex.Message}");
            }

            await Task.Run(() => RemoveSite("prolexnet"));
            await Task.Run(() => RemoveSite("prolexnet_updater"));

            await Task.Run(() => RemovePool("prolexnet"));

            await Task.Run(() => AddSite("prolexnet", sitePort, installationSitePath, "www"));
            await Task.Run(() => AddSite("prolexnet_updater", sitePort + 1, installationSitePath, "updater"));

            await Task.Run(() => AddPool("prolexnet"));

            await Task.Run(() => SetupPool("prolexnet", "prolexnet"));
            await Task.Run(() => SetupPool("prolexnet_updater", "prolexnet"));
        }

        public static void RemoveSite(string site)
        {
            try
            {
                var appcmd = Path.Combine(Environment.ExpandEnvironmentVariables("%windir%"), "system32", "inetsrv", "appcmd.exe");

                Process process = new Process();
                var appcmdArgs = $"stop site /site.name:{site}";
                process.StartInfo.FileName = appcmd;
                process.StartInfo.Arguments = appcmdArgs;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                process.WaitForExit();

                ServerManager iisManager = new ServerManager();
                Site siteToRemove = iisManager.Sites[$"{site}"];
                iisManager.Sites.Remove(siteToRemove);
                iisManager.CommitChanges();

                return;
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"{nameof(ConfigIIS)}:{nameof(RemoveSite)}:{ex.Message}");
            }
        }

        public static void RemovePool(string pool)
        {
            try
            {
                var appcmd = Path.Combine(Environment.ExpandEnvironmentVariables("%windir%"), "system32", "inetsrv", "appcmd.exe");

                Process process = new Process();
                var appcmdArgs = $"delete apppool /apppool.name:{pool}";
                process.StartInfo.FileName = appcmd;
                process.StartInfo.Arguments = appcmdArgs;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                process.WaitForExit();

                return;
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"{nameof(ConfigIIS)}:{nameof(RemovePool)}:{ex.Message}");
            }
        }

        public static void AddSite(string site, int sitePort, string installationSitePath, string siteSuffix)
        {
            try
            {
                var protocol = "http";
                var port = $"*:{sitePort}:";
                var path = Path.Combine(installationSitePath, $"{siteSuffix}");
                ServerManager iisManager = new ServerManager();
                iisManager.Sites.Add(site, protocol, port, path);
                iisManager.CommitChanges();
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"{nameof(ConfigIIS)}:{nameof(AddSite)}:{ex.Message}");
            }
        }

        public static void AddPool(string pool)
        {
            try
            {
                var appcmd = Path.Combine(Environment.ExpandEnvironmentVariables("%windir%"), "system32", "inetsrv", "appcmd.exe");

                Process process = new Process();
                var appcmdArgs = "add apppool /name:prolexnet /managedRuntimeVersion:v4.0 -processModel.identityType:LocalSystem /enable32BitAppOnWin64:true";
                process.StartInfo.FileName = appcmd;
                process.StartInfo.Arguments = appcmdArgs;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"{nameof(ConfigIIS)}:{nameof(AddPool)}:{ex.Message}");
            }
        }

        public static void SetupPool(string site, string pool)
        {
            try
            {
                var appcmd = Path.Combine(Environment.ExpandEnvironmentVariables("%windir%"), "system32", "inetsrv", "appcmd.exe");

                Process process = new Process();
                var appcmdArgs = $"set site /site.name:{site} /[path='/'].applicationPool:{pool}";
                process.StartInfo.FileName = appcmd;
                process.StartInfo.Arguments = appcmdArgs;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"{nameof(ConfigIIS)}:{nameof(SetupPool)}:{ex.Message}");
            }
        }
    }
}