using IWshRuntimeLibrary;
using System;
using System.Diagnostics;
using System.IO;

namespace ProlexNetSetup.Library
{
    internal class CreateShortcut
    {
        public static void ProlexNetClient(string installationSubFolder)
        {
            try
            {
                var startupFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);
                var shell = new WshShell();
                var shortCutLinkFilePath = Path.Combine(startupFolderPath, "ProlexNet.lnk");
                var windowsApplicationShortcut = (IWshShortcut)shell.CreateShortcut(shortCutLinkFilePath);
                var executablePath = Path.Combine(installationSubFolder, "ProlexNet.ExtHost.exe");
                windowsApplicationShortcut.Description = "Executa o ProlexNet";
                windowsApplicationShortcut.WorkingDirectory = installationSubFolder;
                windowsApplicationShortcut.TargetPath = executablePath;
                windowsApplicationShortcut.Save();
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"{nameof(CreateShortcut)}:{nameof(ProlexNetClient)}:{ex.Message}");
            }
        }
    }
}