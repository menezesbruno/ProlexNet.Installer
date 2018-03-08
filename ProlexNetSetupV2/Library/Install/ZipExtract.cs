using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProlexNetSetupV2.Library
{
    internal class ZipExtract
    {
        public static void Run(string file, string folder)
        {
            try
            {
                ZipFile.ExtractToDirectory(file, folder);
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"{nameof(ZipExtract)}:{nameof(Run)}:{ex.Message}");
            }
        }

        public static void Overwrite(string file, string folder, string fileToOverwrite)
        {
            try
            {
                if (File.Exists(fileToOverwrite))
                    File.Delete(fileToOverwrite);
                ZipFile.ExtractToDirectory(file, folder);
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"{nameof(ZipExtract)}:{nameof(Overwrite)}:{ex.Message}");
            }
        }
    }
}