using System.Windows;
using _4lab.Pages.Admin.Ban;
using _4lab.Pages.Admin.Prices;
using _4lab.Pages.Admin.Ads; 

namespace _4lab.Windows
{
    public partial class AdminPanel : Window
    {
        public AdminPanel()
        {
            InitializeComponent();
        }

        private void BanPlayersButton_Click(object sender, RoutedEventArgs e)
        {
            while (MainFrame.CanGoBack)
            {
                MainFrame.RemoveBackEntry();
            }
            MainFrame.Navigate(new BanPlayersPage());
        }

        private void ManagePricesButton_Click(object sender, RoutedEventArgs e)
        {
            while (MainFrame.CanGoBack)
            {
                MainFrame.RemoveBackEntry();
            }
            MainFrame.Navigate(new ManagePricesPage());
        }

        private void ManageAdsButton_Click(object sender, RoutedEventArgs e)
        {
            while (MainFrame.CanGoBack)
            {
                MainFrame.RemoveBackEntry();
            }
            MainFrame.Navigate(new ManageAdsPage());
        }
    }
}