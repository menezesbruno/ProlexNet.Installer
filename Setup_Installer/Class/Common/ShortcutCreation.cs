using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IWshRuntimeLibrary;

namespace ProlexNet.Setup.Class.Common
{
    public static class ShortcutCreation
    {
        public static void ProlexNetClient(string installationSubFolder)
        {
            var startupFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var shell = new WshShell();
            var shortCutLinkFilePath = Path.Combine(startupFolderPath, "ProlexNet.lnk");
            var windowsApplicationShortcut = (IWshShortcut)shell.CreateShortcut(shortCutLinkFilePath);
            var startupPath = Path.Combine(installationSubFolder, "bin");
            var executablePath = Path.Combine(installationSubFolder, "bin", "ProlexNet.ExtHost.exe");
            windowsApplicationShortcut.Description = "Executa o ProlexNet";
            windowsApplicationShortcut.WorkingDirectory = startupPath;
            windowsApplicationShortcut.TargetPath = executablePath;
            windowsApplicationShortcut.Save();

            return;
        }
    }
}
