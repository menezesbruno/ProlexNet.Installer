using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Threading.Tasks;
using ProlexNetSetup.Class.Common;
using ProlexNetSetup.Class.Install;
using ProlexNetSetup.Setup.Class.Common;
using System.IO.Compression;

namespace ProlexNetSetup.Class.Download
{
    public class Download
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
            Installer.Firebird(file, installationPath);
        }
      
        public static async Task ProlexNetServerAsync(string servicePath, string installationPath, string applicationGuid, string windowsUninstallPath)
        {
            var url = DownloadParameters.Instance.ProlexNet_Server_Url;
            var hash = DownloadParameters.Instance.ProlexNet_Server_Hash;
            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(servicePath, downloadFileName);

            await DownloadFileInBackgroundAsync(url, file, hash);

            var installationSubFolder = Path.Combine(installationPath, "ProlexNet Server", "www");
            var installationRootFolder = Path.Combine(installationPath, "ProlexNet Server");
            if (Directory.Exists(installationSubFolder))
            {
                FolderBackup.Backup(servicePath, installationSubFolder);
                Directory.Delete(installationSubFolder);
            }
            ZipFile.ExtractToDirectory(file, installationRootFolder);
            RegistryEntry.CreateProlexNetServerUninstaller(servicePath, installationPath, applicationGuid, windowsUninstallPath);

            return;
        }

        public static async Task ProlexNetUpdaterAsync(string servicePath, string installationPath)
        {
            var url = DownloadParameters.Instance.ProlexNet_Updater_Url;
            var hash = DownloadParameters.Instance.ProlexNet_Updater_Hash;
            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(servicePath, downloadFileName);

            await DownloadFileInBackgroundAsync(url, file, hash);

            var installationSubFolder = Path.Combine(installationPath, "ProlexNet Server", "updater");
            var installationRootFolder = Path.Combine(installationPath, "ProlexNet Server");
            if (Directory.Exists(installationSubFolder))
            {
                FolderBackup.Backup(servicePath, installationSubFolder);
                Directory.Delete(installationSubFolder);
            }
            ZipFile.ExtractToDirectory(file, installationRootFolder);

            return;
        }

        public static async Task ProlexNetClientAsync(string servicePath, string installationPath, string applicationGuid, string windowsUninstallPath)
        {
            var url = DownloadParameters.Instance.ProlexNet_Client_Url;
            var hash = DownloadParameters.Instance.ProlexNet_Client_Hash;
            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(servicePath, downloadFileName);

            await DownloadFileInBackgroundAsync(url, file, hash);
            var installationSubFolder = Path.Combine(installationPath, "ProlexNet Client");
            if (Directory.Exists(installationSubFolder))
                FolderBackup.Backup(servicePath, installationSubFolder);
            await ZipExtractor.Extract(file, installationSubFolder);
            ShortcutCreation.ProlexNetClient(installationSubFolder);
            RegistryEntry.CreateProlexNetClientUninstaller(servicePath, installationPath, applicationGuid, windowsUninstallPath);

            return;
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
                    await ZipExtractor.Extract(file, databaseFolder);

                    return;
                }
                else
                {
                    return;
                }
            }
            else
            {
                await ZipExtractor.Extract(file, databaseFolder);

                return; 
            }
        }

        public static async Task VisualCAsync (string servicePath, string systemType)
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
            Installer.VCRedist(file);

            return;
        }

        public static async Task DotNetAsync(string servicePath)
        {
            var url = DownloadParameters.Instance.DotNet46_Url;
            var hash = DownloadParameters.Instance.DotNet46_Hash;

            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(servicePath, downloadFileName);

            await DownloadFileInBackgroundAsync(url, file, hash);
            Installer.DotNet(file);

            return;
        }

        public static async Task LINQPad5Async(string servicePath)
        {
            var url = DownloadParameters.Instance.LINQPad5_Url;
            var hash = DownloadParameters.Instance.LINQPad5_Hash;

            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(servicePath, downloadFileName);

            await DownloadFileInBackgroundAsync(url, file, hash);
            Installer.LINQPad(file);

            return;
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

            client.DownloadFileCompleted += async (sender, args) =>
            {
                var downloadFileName = Path.GetFileName(url);
                if (HashCheck.Check(file, hash))
                {
                    return;
                }
                else
                {
                    MessageBox.Show($"O download do arquivo {file} não passou no teste MD5 informado: {hash}. Uma nova tentativa de download será feita. Caso o download/erro entre em loop, feche o instalador e comunique o setor de desenvolvimento", "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
                    DownloadFileInBackgroundAsync(url, file, hash);
                }
            };

            await client.DownloadFileTaskAsync(url, file);
            return;
        }
    }
}
