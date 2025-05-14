using System;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Windows;
using System.Windows.Controls;
using _4lab.Migrations;
using Roles;

namespace _4lab
{
    public partial class UserProfilePage : Page
    {
        public UserProfilePage()
        {
            InitializeComponent();
            LoadUserData();
        }

        private void LoadUserData()
        {
            try
            {
                var user = CurrentUser.Instance.GetCurrentUser();
                if (user != null && user is Player playerUser)
                {
                    UsernameTextBlock.Text = user.Username;
                    EmailTextBlock.Text = user.Email;
                    TwitchTextBox.Text = playerUser.TwitchLink ?? "";
                    DiscordTextBox.Text = playerUser.DiscordLink ?? "";

                    try
                    {
                        using (var context = new _4lab.BD.DBContext())
                        {
                            var team = context.Teams
                                .FirstOrDefault(t => t.Id == playerUser.TeamId && t.OwnerId == playerUser.Id);

                            TeamTextBlock.Text = team?.Name ?? "No team";
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка загрузки команды: {ex.Message}", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        TeamTextBlock.Text = "Ошибка загрузки";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveLinksButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new _4lab.BD.DBContext())
                {
                    var user = CurrentUser.Instance.GetCurrentUser();

                    if (user != null)
                    {
                        if (user is Player)
                        {
                            var dbUser = context.Players.Find(user.Id);
                            dbUser.TwitchLink = TwitchTextBox.Text;
                            dbUser.DiscordLink = DiscordTextBox.Text;
                            context.SaveChanges();
                        }
                        //else if(user is Admin)
                        //{

                        //}

                        MessageBox.Show("Ссылки успешно сохранены!", "Успех",
                            MessageBoxButton.OK, MessageBoxImage.Information);
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
                // Clear the current user session
                CurrentUser.Instance.Logout();

                // Navigate to the login page
                NavigationService?.Navigate(new Uri("RegisterUserPage.xaml", UriKind.Relative));

                MessageBox.Show("Вы успешно вышли из аккаунта!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при выходе из аккаунта: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
