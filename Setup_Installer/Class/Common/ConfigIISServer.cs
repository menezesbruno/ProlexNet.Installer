using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProlexNetSetup.Class.Common
{
    public class ConfigIISServer
    {
        public static void RemoveSite(string site)
        {
            try
            {
                string appcmd = Path.Combine(Environment.ExpandEnvironmentVariables("%windir%"), "system32", "inetsrv", "appcmd.exe");

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
                Trace.WriteLine("ConfigIISServer:RemoveSite" + ex.Message);
            }
        }

        public static void RemovePool(string pool)
        {
            try
            {
                string appcmd = Path.Combine(Environment.ExpandEnvironmentVariables("%windir%"), "system32", "inetsrv", "appcmd.exe");

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
                Trace.WriteLine("ConfigIISServer:RemovePool" + ex.Message);
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
                Trace.WriteLine("ConfigIISServer:AddSite" + ex.Message);
            }
        }

        public static void AddPool(string pool)
        {
            try
            {
                string appcmd = Path.Combine(Environment.ExpandEnvironmentVariables("%windir%"), "system32", "inetsrv", "appcmd.exe");

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
                Trace.WriteLine("ConfigIISServer:AddPool" + ex.Message);
            }
        }

        public static void ConfigurePool(string site, string pool)
        {
            try
            {
                string appcmd = Path.Combine(Environment.ExpandEnvironmentVariables("%windir%"), "system32", "inetsrv", "appcmd.exe");

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
                Trace.WriteLine("ConfigIISServer:ConfigurePool" + ex.Message);
            }
        }
    }
}