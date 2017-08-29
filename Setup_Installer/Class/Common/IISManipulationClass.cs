using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProlexNetSetup.Class.Common
{
    public class IISManipulationClass
    {
        public static void StopIIS(string site)
        {
            try
            {
                string appcmd = Path.Combine(Environment.ExpandEnvironmentVariables("%windir%"), "system32", "inetsrv", "appcmd.exe");

                Process process = new Process();
                var appcmdArgs = $"stop site /site.name:{site}";
                process.StartInfo.FileName = appcmd;
                process.StartInfo.Arguments = appcmdArgs;
                process.StartInfo.CreateNoWindow = true;
                process.Start();
                process.WaitForExit();

                return;
            }
            catch
            {

            }
        }
    }
}
