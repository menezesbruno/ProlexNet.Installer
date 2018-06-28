using System.IO;

namespace ProlexNetSetup.Library.Startup
{
    public class CreateServicePath
    {
        public static string ServicePath { get; set; }

        public static void Run(string installPath)
        {
            var servicePath = Path.Combine(installPath, "Instalador");
            Directory.CreateDirectory(servicePath);
            ServicePath = servicePath;
        }
    }
}
