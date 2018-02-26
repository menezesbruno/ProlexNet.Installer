using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace ProlexNetSetup.Class.Common
{
    public class ExtractZIP
    {
        public static void Extract(string file, string folder)
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

        public static void ExtractDatabase(string file, string folder, string databaseDeployed)
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

        public static void ExtractOverwrite(string file, string folder, string fileToOverwrite)
        {
            try
            {
                if (File.Exists(fileToOverwrite))
                    File.Delete(fileToOverwrite);
                ZipFile.ExtractToDirectory(file, folder);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("ExtractZIP:ExtractOverwrite:" + ex.Message);
            }
        }
    }
}
