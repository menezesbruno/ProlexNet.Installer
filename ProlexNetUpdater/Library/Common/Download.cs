using ProlexNetUpdater.Library.Script;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ProlexNetUpdater.Library.Common
{
    internal class Download
    {
        public static async void ProlexNetAsync()
        {
            var installationSubFolder = Registry.InstallationSubFolder;
            var servicePath = Registry.ServicePath;

            var url = DownloadParameters.ApplicationList.ProlexNet.Url;
            var hash = DownloadParameters.ApplicationList.ProlexNet.Hash;
            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(servicePath, downloadFileName);

            await DownloadFileAsync(url, file, hash);

            //Extrai o ProlexNet para a pasta
            Zip.Extract(file, installationSubFolder);
        }

        public static async void ScriptAsync()
        {
            var servicePath = Registry.ServicePath;
            var scriptsFolder = Path.Combine(servicePath, "Scripts");
            Directory.CreateDirectory(scriptsFolder);
            var state = ScriptExec.GetState();

            var scripts = DownloadParameters.ScriptList.ScriptList;
            scripts.OrderBy(s => s.Version);

            //Roda os scripts em ordem
            foreach (var item in scripts)
            {
                if (item.State == state)
                {
                    var url = item.Url;
                    var hash = item.Hash;
                    var downloadFileName = Path.GetFileName(url);
                    var file = Path.Combine(scriptsFolder, downloadFileName);

                    await DownloadFileAsync(url, file, hash);

                    //Executa os script
                    ScriptExec.RunAsync(file, item);
                }
            }
        }

        public static async Task DownloadFileAsync(string url, string file, string hash)
        {
            var uri = new Uri(url);

            using (WebClient client = new WebClient())
            {
                while (!Hash.Check(file, hash))
                    await client.DownloadFileTaskAsync(uri, file);
            }
        }
    }
}