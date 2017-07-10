using System.Diagnostics;

namespace Setup_Installer.Class.System
{
    public class Shutdown
    {
        public static void Reboot()
        {
            Process.Start("ShutDown", "/r /t 0 /f /d p:4:2");
        }
    }
}
