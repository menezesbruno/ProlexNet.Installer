using System;
using System.IO;

namespace ProlexNetSetup.Class.Common
{
    public class RequiredFonts
    {
        public static void Magneto()
        {
            try
            {
                var font = @"Resources\MAGNETOB.ttf";
                var fontName = Path.GetFileName(font);
                var fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), fontName);
                if (File.Exists(fontPath))
                {
                    File.Copy(font, fontPath, true);

                    Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts");
                    key.SetValue("Magneto Bold (TrueType)", fontName);
                    key.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Um erro ocorreu durante a instalação da fonte 'Magneto'.",
                    ex);
            }

            return;
        }
    }
}
