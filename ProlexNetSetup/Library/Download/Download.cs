using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using ProlexNetSetup.Library.Startup;
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
            var servicePath = CreateServicePath.ServicePath;

            var url = DownloadParameters.AppList.ProlexNet.Url;
            var hash = DownloadParameters.AppList.ProlexNet.Hash;
            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(servicePath, downloadFileName);

            var installationSubFolder = Path.Combine(installationPath, "ProlexNet");
            if (Directory.Exists(installationSubFolder))
                Directory.Delete(installationSubFolder);

            await DownloadFileInBackgroundAsync(url, file, hash, callback);
            await Task.Run(() => ZipExtract.Run(file, installationSubFolder));
            await Task.Run(() => CreateRegistryKey.ProlexNet(servicePath, installationPath));
        }

        // VCRedist
        public static async Task VcRedistAsync(string systemType, Action<DownloadProgressChangedEventArgs, double> callback)
        {
            var installationPath = MainWindowViewModel.InstallationPath;
            var servicePath = CreateServicePath.ServicePath;

            var url = DownloadParameters.AppList.VcRedistX86.Url;
            var hash = DownloadParameters.AppList.VcRedistX86.Hash;

            if (systemType == "x64")
            {
                url = DownloadParameters.AppList.VcRedistX64.Url;
                hash = DownloadParameters.AppList.VcRedistX64.Hash;
            }

            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(servicePath, downloadFileName);

            await DownloadFileInBackgroundAsync(url, file, hash, callback);
            await Task.Run(() => Install.VCRedist(file));
        }

        // Net Core
        public static async Task NetCoreAsync(Action<DownloadProgressChangedEventArgs, double> callback)
        {
            var installationPath = MainWindowViewModel.InstallationPath;
            var servicePath = CreateServicePath.ServicePath;

            var url = DownloadParameters.AppList.NetCore.Url;
            var hash = DownloadParameters.AppList.NetCore.Hash;

            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(servicePath, downloadFileName);

            await DownloadFileInBackgroundAsync(url, file, hash, callback);
            await Task.Run(() => Install.NetCore(file));
        }

        // SQL Server
        public static async Task SQLServerAsync(Action<DownloadProgressChangedEventArgs, double> callback)
        {
            var installationPath = MainWindowViewModel.InstallationPath;
            var servicePath = CreateServicePath.ServicePath;

            var url = DownloadParameters.AppList.SQLServer.Url;
            var hash = DownloadParameters.AppList.SQLServer.Hash;

            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(servicePath, downloadFileName);

            await DownloadFileInBackgroundAsync(url, file, hash, callback);
            await Task.Run(() => Install.SQLServer(file));
        }

        // SQL Server Studio
        public static async Task SQLServerStudioAsync(Action<DownloadProgressChangedEventArgs, double> callback)
        {
            var installationPath = MainWindowViewModel.InstallationPath;
            var servicePath = CreateServicePath.ServicePath;

            var url = DownloadParameters.AppList.SQLServerStudio.Url;
            var hash = DownloadParameters.AppList.SQLServerStudio.Hash;

            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(servicePath, downloadFileName);

            await DownloadFileInBackgroundAsync(url, file, hash, callback);
            await Task.Run(() => Install.SQLServerStudio(file));
        }

        // LinqPad
        public static async Task LinqPadAsync(Action<DownloadProgressChangedEventArgs, double> callback)
        {
            var installationPath = MainWindowViewModel.InstallationPath;
            var servicePath = CreateServicePath.ServicePath;

            var url = DownloadParameters.AppList.LinqPad.Url;
            var hash = DownloadParameters.AppList.LinqPad.Hash;

            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(servicePath, downloadFileName);

            await DownloadFileInBackgroundAsync(url, file, hash, callback);
            await Task.Run(() => Install.LinqPad(file));
        }
        
        // Database
        public static async Task DatabaseAsync(Action<DownloadProgressChangedEventArgs, double> callback)
        {
            var installationPath = MainWindowViewModel.InstallationPath;
            var servicePath = CreateServicePath.ServicePath;

            var url = DownloadParameters.AppList.Database.Url;
            var hash = DownloadParameters.AppList.Database.Hash;

            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(servicePath, downloadFileName);

            await DownloadFileInBackgroundAsync(url, file, hash, callback);
            await Task.Run(() => Install.DatabaseAsync(servicePath, installationPath, file));
        }

        // Download Factory
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
                        Application.Current.Shutdown();
                    }
                };

                try
                {
                    await client.DownloadFileTaskAsync(url, file);
                }
                catch
                {
                    MessageBox.Show("Servidor de downloads fora do ar. Informe ao setor de desenvolvimento.", "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
                    Application.Current.Shutdown();
                }
            }
        }
    }
}