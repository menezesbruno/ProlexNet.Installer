using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProlexNetSetupV2.Library
{
    internal class DetectWindows
    {
        public static void Version()
        {
            var majorVersion = Environment.OSVersion.Version.Major;
            var minorVersion = Environment.OSVersion.Version.Minor;
            var supported = false;

            if (majorVersion >= 6 && minorVersion >= 1)
                supported = true;
            else if (majorVersion == 10)
                supported = true;

            if (!supported)
            {
                MessageBox.Show("Versão do Windows não suportada! A instalação exige no mínimo Windows 7 com SP1 instalado.", "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            }
        }

        public static string Bits()
        {
            var bits = "x86";
            if (Environment.Is64BitOperatingSystem)
                bits = "x64";

            return bits;
        }
    }
}