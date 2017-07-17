using System;
using System.Diagnostics;

namespace ProlexNetSetup.Class.OSInfo
{
    public class Shutdown
    {
        public static void Reboot()
        {
            // Reinicializa o computador e explica o motivo ao log de eventos do Windows: Instalação de aplicativo.
            try
            {
                Process.Start("ShutDown", "/r /t 0 /f /d p:4:2");
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Shutdown:Reboot:" + ex.Message);
            }
        }
    }
}
