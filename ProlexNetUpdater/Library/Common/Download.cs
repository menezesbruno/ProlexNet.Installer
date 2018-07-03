using ProlexNetUpdater.Library.Script;
using System.IO;
using System.Linq;
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

            //Extrai o ProlexNet para a pasta
            Zip.Extract(file, installationSubFolder);
        }

        public static void Script()
        {
            var installationSubFolder = Registry.InstallationSubFolder;
            var servicePath = Registry.ServicePath;

            var scripts = DownloadParameters.ApplicationList.ScriptList;
            scripts.OrderBy(s => s.State).ThenBy(s => s.Version);

            //Roda os scripts em ordem
            foreach (var item in scripts)
            {
                var url = DownloadParameters.ApplicationList.ProlexNet.Url;
                var hash = DownloadParameters.ApplicationList.ProlexNet.Hash;
                var downloadFileName = Path.GetFileName(url);
                var file = Path.Combine(servicePath, downloadFileName);

                DownloadFile(url, file, hash);

                //Executa os script
                ScriptExec.Run(file, item);
            }
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