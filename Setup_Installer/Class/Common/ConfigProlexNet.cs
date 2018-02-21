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
    class ConfigProlexNet
    {
        public static void Client(string installationPath, string serverName, string serverPort)
        {
            var updateServerPort = Convert.ToInt32(serverPort) + 1;
            var applicationDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Automatiza", "ProlexNet");
            Directory.CreateDirectory(applicationDataPath);

            var settingsFile = Path.Combine(applicationDataPath, "prolexnet.settings");
            if (!File.Exists(settingsFile))
            {
                using (StreamWriter writer = new StreamWriter(settingsFile, false))
                {
                    writer.WriteLine("{");
                    writer.WriteLine(@"""StartAtLogon"": true,");
                    writer.WriteLine(@"""StartMinimized"": false,");
                    writer.WriteLine(@"""NetworkMode"": 0,");
                    writer.WriteLine(@"""AcquireMode"": ""Automatiza.Image.Files.dll"",");
                    writer.WriteLine($@"""ClientUrl"": ""http://{serverName}:{serverPort}/#/"",");
                    writer.WriteLine($@"""ServerUrl"": ""http://{serverName}:{serverPort}"",");
                    writer.WriteLine($@"""UpdateServerUrl"": ""http://{serverName}:{updateServerPort}"",");
                    writer.WriteLine(@"""UpdateClientUrl"": ""https://automatizabox.azurewebsites.net/repository/prolexnet"",");
                    writer.WriteLine(@"""UpdateFrequencyInHours"": 3");
                    writer.WriteLine("}");
                }
            }
            else
            {
                var originalClientUrl = @"""ClientUrl"": ""http://(.*)/#/"",";
                var replacedClientUrl = $@"""ClientUrl"": ""http://{serverName}:{serverPort}/#/"",";

                var originalServerUrl = @"""ServerUrl"": ""http://(.*)"",";
                var replacedServerUrl = $@"""ServerUrl"": ""http://{serverName}:{serverPort}"",";

                var originalUpdateServerUrl = @"""UpdateServerUrl"": ""http://(.*)"",";
                var replacedUpdateServerUrl = $@"""UpdateServerUrl"": ""http://{serverName}:{updateServerPort}"",";

                File.WriteAllText(settingsFile, Regex.Replace(File.ReadAllText(settingsFile), originalClientUrl, replacedClientUrl));
                File.WriteAllText(settingsFile, Regex.Replace(File.ReadAllText(settingsFile), originalServerUrl, replacedServerUrl));
                File.WriteAllText(settingsFile, Regex.Replace(File.ReadAllText(settingsFile), originalUpdateServerUrl, replacedUpdateServerUrl));
            }
        }

        public static void Server(string installationPath, string serverName, string serverPort)
        {
            var installationSubFolder = Path.Combine(installationPath, "ProlexNet Server", "www");
            var webConfigFile = Path.Combine(installationSubFolder, "Web.config");
            var appConfigFile = Path.Combine(installationSubFolder, "app.config.js");

            var oldString = "\"urlServer\": 'http://(.*?)',";
            var newString = $"\"urlServer\": 'http://{serverName}:{serverPort}',";

            File.WriteAllText(appConfigFile, Regex.Replace(File.ReadAllText(appConfigFile), oldString, newString));

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

        public static async Task Updater(string installationPath)
        {
            var installationSubFolder = Path.Combine(installationPath, "ProlexNet Server", "updater");
            var prolexPath = Path.Combine(installationPath, "ProlexNet Server", "www");
            var webConfigFile = Path.Combine(installationSubFolder, "Web.config");

            var originalProlexPath = @"<add key=""ProlexPath"" value=""(.*)"" />";
            var replacedProlexPath = $@"<add key=""ProlexPath"" value=""{prolexPath}"" />";

            File.WriteAllText(webConfigFile, Regex.Replace(File.ReadAllText(webConfigFile), originalProlexPath, replacedProlexPath));
        }
    }
}
