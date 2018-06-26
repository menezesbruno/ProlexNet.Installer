using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace ProlexNetSetup.Library
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

        public static string ExtractAndGetFile(string servicePath, string file)
        {
            try
            {
                var tempFolder = Path.Combine(servicePath, "Database");
                if (Directory.Exists(tempFolder))
                    Directory.Delete(tempFolder);
                Directory.CreateDirectory(tempFolder);

                ZipFile.ExtractToDirectory(file, tempFolder);

                var extractedFile = Directory.GetFiles(tempFolder).FirstOrDefault();
                return extractedFile;
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"{nameof(ZipExtract)}:{nameof(Run)}:{ex.Message}");
                return null;
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