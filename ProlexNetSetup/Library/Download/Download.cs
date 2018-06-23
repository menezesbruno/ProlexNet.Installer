using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using ProlexNetSetup.ViewModels;

namespace ProlexNetSetup.Library
{
    internal class Download
    {
        public static Stopwatch sw = new Stopwatch();

        // ProlexNet
        public static async Task ProlexNetAsync(Action<DownloadProgressChangedEventArgs, double> callback, string port)
        {
            var installationPath = MainWindowViewModel.InstallationPath;
            var servicePath = MainWindowViewModel.ServicePath;

            var url = DownloadParameters.AppList.ProlexNet.Url;
            var hash = DownloadParameters.AppList.ProlexNet.Hash;
            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(servicePath, downloadFileName);

            var installationSubFolder = Path.Combine(installationPath, "ProlexNet");
            if (Directory.Exists(installationSubFolder))
                Backup.Run(servicePath, installationSubFolder);

            await DownloadFileInBackgroundAsync(url, file, hash, callback);
            await Task.Run(() => ZipExtract.Run(file, installationSubFolder));
            await Task.Run(() => CreateRegistryKey.ProlexNet(servicePath, installationPath));
        }

        // Database
        public static async Task DatabaseAsync(Action<DownloadProgressChangedEventArgs, double> callback)
        {
            var installationPath = MainWindowViewModel.InstallationPath;
            var servicePath = MainWindowViewModel.ServicePath;

            var databaseFolder = Path.Combine(installationPath, "Database");
            Directory.CreateDirectory(databaseFolder);

            var url = DownloadParameters.AppList.Database.Url;
            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(servicePath, downloadFileName);
            var hash = DownloadParameters.AppList.Database.Hash;

            var databaseName = "ProlexNet.prolex";
            var databaseDeployed = Path.Combine(databaseFolder, databaseName);

            await DownloadFileInBackgroundAsync(url, file, hash, callback);
            if (File.Exists(databaseDeployed))
            {
                var overwrite = System.Windows.MessageBox.Show($"O banco de dados {databaseName} já existe na pasta {databaseFolder}. Deseja sobrescrevê-lo? Este processo não poderá ser revertido.", "Aviso!", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (overwrite == MessageBoxResult.Yes)
                    await Task.Run(() => ZipExtract.Overwrite(file, databaseFolder, databaseDeployed));
            }
            else
                await Task.Run(() => ZipExtract.Overwrite(file, databaseFolder, databaseDeployed));
        }

        // NetCore 2.1
        public static async Task NetCore21Async(Action<DownloadProgressChangedEventArgs, double> callback)
        {
            var installationPath = MainWindowViewModel.InstallationPath;
            var servicePath = MainWindowViewModel.ServicePath;

            var url = DownloadParameters.AppList.NetCore21.Url;
            var hash = DownloadParameters.AppList.NetCore21.Hash;

            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(servicePath, downloadFileName);

            await DownloadFileInBackgroundAsync(url, file, hash, callback);
            await Task.Run(() => Install.NetCore21(file));
        }

        // LINQPad5
        public static async Task LINQPad5Async(Action<DownloadProgressChangedEventArgs, double> callback)
        {
            var installationPath = MainWindowViewModel.InstallationPath;
            var servicePath = MainWindowViewModel.ServicePath;

            var url = DownloadParameters.AppList.LINQPad5.Url;
            var hash = DownloadParameters.AppList.LINQPad5.Hash;

            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(servicePath, downloadFileName);

            await DownloadFileInBackgroundAsync(url, file, hash, callback);
            await Task.Run(() => Install.LINQPad5(file));
        }

        // Download
        public async static Task DownloadFileInBackgroundAsync(string url, string file, string hash, Action<DownloadProgressChangedEventArgs, double> callback)
        {
            if (File.Exists(file))
            {
                if (Hash.Check(file, hash))
                    return;
                else
                    File.Delete(file);
            }

            using (WebClient client = new WebClient())
            {
                sw.Start();
                client.DownloadProgressChanged += (sender, args) =>
                {
                    callback(args, sw.Elapsed.TotalSeconds);
                };

                client.DownloadFileCompleted += (sender, args) =>
                {
                    sw.Reset();
                    var downloadFileName = Path.GetFileName(url);
                    if (Hash.Check(file, hash))
                        return;
                    else
                    {
                        MessageBox.Show($"O download do arquivo {file} não passou no teste MD5 informado: {hash}. A instalação será finalizada. Informe ao setor de Desenvolvimento.", "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
                        System.Windows.Application.Current.Shutdown();
                    }
                };

                try
                {
                    await client.DownloadFileTaskAsync(url, file);
                }
                catch
                {
                    MessageBox.Show("Servidor de downloads fora do ar. Informe ao setor de desenvolvimento.", "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
                    System.Windows.Application.Current.Shutdown();
                }
            }
        }
    }
}