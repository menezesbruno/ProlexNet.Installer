using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Win32;

namespace ProlexNetSetup.Library
{
    internal class RequirementsCheck
    {
        public static bool NetCore()
        {
            try
            {
                const string subkey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";
                using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(subkey))
                {
                    if (ndpKey != null && ndpKey.GetValue("Release") != null)
                    {
                        var releaseKey = Convert.ToInt32(ndpKey.GetValue("Release"));
                        if (releaseKey >= 393295)
                            return false;
                    }
                    return true;
                }
            }
            catch
            {
                return true;
            }
        }

        public static bool NetCore21()
        {
            try
            {
                var dotnetArgs = $"--list-runtimes";

                Process process = new Process();
                process.StartInfo.FileName = "dotnet";
                process.StartInfo.Arguments = dotnetArgs;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;
                process.Start();

                var output = process.StandardOutput.ReadToEnd().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                process.WaitForExit();

                foreach (var items in output)
                {
                    var versionList = new List<string>();
                    var version = items.Split(' ');
                    foreach (var item in items)
                    {
                        if (!item.ToString().Contains('-'))
                            versionList.Add(item.ToString());
                    }
                    return false;
                }
                return true;
            }
            catch
            {
                return true;
            }
        }
    }
}