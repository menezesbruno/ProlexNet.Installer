﻿using System;
using System.Windows;

namespace ProlexNetSetup.Library
{
    internal class DetectWindows
    {
        public static void Supported()
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
                Application.Current.Shutdown();
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