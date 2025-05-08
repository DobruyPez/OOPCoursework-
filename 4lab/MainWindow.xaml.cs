using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Globalization;
using System.Threading;
using System.ComponentModel;
using System.Data.Entity;
using System.Runtime.Remoting.Contexts;
using System.Windows.Navigation;

namespace _4lab
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isEnglish = true;

        public MainWindow()
        {
            InitializeComponent();
            SetLanguage(isEnglish); // Set initial language
            LoadInitialContent();
            MainFrame.Navigated += MainFrame_Navigated; // Attach navigation event handler
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
            //DataBaseInteractor.ModifyDatabase();
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

            // Update UI elements
            BackButton.ToolTip = useEnglish ? "Back" : "Назад";
            ForwardButton.ToolTip = useEnglish ? "Forward" : "Вперед";
            LanguageButton.ToolTip = useEnglish ? "Switch to Russian" : "Переключить на английский";

            // Notify UI of culture change
            foreach (var element in LogicalTreeHelper.GetChildren(this))
            {
                if (element is FrameworkElement frameworkElement)
                {
                    frameworkElement.Language = System.Windows.Markup.XmlLanguage.GetLanguage(culture);
                }
            }
        }

        private void RefreshCurrentPage()
        {
            if (MainFrame.Content is Page currentPage)
            {
                // Create a new instance of the current page type
                Page newPage = (Page)Activator.CreateInstance(currentPage.GetType(), this);
                MainFrame.Navigate(newPage);
            }
        }
        private void SubscriptionButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
           
        }
        private void NavigateToUserProfile(object sender, RoutedEventArgs e)
        {
            // Placeholder for current user data; replace with actual user retrieval logic
            string currentUsername = "TestUser"; // Fetch from session or database
            string currentEmail = "test@example.com"; // Fetch from session or database
            MainFrame.Navigate(new UserProfilePage(currentUsername, currentEmail));
        }
    }
}