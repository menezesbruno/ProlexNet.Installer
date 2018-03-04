using ProlexNetSetupV2.ViewModels;
using ProlexNetSetupV2.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace ProlexNetSetupV2.Library
{
    internal class Download
    {
        public static string ServicePath { get; set; }

        public static string InstallationPath { get; set; }

        public Download()
        {
            ServicePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Automatiza", "Instalador");
            Directory.CreateDirectory(ServicePath);

            InstallationPath = MainWindowViewModel.InstallationPath;
        }

        public static async Task FirebirdAsync()
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
            var file = Path.Combine(ServicePath, downloadFileName);

            await DownloadFileInBackgroundAsync(url, file, hash);
            Install.Firebird(file, InstallationPath);
        }

        public static async Task ProlexNetServerAsync()
        {
            var url = DownloadParameters.Instance.ProlexNet_Server_Url;
            var hash = DownloadParameters.Instance.ProlexNet_Server_Hash;
            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(ServicePath, downloadFileName);

            var installationSubFolder = Path.Combine(InstallationPath, "ProlexNet Server", "www");
            var installationRootFolder = Path.Combine(InstallationPath, "ProlexNet Server");
            if (Directory.Exists(installationSubFolder))
            {
                Backup.Run(ServicePath, installationSubFolder);
                Directory.Delete(installationSubFolder);
            }

            await DownloadFileInBackgroundAsync(url, file, hash);
            ZipExtract.Run(file, installationRootFolder);
            CreateRegistryKey.ProlexNetServer(ServicePath, InstallationPath);
        }

        public static async Task ProlexNetUpdaterAsync()
        {
            var url = DownloadParameters.Instance.ProlexNet_Updater_Url;
            var hash = DownloadParameters.Instance.ProlexNet_Updater_Hash;
            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(ServicePath, downloadFileName);

            var installationSubFolder = Path.Combine(InstallationPath, "ProlexNet Server", "updater");
            var installationRootFolder = Path.Combine(InstallationPath, "ProlexNet Server");
            if (Directory.Exists(installationSubFolder))
            {
                Backup.Run(ServicePath, installationSubFolder);
                Directory.Delete(installationSubFolder);
            }

            await DownloadFileInBackgroundAsync(url, file, hash);
            ZipExtract.Run(file, installationRootFolder);
        }

        public static async Task ProlexNetClientAsync()
        {
            var url = DownloadParameters.Instance.ProlexNet_Client_Url;
            var hash = DownloadParameters.Instance.ProlexNet_Client_Hash;
            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(ServicePath, downloadFileName);

            var installationSubFolder = Path.Combine(InstallationPath, "ProlexNet Client");
            if (Directory.Exists(installationSubFolder))
                Backup.Run(ServicePath, installationSubFolder);

            await DownloadFileInBackgroundAsync(url, file, hash);
            ZipExtract.Run(file, installationSubFolder);
            CreateShortcut.ProlexNetClient(installationSubFolder);
            CreateRegistryKey.ProlexNetClient(ServicePath, InstallationPath);
        }

        public static async Task ProlexNetDatabaseAsync()
        {
            var databaseFolder = Path.Combine(InstallationPath, "Database");
            Directory.CreateDirectory(databaseFolder);

            var url = DownloadParameters.Instance.ProlexNet_Database_Url;
            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(ServicePath, downloadFileName);
            var hash = DownloadParameters.Instance.ProlexNet_Database_Hash;

            var databaseName = "ProlexNet.prolex";
            var databaseDeployed = Path.Combine(databaseFolder, databaseName);

            await DownloadFileInBackgroundAsync(url, file, hash);
            if (File.Exists(databaseDeployed))
            {
                var overwrite = System.Windows.MessageBox.Show($"O banco de dados {databaseName} já existe na pasta {databaseFolder}. Deseja sobrescrevê-lo? Este processo não poderá ser revertido.", "Aviso!", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (overwrite == MessageBoxResult.Yes)
                    ZipExtract.Overwrite(file, databaseFolder, databaseDeployed);

            }
            else
                ZipExtract.Overwrite(file, databaseFolder, databaseDeployed);
        }

        public static async Task VisualCAsync(string systemType)
        {
            var url = DownloadParameters.Instance.VisualC2103_X86_Url;
            var hash = DownloadParameters.Instance.VisualC2103_X86_Hash;

            if (systemType == "x64")
            {
                url = DownloadParameters.Instance.VisualC2103_X64_Url;
                hash = DownloadParameters.Instance.VisualC2103_X64_Hash;
            }

            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(ServicePath, downloadFileName);

            await DownloadFileInBackgroundAsync(url, file, hash);
            Install.VCRedist(file);
        }

        public static async Task DotNetAsync()
        {
            var url = DownloadParameters.Instance.DotNet46_Url;
            var hash = DownloadParameters.Instance.DotNet46_Hash;

            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(ServicePath, downloadFileName);

            await DownloadFileInBackgroundAsync(url, file, hash);
            Install.DotNet(file);
        }

        public static async Task LINQPad5Async()
        {
            var url = DownloadParameters.Instance.LINQPad5_Url;
            var hash = DownloadParameters.Instance.LINQPad5_Hash;

            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(ServicePath, downloadFileName);

            await DownloadFileInBackgroundAsync(url, file, hash);
            Install.LINQPad(file);
        }

        public static async Task IBExpertAsync()
        {
            throw new NotImplementedException();
        }

        public async static Task DownloadFileInBackgroundAsync(string url, string file, string hash)
        {
            IProgress<DownloadProgressChangedEventArgs> Progress =
                new Progress<DownloadProgressChangedEventArgs>(((MainWindow)System.Windows.Application.Current.MainWindow).UpdateDownloadProgress);

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
                    System.Windows.MessageBox.Show($"O download do arquivo {file} não passou no teste MD5 informado: {hash}. A instalação será finalizada. Informe ao setor de Desenvolvimento.", "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
                    Environment.Exit(1);
                }
            };

            try
            {
                await client.DownloadFileTaskAsync(url, file);
            }
            catch
            {
                System.Windows.MessageBox.Show("Servidor de downloads fora do ar. Informe ao setor de desenvolvimento.", "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            }
        }
    }
}