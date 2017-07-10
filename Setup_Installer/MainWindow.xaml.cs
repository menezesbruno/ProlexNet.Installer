using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Setup_Installer;

namespace Setup_Installer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int CountPages = 0;
        public MainWindow()
        {
            InitializeComponent();
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
                    break;

                case 3:
                    Page1.IsSelected = false;
                    Page2.IsSelected = false;
                    Page3.IsSelected = false;
                    Page4.IsSelected = true;
                    Page5.IsSelected = false;
                    Page6.IsSelected = false;

                    ButtonBack.Visibility = Visibility.Visible;
                    break;

                case 4:
                    Page1.IsSelected = false;
                    Page2.IsSelected = false;
                    Page3.IsSelected = false;
                    Page4.IsSelected = false;
                    Page5.IsSelected = true;
                    Page6.IsSelected = false;

                    ButtonBack.Visibility = Visibility.Visible;
                    ButtonAdvance.Content = "Próximo >";
                    break;

                case 5:
                    Page1.IsSelected = false;
                    Page2.IsSelected = false;
                    Page3.IsSelected = false;
                    Page4.IsSelected = false;
                    Page5.IsSelected = false;
                    Page6.IsSelected = true;

                    ButtonBack.Visibility = Visibility.Visible;
                    ButtonAdvance.Content = "Instalar";
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
            var close = MessageBox.Show("Você tem certeza que quer sair do Instalador do ProlexNet?", "Instalação do ProlexNet", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (close == MessageBoxResult.Yes)
                Close();
        }

        private void ButtonChangeInstallPath_Click(object sender, RoutedEventArgs e)
        {
            Class.System.Shutdown.Reboot();
        }
    }
}
