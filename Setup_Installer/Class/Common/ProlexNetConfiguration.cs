using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ProlexNetSetup.Class.Download;
using ProlexNetSetup.Class.OSInfo;

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

            File.WriteAllText(configFile, Regex.Replace(File.ReadAllText(configFile), originalClientUrl, replacedClientUrl));
            File.WriteAllText(configFile, Regex.Replace(File.ReadAllText(configFile), originalServerUrl, replacedServerUrl));
        }

        public static async Task DatabaseDeploy(string servicePath, string installationPath)
        {
            var databaseFolder = Path.Combine(installationPath, "Database");
            Directory.CreateDirectory(databaseFolder);

            var url = DownloadParameters.Instance.ProlexNet_Database_Url;
            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(servicePath, downloadFileName);
            var hash = DownloadParameters.Instance.ProlexNet_Database_Hash;

            await Download.Download.DownloadFileInBackgroundAsync(url, file, hash);
            await ZipExtractor.Extract(file, databaseFolder);

            var firebirdPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Firebird");
            if (DetectOSVersion.Is64Bits() == true)
                firebirdPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Firebird");

            var databasesConf = Directory.GetFiles(firebirdPath, "databases.conf", SearchOption.AllDirectories).FirstOrDefault();

            using (StreamWriter writer = new StreamWriter(databasesConf, false))
            {
                writer.WriteLine("# ------------------------------");
                writer.WriteLine("# List of known databases");
                writer.WriteLine("# ------------------------------");
                writer.WriteLine("");
                writer.WriteLine("#");
                writer.WriteLine("# Makes it possible to specify per-database configuration parameters.");
                writer.WriteLine("# See the list of them and description on file firebird.conf.");
                writer.WriteLine("# To place that parameters in this file add them in curly braces");
                writer.WriteLine("# after \"alias = / path / to / database.fdb\" line. Example:");
                writer.WriteLine("#	big = /databases/bigdb.fdb");
                writer.WriteLine("#	{");
                writer.WriteLine("#		LockMemSize = 32M		# We know that bigdb needs a lot of locks");
                writer.WriteLine("#		LockHashSlots = 19927	#	and big enough hash table for them");
                writer.WriteLine("#	}");
                writer.WriteLine("#");
                writer.WriteLine("");
                writer.WriteLine("#");
                writer.WriteLine("# Example Database:");
                writer.WriteLine("#");
                writer.WriteLine("employee.fdb = $(dir_sampleDb)/employee.fdb");
                writer.WriteLine("employee = $(dir_sampleDb)/employee.fdb");
                writer.WriteLine("");
                writer.WriteLine("#");
                writer.WriteLine("# Master security database specific setup.");
                writer.WriteLine("# Do not remove it until you understand well what are you doing!");
                writer.WriteLine("#");
                writer.WriteLine("security.db = $(dir_secDb)/security3.fdb");
                writer.WriteLine("{");
                writer.WriteLine("	RemoteAccess = false");
                writer.WriteLine("	DefaultDbCachePages = 50");
                writer.WriteLine("}");
                writer.WriteLine("");
                writer.WriteLine("#");
                writer.WriteLine("# Live Databases:");
                writer.WriteLine("#");
                writer.WriteLine($@"ProlexNet = {databaseFolder}\ProlexNet.prolex");
            }
        }
    }
}
