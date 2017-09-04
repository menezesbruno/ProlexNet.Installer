using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace ProlexNetSetup.Class.Common
{
    public class ZipExtractor
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
                Trace.WriteLine("ZipExtractor:Extract:" + ex.Message);
            }
            
            return;
        }
    }
}
