using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProlexNet.Setup.Class.Common
{
    class ProlexNetConfiguration
    {
        public static async Task Client(string installationPath, string serverName, string serverPort)
        {
            var installationSubFolder = Path.Combine(installationPath, "ProlexNet Client", "bin");
            var configFile = Path.Combine(installationSubFolder, "ProlexNet.ExtHost.exe.config");

            var originalClientUrl = @"<add key=""ClientUrl"" value=""http://(.*)/#/"" />";
            var replacedClientUrl = $@"<add key=""ClientUrl"" value=""http://{serverName}:{serverPort}/#/"" />";

            var originalServerUrl = @"<add key=""ServerUrl"" value=""http://(.*)"" />";
            var replacedServerUrl = $@"<add key=""ServerUrl"" value=""http://{serverName}:{serverPort}"" />";

            File.WriteAllText(configFile, Regex.Replace(File.ReadAllText(configFile), originalClientUrl, replacedClientUrl));
            File.WriteAllText(configFile, Regex.Replace(File.ReadAllText(configFile), originalServerUrl, replacedServerUrl));
        }
    }
}
