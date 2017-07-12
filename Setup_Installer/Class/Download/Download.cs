using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Threading.Tasks;

namespace ProlexNetSetup.Class.Download
{
    public class Download
    {
        public static async Task FirebirdAsync(string servicePath, bool silentInstallation)
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
            Installer.Firebird(file, silentInstallation);
        }
      
        public static async Task ProlexNetHostAsync(string servicePath, string installationPath)
        {
            var url = DownloadParameters.Instance.ProlexNet_Server_Url;
            var hash = DownloadParameters.Instance.ProlexNet_Server_Hash;
            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(servicePath, downloadFileName);

            await DownloadFileInBackgroundAsync(url, file, hash);
            var installationSubFolder = Path.Combine(installationPath, "ProlexNet Server");
            if(Directory.Exists(installationSubFolder))
                FolderBackup.Backup(servicePath, installationSubFolder);
            ZipExtractor.Extract(file, installationSubFolder);

            return;
        }

        public static async Task ProlexNetClientAsync(string servicePath, string installationPath)
        {
            var url = DownloadParameters.Instance.ProlexNet_Client_Url;
            var hash = DownloadParameters.Instance.ProlexNet_Client_Hash;
            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(servicePath, downloadFileName);

            await DownloadFileInBackgroundAsync(url, file, hash);
            var installationSubFolder = Path.Combine(installationPath, "ProlexNet Client");
            if (Directory.Exists(installationSubFolder))
                FolderBackup.Backup(servicePath, installationSubFolder);
            ZipExtractor.Extract(file, installationSubFolder);

            return;
        }

        public static async Task VisualCAsync (string servicePath)
        {
            var url = DownloadParameters.Instance.VisualC2103_X86_Url;
            var hash = DownloadParameters.Instance.VisualC2103_X86_Hash;

            var systemType = Environment.Is64BitOperatingSystem;
            if (systemType)
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

        public static async Task DotNetAsync(string servicePath, string dotNetVersion)
        {
            var url = DownloadParameters.Instance.DotNet46_Url;
            var hash = DownloadParameters.Instance.DotNet46_Hash;

            if (dotNetVersion == "4.7")
            {
                url = DownloadParameters.Instance.DotNet47_Url;
                hash = DownloadParameters.Instance.DotNet47_Hash;
            }

            var downloadFileName = Path.GetFileName(url);
            var file = Path.Combine(servicePath, downloadFileName);

            await DownloadFileInBackgroundAsync(url, file, hash);
            Installer.DotNet(file);

            return;
        }
        
        public async static Task DownloadFileInBackgroundAsync(string url, string file, string hash)
        {
            IProgress<DownloadProgressChangedEventArgs> Progress;
            MainWindow mainWindow = new MainWindow();
            var progress = new Progress<DownloadProgressChangedEventArgs>(args =>
            {
                mainWindow.ProgressBar.Maximum = args.TotalBytesToReceive;
                mainWindow.ProgressBar.Value = args.BytesReceived;
            });

            Progress = progress;
            WebClient client = new WebClient();

            client.DownloadFileCompleted += async (sender, args) =>
            {
                var downloadFileName = Path.GetFileName(url);
                if (HashCheck.Check(file, hash))
                {
                    return;
                }
                else
                {
                    await DownloadFileInBackgroundAsync(url, file, hash);
                }
            };
            client.DownloadProgressChanged += (sender, args) => Progress.Report(args);
            await client.DownloadFileTaskAsync(url, file);
            return;
        }
    }
}
