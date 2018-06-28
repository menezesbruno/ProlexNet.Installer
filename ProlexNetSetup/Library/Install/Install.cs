using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProlexNetSetup.Library.Startup;
using ProlexNetSetup.ViewModels;

namespace ProlexNetSetup.Library
{
    internal class Install
    {
        public static async Task IIS(string installationPath, string port)
        {
            try
            {
                string dismVersion = Path.Combine(Environment.ExpandEnvironmentVariables("%windir%"), "system32", "dism.exe");
                if (Environment.Is64BitOperatingSystem && !Environment.Is64BitProcess)
                    dismVersion = Path.Combine(Environment.ExpandEnvironmentVariables("%windir%"), "sysnative", "dism.exe");

                var servicePath = CreateServicePath.ServicePath;
                var dismOutputFile = Path.Combine(servicePath, "dismOutput.txt");

                var tempComparer = new string[0];
                if (File.Exists(dismOutputFile))
                    tempComparer = File.ReadAllLines(dismOutputFile);

                var dismArgs = $"/Online /Get-Features /Format:Table";

                Process process = new Process();
                process.StartInfo.FileName = dismVersion;
                process.StartInfo.Arguments = dismArgs;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;
                process.Start();

                var output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                File.WriteAllText(dismOutputFile, output, Encoding.UTF8);

                var dismChecker = false;
                string[] dismOutputFileComparer = File.ReadAllLines(dismOutputFile);
                if (dismOutputFileComparer.Count() != tempComparer.Count())
                    dismChecker = true;
                else
                {
                    for (int line = 0; line < tempComparer.Length; line++)
                    {
                        if (line < dismOutputFileComparer.Length)
                        {
                            if (!tempComparer[line].Equals(dismOutputFileComparer[line]))
                                dismChecker = true;
                        }
                    }
                }

                if (dismChecker)
                    IISEnablePackages(servicePath, dismOutputFile, dismVersion);

                await Task.Run(() => ConfigIIS.ProlexNetSettingsAsync(installationPath, port));
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"{nameof(Install)}:{nameof(IIS)}:{ex.Message}");
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
                Trace.WriteLine($"{nameof(Install)}:{nameof(IISEnablePackages)}:{ex.Message}");
            }
        }

        public static void VCRedist(string file)
        {
            var installArgs = "/install /passive /norestart";
            InstallFactory(file, installArgs);
        }

        public static void NetCore(string file)
        {
            var installArgs = "/passive /norestart";
            InstallFactory(file, installArgs);
        }

        public static void SQLServer(string file)
        {
            var installArgs = "/ACTION=\"Install\" /IACCEPTSQLSERVERLICENSETERMS /Q";
            InstallFactory(file, installArgs);
        }

        public static void SQLServerStudio(string file)
        {
            var installArgs = "/passive /norestart";
            InstallFactory(file, installArgs);
        }

        public static async void DatabaseAsync(string servicePath, string installationPath, string file)
        {
            var computerName = Environment.GetEnvironmentVariable("COMPUTERNAME");
            var databaseFolder = Path.Combine(installationPath, "Database");
            Directory.CreateDirectory(databaseFolder);

            var database = await Task.Run(() => ZipExtract.ExtractAndGetFile(servicePath, file));
            var databaseName = Path.GetFileName(database);
            var databaseWithoutExt = Path.GetFileNameWithoutExtension(databaseName);

            var installArgs = $"-E -S {computerName}\\SQLEXPRESS -Q \"RESTORE DATABASE[ProlexNet] FROM DISK = '{database}' WITH RECOVERY, MOVE '{databaseWithoutExt}_DATA' TO '{databaseFolder}\\{databaseWithoutExt}_DATA.mdf', MOVE '{databaseWithoutExt}_Log' TO '{databaseFolder}\\{databaseWithoutExt}_Log.ldf'\"";

            InstallFactory("SqlCmd", installArgs);

        }

        public static void LinqPad(string file)
        {
            var installArgs = "/silent";
            InstallFactory(file, installArgs);
        }

        public static void InstallFactory(string file, string installArgs)
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = file;
                process.StartInfo.Arguments = installArgs;
                process.Start();
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"{nameof(Install)}:{nameof(InstallFactory)}:{ex.Message}");
            }
        }
    }
}