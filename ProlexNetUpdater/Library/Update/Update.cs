namespace ProlexNetUpdater.Library.Update
{
    public class Update
    {
        public static void Run()
        {
            //Vasculha registro pela pasta do ProlexNet             
            Registry.LoadPath();

            //Download da lista 
            DownloadParameters.LoadApplicationList();

            //Pára o IIS
            IIS.Stop();

            //Download e extração do ProlexNet
            Download.ProlexNet();

            //Inicia o IIS
            IIS.Start();
        }
    }
}