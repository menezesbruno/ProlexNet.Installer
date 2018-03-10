using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ProlexNetSetupV2.Library
{
    internal class Firewall
    {
        public static async Task AddRules(string serverPort)
        {
            var updaterPort = Convert.ToInt32(serverPort) + 1;
            var firebirdPort = "3050";

            await Task.Run(() => RemoveRules());

            await Task.Run(() => NetshAdd("ProlexNet_Firebird", "in", $"{firebirdPort}"));
            await Task.Run(() => NetshAdd("ProlexNet_Firebird", "out", $"{firebirdPort}"));

            await Task.Run(() => NetshAdd("ProlexNet", "in", $"{serverPort}"));
            await Task.Run(() => NetshAdd("ProlexNet", "out", $"{serverPort}"));

            await Task.Run(() => NetshAdd("ProlexNet_Updater", "in", $"{updaterPort}"));
            await Task.Run(() => NetshAdd("ProlexNet_Updater", "out", $"{updaterPort}"));
        }

        public static void RemoveRules()
        {
            NetshRemove("ProlexNet_Firebird");
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
            catch (Exception ex)
            {
                Trace.WriteLine($"{nameof(Firewall)}:{nameof(NetshRemove)}:{ex.Message}");
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
            catch (Exception ex)
            {
                Trace.WriteLine($"{nameof(Firewall)}:{nameof(NetshAdd)}:{ex.Message}");
            }
        }
    }
}