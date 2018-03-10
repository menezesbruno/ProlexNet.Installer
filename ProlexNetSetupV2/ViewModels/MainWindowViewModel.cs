using Prism.Commands;
using Prism.Mvvm;
using ProlexNetSetupV2.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace ProlexNetSetupV2.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
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

        private List<Func<Task>> InstallQueue = new List<Func<Task>>();

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

        public bool InstallFirebird
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

        #endregion Checkboxes

        #region Fields

        public static string InstallationPath = @"C:\Automatiza";

        public string InstallPath
        {
            get { return InstallationPath; }
            set { SetProperty(ref InstallationPath, value); }
        }

        private string _prolexNetServerPort = "18520";

        public string ProlexNetServerPort
        {
            get { return _prolexNetServerPort; }
            set { SetProperty(ref _prolexNetServerPort, value); }
        }

        public static string ServicePath { get; set; }

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

        private string _progressBarSpeed;

        public string ProgressBarSpeed
        {
            get { return _progressBarSpeed; }
            set { SetProperty(ref _progressBarSpeed, value); }
        }

        #endregion ProgressBar

        #region Commands

        public ICommand BackCommand { get; set; }

        public ICommand NextCommand { get; set; }

        public ICommand CancelCommand { get; set; }

        public ICommand ChangeInstallPathCommand { get; set; }

        #endregion Commands

        public MainWindowViewModel()
        {
            ChangePagesAsync();
            CreateServicePath();

            BackCommand = new DelegateCommand(Back);
            NextCommand = new DelegateCommand(Next);
            CancelCommand = new DelegateCommand(Cancel);
            ChangeInstallPathCommand = new DelegateCommand(ChangeInstallPath);
        }

        private void CreateServicePath()
        {
            ServicePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Automatiza", "Instalador");
            Directory.CreateDirectory(ServicePath);
        }

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
                    Environment.Exit(1);
            }
        }

        private async void ChangePagesAsync()
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

                    BackVisibility = Visibility.Hidden;
                    break;

                case 1:
                    Page1 = false;
                    Page2 = true;
                    Page3 = false;
                    Page4 = false;
                    Page5 = false;
                    Page6 = false;

                    BackVisibility = Visibility.Visible;
                    break;

                case 2:
                    Page1 = false;
                    Page2 = false;
                    Page3 = true;
                    Page4 = false;
                    Page5 = false;
                    Page6 = false;

                    BackVisibility = Visibility.Visible;
                    NextContent = "Próximo >";
                    break;

                case 3:
                    Page1 = false;
                    Page2 = false;
                    Page3 = false;
                    Page4 = true;
                    Page5 = false;
                    Page6 = false;

                    BackVisibility = Visibility.Visible;
                    NextContent = "Instalar >";

                    ListToInstall();
                    break;

                case 4:
                    Page1 = false;
                    Page2 = false;
                    Page3 = false;
                    Page4 = false;
                    Page5 = true;
                    Page6 = false;

                    BackVisibility = Visibility.Hidden;
                    NextVisibility = Visibility.Hidden;
                    CancelVisibility = Visibility.Visible;

                    await InstallComponentsAsync();
                    Next();
                    break;

                case 5:
                    Page1 = false;
                    Page2 = false;
                    Page3 = false;
                    Page4 = false;
                    Page5 = false;
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

        public void ProgressChanged(DownloadProgressChangedEventArgs callback)
        {
            ProgressBarMaximum = callback.TotalBytesToReceive;
            ProgressBarValue = callback.BytesReceived;
            ProgressBarPercentage = $"{callback.ProgressPercentage}%";

            decimal total = ProgressBarMaximum;
            decimal received = ProgressBarValue;
            ProgressBarSpeed = $"{(received / 1048576):n1} MB / {(total / 1048576):n1} MB";
        }

        private async Task InstallComponentsAsync()
        {
            foreach (var item in InstallQueue)
            {
                await item.Invoke();
            }
        }

        private void ListToInstall()
        {
            var list = new List<string>();
            var bits = DetectWindows.Bits();

            #region VisualC2013

            if (RequirementsCheck.VisualC2013x86())
            {
                list.Add(Constants.VisualC2013X86);
                InstallQueue.Add(() => Download.VisualCAsync(bits, ProgressChanged));
            }

            if (bits == "x64")
            {
                if (RequirementsCheck.VisualC2013x64())
                {
                    list.Add(Constants.VisualC2013X64);
                    InstallQueue.Add(() => Download.VisualCAsync(bits, ProgressChanged));
                }
            }

            #endregion VisualC2013

            #region DotNet46

            if (RequirementsCheck.DotNet())
            {
                list.Add(Constants.DotNet46);
                InstallQueue.Add(() => Download.DotNetAsync(ProgressChanged));
            }

            #endregion DotNet46

            #region ProlexNetServer

            if (InstallProlexNetServer)
            {
                #region InstallIIS

                list.Add(Constants.IIS);
                InstallQueue.Add(() => Install.IIS());

                #endregion InstallIIS

                #region Firebird

                if (InstallFirebird)
                {
                    if (bits == "x64")
                        list.Add(Constants.FirebirdX64);
                    else
                        list.Add(Constants.FirebirdX86);

                    InstallQueue.Add(() => Download.FirebirdAsync(ProgressChanged));
                }

                #endregion Firebird

                #region IBExpert

                if (InstallIBExpert)
                {
                    list.Add(Constants.IBExpert);
                    InstallQueue.Add(() => Download.IBExpertSetupAsync(ProgressChanged));
                }

                #endregion IBExpert

                #region LinqPad

                if (InstallLinqPad)
                {
                    list.Add(Constants.LinqPad);
                    InstallQueue.Add(() => Download.LINQPad5Async(ProgressChanged));
                }

                #endregion LinqPad

                #region Database

                if (InstallProlexNetDatabase)
                {
                    list.Add(Constants.ProlexNetDatabase);
                    InstallQueue.Add(() => Download.ProlexNetDatabaseAsync(ProgressChanged));
                }

                #endregion Database

                #region ProlexNetServer

                list.Add(Constants.ProlexNetServer);
                InstallQueue.Add(() => Download.ProlexNetServerAsync(ProgressChanged));
                InstallQueue.Add(() => ConfigProlexNet.Server(InstallationPath, ProlexNetServerPort));
                InstallQueue.Add(() => Firewall.AddRules(ProlexNetServerPort));

                #endregion ProlexNetServer

                #region ProlexNetUpdater

                list.Add(Constants.ProlexNetUpdater);
                InstallQueue.Add(() => Download.ProlexNetUpdaterAsync(ProgressChanged));
                InstallQueue.Add(() => ConfigProlexNet.Updater(InstallationPath));

                #endregion ProlexNetUpdater

                #region ConfigIIS

                InstallQueue.Add(() => ConfigIIS.ProlexNetSettingsAsync(InstallationPath, ProlexNetServerPort));

                #endregion ConfigIIS
            }

            #endregion ProlexNetServer

            #region ProlexNetClient

            if (InstallProlexNetClient)
            {
                #region ProlexNetClient

                list.Add(Constants.ProlexNetClient);
                InstallQueue.Add(() => Download.ProlexNetClientAsync(ProgressChanged));

                #endregion ProlexNetClient
            }

            #endregion ProlexNetClient

            InstallList = list;
        }

        private void ChangeInstallPath()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                    InstallPath = Path.GetFullPath(folderBrowserDialog.SelectedPath);
            }
        }
    }
}