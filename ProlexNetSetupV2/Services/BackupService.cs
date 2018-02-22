﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProlexNetSetupV2.Services
{
    internal class BackupService
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
                Trace.WriteLine($"{nameof(BackupService)}:{nameof(Backup)}:{ex.Message}");
            }
        }
    }
}