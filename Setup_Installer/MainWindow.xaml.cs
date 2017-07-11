using System;
using System.IO;
using System.Windows;
using Forms = System.Windows.Forms;
using System.Windows.Controls;
using Setup_Installer.Class.Download;

namespace Setup_Installer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int CountPages = 0;
        public string ServicePath { get; set; }
        public string InstallationPath = @"C:\Automatiza";

        public MainWindow()

        {
            InitializeComponent();

            ServicePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Automatiza", "Instalador");
            Directory.CreateDirectory(ServicePath);
            InstallationPathField.Content = InstallationPath;
        }

        private void ChangePages(int count)
        {
            switch (count)
            {
                case 0:
                    Page1.IsSelected = true;
                    Page2.IsSelected = false;
                    Page3.IsSelected = false;
                    Page4.IsSelected = false;
                    Page5.IsSelected = false;
                    Page6.IsSelected = false;

                    ButtonBack.Visibility = Visibility.Hidden;
                    break;

                case 1:
                    Page1.IsSelected = false;
                    Page2.IsSelected = true;
                    Page3.IsSelected = false;
                    Page4.IsSelected = false;
                    Page5.IsSelected = false;
                    Page6.IsSelected = false;

                    ButtonBack.Visibility = Visibility.Visible;
                    break;

                case 2:
                    Page1.IsSelected = false;
                    Page2.IsSelected = false;
                    Page3.IsSelected = true;
                    Page4.IsSelected = false;
                    Page5.IsSelected = false;
                    Page6.IsSelected = false;

                    ButtonBack.Visibility = Visibility.Visible;
                    ButtonAdvance.Content = "Próximo >";
                    ButtonAdvance.Click += BeforeInstallation;
                    break;

                case 3:
                    Page1.IsSelected = false;
                    Page2.IsSelected = false;
                    Page3.IsSelected = false;
                    Page4.IsSelected = true;
                    Page5.IsSelected = false;
                    Page6.IsSelected = false;

                    ButtonBack.Visibility = Visibility.Visible;
                    ButtonAdvance.Content = "Instalar >";
                    ButtonAdvance.Click -= BeforeInstallation;
                    ButtonAdvance.Click += StartInstallationAsync;
                    break;

                case 4:
                    Page1.IsSelected = false;
                    Page2.IsSelected = false;
                    Page3.IsSelected = false;
                    Page4.IsSelected = false;
                    Page5.IsSelected = true;
                    Page6.IsSelected = false;

                    ButtonBack.Visibility = Visibility.Hidden;
                    ButtonCancel.Visibility = Visibility.Visible;
                    ButtonAdvance.Visibility = Visibility.Hidden;
                    ButtonAdvance.Content = "Próximo >";
                    ButtonAdvance.Click -= StartInstallationAsync;
                    break;

                case 5:
                    Page1.IsSelected = false;
                    Page2.IsSelected = false;
                    Page3.IsSelected = false;
                    Page4.IsSelected = false;
                    Page5.IsSelected = false;
                    Page6.IsSelected = true;

                    ButtonBack.Visibility = Visibility.Hidden;
                    ButtonAdvance.Visibility = Visibility.Hidden;
                    ButtonCancel.Visibility = Visibility.Visible;
                    ButtonCancel.Content = "Finalizar";
                    break;

                default:
                    break;
            }
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            CountPages = CountPages - 1;
            if (CountPages < 0)
                CountPages = 0;
            ChangePages(CountPages);
        }

        private void ButtonAdvance_Click(object sender, RoutedEventArgs e)
        {
            CountPages = CountPages + 1;
            if (CountPages > 5)
                CountPages = 5;
            ChangePages(CountPages);
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            if (CountPages < 5)
            {
                var close = MessageBox.Show("Você tem certeza que quer sair do Instalador do ProlexNet?", "Instalação do ProlexNet", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (close == MessageBoxResult.Yes)
                    Environment.Exit(1);
            }
            else
                Environment.Exit(1);
        }

        private void ButtonChangeInstallPath_Click(object sender, RoutedEventArgs e)
        {
            Forms.FolderBrowserDialog folderBrowserDialog = new Forms.FolderBrowserDialog();
            {
                if (folderBrowserDialog.ShowDialog() == Forms.DialogResult.OK)
                {
                    InstallationPath = System.IO.Path.GetFullPath(folderBrowserDialog.SelectedPath);
                    InstallationPathField.Content = InstallationPath;
                }
            }
        }

        private void BeforeInstallation(object sender, RoutedEventArgs e)
        {
            var systemVersion = "x86";
            if (Environment.Is64BitOperatingSystem)
                systemVersion = "x64";

            var dotNetVersion = "4.6";
            if (CheckBoxDotNet47.IsChecked == true)
                dotNetVersion = "4.7";

            ComponentsToBeInstalled.Text = "";
            ComponentsToBeInstalled.Text += $"Firebird 3 {systemVersion}" + Environment.NewLine;
            ComponentsToBeInstalled.Text += $"Microsoft .NET Framework {dotNetVersion}" + Environment.NewLine;
            ComponentsToBeInstalled.Text += $"Microsoft Visual C++ 2013 {systemVersion}" + Environment.NewLine;
            ComponentsToBeInstalled.Text += "Serviços de Informações da Internet - IIS" + Environment.NewLine;
            ComponentsToBeInstalled.Text += "" + Environment.NewLine;
            if (checkbox_ProlexNetServer.IsChecked == true)
                ComponentsToBeInstalled.Text += "ProlexNet Server" + Environment.NewLine;
            if (checkbox_ProlexNetClient.IsChecked == true)
                ComponentsToBeInstalled.Text += "ProlexNet Client";

        }

        private async void StartInstallationAsync(object sender, RoutedEventArgs e)
        {
            ProgressBar.IsIndeterminate = true;

            var silentInstallation = CheckBoxFirebirdSilentInstallation.IsChecked == true;

            var systemVersion = "x86";
            if (Environment.Is64BitOperatingSystem)
                systemVersion = "x64";

            var dotNetVersion = "4.6";
            if (CheckBoxDotNet47.IsChecked == true)
                dotNetVersion = "4.7";

            if (!Directory.Exists(InstallationPath))
                Directory.CreateDirectory(InstallationPath);

            InstallationStatus.Text += "Instalando o Firebird 3... ";
            await Download.FirebirdAsync(ServicePath, silentInstallation);
            InstallationStatus.Text += "OK" + Environment.NewLine;

            InstallationStatus.Text += $"Instalando o Microsoft .NET Framework {dotNetVersion}... ";
            await Download.DotNetAsync(ServicePath, dotNetVersion);
            InstallationStatus.Text += "OK" + Environment.NewLine;

            InstallationStatus.Text += $"Instalando o Microsoft Visual C++ 2013 {systemVersion}... ";
            await Download.VisualCAsync(ServicePath);
            InstallationStatus.Text += "OK" + Environment.NewLine;

            InstallationStatus.Text += "Instalando o Serviços de Informações da Internet - IIS... ";
            await Installer.IISAsync(ServicePath);
            InstallationStatus.Text += "OK" + Environment.NewLine;

            if (checkbox_ProlexNetServer.IsChecked == true)
            {
                try
                {
                    InstallationStatus.Text += "Instalando o ProlexNet Server... ";
                    await Download.ProlexNetHostAsync(ServicePath, InstallationPath);
                    InstallationStatus.Text += "OK" + Environment.NewLine;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            if (checkbox_ProlexNetClient.IsChecked == true)
            {
                try
                {
                    InstallationStatus.Text += "Instalando o ProlexNet Client... ";
                    await Download.ProlexNetClientAsync(ServicePath, InstallationPath);
                    InstallationStatus.Text += "OK" + Environment.NewLine;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            Class.Common.IISConfiguration.ProlexSettings(InstallationPath);

            ButtonAdvance_Click(null, null);
        }
    }
}
