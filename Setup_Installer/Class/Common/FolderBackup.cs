using System;
using System.IO;

namespace Setup_Installer.Class.Download
{
    public class FolderBackup
    {
        public static void Backup(string servicePath, string folderToBackup)
        {
            var backupFolder = Path.Combine(servicePath, "Backup");
            Directory.CreateDirectory(backupFolder);
            var timeStamp = DateTime.Now.ToString("yyyy.MM.dd.HH.mm");
            var folderName = Path.GetFileNameWithoutExtension(folderToBackup);
            var backupDateTime = Path.Combine(backupFolder, $"{folderName} {timeStamp}");
            Directory.Move(folderToBackup, backupDateTime);
            return;
        }
    }
}
