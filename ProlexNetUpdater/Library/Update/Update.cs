using ProlexNetUpdater.Library.Common;

namespace ProlexNetUpdater.Library.Update
{
    public class Update
    {
        public static void Run()
        {
            //Pára o IIS
            IIS.Stop();

            //Download e extração do ProlexNet
            Download.ProlexNetAsync();

            //Inicia o IIS
            IIS.Start();
        }
    }
}