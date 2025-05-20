using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Roles;
using static Roles.CurrentTeam;

namespace _4lab
{
    public partial class UserProfilePage : Page
    {
        public UserProfilePage()
        {
            InitializeComponent();
            LoadUserData();

            // Подписываемся на изменение текущей команды
            CurrentTeam.TeamChanged += (s, e) => UpdateTeamName();
        }

        private void LoadUserData()
        {
            try
            {
                var user = CurrentUser.Instance.GetCurrentUser();
                if (user != null && user is Player playerUser)
                {
                    UsernameTextBlock.Text = user.Name;
                    EmailTextBlock.Text = user.Email;
                    TwitchTextBox.Text = playerUser.TwitchLink ?? "";
                    DiscordTextBox.Text = playerUser.DiscordLink ?? "";

                    // Обновляем название команды
                    UpdateTeamName();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateTeamName()
        {
            // Берем название команды напрямую из CurrentTeam
            TeamTextBlock.Text = CurrentTeam.Team?.Name ?? "No team";
        }

        private void SaveLinksButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new _4lab.BD.DBContext())
                {
                    var user = CurrentUser.Instance.GetCurrentUser();

                    if (user is Player player)
                    {
                        var dbUser = context.Players.Find(player.Id);
                        if (dbUser != null)
                        {
                            dbUser.TwitchLink = TwitchTextBox.Text;
                            dbUser.DiscordLink = DiscordTextBox.Text;
                            context.SaveChanges();

                            // Обновляем текущего пользователя
                            CurrentUser.Instance.Login(null, dbUser);

                            MessageBox.Show("Ссылки успешно сохранены!", "Успех",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении ссылок: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Очищаем текущего пользователя и команду
                CurrentUser.Instance.Logout();
                CurrentTeam.Clear();

                // Находим главное окно
                var mainWindow = Application.Current.MainWindow as MainWindow;

                // Переходим на страницу регистрации
                mainWindow?.MainFrame.Navigate(new RegisterUserPage());

                MessageBox.Show("Вы успешно вышли из аккаунта!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при выходе из аккаунта: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ToggleThemeButton_Click(object sender, RoutedEventArgs e)
        {
            // Реализация переключения темы остается без изменений
            var currentTheme = Application.Current.Resources.MergedDictionaries
                .FirstOrDefault(d => d.Source != null &&
                                   (d.Source.ToString().Contains("DarkTheme.xaml") ||
                                    d.Source.ToString().Contains("LightTheme.xaml")));

            var preservedDictionaries = Application.Current.Resources.MergedDictionaries
                .Where(d => d.Source == null ||
                          (!d.Source.ToString().Contains("DarkTheme.xaml") &&
                           !d.Source.ToString().Contains("LightTheme.xaml")))
                .ToList();

            Application.Current.Resources.MergedDictionaries.Clear();

            foreach (var dict in preservedDictionaries)
            {
                Application.Current.Resources.MergedDictionaries.Add(dict);
            }

            var newTheme = new ResourceDictionary();
            if (currentTheme != null && currentTheme.Source.ToString().Contains("DarkTheme.xaml"))
            {
                newTheme.Source = new Uri("Resources/Stules/LightTheme.xaml", UriKind.Relative);
            }
            else
            {
                newTheme.Source = new Uri("Resources/Stules/DarkTheme.xaml", UriKind.Relative);
            }
            Application.Current.Resources.MergedDictionaries.Add(newTheme);
        }
    }
}