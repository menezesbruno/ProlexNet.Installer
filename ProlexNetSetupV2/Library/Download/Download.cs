using ProlexNetSetupV2.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProlexNetSetupV2.Library
{
    internal class Download
    {
        public static async Task FirebirdAsync(string servicePath, string installationPath)
        {
            var firebird_Url_X86 = DownloadParameters.Instance.Firebird_X86_Url;
            var firebird_Hash_X86 = DownloadParameters.Instance.Firebird_X86_Hash;
            var firebird_Url_X64 = DownloadParameters.Instance.Firebird_X64_Url;
            var firebird_Hash_X64 = DownloadParameters.Instance.Firebird_X64_Hash;

            var url = firebird_Url_X86;
            var hash = firebird_Hash_X86;
            var systemType = Environment.Is64BitOperatingSystem;
            if (systemType)
            {
                url = firebird_Url_X64;
                hash = firebird_Hash_X64;
            }

            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(servicePath, downloadFileName);

            await DownloadFileInBackgroundAsync(url, file, hash);
            Install.Firebird(file, installationPath);
        }

        public static async Task ProlexNetServerAsync(string servicePath, string installationPath)
        {
            var url = DownloadParameters.Instance.ProlexNet_Server_Url;
            var hash = DownloadParameters.Instance.ProlexNet_Server_Hash;
            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(servicePath, downloadFileName);

            var installationSubFolder = Path.Combine(installationPath, "ProlexNet Server", "www");
            var installationRootFolder = Path.Combine(installationPath, "ProlexNet Server");
            if (Directory.Exists(installationSubFolder))
            {
                Backup.Run(servicePath, installationSubFolder);
                Directory.Delete(installationSubFolder);
            }

            await DownloadFileInBackgroundAsync(url, file, hash);
            ZipExtract.Run(file, installationRootFolder);
            CreateRegistryKey.ProlexNetServer(servicePath, installationPath);
        }

        public static async Task ProlexNetUpdaterAsync(string servicePath, string installationPath)
        {
            var url = DownloadParameters.Instance.ProlexNet_Updater_Url;
            var hash = DownloadParameters.Instance.ProlexNet_Updater_Hash;
            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(servicePath, downloadFileName);

            var installationSubFolder = Path.Combine(installationPath, "ProlexNet Server", "updater");
            var installationRootFolder = Path.Combine(installationPath, "ProlexNet Server");
            if (Directory.Exists(installationSubFolder))
            {
                Backup.Run(servicePath, installationSubFolder);
                Directory.Delete(installationSubFolder);
            }

            await DownloadFileInBackgroundAsync(url, file, hash);
            ZipExtract.Run(file, installationRootFolder);
        }

        public static async Task ProlexNetClientAsync(string servicePath, string installationPath)
        {
            var url = DownloadParameters.Instance.ProlexNet_Client_Url;
            var hash = DownloadParameters.Instance.ProlexNet_Client_Hash;
            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(servicePath, downloadFileName);

            var installationSubFolder = Path.Combine(installationPath, "ProlexNet Client");
            if (Directory.Exists(installationSubFolder))
                Backup.Run(servicePath, installationSubFolder);

            await DownloadFileInBackgroundAsync(url, file, hash);
            ZipExtract.Run(file, installationSubFolder);
            CreateShortcut.ProlexNetClient(installationSubFolder);
            CreateRegistryKey.ProlexNetClient(servicePath, installationPath);
        }

        public static async Task ProlexNetDatabaseAsync(string servicePath, string installationPath)
        {
            var databaseFolder = Path.Combine(installationPath, "Database");
            Directory.CreateDirectory(databaseFolder);

            var url = DownloadParameters.Instance.ProlexNet_Database_Url;
            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(servicePath, downloadFileName);
            var hash = DownloadParameters.Instance.ProlexNet_Database_Hash;

            var databaseName = "ProlexNet.prolex";
            var databaseDeployed = Path.Combine(databaseFolder, databaseName);

            await DownloadFileInBackgroundAsync(url, file, hash);
            if (File.Exists(databaseDeployed))
            {
                var overwrite = MessageBox.Show($"O banco de dados {databaseName} já existe na pasta {databaseFolder}. Deseja sobrescrevê-lo? Este processo não poderá ser revertido.", "Aviso!", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (overwrite == MessageBoxResult.Yes)
                {
                    ZipExtract.Overwrite(file, databaseFolder, databaseDeployed);
                    return;
                }
                else
                    return;
            }
            else
                ZipExtract.Overwrite(file, databaseFolder, databaseDeployed);
        }

        public static async Task VisualCAsync(string servicePath, string systemType)
        {
            var url = DownloadParameters.Instance.VisualC2103_X86_Url;
            var hash = DownloadParameters.Instance.VisualC2103_X86_Hash;

            if (systemType == "x64")
            {
                url = DownloadParameters.Instance.VisualC2103_X64_Url;
                hash = DownloadParameters.Instance.VisualC2103_X64_Hash;
            }

            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(servicePath, downloadFileName);

            await DownloadFileInBackgroundAsync(url, file, hash);
            Install.VCRedist(file);
        }

        public static async Task DotNetAsync(string servicePath)
        {
            var url = DownloadParameters.Instance.DotNet46_Url;
            var hash = DownloadParameters.Instance.DotNet46_Hash;

            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(servicePath, downloadFileName);

            await DownloadFileInBackgroundAsync(url, file, hash);
            Install.DotNet(file);
        }

        public static async Task LINQPad5Async(string servicePath)
        {
            var url = DownloadParameters.Instance.LINQPad5_Url;
            var hash = DownloadParameters.Instance.LINQPad5_Hash;

            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(servicePath, downloadFileName);

            await DownloadFileInBackgroundAsync(url, file, hash);
            Install.LINQPad(file);
        }

        public async static Task DownloadFileInBackgroundAsync(string url, string file, string hash)
        {
            IProgress<DownloadProgressChangedEventArgs> Progress =
                new Progress<DownloadProgressChangedEventArgs>(((MainWindow)Application.Current.MainWindow).UpdateDownloadProgress);

            WebClient client = new WebClient();

            client.DownloadProgressChanged += (sender, args) =>
            {
                Progress.Report(args);
            };

            client.DownloadFileCompleted += (sender, args) =>
            {
                var downloadFileName = Path.GetFileName(url);
                if (Hash.Check(file, hash))
                    return;
                else
                {
                    MessageBox.Show($"O download do arquivo {file} não passou no teste MD5 informado: {hash}. A instalação será finalizada. Informe ao setor de Desenvolvimento.", "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
                    Environment.Exit(1);
                }
            };

            try
            {
                await client.DownloadFileTaskAsync(url, file);
            }
            catch
            {
                MessageBox.Show("Servidor de downloads fora do ar. Informe ao setor de desenvolvimento.", "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            }
        }
    }
}