using Prism.Mvvm;

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

        public MainWindowViewModel()
        {
        }
    }
}