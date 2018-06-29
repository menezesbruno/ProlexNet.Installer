using System.IO;
using System.IO.Compression;

namespace ProlexNetUpdater.Library.Update
{
    internal class Zip
    {
        public static void Extract(string file, string folder)
        {
            if (Directory.Exists(folder))
                Directory.Delete(folder, true);

            Directory.CreateDirectory(folder);
            ZipFile.ExtractToDirectory(file, folder);
        }
    }
}