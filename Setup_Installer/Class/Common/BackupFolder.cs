using System;
using System.Diagnostics;
using System.IO;

namespace ProlexNetSetup.Class.Common
{
    public class BackupFolder
    {
        public static void Backup(string servicePath, string folderToBackup)
        {
            try
            {
                var backupFolder = Path.Combine(servicePath, "Backup");
                Directory.CreateDirectory(backupFolder);

                var timeStamp = DateTime.Now.ToString("dd.MM.yyyy.HH.mm");
                var folderName = Path.GetFileNameWithoutExtension(folderToBackup);
                var backupDateTime = Path.Combine(backupFolder, $"{folderName} {timeStamp}");
                Directory.Move(folderToBackup, backupDateTime);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("BackupFolder:Backup:" + ex.Message);
            }
        }
    }
}