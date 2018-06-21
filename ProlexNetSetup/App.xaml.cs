using ProlexNetSetup.Library;
using System.Windows;

namespace ProlexNetSetup
{
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            DetectWindows.Supported();

            base.OnStartup(e);

            Uninstall.Run();
            await DownloadParameters.ApplicationListAsync();

            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }
    }
}