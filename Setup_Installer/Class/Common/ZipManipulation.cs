using System.IO;
using System.IO.Compression;

namespace Setup_Installer.Class.Download
{
    public class ZipExtractor
    {
        public static void Extract(string file, string installationSubFolder)
        {
            if (Directory.Exists(installationSubFolder))
                Directory.Delete(installationSubFolder);
            ZipFile.ExtractToDirectory(file, installationSubFolder);
            return;
        }

        public static void Compress(string file, string installationSubFolder)
        {

        }
    }
}
