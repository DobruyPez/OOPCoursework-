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

namespace _4lab
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadInitialContent();
        }

        private void LoadInitialContent()
        {
            MainFrame.Navigate(new MainMenuPage(this));
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            BackButton.IsEnabled = MainFrame.CanGoBack;
            ForwardButton.IsEnabled = MainFrame.CanGoForward;
        }

        public void ShowContent(Page page)
        {
            MainFrame.Navigate(page);
        }

        private void NavigateToHome(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new MainMenuPage(this));
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoBack)
            {
                MainFrame.GoBack();
            }
        }

        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoForward)
            {
                MainFrame.GoForward();
            }
        }
        private void RegisterUser(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new RegisterUserPage());
        }
    }
}
