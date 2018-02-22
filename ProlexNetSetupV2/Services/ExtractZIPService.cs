using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProlexNetSetupV2.Services
{
    class ExtractZIPService
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
                Trace.WriteLine($"{nameof(ExtractZIPService)}:{nameof(Extract)}:{ex.Message}");
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
                Trace.WriteLine($"{nameof(ExtractZIPService)}:{nameof(ExtractDatabase)}:{ex.Message}");
            }
        }
    }
}