using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProlexNetSetupV2.Services
{
    internal class DetectOS
    {
        public static bool Windows()
        {
            var majorVersion = Environment.Version.Major;
            var minorVersion = Environment.Version.Minor;

            if (majorVersion >= 6 && minorVersion >= 1)
                return true;
            else if (majorVersion == 10)
                return true;
            else
                return false;
        }

        public static string Is64Bits()
        {
            if (Environment.Is64BitOperatingSystem)
                return "x64";
            else
                return "x86";
        }
    }
}