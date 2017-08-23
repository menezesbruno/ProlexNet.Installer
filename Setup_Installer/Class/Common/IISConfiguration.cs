using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Web.Administration;

namespace ProlexNetSetup.Class.Common
{
    public class IISConfiguration
    {
        public static async Task ProlexNetSettings(string installationPath, string serverPort)
        {
            Process process = new Process();
            // Chama o aspnet_regiis para informar o IIS sobre a versão do DotNet
            try
            {
                var regiisVersion = Path.Combine(Environment.ExpandEnvironmentVariables("%windir%"), "Microsoft.NET", "Framework", "v4.0.30319", "aspnet_regiis.exe");
                if (Environment.Is64BitOperatingSystem && !Environment.Is64BitProcess)
                    regiisVersion = Path.Combine(Environment.ExpandEnvironmentVariables("%windir%"), "Microsoft.NET", "Framework64", "v4.0.30319", "aspnet_regiis.exe");

                var regiisArgs = "-i";
                process.StartInfo.FileName = regiisVersion;
                process.StartInfo.Arguments = regiisArgs;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                Trace.WriteLine("IISConfiguration:ProlexNetSettings:" + ex.Message);
            }
            
            // Remove, se houver, uma configuração já existente do Site "prolexnet" no IIS.
            try
            {
                ServerManager iisManager = new ServerManager();
                Site site = iisManager.Sites[@"prolexnet"];
                iisManager.Sites.Remove(site);
                iisManager.CommitChanges();
            }
            catch (Exception ex)
            {
                Trace.WriteLine("IISConfiguration:ProlexNetSettings:" + ex.Message);
            }

            // Adiciona uma nova configuração do Site "prolexnet" ao IIS.
            try
            {
                var site = "prolexnet";
                var protocol = "http";
                var port = $"*:{serverPort}:";
                var installationSiteFolder = Path.Combine(installationPath, "ProlexNet Server", "www");
                ServerManager iisManager = new ServerManager();
                iisManager.Sites.Add(site, protocol, port, installationSiteFolder);
                iisManager.CommitChanges();
            }
            catch (Exception ex)
            {
                Trace.WriteLine("IISConfiguration:ProlexNetSettings:" + ex.Message);
            }

            // Detecta qual a versão correta do AppCMD deverá ser executada de acordo com a versão do Windows e redireciona ao processo.
            string appcmdVersion = Path.Combine(Environment.ExpandEnvironmentVariables("%windir%"), "system32", "inetsrv", "appcmd.exe");
            if (Environment.Is64BitOperatingSystem && !Environment.Is64BitProcess)
                appcmdVersion = Path.Combine(Environment.ExpandEnvironmentVariables("%windir%"), "sysnative", "inetsrv", "appcmd.exe");

            // Remove, se houver, uma configuração já existente do Pool de aplicativo "prolexnet".
            try
            {
                try
                {
                    var appcmdArgs = "delete apppool /name:prolexnet";
                    process.StartInfo.FileName = appcmdVersion;
                    process.StartInfo.Arguments = appcmdArgs;
                    process.StartInfo.CreateNoWindow = true;
                    process.Start();
                    process.WaitForExit();
                }
                catch
                {
                    appcmdVersion = Path.Combine(Environment.ExpandEnvironmentVariables("%windir%"), "system32", "inetsrv", "appcmd.exe");
                    var appcmdArgs = "delete apppool /name:prolexnet";
                    process.StartInfo.FileName = appcmdVersion;
                    process.StartInfo.Arguments = appcmdArgs;
                    process.StartInfo.CreateNoWindow = true;
                    process.Start();
                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("IISConfiguration:ProlexNetSettings:" + ex.Message);
            }

            // Adiciona um novo Pool de aplicativo .Net 4.0 ao IIS chamado "prolexnet".
            try
            {
                try
                {
                    var appcmdArgs = "add apppool /name:prolexnet /managedRuntimeVersion:v4.0";
                    process.StartInfo.FileName = appcmdVersion;
                    process.StartInfo.Arguments = appcmdArgs;
                    process.StartInfo.CreateNoWindow = true;
                    process.Start();
                    process.WaitForExit();
                }
                catch
                {
                    appcmdVersion = Path.Combine(Environment.ExpandEnvironmentVariables("%windir%"), "system32", "inetsrv", "appcmd.exe");
                    var appcmdArgs = "add apppool /name:prolexnet /managedRuntimeVersion:v4.0";
                    process.StartInfo.FileName = appcmdVersion;
                    process.StartInfo.Arguments = appcmdArgs;
                    process.StartInfo.CreateNoWindow = true;
                    process.Start();
                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("IISConfiguration:ProlexNetSettings:" + ex.Message);
            }

            // Registra o Site "prolexnet" para utilizar Pool "prolexnet" anteriormente criado.
            try
            {
                try
                {
                    var appcmdArgs = $"set site /site.name:prolexnet /[path='/'].applicationPool:prolexnet";
                    process.StartInfo.FileName = appcmdVersion;
                    process.StartInfo.Arguments = appcmdArgs;
                    process.StartInfo.CreateNoWindow = true;
                    process.Start();
                    process.WaitForExit();
                }
                catch
                {
                    appcmdVersion = Path.Combine(Environment.ExpandEnvironmentVariables("%windir%"), "system32", "inetsrv", "appcmd.exe");
                    var appcmdArgs = $"set site /site.name:prolexnet /[path='/'].applicationPool:prolexnet";
                    process.StartInfo.FileName = appcmdVersion;
                    process.StartInfo.Arguments = appcmdArgs;
                    process.StartInfo.CreateNoWindow = true;
                    process.Start();
                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("IISConfiguration:ProlexNetSettings:" + ex.Message);
            }

            return;
        }
    }
}
