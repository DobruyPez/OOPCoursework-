using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Globalization;
using System.Threading;
using System.Windows.Markup;
using Roles;
using System.Windows.Forms;
using static Roles.CurrentTeam;

namespace _4lab
{
    public partial class MainWindow : Window
    {
        private bool isEnglish = true;

        public MainWindow()
        {
            InitializeComponent();
            SetLanguage(isEnglish);
            LoadInitialContent();
            MainFrame.Navigated += MainFrame_Navigated;
            //var teamPage = new TeamPage();
            //teamPage.NavigateToRegisterRequested += () => MainFrame.Navigate(new RegisterUserPage());
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

        private void LanguageButton_Click(object sender, RoutedEventArgs e)
        {
            isEnglish = !isEnglish;
            SetLanguage(isEnglish);
            LanguageButton.Content = isEnglish ? "EN" : "RU";
            RefreshCurrentPage();
        }

        private void SetLanguage(bool useEnglish)
        {
            string culture = useEnglish ? "en-US" : "ru-RU";
            Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);

            System.Windows.Application.Current.Resources.MergedDictionaries.Clear();
            var newDict = new ResourceDictionary
            {
                Source = new Uri($"Resources/Languages/{(useEnglish ? "English.xaml" : "Russian.xaml")}", UriKind.Relative)
            };
            System.Windows.Application.Current.Resources.MergedDictionaries.Add(newDict);

            foreach (var element in LogicalTreeHelper.GetChildren(this))
            {
                if (element is FrameworkElement frameworkElement)
                {
                    frameworkElement.Language = XmlLanguage.GetLanguage(culture);
                }
            }
        }

        private void RefreshCurrentPage()
        {
            if (MainFrame.Content is Page currentPage)
            {
                Page newPage;
                Type pageType = currentPage.GetType();

                if (pageType == typeof(MainMenuPage))
                {
                    newPage = new MainMenuPage(this);
                }
                else if (pageType == typeof(UserProfilePage))
                {
                    newPage = new UserProfilePage();
                }
                else if (pageType == typeof(TeamMatchesPage))
                {
                    newPage = new TeamMatchesPage(this);
                }
                else
                {
                    newPage = (Page)Activator.CreateInstance(pageType);
                }

                MainFrame.Navigate(newPage);
            }
        }

        private void NavigateToUserProfile(object sender, RoutedEventArgs e)
        {
            if (CurrentUser.Instance.IsLoggedIn)
            {
                MainFrame.Navigate(new UserProfilePage());
            }
            else
            {
                System.Windows.MessageBox.Show("Please register or sign in first.", "Not Logged In",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                MainFrame.Navigate(new RegisterUserPage());
            }
        }
        private void NavigateToTeamProfile(object sender, RoutedEventArgs e)
        {
          
           MainFrame.Navigate(new TeamPage());
        }
    }
}