using System;
using System.Linq;
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

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Email и пароль должны быть заполнены.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var context = new _4lab.BD.DBContext())
            {

                var user = context.Users.FirstOrDefault(u => u.Email == email);
                var baseUser = context.Users.OfType<User>().FirstOrDefault(u => u.Id == user.Id && u.Role == UserRole.Admin);
                var admin = context.Users.OfType<Admin>().FirstOrDefault(a => a.Id == user.Id);
                if (baseUser != null)
                {
                    CurrentUser.Instance.Login(new Admin(), null);
                    OpenAdminPanel();
                    return;
                }

                if (user == null || !DataBaseInteractor.VerifyPassword(password, user.PasswordHash))
                {
                    MessageBox.Show("Неверный email или пароль.", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Player player = null;
                if (user.Role == UserRole.Player)
                {
                    player = context.Players.FirstOrDefault(p => p.Id == user.Id);
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