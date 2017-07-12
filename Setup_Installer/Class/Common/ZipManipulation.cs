using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;

namespace ProlexNetSetup.Class.Download
{
    public class ZipExtractor
    {
        public static void Extract(string file, string installationSubFolder)
        {
            try
            {
                if (Directory.Exists(installationSubFolder))
                    Directory.Delete(installationSubFolder);
                ZipFile.ExtractToDirectory(file, installationSubFolder);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("ZipExtractor:Extract:" + ex.Message);
            }
            
            return;
        }
    }
}
