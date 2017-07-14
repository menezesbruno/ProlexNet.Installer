using System;
using System.Windows;
using ProlexNetSetup.Class.Download;
using ProlexNetSetup.Class.System;

namespace ProlexNetSetup
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            if (DetectOSVersion.Windows())
            {
                MessageBox.Show("Versão do Windows não suportada! A instalação exige no mínimo Windows 7 com SP1 instalado.", "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            }

            base.OnStartup(e);
            await DownloadParameters.ApplicationListAsync();
        }
    }
}
