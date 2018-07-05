using ProlexNetSetup.ViewModels;

namespace ProlexNetSetup.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Next_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var count = MainWindowViewModel._countPages;
            if (count >= 4)
            {
                Next.IsEnabled = false;
                Back.IsEnabled = false;
            }
        }
    }
}