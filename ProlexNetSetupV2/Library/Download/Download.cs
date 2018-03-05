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
        public static async void FirebirdAsync(Action<DownloadProgressChangedEventArgs> callback)
        {
            var installationPath = MainWindowViewModel.InstallationPath;
            var servicePath = MainWindowViewModel.ServicePath;

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

            await DownloadFileInBackgroundAsync(url, file, hash, callback);
            Install.Firebird(file, installationPath);
        }

        public static async void ProlexNetServerAsync(Action<DownloadProgressChangedEventArgs> callback)
        {
            var installationPath = MainWindowViewModel.InstallationPath;
            var servicePath = MainWindowViewModel.ServicePath;

            var url = DownloadParameters.Instance.ProlexNet_Server_Url;
            var hash = DownloadParameters.Instance.ProlexNet_Server_Hash;
            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(servicePath, downloadFileName);

            var installationSubFolder = Path.Combine(installationPath, "ProlexNet Server", "www");
            var installationRootFolder = Path.Combine(installationPath, "ProlexNet Server");
            if (Directory.Exists(installationSubFolder))
                Backup.Run(servicePath, installationSubFolder);

            await DownloadFileInBackgroundAsync(url, file, hash, callback);
            ZipExtract.Run(file, installationRootFolder);
            CreateRegistryKey.ProlexNetServer(servicePath, installationPath);
        }

        public static async void ProlexNetUpdaterAsync(Action<DownloadProgressChangedEventArgs> callback)
        {
            var installationPath = MainWindowViewModel.InstallationPath;
            var servicePath = MainWindowViewModel.ServicePath;

            var url = DownloadParameters.Instance.ProlexNet_Updater_Url;
            var hash = DownloadParameters.Instance.ProlexNet_Updater_Hash;
            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(servicePath, downloadFileName);

            var installationSubFolder = Path.Combine(installationPath, "ProlexNet Server", "updater");
            var installationRootFolder = Path.Combine(installationPath, "ProlexNet Server");
            if (Directory.Exists(installationSubFolder))
                Backup.Run(servicePath, installationSubFolder);

            await DownloadFileInBackgroundAsync(url, file, hash, callback);
            ZipExtract.Run(file, installationRootFolder);
        }

        public static async void ProlexNetClientAsync(Action<DownloadProgressChangedEventArgs> callback)
        {
            var installationPath = MainWindowViewModel.InstallationPath;
            var servicePath = MainWindowViewModel.ServicePath;

            var url = DownloadParameters.Instance.ProlexNet_Client_Url;
            var hash = DownloadParameters.Instance.ProlexNet_Client_Hash;
            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(servicePath, downloadFileName);

            var installationSubFolder = Path.Combine(installationPath, "ProlexNet Client");
            if (Directory.Exists(installationSubFolder))
                Backup.Run(servicePath, installationSubFolder);

            await DownloadFileInBackgroundAsync(url, file, hash, callback);
            ZipExtract.Run(file, installationSubFolder);
            CreateShortcut.ProlexNetClient(installationSubFolder);
            CreateRegistryKey.ProlexNetClient(servicePath, installationPath);
        }

        public static async void ProlexNetDatabaseAsync(Action<DownloadProgressChangedEventArgs> callback)
        {
            var installationPath = MainWindowViewModel.InstallationPath;
            var servicePath = MainWindowViewModel.ServicePath;

            var databaseFolder = Path.Combine(installationPath, "Database");
            Directory.CreateDirectory(databaseFolder);

            var url = DownloadParameters.Instance.ProlexNet_Database_Url;
            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(servicePath, downloadFileName);
            var hash = DownloadParameters.Instance.ProlexNet_Database_Hash;

            var databaseName = "ProlexNet.prolex";
            var databaseDeployed = Path.Combine(databaseFolder, databaseName);

            await DownloadFileInBackgroundAsync(url, file, hash, callback);
            if (File.Exists(databaseDeployed))
            {
                var overwrite = System.Windows.MessageBox.Show($"O banco de dados {databaseName} já existe na pasta {databaseFolder}. Deseja sobrescrevê-lo? Este processo não poderá ser revertido.", "Aviso!", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (overwrite == MessageBoxResult.Yes)
                    ZipExtract.Overwrite(file, databaseFolder, databaseDeployed);
            }
            else
                ZipExtract.Overwrite(file, databaseFolder, databaseDeployed);
        }

        public static async void VisualCAsync(string systemType, Action<DownloadProgressChangedEventArgs> callback)
        {
            var installationPath = MainWindowViewModel.InstallationPath;
            var servicePath = MainWindowViewModel.ServicePath;

            var url = DownloadParameters.Instance.VisualC2103_X86_Url;
            var hash = DownloadParameters.Instance.VisualC2103_X86_Hash;

            if (systemType == "x64")
            {
                url = DownloadParameters.Instance.VisualC2103_X64_Url;
                hash = DownloadParameters.Instance.VisualC2103_X64_Hash;
            }

            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(servicePath, downloadFileName);

            await DownloadFileInBackgroundAsync(url, file, hash, callback);
            Install.VCRedist(file);
        }

        public static async void DotNetAsync(Action<DownloadProgressChangedEventArgs> callback)
        {
            var installationPath = MainWindowViewModel.InstallationPath;
            var servicePath = MainWindowViewModel.ServicePath;

            var url = DownloadParameters.Instance.DotNet46_Url;
            var hash = DownloadParameters.Instance.DotNet46_Hash;

            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(servicePath, downloadFileName);

            await DownloadFileInBackgroundAsync(url, file, hash, callback);
            Install.DotNet(file);
        }

        public static async void LINQPad5Async(Action<DownloadProgressChangedEventArgs> callback)
        {
            var installationPath = MainWindowViewModel.InstallationPath;
            var servicePath = MainWindowViewModel.ServicePath;

            var url = DownloadParameters.Instance.LINQPad5_Url;
            var hash = DownloadParameters.Instance.LINQPad5_Hash;

            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(servicePath, downloadFileName);

            await DownloadFileInBackgroundAsync(url, file, hash, callback);
            Install.LINQPad(file);
        }

        public static async void IBExpertSetupAsync(Action<DownloadProgressChangedEventArgs> callback)
        {
            var installationPath = MainWindowViewModel.InstallationPath;
            var servicePath = MainWindowViewModel.ServicePath;

            var url = DownloadParameters.Instance.IBExpertSetup_Url;
            var hash = DownloadParameters.Instance.IBExpertSetup_Hash;

            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(servicePath, downloadFileName);

            await DownloadFileInBackgroundAsync(url, file, hash, callback);
            Install.IBExpertSetup(file);

            IBExpertAsync(callback);
        }

        public static async void IBExpertAsync(Action<DownloadProgressChangedEventArgs> callback)
        {
            var installationPath = MainWindowViewModel.InstallationPath;
            var servicePath = MainWindowViewModel.ServicePath;

            var url = DownloadParameters.Instance.IBExpert_Url;
            var hash = DownloadParameters.Instance.IBExpert_Hash;

            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(servicePath, downloadFileName);

            var installFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "HK-Software", "IBExpert");

            if (Environment.Is64BitOperatingSystem)
                installFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "HK-Software", "IBExpert");

            var ibExpert = "IBExpert.exe";
            var ibExpertDeployed = Path.Combine(installFolder, ibExpert);

            await DownloadFileInBackgroundAsync(url, file, hash, callback);
            ZipExtract.Overwrite(file, installFolder, ibExpertDeployed);
        }

        public async static Task DownloadFileInBackgroundAsync(string url, string file, string hash, Action<DownloadProgressChangedEventArgs> callback)
        {
            WebClient client = new WebClient();

            client.DownloadProgressChanged += (sender, args) =>
            {
                callback(args);
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