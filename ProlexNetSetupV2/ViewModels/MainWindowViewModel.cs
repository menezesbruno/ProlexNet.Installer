using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace ProlexNetSetupV2.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Instalação do ProlexNet";

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        #region Buttons
        private string _backContent = "< Voltar";

        public string BackContent
        {
            get { return _backContent; }
            set { SetProperty(ref _backContent, value); }
        }

        private string _backVisibility = "Hidden";

        public string BackVisibility
        {
            get { return _backVisibility; }
            set { SetProperty(ref _backVisibility, value); }
        }

        private string _nextContent = "Próximo >";

        public string NextContent
        {
            get { return _nextContent; }
            set { SetProperty(ref _nextContent, value); }
        }

        private string _nextVisibility = "Visible";

        public string NextVisibility
        {
            get { return _nextVisibility; }
            set { SetProperty(ref _nextVisibility, value); }
        }

        private string _cancelContent = "Cancelar";

        public string CancelContent
        {
            get { return _cancelContent; }
            set { SetProperty(ref _cancelContent, value); }
        }

        private string _cancelVisibility = "Visible";

        public string CancelVisibility
        {
            get { return _cancelVisibility; }
            set { SetProperty(ref _cancelVisibility, value); }
        }
        #endregion

        #region Checkboxes
        private bool _installProlexNetServer = false;

        public bool InstallProlexNetServer
        {
            get { return _installProlexNetServer; }
            set { SetProperty(ref _installProlexNetServer, value); }
        }

        private bool _installProlexNetClient = false;

        public bool InstallProlexNetClient
        {
            get { return _installProlexNetClient; }
            set { SetProperty(ref _installProlexNetClient, value); }
        }

        private bool _installProlexNetDatabase = false;

        public bool InstallProlexNetDatabase
        {
            get { return _installProlexNetDatabase; }
            set { SetProperty(ref _installProlexNetDatabase, value); }
        }

        private bool _installFirebird = false;

        public bool InstallFirbird
        {
            get { return _installFirebird; }
            set { SetProperty(ref _installFirebird, value); }
        }

        private bool _installIBExpert = false;

        public bool InstallIBExpert
        {
            get { return _installIBExpert; }
            set { SetProperty(ref _installIBExpert, value); }
        }

        private bool _installLinqPad = false;

        public bool InstallLinqPad
        {
            get { return _installLinqPad; }
            set { SetProperty(ref _installLinqPad, value); }
        }
        #endregion

        #region Fields
        private string _installationPath = @"C:\Automatiza";

        public string InstallationPath
        {
            get { return _installationPath; }
            set { SetProperty(ref _installationPath, value); }
        }

        private string _prolexNetServerPort = "18520";

        public string ProlexNetServerPort
        {
            get { return _prolexNetServerPort; }
            set { SetProperty(ref _prolexNetServerPort, value); }
        }

        #endregion

        #region Pages
        private int _countPages = 0;

        public int CountPages
        {
            get { return _countPages; }
            set { SetProperty(ref _countPages, value); }
        }

        private bool _page1 = false;

        public bool Page1
        {
            get { return _page1; }
            set { SetProperty(ref _page1, value); }
        }

        private bool _page2 = false;

        public bool Page2
        {
            get { return _page2; }
            set { SetProperty(ref _page2, value); }
        }

        private bool _page3 = false;

        public bool Page3
        {
            get { return _page3; }
            set { SetProperty(ref _page3, value); }
        }

        private bool _page4 = false;

        public bool Page4
        {
            get { return _page4; }
            set { SetProperty(ref _page4, value); }
        }

        private bool _page5 = false;

        public bool Page5
        {
            get { return _page5; }
            set { SetProperty(ref _page5, value); }
        }

        private bool _page6 = false;

        public bool Page6
        {
            get { return _page6; }
            set { SetProperty(ref _page6, value); }
        }
        #endregion

        #region Commands
        public ICommand BackCommand { get; set; }

        public ICommand NextCommand { get; set; }

        public ICommand CancelCommand { get; set; }

        public ICommand ChangeInstallPathCommand { get; set; }
        #endregion

        public MainWindowViewModel()
        {
            ChangePages();

            BackCommand = new DelegateCommand(Back);
            NextCommand = new DelegateCommand(Next);
            CancelCommand = new DelegateCommand(Cancel);
            ChangeInstallPathCommand = new DelegateCommand(ChangeInstallPath);
        }

        private void Back()
        {
            CountPages = CountPages - 1;
            if (CountPages < 0)
                CountPages = 0;
            ChangePages();
        }

        private void Next()
        {
            CountPages = CountPages + 1;
            if (CountPages > 5)
                CountPages = 5;
            ChangePages();
        }

        private void Cancel()
        {
            if (CountPages < 5)
            {
                var close = System.Windows.MessageBox.Show("Você tem certeza que deseja sair do Instalador do ProlexNet?", "Instalação do ProlexNet", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (close == MessageBoxResult.Yes)
                    Environment.Exit(1);
            }
        }

        private void ChangePages()
        {
            switch (CountPages)
            {
                case 0:
                    Page1 = true;
                    Page2 = false;
                    Page3 = false;
                    Page4 = false;
                    Page5 = false;
                    Page6 = false;

                    BackVisibility = "Hidden";
                    break;

                case 1:
                    Page1 = false;
                    Page2 = true;
                    Page3 = false;
                    Page4 = false;
                    Page5 = false;
                    Page6 = false;

                    BackVisibility = "Visible";
                    break;

                case 2:
                    Page1 = false;
                    Page2 = false;
                    Page3 = true;
                    Page4 = false;
                    Page5 = false;
                    Page6 = false;

                    //if (checkbox_ProlexNetServer.IsChecked == true)
                    //{
                    //    ProlexNetServer_settings.IsEnabled = true;
                    //    CheckBoxDatabaseDeploy.IsEnabled = true;
                    //    CheckBoxFirebirdInstallation.IsEnabled = true;
                    //    CheckBoxIBExpertInstallation.IsEnabled = true;
                    //    CheckBoxLINQPadInstallation.IsEnabled = true;
                    //    ServerNameOverNetwork.Text = Environment.GetEnvironmentVariable("COMPUTERNAME");
                    //}
                    //else
                    //{
                    //    ProlexNetServer_settings.IsEnabled = false;
                    //    CheckBoxDatabaseDeploy.IsEnabled = false;
                    //    CheckBoxFirebirdInstallation.IsEnabled = false;
                    //    CheckBoxIBExpertInstallation.IsEnabled = false;
                    //    CheckBoxLINQPadInstallation.IsEnabled = false;
                    //    ServerNameOverNetwork.Text = "localhost";
                    //}

                    BackVisibility = "Visible";
                    NextContent = "Próximo >";
                    //ButtonAdvance.Click += BeforeInstallation;
                    //ButtonAdvance.Click -= StartInstallationAsync;
                    break;

                case 3:
                    Page1 = false;
                    Page2 = false;
                    Page3 = false;
                    Page4 = true;
                    Page5 = false;
                    Page6 = false;

                    BackVisibility = "Visible";
                    NextContent = "Instalar >";
                    //ButtonAdvance.Click -= BeforeInstallation;
                    //ButtonAdvance.Click += StartInstallationAsync;
                    break;

                case 4:
                    Page1 = false;
                    Page2 = false;
                    Page3 = false;
                    Page4 = false;
                    Page5 = true;
                    Page6 = false;

                    //ButtonBack.Visibility = Visibility.Hidden;
                    //ButtonCancel.Visibility = Visibility.Visible;
                    //ButtonAdvance.Visibility = Visibility.Hidden;
                    NextContent = "Próximo >";
                    //ButtonAdvance.Click -= StartInstallationAsync;
                    break;

                case 5:
                    Page1 = false;
                    Page2 = false;
                    Page3 = false;
                    Page4 = false;
                    Page5 = false;
                    Page6 = true;

                    //ButtonBack.Visibility = Visibility.Hidden;
                    //ButtonAdvance.Visibility = Visibility.Hidden;
                    //ButtonCancel.Visibility = Visibility.Visible;
                    //ButtonCancel.Content = "Finalizar";
                    break;

                default:
                    break;
            }
        }

        private void ChangeInstallPath()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                    InstallationPath = Path.GetFullPath(folderBrowserDialog.SelectedPath);
            }
        }
    }
}