using System.Windows;
using ProlexNetSetup.Library;
using ProlexNetSetup.Library.Startup;

namespace ProlexNetSetup
{
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            DetectWindows.Supported();

            base.OnStartup(e);

            Uninstall.Run();
            await DownloadParameters.AppListAsync();

            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }
    }
}