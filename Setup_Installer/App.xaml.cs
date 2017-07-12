using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ProlexNetSetup
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            if (Environment.OSVersion.Version.Major < 6)
            {
                if (Environment.OSVersion.Version.Minor < 1)
                {
                    MessageBox.Show("Este programa requer no mínimo o Windows 7 SP1", "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
                    MainWindow.Close();
                }
            }

            base.OnStartup(e);
            await Class.Download.DownloadParameters.ApplicationListAsync();
        }
    }
}
