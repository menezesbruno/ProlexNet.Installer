using System.IO;
using System.IO.Compression;

namespace ProlexNetUpdater.Library.Common
{
    internal class Zip
    {
        public static void Extract(string file, string folder)
        {
            if (Directory.Exists(folder))
            {
                var bakFolder = folder + "bak";
                if (Directory.Exists(bakFolder))
                    Directory.Delete(bakFolder, true);

                Directory.Move(folder, bakFolder);
            }

            Directory.CreateDirectory(folder);
            ZipFile.ExtractToDirectory(file, folder);
        }
    }
}