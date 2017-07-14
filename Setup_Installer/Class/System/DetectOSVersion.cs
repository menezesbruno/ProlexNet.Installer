using System;

namespace ProlexNetSetup.Class.System
{
    public class DetectOSVersion
    {
        public static bool Windows()
        {
            var majorVersion = Environment.Version.Major;
            var minorVersion = Environment.Version.Minor;

            if (majorVersion >= 6 && minorVersion >=1 )
                return true;

            else if (majorVersion == 10)
                return true;

            else
                return false;
        }

        public static bool Is64Bits()
        {
            if (Environment.Is64BitOperatingSystem)
                return true;
            else
                return false;
        }
    }
}
