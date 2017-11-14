using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace ProlexNetSetup.Class.Common
{
    public class ExtractZIP
    {
        public static async Task Extract(string file, string folder)
        {
            try
            {
                if (Directory.Exists(folder))
                    Directory.Delete(folder);
                ZipFile.ExtractToDirectory(file, folder);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("ExtractZIP:Extract:" + ex.Message);
            }          
        }

        public static async Task ExtractDatabase(string file, string folder, string databaseDeployed)
        {
            try
            {
                if (File.Exists(databaseDeployed))
                    File.Delete(databaseDeployed);
                ZipFile.ExtractToDirectory(file, folder);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("ExtractZIP:ExtractDatabase:" + ex.Message);
            }
        }
    }
}
