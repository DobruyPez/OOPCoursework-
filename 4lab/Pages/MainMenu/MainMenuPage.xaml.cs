using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Логика взаимодействия для MainMenuPage.xaml
    /// </summary>
    public partial class MainMenuPage : Page
    {
        private MainWindow _parentWindow;

        public MainMenuPage(MainWindow parent)
        {
            InitializeComponent();
            _parentWindow = parent;
        }

        private void CreateTeamButton_Click(object sender, RoutedEventArgs e)
        {
            _parentWindow.ShowContent(new RegisterTeamPage());
        }

        private void FindPraccsButton_Click(object sender, RoutedEventArgs e)
        {
            _parentWindow.ShowContent(new _4lab.Pages.TeamMatches.TeamMatchesPage(_parentWindow));
        }

        public void RegisterUser()
        {
            _parentWindow.ShowContent(new RegisterUserPage());
        }

        private void SubscriptionButton_Click(object sender, RoutedEventArgs e)
        {
            _parentWindow.ShowContent(new PremiumSubscrationPage());
        }
    }
}