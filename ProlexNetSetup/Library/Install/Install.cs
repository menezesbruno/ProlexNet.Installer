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

                var servicePath = ServicePath.Path;
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
                    IISEnablePackagesAsync(servicePath, dismOutputFile, dismVersion);

                await Task.Run(() => ConfigIIS.ProlexNetSettingsAsync(installationPath, port));
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"{nameof(Install)}:{nameof(IIS)}:{ex.Message}");
            }
        }

        public static void IISEnablePackagesAsync(string servicePath, string dismOutputFile, string dismVersion)
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
                Trace.WriteLine($"{nameof(Install)}:{nameof(IISEnablePackagesAsync)}:{ex.Message}");
            }
        }

        public static void NetCore21(string file)
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
                Trace.WriteLine($"{nameof(Install)}:{nameof(NetCore21)}:{ex.Message}");
            }
        }

        public static void LINQPad5(string file)
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
                Trace.WriteLine($"{nameof(Install)}:{nameof(LINQPad5)}:{ex.Message}");
            }
        }
    }
}