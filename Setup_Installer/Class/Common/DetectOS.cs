using System;

namespace ProlexNetSetup.Class.Common
{
    public class DetectOS
    {
        public static bool Windows()
        {
            var majorVersion = Environment.OSVersion.Version.Major;
            var minorVersion = Environment.OSVersion.Version.Minor;

            if (majorVersion >= 6 && minorVersion >=1 )
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
