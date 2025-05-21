using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Globalization;
using System.Threading;
using System.Windows.Markup;
using Roles;
using static Roles.CurrentTeam;
using System.Threading.Tasks;
using _4lab.BD;
using _4lab.Resources;
using System.Diagnostics;
using System.IO;

namespace _4lab
{
    public partial class MainWindow : Window
    {
        private bool isEnglish = true;
        private DBContext dbContext = new DBContext(); // Контекст БД
        private string currentUserName; // Имя текущего пользователя из БД
        private int _selectedUserId = -1;

        public MainWindow()
        {
            InitializeComponent();
            SetLanguage(isEnglish);
            LoadInitialContent();
            MainFrame.Navigated += MainFrame_Navigated;
            CurrentTeam.TeamChangedAct += NavigateToHomeOnMembersCleared;
            CurrentTeam.MembersCleared += NavigateToHomeOnMembersCleared;
            InitializeChat();
            Console.WriteLine($"Application started at {DateTime.Now} (CEST: 06:03 PM, May 18, 2025)");
        }

        private void AdButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;
                string link = button.Tag as string;
                if (!string.IsNullOrEmpty(link))
                {
                    Process.Start(new ProcessStartInfo(link) { UseShellExecute = true });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии ссылки: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void NavigateToHomeOnMembersCleared()
        {
            // Просто перенаправляем на главную страницу
            MainFrame.Navigate(new MainMenuPage(this));
            //RefreshCurrentPage();
        }

        private void InitializeChat()
        {
            if (CurrentUser.Instance.IsLoggedIn)
            {
                try
                {
                    // Загружаем имя пользователя из БД
                    var user = dbContext.Users.FirstOrDefault(u => u.Id == CurrentUser.Instance.Id);
                    if (user != null)
                    {
                        currentUserName = user.Name;
                        ChatButton.IsEnabled = true; // Включаем кнопку чата только если пользователь найден
                        Console.WriteLine($"Chat initialized for user: {currentUserName}");
                    }
                    else
                    {
                        Console.WriteLine("User not found in database.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Chat initialization failed: {ex.Message}");
                }
            }
        }

        private void ChatButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CurrentUser.Instance.IsLoggedIn)
            {
                MessageBox.Show("Для использования чата необходимо авторизоваться.",
                               "Доступ запрещен",
                               MessageBoxButton.OK,
                               MessageBoxImage.Warning);
                return;
            }

            LoadUserList();
            UserSelectionPopup.IsOpen = true;
        }

        private void LoadUserList()
        {
            UsersList.Children.Clear();

            try
            {
                using (var context = new DBContext())
                {
                    var currUser = CurrentUser.Instance.GetCurrentUser();
                    var users = context.Players
                        .Where(u => u.Id != currUser.Id && !u.Banned)
                        .ToList();

                    foreach (var user in users)
                    {
                        var userButton = new Button
                        {
                            Content = user.Name,
                            Tag = user.Id,
                            Style = (Style)FindResource("ButtonStyle"),
                            Margin = new Thickness(0, 2, 0, 2),
                            HorizontalContentAlignment = HorizontalAlignment.Left
                        };
                        userButton.Click += (s, args) =>
                        {
                            _selectedUserId = (int)((Button)s).Tag;
                            UserMessageInput.Focus();
                        };

                        UsersList.Children.Add(userButton);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки списка пользователей: {ex.Message}",
                               "Ошибка",
                               MessageBoxButton.OK,
                               MessageBoxImage.Error);
            }
        }

        private void SendToSelectedUser_Click(object sender, RoutedEventArgs e)
        {
            var currUser = CurrentUser.Instance.GetCurrentUser();
            if (_selectedUserId == -1)
            {
                MessageBox.Show("Выберите пользователя из списка",
                               "Ошибка",
                               MessageBoxButton.OK,
                               MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(UserMessageInput.Text))
            {
                MessageBox.Show("Введите текст сообщения",
                               "Ошибка",
                               MessageBoxButton.OK,
                               MessageBoxImage.Warning);
                return;
            }

            try
            {

                if (!MessageService.CanSendMessage(currUser.Id, _selectedUserId))
                {
                    MessageBox.Show("Невозможно отправить сообщение - пользователь не найден");
                    return;
                }
                MessageService.SendMessage(
                    currUser.Id,
                    _selectedUserId,
                    UserMessageInput.Text,
                    MessageType.Default);

                UserMessageInput.Text = string.Empty;
                MessageBox.Show("Сообщение отправлено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка отправки сообщения: {ex.Message}",
                               "Ошибка",
                               MessageBoxButton.OK,
                               MessageBoxImage.Error);
            }
        }

        private void LoadInitialContent()
        {
            MainFrame.Navigate(new RegisterUserPage());
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
            if (!CurrentUser.Instance.IsLoggedIn)
            {
                System.Windows.MessageBox.Show(
                    "Зарегестрируйся",
                    "Доступ запрещен",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                Console.WriteLine("User not logged in, cannot open chat.");

                if (CurrentUser.Instance.GetCurrentUser() is Admin)
                {
                    var adminPanel = new _4lab.Windows.AdminPanel();
                    adminPanel.Show();
                    this.Close();
                    return;
                }

                // Перенаправляем на страницу регистрации/авторизации
                MainFrame.Navigate(new RegisterUserPage());
                return;
            }
            MainFrame.Navigate(new MainMenuPage(this));
        }

        private void NavigateToMessagesPage(object sender, RoutedEventArgs e)
        {
            if(!CurrentUser.Instance.IsLoggedIn)
            {
                System.Windows.MessageBox.Show(
                    "Для просмотра сообщений необходимо авторизоваться.",
                    "Доступ запрещен",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                Console.WriteLine("User not logged in, cannot open chat.");

                // Перенаправляем на страницу регистрации/авторизации
                MainFrame.Navigate(new RegisterUserPage());
                return;
            }
            MainFrame.Navigate(new MessagesPage());
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
                else if (pageType == typeof(_4lab.Pages.TeamMatches.TeamMatchesPage))
                {
                    newPage = new _4lab.Pages.TeamMatches.TeamMatchesPage(this);
                }
                else if (pageType == typeof(TeamPage))
                {
                    newPage = new TeamPage();
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

            if (!CurrentUser.Instance.IsLoggedIn)
            {
                System.Windows.MessageBox.Show(
                    "Пожалуйста, войдите в систему и выберите команду. ",
                    "Доступ запрещен",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                Console.WriteLine("User not logged in, cannot open chat.");

                // Перенаправляем на страницу регистрации/авторизации
                MainFrame.Navigate(new RegisterUserPage());
                return;
            }
            MainFrame.Navigate(new TeamPage());
        }
    }
}