using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Windows;
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
        private string _backContent = "&lt; Voltar";

        public string BackContent
        {
            get { return _backContent; }
            set { SetProperty(ref _backContent, value); }
        }

        private bool _backVisibility = false;

        public bool BackVisibility
        {
            get { return _backVisibility; }
            set { SetProperty(ref _backVisibility, value); }
        }

        private string _nextContent = "Próximo &gt;";

        public string NextContent
        {
            get { return _nextContent; }
            set { SetProperty(ref _nextContent, value); }
        }

        private bool _nextVisibility = true;

        public bool NextVisibility
        {
            get { return _nextVisibility; }
            set { SetProperty(ref _nextVisibility, value); }
        }

        private string _cancelContent = "Cancelar";

        public string CancelContent
        {
            get { return _backContent; }
            set { SetProperty(ref _cancelContent, value); }
        }

        private bool _cancelVisibility = true;

        public bool CancelVisibility
        {
            get { return _cancelVisibility; }
            set { SetProperty(ref _cancelVisibility, value); }
        }
        #endregion

        #region Pages
        public int CountPages { get; set; }

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
        #endregion

        public MainWindowViewModel()
        {
            BackCommand = new DelegateCommand(Back);
            NextCommand = new DelegateCommand(Next);
            CancelCommand = new DelegateCommand(Cancel);
        }

        private void Back()
        {
            CountPages = CountPages - 1;
            if (CountPages < 0)
                CountPages = 0;
            ChangePages(CountPages);
        }

        private void Next()
        {
            CountPages = CountPages + 1;
            if (CountPages > 5)
                CountPages = 5;
            ChangePages(CountPages);
        }

        private void Cancel()
        {
            if (CountPages < 5)
            {
                var close = MessageBox.Show("Você tem certeza que deseja sair do Instalador do ProlexNet?", "Instalação do ProlexNet", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (close == MessageBoxResult.Yes)
                    Environment.Exit(1);
            }
            Environment.Exit(1);
        }

        private void ChangePages(int page)
        {
            switch (page)
            {
                case 0:
                    Page1 = true;
                    Page2 = false;
                    Page3 = false;
                    Page4 = false;
                    Page5 = false;
                    Page6 = false;

                    BackVisibility = false;
                    break;

                case 1:
                    Page1 = false;
                    Page2 = true;
                    Page3 = false;
                    Page4 = false;
                    Page5 = false;
                    Page6 = false;

                    BackVisibility = true;
                    break;

                case 2:
                    Page1 = false;
                    Page2 = false;
                    Page3 = true;
                    Page4 = false;
                    Page5 = false;
                    Page6 = false;

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

                    BackVisibility = true;
                    NextContent = "Próximo >";
                    ButtonAdvance.Click += BeforeInstallation;
                    ButtonAdvance.Click -= StartInstallationAsync;
                    break;

                case 3:
                    Page1 = false;
                    Page2 = false;
                    Page3 = false;
                    Page4 = true;
                    Page5 = false;
                    Page6 = false;

                    BackVisibility = true;
                    NextContent = "Instalar >";
                    ButtonAdvance.Click -= BeforeInstallation;
                    ButtonAdvance.Click += StartInstallationAsync;
                    break;

                case 4:
                    Page1 = false;
                    Page2 = false;
                    Page3 = false;
                    Page4 = false;
                    Page5 = true;
                    Page6 = false;

                    ButtonBack.Visibility = Visibility.Hidden;
                    ButtonCancel.Visibility = Visibility.Visible;
                    ButtonAdvance.Visibility = Visibility.Hidden;
                    NextContent = "Próximo >";
                    ButtonAdvance.Click -= StartInstallationAsync;
                    break;

                case 5:
                    Page1 = false;
                    Page2 = false;
                    Page3 = false;
                    Page4 = false;
                    Page5 = false;
                    Page6 = true;

                    ButtonBack.Visibility = Visibility.Hidden;
                    ButtonAdvance.Visibility = Visibility.Hidden;
                    ButtonCancel.Visibility = Visibility.Visible;
                    ButtonCancel.Content = "Finalizar";
                    break;

                default:
                    break;
            }
        }
    }
}