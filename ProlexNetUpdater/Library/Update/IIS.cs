using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProlexNetUpdater.Library.Update
{
    class IIS
    {
        public static void Stop()
        {
            RunIISCommand("stop");
        }

        public static void Start()
        {
            RunIISCommand("start");
            RecyclePool();
        }

        public static void RecyclePool()
        {
            try
            {
                string appcmd = Path.Combine(Environment.ExpandEnvironmentVariables("%windir%"), "system32", "inetsrv", "appcmd.exe");

                Process process = new Process();
                var appcmdArgs = $"recycle apppool /apppool.name:prolexnet";
                process.StartInfo.FileName = appcmd;
                process.StartInfo.Arguments = appcmdArgs;
                process.StartInfo.CreateNoWindow = false;
                process.StartInfo.UseShellExecute = false;
                process.Start();
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                Trace.TraceError($"{nameof(IIS)}.{nameof(RecyclePool)}:{ex.Message}");
            }
        }

        private static void RunIISCommand(string action)
        {
            try
            {
                string appcmd = Path.Combine(Environment.ExpandEnvironmentVariables("%windir%"), "system32", "inetsrv", "appcmd.exe");

                Process process = new Process();
                var appcmdArgs = $"{action} site /site.name:prolexnet";
                process.StartInfo.FileName = appcmd;
                process.StartInfo.Arguments = appcmdArgs;
                process.StartInfo.CreateNoWindow = false;
                process.StartInfo.UseShellExecute = false;
                process.Start();
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                Trace.TraceError($"{nameof(IIS)}.{nameof(RunIISCommand)}:{ex.Message}");
            }
        }
    }
}
