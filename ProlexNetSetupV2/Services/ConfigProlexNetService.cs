using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProlexNetSetupV2.Services
{
    internal class ConfigProlexNetService
    {
        public static void Server(string installationPath, string serverName, string serverPort)
        {
            try
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
            catch (Exception ex)
            {
                Trace.WriteLine($"{nameof(ConfigProlexNetService)}:{nameof(Server)}:{ex.Message}");
            }
        }

        public static async Task Updater(string installationPath)
        {
            try
            {
                var installationSubFolder = Path.Combine(installationPath, "ProlexNet Server", "updater");
                var prolexPath = Path.Combine(installationPath, "ProlexNet Server", "www");
                var webConfigFile = Path.Combine(installationSubFolder, "Web.config");

                var originalProlexPath = @"<add key=""ProlexPath"" value=""(.*)"" />";
                var replacedProlexPath = $@"<add key=""ProlexPath"" value=""{prolexPath}"" />";

                File.WriteAllText(webConfigFile, Regex.Replace(File.ReadAllText(webConfigFile), originalProlexPath, replacedProlexPath));
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"{nameof(ConfigProlexNetService)}:{nameof(Updater)}:{ex.Message}");
            }
        }
    }
}