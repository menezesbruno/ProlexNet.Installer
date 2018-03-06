using ProlexNetSetupV2.Library;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ProlexNetSetupV2
{
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            DetectWindows.Version();

            base.OnStartup(e);

            Uninstall.ProlexNetClient();
            Uninstall.ProlexNetServer();
            await DownloadParameters.ApplicationListAsync();

            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }
    }
}