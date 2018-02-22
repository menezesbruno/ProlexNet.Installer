using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProlexNetSetupV2.Services
{
    class InstallService
    {
        public static void Firebird(string file, string installationPath)
        {
            try
            {
                var installArgsComponents = "/COMPONENTS=" + "\"ServerComponent,DevAdminComponent,ClientComponent\"";
                var installArgsTasks = " /TASKS=" + "\"UseSuperServerTask,UseServiceTask,AutoStartTask,MenuGroupTask,CopyFbClientToSysTask,CopyFbClientAsGds32Task,EnableLegacyClientAuth\"";
                var installArgsSecurity = " /SYSDBAPASSWORD=masterkey";
                var installArgsSilent = " /FORCE /SILENT /SP-";

                Process process = new Process();
                process.StartInfo.FileName = file;
                process.StartInfo.Arguments = installArgsComponents;
                process.StartInfo.Arguments += installArgsTasks;
                process.StartInfo.Arguments += installArgsSecurity;
                process.StartInfo.Arguments += installArgsSilent;
                process.Start();
                process.WaitForExit();

                var databaseFolder = Path.Combine(installationPath, "Database");
                var databasesConf = Directory.GetFiles(@"C:\Program Files\Firebird", "databases.conf", SearchOption.AllDirectories).FirstOrDefault();
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
            catch (Exception ex)
            {
                Trace.WriteLine("Installer:FirebirdAsync:" + ex.Message);
            }
        }

        public static void IISAvailablePackages(string servicePath)
        {
            try
            {
                string dismVersion = Path.Combine(Environment.ExpandEnvironmentVariables("%windir%"), "system32", "dism.exe");
                if (Environment.Is64BitOperatingSystem && !Environment.Is64BitProcess)
                    dismVersion = Path.Combine(Environment.ExpandEnvironmentVariables("%windir%"), "sysnative", "dism.exe");

                var dismOutputFile = Path.Combine(servicePath, "dismOutput.txt");
                var dismArgs = $"/Online /Get-Features /Format:Table";

                Process process = new Process();
                process.StartInfo.FileName = dismVersion;
                process.StartInfo.Arguments = dismArgs;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = false;
                process.Start();

                var output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                File.WriteAllText(dismOutputFile, output, Encoding.UTF8);

                IISEnablePackages(servicePath, dismOutputFile, dismVersion);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Installer:IISAsync:" + ex.Message);
            }
        }

        public static void IISEnablePackages(string servicePath, string dismOutputFile, string dismVersion)
        {
            try
            {
                List<string> packagesToInstall = new List<string>();

                var packagesList = File.ReadAllLines(dismOutputFile);
                foreach (string line in packagesList)
                {
                    if (line.StartsWith("IIS"))
                    {
                        if (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor == 1)
                            packagesToInstall.Add($"/FeatureName:{line.Split(' ').FirstOrDefault()}");
                        else
                            packagesToInstall.Add($"/FeatureName:{line.Split(' ').FirstOrDefault()} /All");
                    }
                }

                string concat = String.Join(" ", packagesToInstall.ToArray());

                var dismArgs = $"/Online /Enable-Feature {concat}";
                Process process = new Process();
                process.StartInfo.FileName = dismVersion;
                process.StartInfo.Arguments = dismArgs;
                process.StartInfo.CreateNoWindow = false;
                process.Start();
                process.WaitForExit();

                dismArgs = $"/Online /Get-Features /Format:Table";
                process.StartInfo.FileName = dismVersion;
                process.StartInfo.Arguments = dismArgs;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;
                process.Start();

                var output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                File.WriteAllText(dismOutputFile, output, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Installer:IISDismService:" + ex.Message);
            }
        }

        public static void DotNet(string file)
        {
            try
            {
                var installArgs = "/passive /norestart";

                Process process = new Process();
                process.StartInfo.FileName = file;
                process.StartInfo.Arguments = installArgs;
                process.Start();
                while (!process.HasExited) // Truque para evitar congelamento do instalador no Windows 8.1
                {

                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Installer:DotNet:" + ex.Message);
            }
        }

        public static void VCRedist(string file)
        {
            try
            {
                var installArgs = "/install /passive /norestart";

                Process process = new Process();
                process.StartInfo.FileName = file;
                process.StartInfo.Arguments = installArgs;
                process.Start();
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Installer:VCRedist:" + ex.Message);
            }
        }

        public static void LINQPad(string file)
        {
            try
            {
                var installArgs = "/silent";

                Process process = new Process();
                process.StartInfo.FileName = file;
                process.StartInfo.Arguments = installArgs;
                process.Start();
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Installer:LINQPad:" + ex.Message);
            }
        }
    }
}