using System;
using System.IO;
using System.Windows;
using Forms = System.Windows.Forms;
using System.Windows.Controls;
using System.Net;
using Microsoft.Win32;
using ProlexNetSetup.Class.Download;
using ProlexNetSetup.Class.Install;
using ProlexNetSetup.Class.Common;

namespace ProlexNetSetup
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        public int CountPages = 0;
        public string ServicePath { get; set; }
        public string InstallationPath = @"C:\Automatiza";

        // GUID usado para criar entrada de registro de desinstalação do Windows.
        // Deve ser ÚNICO por aplicativo e deve também ser IMUTÁVEL durante toda a vida do aplicativo.
        // Pode ser criado em: https://www.guidgenerator.com/online-guid-generator.aspx
        // Opções a serem selecionadas: Braces + Hyphens
        public static string ClientApplicationGuid = "{ee152ba9-9db3-47c5-ba10-b6d08cdb74f4}"; // Não alterar!
        public static string ServerApplicationGuid = "{5f64652c-65c0-4e53-9f55-cd25f0afa39c}"; // Não alterar!

        // Caminho padrão do Windows onde ficam salvos os registros de instalação
        public static string WindowsUninstallPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

        public MainWindow()
        {
            ServicePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Automatiza", "Instalador");
            Directory.CreateDirectory(ServicePath);

            // Comandos de desinstalação
            Uninstaller.ProlexNetClient(ClientApplicationGuid, WindowsUninstallPath);
            Uninstaller.ProlexNetServer(ServerApplicationGuid, WindowsUninstallPath);

            InitializeComponent();
            InstallationPathField.Content = InstallationPath;
        }

        private void ChangePages(int count)
        {
            // Navega pelas páginas do instalador
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

                    if (checkbox_ProlexNetServer.IsChecked == true)
                    {
                        CheckBoxFirebirdSilentInstallation.IsEnabled = true;
                        CheckBoxLINQPadInstallation.IsEnabled = true;
                        ServerNameOverNetwork.Text = Environment.GetEnvironmentVariable("COMPUTERNAME");
                    }
                    if (checkbox_ProlexNetServer.IsChecked == false)
                    {
                        CheckBoxFirebirdSilentInstallation.IsEnabled = false;
                        CheckBoxLINQPadInstallation.IsEnabled = false;
                    }

                    ButtonBack.Visibility = Visibility.Visible;
                    ButtonAdvance.Content = "Próximo >";
                    ButtonAdvance.Click += BeforeInstallation;
                    ButtonAdvance.Click -= StartInstallationAsync;
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

        public void UpdateDownloadProgress(DownloadProgressChangedEventArgs args)
        {
            // Progress bar dos downloads
            Dispatcher.Invoke(() =>
            {
                ProgressBar.Maximum = args.TotalBytesToReceive;
                ProgressBar.Value = args.BytesReceived;
                ProgressBarValue.Content = args.ProgressPercentage + "%";

                decimal total = args.TotalBytesToReceive;
                decimal received = args.BytesReceived;
                ProgressBarSpeed.Content = $"{(received / 1048576):n3} mb / {(total / 1048576):n3} mb";
            });
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            // Ação do botão 'voltar'
            CountPages = CountPages - 1;
            if (CountPages < 0)
                CountPages = 0;
            ChangePages(CountPages);
        }

        private void ButtonAdvance_Click(object sender, RoutedEventArgs e)
        {
            // Ação do botão 'avançar'
            CountPages = CountPages + 1;
            if (CountPages > 5)
                CountPages = 5;
            ChangePages(CountPages);
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            // Ação do botão 'cancelar'
            if (CountPages < 5)
            {
                var close = MessageBox.Show("Você tem certeza que deseja sair do Instalador do ProlexNet?", "Instalação do ProlexNet", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (close == MessageBoxResult.Yes)
                    Environment.Exit(1);
            }
            Environment.Exit(1);
        }

        private void ButtonChangeInstallPath_Click(object sender, RoutedEventArgs e)
        {
            Forms.FolderBrowserDialog folderBrowserDialog = new Forms.FolderBrowserDialog();
            {
                if (folderBrowserDialog.ShowDialog() == Forms.DialogResult.OK)
                {
                    InstallationPath = Path.GetFullPath(folderBrowserDialog.SelectedPath);
                    InstallationPathField.Content = InstallationPath;
                }
            }
        }

        private void BeforeInstallation(object sender, RoutedEventArgs e)
        {
            var systemVersion = DetectOSVersion.Is64Bits();

            ComponentsToBeInstalled.Text = "";

            if (InstallationDetect.VCRedist_X86())
                ComponentsToBeInstalled.Text += $"Microsoft Visual C++ 2013 x86" + Environment.NewLine;

            if (Environment.Is64BitOperatingSystem)
            {
                if (InstallationDetect.VCRedist_X64())
                    ComponentsToBeInstalled.Text += $"Microsoft Visual C++ 2013 x64" + Environment.NewLine;
            }

            if (InstallationDetect.DotNet())
                ComponentsToBeInstalled.Text += $"Microsoft .NET Framework 4.6" + Environment.NewLine;

            if (checkbox_ProlexNetServer.IsChecked == true)
            {
                ComponentsToBeInstalled.Text += "Serviços de Informações da Internet - IIS" + Environment.NewLine;
                ComponentsToBeInstalled.Text += $"Firebird 3 {systemVersion}" + Environment.NewLine;
                if(CheckBoxLINQPadInstallation.IsChecked == true)
                    ComponentsToBeInstalled.Text += "LINQPad 5" + Environment.NewLine;
                ComponentsToBeInstalled.Text += "ProlexNet Server" + Environment.NewLine;
            }

            if (checkbox_ProlexNetClient.IsChecked == true)
                ComponentsToBeInstalled.Text += "ProlexNet Client";
        }

        private async void StartInstallationAsync(object sender, RoutedEventArgs e)
        {
            var serverName = ServerNameOverNetwork.Text;
            var serverPort = ServerPortOverNetrork.Text;
            var silentInstallation = CheckBoxFirebirdSilentInstallation.IsChecked == true;

            var systemVersion = DetectOSVersion.Is64Bits();

            if (!Directory.Exists(InstallationPath))
                Directory.CreateDirectory(InstallationPath);

            // Download e instalação acontece aqui.
            try
            {
                // Chama a classe que verifica se há necessidade de fazer o download e instalar o VC++ Redist 2013 x86.
                if (InstallationDetect.VCRedist_X86())
                {
                    try
                    {
                        InstallationStatus.Text += $"Microsoft Visual C++ 2013 x86... ";
                        await Download.VisualCAsync(ServicePath, "x86");
                        InstallationStatus.Text += "OK" + Environment.NewLine;
                    }
                    catch (Exception ex)
                    {
                        InstallationStatus.Text += "Erro" + Environment.NewLine;
                        MessageBox.Show(ex.Message, "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                // Chama a classe que verifica se há necessidade de fazer o download e instalar o VC++ Redist 2013 x64.
                if (Environment.Is64BitOperatingSystem)
                {
                    if (InstallationDetect.VCRedist_X64())
                    {
                        try
                        {
                            InstallationStatus.Text += $"Microsoft Visual C++ 2013 x64... ";
                            await Download.VisualCAsync(ServicePath, "x64");
                            InstallationStatus.Text += "OK" + Environment.NewLine;

                        }
                        catch (Exception ex)
                        {
                            InstallationStatus.Text += "Erro" + Environment.NewLine;
                            MessageBox.Show(ex.Message, "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }

                // Chama a classe que verifica se há necessidade de fazer o download e instalar o DotNet 4.6
                if (InstallationDetect.DotNet())
                {
                    try
                    {
                        InstallationStatus.Text += $"Microsoft .NET Framework 4.6... ";
                        await Download.DotNetAsync(ServicePath);
                        InstallationStatus.Text += "OK" + Environment.NewLine;
                    }
                    catch (Exception ex)
                    {
                        InstallationStatus.Text += "Erro" + Environment.NewLine;
                        MessageBox.Show(ex.Message, "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                if (checkbox_ProlexNetServer.IsChecked == true)
                {
                    // Chama a classe que faz o download e instala o IIS
                    try
                    {
                        InstallationStatus.Text += "Serviços de Informações da Internet - IIS... ";
                        await Installer.IISAsync(ServicePath);
                        InstallationStatus.Text += "OK" + Environment.NewLine;
                    }
                    catch (Exception ex)
                    {
                        InstallationStatus.Text += "Erro" + Environment.NewLine;
                        MessageBox.Show(ex.Message, "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    // Chama a classe que faz o download e instala o Firebird 3 baseado na versão do Sistema Operacional.
                    try
                    {
                        if (InstallationDetect.Firebird())
                        {
                            var fbInstall = MessageBox.Show("O Firebird já está instalado no computador. Deseja desinstalar a versão atual?", "Aviso!", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if (fbInstall == MessageBoxResult.Yes)
                                Uninstaller.Firebird();
                        }

                        InstallationStatus.Text += $"Firebird 3 {systemVersion}... ";
                        await Download.FirebirdAsync(ServicePath, silentInstallation, InstallationPath);
                        InstallationStatus.Text += "OK" + Environment.NewLine;
                    }
                    catch (Exception ex)
                    {
                        InstallationStatus.Text += "Erro" + Environment.NewLine;
                        MessageBox.Show(ex.Message, "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    // Chama a classe que faz o download e instala o LINQPad 5.
                    if (CheckBoxLINQPadInstallation.IsChecked == true)
                    {
                        try
                        {
                            InstallationStatus.Text += "LINQPad 5... ";
                            await Download.LINQPad5Async(ServicePath);
                            InstallationStatus.Text += "OK" + Environment.NewLine;
                        }
                        catch (Exception ex)
                        {
                            InstallationStatus.Text += "Erro" + Environment.NewLine;
                            MessageBox.Show(ex.Message, "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }

                    // Chama a classe que faz o download e instala o ProlexNet Server.
                    try
                    {
                        InstallationStatus.Text += "ProlexNet Server... ";
                        await Download.ProlexNetServerAsync(ServicePath, InstallationPath, ServerApplicationGuid, WindowsUninstallPath);
                        await Download.ProlexNetUpdaterAsync(ServicePath, InstallationPath);
                        await ProlexNetConfiguration.Server(InstallationPath, serverName, serverPort);
                        InstallationStatus.Text += "OK" + Environment.NewLine;
                    }
                    catch (Exception ex)
                    {
                        InstallationStatus.Text += "Erro" + Environment.NewLine;
                        MessageBox.Show(ex.Message, "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    // Chama a classe que configura o IIS para o uso.
                    try
                    {
                        InstallationStatus.Text += "Configurando o IIS... ";
                        await IISConfiguration.ProlexNetSettings(InstallationPath, serverPort);
                        InstallationStatus.Text += "OK" + Environment.NewLine;
                    }
                    catch (Exception ex)
                    {
                        InstallationStatus.Text += "Erro" + Environment.NewLine;
                        MessageBox.Show(ex.Message, "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                if (checkbox_ProlexNetClient.IsChecked == true)
                {
                    // Chama a classe que faz o download e instala o ProlexNet Client.
                    try
                    {
                        InstallationStatus.Text += "ProlexNet Client... ";
                        await Download.ProlexNetClientAsync(ServicePath, InstallationPath, ClientApplicationGuid, WindowsUninstallPath);
                        await ProlexNetConfiguration.Client(InstallationPath, serverName, serverPort);
                        InstallationStatus.Text += "OK" + Environment.NewLine;
                    }
                    catch (Exception ex)
                    {
                        InstallationStatus.Text += "Erro" + Environment.NewLine;
                        MessageBox.Show(ex.Message, "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch
            {
                LabelInstallationResult.Content = "Houve um ou mais erros durante a instalação. Reinicie o computador e tente instalar novamente.";
            }

            ButtonAdvance_Click(null, null);
        }
    }
}
