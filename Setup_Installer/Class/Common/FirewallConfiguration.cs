using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProlexNetSetup.Class.Common
{
    public class FirewallConfiguration
    {
        public static void AddRules(string serverPort)
        {
            var updaterPort = Convert.ToInt32(serverPort) + 1;
            RemoveRules();

            NetshAdd("ProlexNet", "in", $"{serverPort}");
            NetshAdd("ProlexNet", "out", $"{serverPort}");

            NetshAdd("ProlexNet_Updater", "in", $"{updaterPort}");
            NetshAdd("ProlexNet_Updater", "out", $"{updaterPort}");
        }

        public static void RemoveRules()
        {
            NetshRemove("ProlexNet");
            NetshRemove("ProlexNet_Updater");
        }

        private static void NetshRemove(string name)
        {
            try
            {
                Process process = new Process();
                var netshArgs = $"netsh advfirewall firewall delete rule name={name}";
                process.StartInfo.FileName = "netsh";
                process.StartInfo.Arguments = netshArgs;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                process.WaitForExit();
            }
            catch
            {

            }
        }

        private static void NetshAdd(string name, string direction, string port)
        {
            try
            {
                Process process = new Process();
                var netshArgs = $"advfirewall firewall add rule name=\"{name}\" dir={direction} action=allow protocol=TCP localport={port}";
                process.StartInfo.FileName = "netsh";
                process.StartInfo.Arguments = netshArgs;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                process.WaitForExit();
            }
            catch
            {

            }
        }
    }
}
