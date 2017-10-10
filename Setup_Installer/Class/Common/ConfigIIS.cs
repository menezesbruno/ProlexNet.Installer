using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Web.Administration;
using ProlexNetSetup.Class.Common;

namespace ProlexNetSetup.Class.Common
{
    public class ConfigIIS
    {
        public static async Task ProlexNetSettings(string installationPath, string serverPort)
        {
            var installationSitePath = Path.Combine(installationPath, "ProlexNet Server");
            int sitePort = Convert.ToInt32(serverPort);
            
            // Chama o aspnet_regiis para informar o IIS sobre a versão do DotNet
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
                Trace.WriteLine("ConfigIIS:ProlexNetSettings:" + ex.Message);
            }

            // Remove os Sites "prolexnet" e "prolexnet_updater" do IIS.
            ConfigIISServer.RemoveSite("prolexnet");
            ConfigIISServer.RemoveSite("prolexnet_updater");

            // Remove o Pool de aplicativo "prolexnet".
            ConfigIISServer.RemovePool("prolexnet");
            
            // Adiciona os Sites "prolexnet" e "prolexnet_updater" ao IIS.
            ConfigIISServer.AddSite("prolexnet", sitePort, installationSitePath, "www");
            ConfigIISServer.AddSite("prolexnet_updater", sitePort+1, installationSitePath, "updater");

            // Adiciona o Pool de aplicativo .Net 4.0 ao IIS chamado "prolexnet".
            ConfigIISServer.AddPool("prolexnet");

            // Registra o Site "prolexnet" e "prolexnet_updater" para utilizar Pool "prolexnet" anteriormente criado.
            ConfigIISServer.ConfigurePool("prolexnet", "prolexnet");
            ConfigIISServer.ConfigurePool("prolexnet_updater", "prolexnet");
        }
    }
}
