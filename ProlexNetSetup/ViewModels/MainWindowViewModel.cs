using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using ProlexNetSetup.Library;
using ProlexNetSetup.Models;

namespace ProlexNetSetup.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {

        // Props
        #region Common

        private string _title = "Instalação do ProlexNet";

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private int _countPages = 0;

        public int CountPages
        {
            get { return _countPages; }
            set { SetProperty(ref _countPages, value); }
        }

        private List<string> _installList;

        public List<string> InstallList
        {
            get { return _installList; }
            set { SetProperty(ref _installList, value); }
        }

        public ObservableCollection<string> InstallStatus { get; set; }

        public ObservableCollection<Files> States { get; set; }

        private List<Func<Task>> InstallQueue { get; set; }

        #endregion Common

        #region Buttons

        private string _backContent = "< Voltar";

        public string BackContent
        {
            get { return _backContent; }
            set { SetProperty(ref _backContent, value); }
        }

        private Visibility _backVisibility = Visibility.Hidden;

        public Visibility BackVisibility
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

        private Visibility _nextVisibility = Visibility.Visible;

        public Visibility NextVisibility
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

        private Visibility _cancelVisibility = Visibility.Visible;

        public Visibility CancelVisibility
        {
            get { return _cancelVisibility; }
            set { SetProperty(ref _cancelVisibility, value); }
        }

        #endregion Buttons

        #region Checkboxes

        private bool _installProlexNet = true;

        public bool InstallProlexNet
        {
            get { return _installProlexNet; }
            set { SetProperty(ref _installProlexNet, value); }
        }

        private bool _installNetCore21 = true;

        public bool InstallNetCore21
        {
            get { return _installNetCore21; }
            set { SetProperty(ref _installNetCore21, value); }
        }

        private bool _installSQLServer = true;

        public bool InstallSQLServer
        {
            get { return _installSQLServer; }
            set { SetProperty(ref _installSQLServer, value); }
        }

        private bool _installDatabase = true;

        public bool InstallDatabase
        {
            get { return _installDatabase; }
            set { SetProperty(ref _installDatabase, value); }
        }

        private bool _installSQLServerStudio = true;

        public bool InstallSQLServerStudio
        {
            get { return _installSQLServerStudio; }
            set { SetProperty(ref _installSQLServerStudio, value); }
        }

        private bool _installLinqPad = true;

        public bool InstallLinqPad
        {
            get { return _installLinqPad; }
            set { SetProperty(ref _installLinqPad, value); }
        }

        #endregion Checkboxes

        #region Fields

        public static string InstallationPath = @"C:\Automatiza";

        public string InstallPath
        {
            get { return InstallationPath; }
            set { SetProperty(ref InstallationPath, value); }
        }

        private string _prolexNetServerPort = "5000";

        public string Port
        {
            get { return _prolexNetServerPort; }
            set { SetProperty(ref _prolexNetServerPort, value); }
        }

        private string _installationResult = "O ProlexNet foi instalado com sucesso!";

        public string InstallationResult
        {
            get { return _installationResult; }
            set { SetProperty(ref _installationResult, value); }
        }

        private Files _selectedItem;

        public Files SelectedItem
        {
            get { return _selectedItem; }
            set { SetProperty(ref _selectedItem, value); }
        }

        #endregion Fields

        #region Pages

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

        #endregion Pages

        #region ProgressBar

        private long _progressBarMaximum = 0;

        public long ProgressBarMaximum
        {
            get { return _progressBarMaximum; }
            set { SetProperty(ref _progressBarMaximum, value); }
        }

        private long _progressBarValue = 0;

        public long ProgressBarValue
        {
            get { return _progressBarValue; }
            set { SetProperty(ref _progressBarValue, value); }
        }

        private string _progressBarPercentage;

        public string ProgressBarPercentage
        {
            get { return _progressBarPercentage; }
            set { SetProperty(ref _progressBarPercentage, value); }
        }

        private string _progressBarBytesReceived;

        public string ProgressBarBytesReceived
        {
            get { return _progressBarBytesReceived; }
            set { SetProperty(ref _progressBarBytesReceived, value); }
        }

        public string _progressBarDownloadSpeed;

        public string ProgressBarDownloadSpeed
        {
            get { return _progressBarDownloadSpeed; }
            set { SetProperty(ref _progressBarDownloadSpeed, value); }
        }

        #endregion ProgressBar

        #region Commands

        public ICommand BackCommand { get; set; }

        public ICommand NextCommand { get; set; }

        public ICommand CancelCommand { get; set; }

        public ICommand ChangeInstallPathCommand { get; set; }

        #endregion Commands


        // Methods
        public MainWindowViewModel()
        {
            ChangePagesAsync();
            GetStates();

            InstallStatus = new ObservableCollection<string>();

            BackCommand = new DelegateCommand(Back);
            NextCommand = new DelegateCommand(Next);
            CancelCommand = new DelegateCommand(Cancel);
            ChangeInstallPathCommand = new DelegateCommand(ChangeInstallPath);
        }

        private void ChangeInstallPath()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                    InstallPath = Path.GetFullPath(folderBrowserDialog.SelectedPath);
            }
        }

        private void GetStates()
        {
            var getStates = DownloadParameters.AppList.StateList;
            States = new ObservableCollection<Files>();
            States.AddRange(getStates);
        }

        #region Navigation

        private void Back()
        {
            CountPages = CountPages - 1;
            if (CountPages < 0)
                CountPages = 0;
            ChangePagesAsync();
        }

        private void Next()
        {
            CountPages = CountPages + 1;
            if (CountPages > 5)
                CountPages = 5;
            ChangePagesAsync();
        }

        private void Cancel()
        {
            if (CountPages < 5)
            {
                var close = System.Windows.MessageBox.Show("Você tem certeza que deseja sair do Instalador do ProlexNet?", "Instalação do ProlexNet", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (close == MessageBoxResult.Yes)
                    System.Windows.Application.Current.Shutdown();
            }
            else
                System.Windows.Application.Current.Shutdown();
        }

        private async void ChangePagesAsync()
        {
            switch (CountPages)
            {
                case 0:
                    Page1 = true;

                    BackVisibility = Visibility.Hidden;
                    break;

                case 1:
                    Page2 = true;

                    BackVisibility = Visibility.Visible;
                    break;

                case 2:
                    Page3 = true;

                    BackVisibility = Visibility.Visible;
                    NextContent = "Próximo >";
                    break;

                case 3:
                    Page4 = true;

                    SetStateToDownload();
                    BackVisibility = Visibility.Visible;
                    NextContent = "Instalar >";

                    ListToInstall();
                    break;

                case 4:
                    Page5 = true;

                    BackVisibility = Visibility.Hidden;
                    NextVisibility = Visibility.Hidden;
                    CancelVisibility = Visibility.Visible;

                    await InstallComponentsAsync();
                    Next();
                    break;

                case 5:
                    Page6 = true;

                    BackVisibility = Visibility.Hidden;
                    NextVisibility = Visibility.Hidden;
                    CancelVisibility = Visibility.Visible;
                    CancelContent = "Finalizar";
                    break;

                default:
                    break;
            }
        }

        #endregion

        private void SetStateToDownload()
        {
            if (InstallDatabase)
            {
                var selectedItem = SelectedItem;
                if (selectedItem == null)
                {
                    System.Windows.MessageBox.Show("Estado não selecionado", "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
                    Back();
                }
                else
                {
                    DownloadParameters.AppList.Database.Url = selectedItem.Url;
                    DownloadParameters.AppList.Database.Hash = selectedItem.Hash;
                }
            }
        }

        public void ProgressChanged(DownloadProgressChangedEventArgs callback, double stopWatch)
        {
            ProgressBarMaximum = callback.TotalBytesToReceive;
            ProgressBarValue = callback.BytesReceived;
            ProgressBarPercentage = $"{callback.ProgressPercentage}%";

            decimal total = ProgressBarMaximum;
            decimal received = ProgressBarValue;
            ProgressBarBytesReceived = $"{(received / 1048576):n1} MB / {(total / 1048576):n1} MB";

            ProgressBarDownloadSpeed = string.Format("{0} kb/s", (callback.BytesReceived / 1024d / stopWatch).ToString("0"));
        }

        private async Task InstallComponentsAsync()
        {
            foreach (var item in InstallQueue)
            {
                try
                {
                    InstallStatus.Add(@"..\Resources\installer-arrow.png");
                    await item.Invoke();
                    InstallStatus.Remove(@"..\Resources\installer-arrow.png");
                    InstallStatus.Add(@"..\Resources\installer-ok.png");
                }
                catch (Exception ex)
                {
                    InstallationResult = "Houveram um ou mais erros durante a instalação! A operação não foi bem sucedida.";
                    InstallStatus.Remove(@"..\Resources\installer-arrow.png");
                    InstallStatus.Add(@"..\Resources\installer-error.png");
                    System.Windows.MessageBox.Show(ex.Message, "Erro!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ListToInstall()
        {
            var appList = DownloadParameters.AppList;
            var list = new List<string>();
            var installQueue = new List<Func<Task>>();
            var bits = DetectWindows.Bits();

            #region InstallIIS

            list.Add(Constants.IIS);
            installQueue.Add(() => Install.IIS(InstallationPath, Port));

            #endregion InstallIIS

            #region VCRedist

            if (RequirementsCheck.VcRedistX86())
            {
                list.Add(appList.VcRedistX86.Name);
                installQueue.Add(() => Download.VcRedistAsync("x86", ProgressChanged));
            }

            if (bits == "x64")
            {       
                if (RequirementsCheck.VcRedistX64())
                {
                    list.Add(appList.VcRedistX64.Name);
                    installQueue.Add(() => Download.VcRedistAsync("x64", ProgressChanged));
                }
            }

            #endregion

            #region NetCore

            list.Add(appList.NetCore.Name);
            installQueue.Add(() => Download.NetCoreAsync(ProgressChanged));

            #endregion NetCore

            #region SQLServer



            #endregion

            #region SQLServerStudio



            #endregion

            #region LinqPad

            if (InstallLinqPad)
            {
                list.Add(Constants.LINQPad5);
                installQueue.Add(() => Download.LINQPad5Async(ProgressChanged));
            }

            #endregion LinqPad

            #region Database

            if (InstallDatabase)
            {
                list.Add(Constants.Database);
                installQueue.Add(() => Download.DatabaseAsync(ProgressChanged));
            }

            #endregion Database

            #region ProlexNet

            list.Add(Constants.ProlexNet);
            installQueue.Add(() => Download.ProlexNetAsync(ProgressChanged, Port));

            #endregion ProlexNet

            InstallList = list;
            InstallQueue = installQueue;
        }
    }
}