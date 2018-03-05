using Microsoft.Win32;
using ProlexNetSetup.Class;
using ProlexNetSetup.Class.Common;
using ProlexNetSetup.Class.Download;
using ProlexNetSetup.Class.Install;
using SegmentDownloader.Core;
using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using Forms = System.Windows.Forms;

namespace ProlexNetSetup
{
    public partial class MainWindow : Window
    {
        public string InstallationPath = @"C:\Automatiza";
        public string ServicePath { get; set; }
        public int CountPages = 0;

        public MainWindow()
        {
            ServicePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Automatiza", "Instalador");
            Directory.CreateDirectory(ServicePath);

            // Comandos de desinstalação
            Uninstaller.ProlexNetClient();
            Uninstaller.ProlexNetServer();

            InitializeComponent();
            InstallationPathField.Content = InstallationPath;
        }

        #region Navegação

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
                        ProlexNetServer_settings.IsEnabled = true;
                        CheckBoxDatabaseDeploy.IsEnabled = true;
                        CheckBoxFirebirdInstallation.IsEnabled = true;
                        CheckBoxIBExpertInstallation.IsEnabled = true;
                        CheckBoxLINQPadInstallation.IsEnabled = true;
                        ServerNameOverNetwork.Text = Environment.GetEnvironmentVariable("COMPUTERNAME");
                    }
                    else
                    {
                        ProlexNetServer_settings.IsEnabled = false;
                        CheckBoxDatabaseDeploy.IsEnabled = false;
                        CheckBoxFirebirdInstallation.IsEnabled = false;
                        CheckBoxIBExpertInstallation.IsEnabled = false;
                        CheckBoxLINQPadInstallation.IsEnabled = false;
                        ServerNameOverNetwork.Text = "localhost";
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

        #region Botões de Navegação

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            // Ação do botão voltar
            CountPages = CountPages - 1;
            if (CountPages < 0)
                CountPages = 0;
            ChangePages(CountPages);
        }

        private void ButtonAdvance_Click(object sender, RoutedEventArgs e)
        {
            // Ação do botão avançar
            CountPages = CountPages + 1;
            if (CountPages > 5)
                CountPages = 5;
            ChangePages(CountPages);
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            // Ação do botão cancelar
            if (CountPages < 5)
            {
                var close = MessageBox.Show("Você tem certeza que deseja sair do Instalador do ProlexNet?", "Instalação do ProlexNet", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (close == MessageBoxResult.Yes)
                    Environment.Exit(1);
            }
            Environment.Exit(1);
        }

        #endregion Botões de Navegação

        #endregion Navegação

        #region Progressbar

        public void UpdateDownloadProgress(DownloadProgressChangedEventArgs args)
        {
            // Progressbar dos downloads
            Dispatcher.Invoke(() =>
            {
                ProgressBar.Maximum = args.TotalBytesToReceive;
                ProgressBar.Value = args.BytesReceived;
                ProgressBarValue.Content = args.ProgressPercentage + "%";

                decimal total = args.TotalBytesToReceive;
                decimal received = args.BytesReceived;
                ProgressBarSpeed.Content = $"{(received / 1048576):n3} MB / {(total / 1048576):n3} MB";
            });
        }

        #endregion Progressbar

        #region Local de Instalação

        private void ButtonChangeInstallPath_Click(object sender, RoutedEventArgs e)
        {
            // Ação para alterar a pasta de instalação
            Forms.FolderBrowserDialog folderBrowserDialog = new Forms.FolderBrowserDialog();
            {
                if (folderBrowserDialog.ShowDialog() == Forms.DialogResult.OK)
                {
                    InstallationPath = Path.GetFullPath(folderBrowserDialog.SelectedPath);
                    InstallationPathField.Content = InstallationPath;
                }
            }
        }

        #endregion Local de Instalação

        #region Checklist da Instalação

        private void BeforeInstallation(object sender, RoutedEventArgs e)
        {
            var systemVersion = DetectOS.Is64Bits();
            ComponentsToBeInstalled.Text = "";

            #region VCRedist

            if (Requirements.VCRedist_X86())
                ComponentsToBeInstalled.Text += "Microsoft Visual C++ 2013 x86" + Environment.NewLine;

            if (Environment.Is64BitOperatingSystem)
            {
                if (Requirements.VCRedist_X64())
                    ComponentsToBeInstalled.Text += "Microsoft Visual C++ 2013 x64" + Environment.NewLine;
            }

            #endregion VCRedist

            #region DotNet

            if (Requirements.DotNet())
                ComponentsToBeInstalled.Text += "Microsoft .NET Framework 4.6" + Environment.NewLine;

            #endregion DotNet

            #region ProlexNet Server

            if (checkbox_ProlexNetServer.IsChecked == true)
            {
                ComponentsToBeInstalled.Text += "Serviços de Informações da Internet - IIS" + Environment.NewLine;
                if (CheckBoxFirebirdInstallation.IsChecked == true)
                    ComponentsToBeInstalled.Text += $"Firebird 3 {systemVersion}" + Environment.NewLine;
                if (CheckBoxDatabaseDeploy.IsChecked == true)
                    ComponentsToBeInstalled.Text += $"Banco de dados" + Environment.NewLine;
                if (CheckBoxIBExpertInstallation.IsChecked == true)
                    ComponentsToBeInstalled.Text += $"IBExpert" + Environment.NewLine;
                if (CheckBoxLINQPadInstallation.IsChecked == true)
                    ComponentsToBeInstalled.Text += "LINQPad 5" + Environment.NewLine;
                ComponentsToBeInstalled.Text += "ProlexNet Server" + Environment.NewLine;
            }

            #endregion ProlexNet Server

            #region ProlexNet Client

            if (checkbox_ProlexNetClient.IsChecked == true)
                ComponentsToBeInstalled.Text += "ProlexNet Client";

            #endregion ProlexNet Client
        }

        #endregion Checklist da Instalação

        #region Instalação

        private async void StartInstallationAsync(object sender, RoutedEventArgs e)
        {
            var serverName = ServerNameOverNetwork.Text;
            var serverPort = ServerPortOverNetrork.Text;
            var systemVersion = DetectOS.Is64Bits();

            if (!Directory.Exists(InstallationPath))
                Directory.CreateDirectory(InstallationPath);

            // Download e instalação acontece aqui.
            try
            {
                #region VCRedist

                // Verifica se há necessidade de fazer o download e instalar o VC++ Redist 2013 x86.
                if (Requirements.VCRedist_X86())
                {
                    try
                    {
                        InstallationStatus.Text += "Microsoft Visual C++ 2013 x86... ";
                        await Download.VisualCAsync(ServicePath, "x86");
                        InstallationStatus.Text += "OK" + Environment.NewLine;
                    }
                    catch (Exception ex)
                    {
                        InstallationStatus.Text += "Erro" + Environment.NewLine;
                        MessageBox.Show(ex.Message, "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                // Verifica se há necessidade de fazer o download e instalar o VC++ Redist 2013 x64.
                if (Environment.Is64BitOperatingSystem)
                {
                    if (Requirements.VCRedist_X64())
                    {
                        try
                        {
                            InstallationStatus.Text += "Microsoft Visual C++ 2013 x64... ";
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

                #endregion VCRedist

                #region DotNet

                // Verifica se há necessidade de fazer o download e instalar o DotNet 4.6
                if (Requirements.DotNet())
                {
                    try
                    {
                        InstallationStatus.Text += "Microsoft .NET Framework 4.6... ";
                        await Download.DotNetAsync(ServicePath);
                        InstallationStatus.Text += "OK" + Environment.NewLine;
                    }
                    catch (Exception ex)
                    {
                        InstallationStatus.Text += "Erro" + Environment.NewLine;
                        MessageBox.Show(ex.Message, "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                #endregion DotNet

                #region ProlexNet Server

                if (checkbox_ProlexNetServer.IsChecked == true)
                {
                    // Faz o download e instala o IIS
                    try
                    {
                        InstallationStatus.Text += "Serviços de Informações da Internet - IIS... ";
                        Installer.IISAvailablePackages(ServicePath);
                        InstallationStatus.Text += "OK" + Environment.NewLine;
                    }
                    catch (Exception ex)
                    {
                        InstallationStatus.Text += "Erro" + Environment.NewLine;
                        MessageBox.Show(ex.Message, "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    // Faz o download e instala o Firebird 3 baseado na versão do Sistema Operacional.
                    if (CheckBoxFirebirdInstallation.IsChecked == true)
                    {
                        try
                        {
                            if (Requirements.Firebird())
                            {
                                var fbInstall = MessageBox.Show("O Firebird já está instalado no computador. Deseja desinstalar a versão atual?", "Aviso!", MessageBoxButton.YesNo, MessageBoxImage.Question);
                                if (fbInstall == MessageBoxResult.Yes)
                                    Uninstaller.Firebird();
                            }
                            InstallationStatus.Text += $"Firebird 3 {systemVersion}... ";
                            await Download.FirebirdAsync(ServicePath, InstallationPath);
                            InstallationStatus.Text += "OK" + Environment.NewLine;
                        }
                        catch (Exception ex)
                        {
                            InstallationStatus.Text += "Erro" + Environment.NewLine;
                            MessageBox.Show(ex.Message, "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }

                    // Chama a classe que faz o download e extrai o Banco de dados do ProlexNet.
                    if (CheckBoxDatabaseDeploy.IsChecked == true)
                    {
                        try
                        {
                            InstallationStatus.Text += "Banco de dados... ";
                            await Download.ProlexNetDatabaseAsync(ServicePath, InstallationPath);
                            InstallationStatus.Text += "OK" + Environment.NewLine;
                        }
                        catch (Exception ex)
                        {
                            InstallationStatus.Text += "Erro" + Environment.NewLine;
                            MessageBox.Show(ex.Message, "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }


                    // Chama a classe que faz o download e instala o IBExpert.
                    if (CheckBoxIBExpertInstallation.IsChecked == true)
                    {
                        try
                        {
                            InstallationStatus.Text += "IBExpert... ";
                            await Download.IBExpertSetupAsync(ServicePath);
                            await Download.IBExpertAsync(ServicePath);
                            InstallationStatus.Text += "OK" + Environment.NewLine;
                        }
                        catch (Exception ex)
                        {
                            InstallationStatus.Text += "Erro" + Environment.NewLine;
                            MessageBox.Show(ex.Message, "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
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
                        await Download.ProlexNetServerAsync(ServicePath, InstallationPath);
                        await Download.ProlexNetUpdaterAsync(ServicePath, InstallationPath);
                        ConfigProlexNet.Server(InstallationPath, serverName, serverPort);
                        ConfigProlexNet.Updater(InstallationPath);
                        ConfigFirewall.AddRules(serverPort);
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
                        ConfigIIS.ProlexNetSettings(InstallationPath, serverPort);
                        InstallationStatus.Text += "OK" + Environment.NewLine;
                    }
                    catch (Exception ex)
                    {
                        InstallationStatus.Text += "Erro" + Environment.NewLine;
                        MessageBox.Show(ex.Message, "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                #endregion ProlexNet Server

                #region ProlexNet Client

                if (checkbox_ProlexNetClient.IsChecked == true)
                {
                    // Chama a classe que faz o download e instala o ProlexNet Client.
                    try
                    {
                        InstallationStatus.Text += "ProlexNet Client... ";
                        await Download.ProlexNetClientAsync(ServicePath, InstallationPath);
                        ConfigProlexNet.Client(InstallationPath, serverName, serverPort);
                        InstallationStatus.Text += "OK" + Environment.NewLine;
                    }
                    catch (Exception ex)
                    {
                        InstallationStatus.Text += "Erro" + Environment.NewLine;
                        MessageBox.Show(ex.Message, "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                #endregion ProlexNet Client
            }
            catch
            {
                LabelInstallationResult.Content = "Houve um ou mais erros durante a instalação. Reinicie o computador e tente instalar novamente.";
            }

            ButtonAdvance_Click(null, null);
        }

        #endregion Instalação
    }
}