using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProlexNetSetup.Library.Startup
{
    public class CreateServicePath
    {
        public static string ServicePath { get; set; }

        public static void Run()
        {
            ServicePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Automatiza", "Instalador");
            Directory.CreateDirectory(ServicePath);
        }
    }
}
