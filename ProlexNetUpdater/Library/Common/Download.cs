using System.IO;
using System.Net;

namespace ProlexNetUpdater.Library.Common
{
    internal class Download
    {
        public static void ProlexNet()
        {
            var installationSubFolder = Registry.InstallationSubFolder;
            var servicePath = Registry.ServicePath;

            var url = DownloadParameters.ApplicationList.ProlexNet.Url;
            var hash = DownloadParameters.ApplicationList.ProlexNet.Hash;
            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(servicePath, downloadFileName);

            DownloadFile(url, file, hash);

            Zip.Extract(file, installationSubFolder);
        }
        public static void DownloadFile(string url, string file, string hash)
        {
            using (WebClient client = new WebClient())
            {
                while (!Hash.Check(file, hash))
                    client.DownloadFileTaskAsync(url, file).GetAwaiter().GetResult();
            }
        }
    }
}