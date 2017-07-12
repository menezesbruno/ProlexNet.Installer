using System;
using System.Diagnostics;
using System.IO;

namespace ProlexNetSetup.Class.Download
{
    public class FolderBackup
    {
        public static void Backup(string servicePath, string folderToBackup)
        {
            try
            {
                var backupFolder = Path.Combine(servicePath, "Backup");
                Directory.CreateDirectory(backupFolder);

                var timeStamp = DateTime.Now.ToString("yyyy.MM.dd.HH.mm");
                var folderName = Path.GetFileNameWithoutExtension(folderToBackup);
                var backupDateTime = Path.Combine(backupFolder, $"{folderName} {timeStamp}");
                Directory.Move(folderToBackup, backupDateTime);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("FolderBackup:Backup:" + ex.Message);
            }

            return;
        }
    }
}
