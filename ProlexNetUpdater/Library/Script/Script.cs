using ProlexNetUpdater.Library.Common;
using ProlexNetUpdater.Library.Update;

namespace ProlexNetUpdater.Library.Script
{
    public class Script
    {
        public static void Run()
        {
            //Pára o IIS
            IIS.Stop();

            //Download e extração do ProlexNet
            Download.ProlexNet();

            //Inicia o IIS
            IIS.Start();
        }
    }
}
