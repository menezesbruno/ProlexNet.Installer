using IWshRuntimeLibrary;
using System;
using System.IO;

namespace ProlexNetSetup.Setup.Class.Common
{
    public static class CreateShortcut
    {
        public static void ProlexNetClient(string installationSubFolder)
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
    }
}