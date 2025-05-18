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

namespace _4lab
{
    public partial class MainWindow : Window
    {
        private bool isEnglish = true;
        private DBContext dbContext = new DBContext(); // Контекст БД
        private string currentUserName; // Имя текущего пользователя из БД

        public MainWindow()
        {
            InitializeComponent();
            SetLanguage(isEnglish);
            LoadInitialContent();
            MainFrame.Navigated += MainFrame_Navigated;
            InitializeChat();
            Console.WriteLine($"Application started at {DateTime.Now} (CEST: 06:03 PM, May 18, 2025)");
        }

        private void InitializeChat()
        {
            // Изначально кнопка чата выключена
            ChatButton.IsEnabled = false;

            if (CurrentUser.Instance.IsLoggedIn)
            {
                try
                {
                    // Загружаем имя пользователя из БД
                    var user = dbContext.Users.FirstOrDefault(u => u.Id == CurrentUser.Instance.Id);
                    if (user != null)
                    {
                        currentUserName = user.Username;
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
            Console.WriteLine("ChatButton_Click triggered.");

            if (!CurrentUser.Instance.IsLoggedIn)
            {
                System.Windows.MessageBox.Show(
                    "Для использования чата необходимо авторизоваться.",
                    "Доступ запрещен",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                Console.WriteLine("User not logged in, cannot open chat.");

                // Перенаправляем на страницу регистрации/авторизации
                MainFrame.Navigate(new RegisterUserPage());
                return;
            }

            try
            {
                ChatPopup.IsOpen = !ChatPopup.IsOpen;
                Console.WriteLine($"ChatPopup.IsOpen set to: {ChatPopup.IsOpen}");

                if (ChatPopup.IsOpen)
                {
                    ChatInput.Focus();
                    LoadChatMessages(); // Загружаем сообщения при открытии
                    Console.WriteLine("Chat opened and messages loaded.");
                }
                else
                {
                    Console.WriteLine("Chat closed.");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(
                    $"Ошибка при открытии чата: {ex.Message}",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                Console.WriteLine($"Error opening chat: {ex.Message}");
            }
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

        // Обработчик отправки сообщения по кнопке
        private void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        // Обработчик нажатия Enter в поле ввода
        private void ChatInput_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage();
            }
        }

        // Загрузка сообщений из БД
        private void LoadChatMessages()
        {
            try
            {
                ChatMessages.Children.Clear(); // Очищаем текущие сообщения
                var messages = dbContext.ChatMessages
                    .OrderBy(m => m.Timestamp)
                    .ToList();

                foreach (var message in messages)
                {
                    var user = dbContext.Users.FirstOrDefault(u => u.Id == message.UserId);
                    if (user != null)
                    {
                        string formattedMessage = $"{user.Username}: {message.Message} ({message.Timestamp:HH:mm})";
                        TextBlock messageBlock = new TextBlock
                        {
                            Text = formattedMessage,
                            Style = (Style)FindResource("ChatTextBlockStyle")
                        };
                        ChatMessages.Children.Add(messageBlock);
                    }
                }

                // Прокручиваем вниз
                var scrollViewer = ChatMessages.Parent as ScrollViewer;
                scrollViewer?.ScrollToBottom();
                Console.WriteLine("Chat messages loaded successfully.");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка при загрузке сообщений: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine($"Error loading chat messages: {ex.Message}");
            }
        }

        // Логика отправки сообщения
        private void SendMessage()
        {
            if (!CurrentUser.Instance.IsLoggedIn || string.IsNullOrWhiteSpace(ChatInput.Text))
                return;

            try
            {
                string messageText = ChatInput.Text.Trim();
                var user = dbContext.Users.FirstOrDefault(u => u.Id == CurrentUser.Instance.Id);
                if (user == null) return;

                var chatMessage = new ChatMessage
                {
                    UserId = user.Id,
                    Message = messageText,
                    Timestamp = DateTime.Now
                };

                dbContext.ChatMessages.Add(chatMessage);
                dbContext.SaveChanges(); // Сохраняем в БД

                LoadChatMessages(); // Обновляем отображение
                ChatInput.Text = string.Empty;
                Console.WriteLine("Message sent successfully.");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка при отправке сообщения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine($"Error sending message: {ex.Message}");
            }
        }
    }
}