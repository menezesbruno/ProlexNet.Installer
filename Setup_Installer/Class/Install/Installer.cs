using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setup_Installer.Class.Download
{
    public class Installer
    {
        public static async Task FirebirdAsync(string file, bool silentInstallation)
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
            return;
        }

        public static async Task IISAsync(string servicePath)
        {
            string dismVersion = Path.Combine(Environment.ExpandEnvironmentVariables("%windir%"), "system32", "dism.exe");
            if (Environment.Is64BitOperatingSystem && !Environment.Is64BitProcess)
            {
                dismVersion = Path.Combine(Environment.ExpandEnvironmentVariables("%windir%"), "sysnative", "dism.exe");
            }

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
            await IISDismService(servicePath, dismOutputFile);

            return;
        }

        public static async Task IISDismService(string servicePath, string dismOutputFile)
        {
            string dismVersion = Path.Combine(Environment.ExpandEnvironmentVariables("%windir%"), "system32", "dism.exe");
            if (Environment.Is64BitOperatingSystem && !Environment.Is64BitProcess)
            {
                dismVersion = Path.Combine(Environment.ExpandEnvironmentVariables("%windir%"), "sysnative", "dism.exe");
            }

            List<string> packagesToInstall = new List<string>();
            var packagesList = File.ReadAllLines(dismOutputFile);

            foreach (string line in packagesList)
            {
                if (line.StartsWith("IIS"))
                {
                    packagesToInstall.Add($"/featurename:{line.Split(' ').FirstOrDefault()}");
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
            return;
        }

        public static async Task DotNet(string file)
        {
            var installArgs = "/repair /passive /norestart";

            Process process = new Process();
            process.StartInfo.FileName = file;
            process.StartInfo.Arguments = installArgs;

            process.Start();
            process.WaitForExit();
            return;
        }

        public static async Task VCRedist(string file)
        {
            var installArgs = "/install /repair /passive /norestart ";

            Process process = new Process();
            process.StartInfo.FileName = file;
            process.StartInfo.Arguments = installArgs;

            process.Start();
            process.WaitForExit();
            return;
        }
    }
}
