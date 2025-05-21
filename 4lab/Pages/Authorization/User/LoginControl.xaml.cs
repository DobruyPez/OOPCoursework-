using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using _4lab.DB;
using Roles;
using static Roles.CurrentTeam;

namespace _4lab
{
    public partial class LoginControl : UserControl
    {
        public LoginControl()
        {
            InitializeComponent();
        }

        private void OpenAdminPanel()
        {
            var mainWindow = Window.GetWindow(this) as MainWindow;
            if (mainWindow != null)
            {
                var adminPanel = new _4lab.Windows.AdminPanel();
                adminPanel.Show();
                mainWindow.Close();
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailBox.Text;
            string password = PasswordBox.Password;

            // Проверка на пустые поля
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Email и пароль должны быть заполнены.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Проверка на отсутствие русских символов
            if (Regex.IsMatch(email, @"[а-яА-ЯёЁ]") || Regex.IsMatch(password, @"[а-яА-ЯёЁ]"))
            {
                MessageBox.Show("Email и пароль не должны содержать русские символы.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var context = new _4lab.BD.DBContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Email == email);
                
                if (user == null)
                {
                    MessageBox.Show("Пользователь не найден.", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Проверка администратора
                var baseUser = context.Users.OfType<User>().FirstOrDefault(u => u.Id == user.Id && u.Role == UserRole.Admin);
                if (baseUser != null)
                {
                    CurrentUser.Instance.Login(new Admin(), null);
                    OpenAdminPanel();
                    return;
                }

                // Проверка пароля
                if (!DataBaseInteractor.VerifyPassword(password, user.PasswordHash))
                {
                    MessageBox.Show("Неверный пароль.", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Player player = null;
                if (user.Role == UserRole.Player)
                {
                    player = context.Players.FirstOrDefault(p => p.Id == user.Id);
                    if (player.Banned)
                    {
                        MessageBox.Show("Вы забанены.", "Информация",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                }

                CurrentUser.Instance.Login(null, player);

                if (player?.TeamId.HasValue == true)
                {
                    try
                    {
                        CurrentTeam.SetCurrentTeam(player.TeamId.Value);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            $"{Application.Current.Resources["TeamDataLoadError"]?.ToString() ?? "Ошибка загрузки данных команды"}: {ex.Message}",
                            Application.Current.Resources["ErrorTitle"]?.ToString() ?? "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                MessageBox.Show("Вход выполнен успешно!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                var page = this.FindAncestor<Page>();
                page?.NavigationService.Navigate(new MainMenuPage((MainWindow)Window.GetWindow(this)));
            }
        }
    }

    // Helper extension method to find ancestor
    public static class DependencyObjectExtensions
    {
        public static T FindAncestor<T>(this DependencyObject dependencyObject) where T : DependencyObject
        {
            while (dependencyObject != null)
            {
                if (dependencyObject is T target)
                    return target;
                dependencyObject = LogicalTreeHelper.GetParent(dependencyObject) ??
                                 VisualTreeHelper.GetParent(dependencyObject);
            }
            return null;
        }
    }
}