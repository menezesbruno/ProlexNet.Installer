using ProlexNetSetup.ViewModels;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;

namespace ProlexNetSetup.Library
{
    internal class Download
    {
        public static Stopwatch sw = new Stopwatch();

        public static async Task FirebirdAsync(Action<DownloadProgressChangedEventArgs, double> callback)
        {
            var installationPath = MainWindowViewModel.InstallationPath;
            var servicePath = MainWindowViewModel.ServicePath;

            var firebird_Url_X86 = DownloadParameters.AppList.Firebird_X86_Url;
            var firebird_Hash_X86 = DownloadParameters.AppList.Firebird_X86_Hash;
            var firebird_Url_X64 = DownloadParameters.AppList.Firebird_X64_Url;
            var firebird_Hash_X64 = DownloadParameters.AppList.Firebird_X64_Hash;

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
            await Task.Run(() => Install.Firebird(file, installationPath));
        }

        public static async Task ProlexNetServerAsync(Action<DownloadProgressChangedEventArgs, double> callback, string prolexNetServerPort)
        {
            var installationPath = MainWindowViewModel.InstallationPath;
            var servicePath = MainWindowViewModel.ServicePath;

            var url = DownloadParameters.AppList.ProlexNet_Server_Url;
            var hash = DownloadParameters.AppList.ProlexNet_Server_Hash;
            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(servicePath, downloadFileName);

            var installationSubFolder = Path.Combine(installationPath, "ProlexNet Server", "www");
            var installationRootFolder = Path.Combine(installationPath, "ProlexNet Server");
            if (Directory.Exists(installationSubFolder))
                Backup.Run(servicePath, installationSubFolder);

            await DownloadFileInBackgroundAsync(url, file, hash, callback);
            await Task.Run(() => ZipExtract.Run(file, installationRootFolder));
            await Task.Run(() => CreateRegistryKey.ProlexNetServer(servicePath, installationPath));
            await Task.Run(() => ConfigProlexNet.Server(installationPath, prolexNetServerPort));
            await Task.Run(() => Firewall.AddRules(prolexNetServerPort));
        }

        public static async Task ProlexNetUpdaterAsync(Action<DownloadProgressChangedEventArgs, double> callback)
        {
            var installationPath = MainWindowViewModel.InstallationPath;
            var servicePath = MainWindowViewModel.ServicePath;

            var url = DownloadParameters.AppList.ProlexNet_Updater_Url;
            var hash = DownloadParameters.AppList.ProlexNet_Updater_Hash;
            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(servicePath, downloadFileName);

            var installationSubFolder = Path.Combine(installationPath, "ProlexNet Server", "updater");
            var installationRootFolder = Path.Combine(installationPath, "ProlexNet Server");
            if (Directory.Exists(installationSubFolder))
                Backup.Run(servicePath, installationSubFolder);

            await DownloadFileInBackgroundAsync(url, file, hash, callback);
            await Task.Run(() => ZipExtract.Run(file, installationRootFolder));
            await Task.Run(() => ConfigProlexNet.Updater(installationPath));
        }

        public static async Task ProlexNetClientAsync(Action<DownloadProgressChangedEventArgs, double> callback)
        {
            var installationPath = MainWindowViewModel.InstallationPath;
            var servicePath = MainWindowViewModel.ServicePath;

            var url = DownloadParameters.AppList.ProlexNet_Client_Url;
            var hash = DownloadParameters.AppList.ProlexNet_Client_Hash;
            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(servicePath, downloadFileName);

            var installationSubFolder = Path.Combine(installationPath, "ProlexNet Client");
            if (Directory.Exists(installationSubFolder))
                Backup.Run(servicePath, installationSubFolder);

            await DownloadFileInBackgroundAsync(url, file, hash, callback);
            await Task.Run(() => ZipExtract.Run(file, installationSubFolder));
            await Task.Run(() => CreateShortcut.ProlexNetClient(installationSubFolder));
            await Task.Run(() => CreateRegistryKey.ProlexNetClient(servicePath, installationPath));
        }

        public static async Task ProlexNetDatabaseAsync(Action<DownloadProgressChangedEventArgs, double> callback)
        {
            var installationPath = MainWindowViewModel.InstallationPath;
            var servicePath = MainWindowViewModel.ServicePath;

            var databaseFolder = Path.Combine(installationPath, "Database");
            Directory.CreateDirectory(databaseFolder);

            var url = DownloadParameters.AppList.ProlexNet_Database_Url;
            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(servicePath, downloadFileName);
            var hash = DownloadParameters.AppList.ProlexNet_Database_Hash;

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

        public static async Task VisualCAsync(string systemType, Action<DownloadProgressChangedEventArgs, double> callback)
        {
            var installationPath = MainWindowViewModel.InstallationPath;
            var servicePath = MainWindowViewModel.ServicePath;

            var url = DownloadParameters.AppList.VisualC2103_X86_Url;
            var hash = DownloadParameters.AppList.VisualC2103_X86_Hash;

            if (systemType == "x64")
            {
                url = DownloadParameters.AppList.VisualC2103_X64_Url;
                hash = DownloadParameters.AppList.VisualC2103_X64_Hash;
            }

            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(servicePath, downloadFileName);

            await DownloadFileInBackgroundAsync(url, file, hash, callback);
            await Task.Run(() => Install.VCRedist(file));
        }

        public static async Task DotNetAsync(Action<DownloadProgressChangedEventArgs, double> callback)
        {
            var installationPath = MainWindowViewModel.InstallationPath;
            var servicePath = MainWindowViewModel.ServicePath;

            var url = DownloadParameters.AppList.DotNet46_Url;
            var hash = DownloadParameters.AppList.DotNet46_Hash;

            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(servicePath, downloadFileName);

            await DownloadFileInBackgroundAsync(url, file, hash, callback);
            await Task.Run(() => Install.DotNet(file));
        }

        public static async Task LINQPad5Async(Action<DownloadProgressChangedEventArgs, double> callback)
        {
            var installationPath = MainWindowViewModel.InstallationPath;
            var servicePath = MainWindowViewModel.ServicePath;

            var url = DownloadParameters.AppList.LINQPad5_Url;
            var hash = DownloadParameters.AppList.LINQPad5_Hash;

            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(servicePath, downloadFileName);

            await DownloadFileInBackgroundAsync(url, file, hash, callback);
            await Task.Run(() => Install.LINQPad(file));
        }

        public static async Task IBExpertSetupAsync(Action<DownloadProgressChangedEventArgs, double> callback)
        {
            var installationPath = MainWindowViewModel.InstallationPath;
            var servicePath = MainWindowViewModel.ServicePath;

            var url = DownloadParameters.AppList.IBExpertSetup_Url;
            var hash = DownloadParameters.AppList.IBExpertSetup_Hash;

            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(servicePath, downloadFileName);

            await DownloadFileInBackgroundAsync(url, file, hash, callback);
            await Task.Run(() => Install.IBExpertSetup(file));

            await Task.Run(() => IBExpertAsync(callback));
        }

        public static async Task IBExpertAsync(Action<DownloadProgressChangedEventArgs, double> callback)
        {
            var installationPath = MainWindowViewModel.InstallationPath;
            var servicePath = MainWindowViewModel.ServicePath;

            var url = DownloadParameters.AppList.IBExpert_Url;
            var hash = DownloadParameters.AppList.IBExpert_Hash;

            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(servicePath, downloadFileName);

            var installFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "HK-Software", "IBExpert");

            if (Environment.Is64BitOperatingSystem)
                installFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "HK-Software", "IBExpert");

            var ibExpert = "IBExpert.exe";
            var ibExpertDeployed = Path.Combine(installFolder, ibExpert);

            await DownloadFileInBackgroundAsync(url, file, hash, callback);
            await Task.Run(() => ZipExtract.Overwrite(file, installFolder, ibExpertDeployed));
        }

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