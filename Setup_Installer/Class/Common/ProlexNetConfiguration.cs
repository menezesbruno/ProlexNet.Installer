using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using ProlexNetSetup.Class.Download;
using System.Windows;

namespace ProlexNetSetup.Class.Common
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

            var updateServerPort = Convert.ToInt32(serverPort) + 1;
            var originalUpdateServerUrl = @"<add key=""UpdateServerUrl"" value=""http://(.*)"" />";
            var replacedUpdateServerUrl = $@"<add key=""UpdateServerUrl"" value=""http://{serverName}:{updateServerPort}"" />";

            File.WriteAllText(configFile, Regex.Replace(File.ReadAllText(configFile), originalClientUrl, replacedClientUrl));
            File.WriteAllText(configFile, Regex.Replace(File.ReadAllText(configFile), originalServerUrl, replacedServerUrl));
            File.WriteAllText(configFile, Regex.Replace(File.ReadAllText(configFile), originalUpdateServerUrl, replacedUpdateServerUrl));
        }

        public static async Task Server(string installationPath, string serverName, string serverPort)
        {
            var installationSubFolder = Path.Combine(installationPath, "ProlexNet Server", "www");
            var webConfigFile = Path.Combine(installationSubFolder, "Web.config");
            var appConfigFile = Path.Combine(installationSubFolder, "scripts", "app.config.js");

            var oldString = "\"urlServer\": 'http://(.*?)',";
            var newString = $"\"urlServer\": 'http://{serverName}:{serverPort}',";

            File.WriteAllText(appConfigFile, Regex.Replace(File.ReadAllText(appConfigFile), oldString, newString));

            //
            var database = "ProlexNet";

            var doc = XDocument.Load(webConfigFile);
            var connectionStringsElement = doc.Root.Element("connectionStrings");
            var firstConnectionString = connectionStringsElement.Element("add");

            var name = firstConnectionString.Attribute("name");
            name.Value = database;

            var conn = firstConnectionString.Attribute("connectionString");
            conn.Value = $"port=3050;charset=UTF8;dialect=3;servertype=0;datasource={serverName};database={database};user=sysdba;password=masterkey;Connection Lifetime=10;Pooling=true";

            var appSettingsElement = doc.Root.Element("appSettings");
            var appSetting = appSettingsElement.Elements().FirstOrDefault(c => c.Attribute("key").Value == "Automatiza.EF6.DefaultConnectionString");
            var defaultConnectionString = appSetting.Attribute("value");
            defaultConnectionString.Value = database;

            connectionStringsElement.RemoveAll();
            connectionStringsElement.Add(firstConnectionString);
            doc.Save(webConfigFile);
        }
    }
}
