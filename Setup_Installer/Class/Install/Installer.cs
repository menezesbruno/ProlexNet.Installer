using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProlexNetSetup.Class.Install
{
    public class Installer
    {
        public static void Firebird(string file, bool silentInstallation)
        {
            // Argumentos para a correta instalação do Firebird 3.
            try
            {
                var installargsComponents = "/COMPONENTS=" + "\"ServerComponent,DevAdminComponent,ClientComponent\"";
                var installargsTasks = " /TASKS=" + "\"UseSuperServerTask,UseServiceTask,AutoStartTask,MenuGroupTask,CopyFbClientToSysTask,CopyFbClientAsGds32Task,EnableLegacyClientAuth\"";
                var installargsSecurity = " /SYSDBAPASSWORD=masterkey";
                var installargsSilent = " /FORCE /SILENT /SP-";

                Process process = new Process();
                process.StartInfo.FileName = file;
                process.StartInfo.Arguments = installargsComponents;
                process.StartInfo.Arguments += installargsTasks;
                process.StartInfo.Arguments += installargsSecurity;
                if (silentInstallation)
                    process.StartInfo.Arguments += installargsSilent;
                process.Start();
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Installer:FirebirdAsync:" + ex.Message);
            }

            return;
        }

        public static async Task IISAsync(string servicePath)
        {
            // Carrega o DISM e lista os pacotes disponíveis para a instalaçao do IIS no Sistema Operacional.
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
                process.StartInfo.CreateNoWindow = true;
                process.Start();

                var output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                File.WriteAllText(dismOutputFile, output, Encoding.UTF8);
                IISDismService(servicePath, dismOutputFile);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Installer:IISAsync:" + ex.Message);
            }

            return;
        }

        public static void IISDismService(string servicePath, string dismOutputFile)
        {
            // Carrega o DISM e instala os pacotes do IIS.
            try
            {
                List<string> packagesToInstall = new List<string>();

                string dismVersion = Path.Combine(Environment.ExpandEnvironmentVariables("%windir%"), "system32", "dism.exe");
                if (Environment.Is64BitOperatingSystem && !Environment.Is64BitProcess)
                    dismVersion = Path.Combine(Environment.ExpandEnvironmentVariables("%windir%"), "sysnative", "dism.exe");

                var packagesList = File.ReadAllLines(dismOutputFile);
                foreach (string line in packagesList)
                {
                    if (line.StartsWith("IIS"))
                    {
                        if (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor == 1)
                        {
                            packagesToInstall.Add($"/FeatureName:{line.Split(' ').FirstOrDefault()}");
                        }
                        else
                        {
                            packagesToInstall.Add($"/FeatureName:{line.Split(' ').FirstOrDefault()} /All");
                        }
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

            return;
        }

        public static void DotNet(string file)
        {
            // Argumentos para a correta instalação do DotNet.
            try
            {
                var installArgs = "/passive /norestart";

                Process process = new Process();
                process.StartInfo.FileName = file;
                process.StartInfo.Arguments = installArgs;
                process.Start();
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Installer:DotNet:" + ex.Message);
            }

            return;
        }

        public static void VCRedist(string file)
        {
            // Argumentos para a correta instalação do VCRedist.
            try
            {
                var installArgs = "/install /repair /passive /norestart";

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

            return;
        }

    }
}
