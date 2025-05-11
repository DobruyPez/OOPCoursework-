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
            var user = CurrentUser.Instance.GetCurrentUser();
            if (user != null)
            {
                if(user is Player)
                {
                    Player playerUser = (Player)user;
                    UsernameTextBlock.Text = user.Username;
                    EmailTextBlock.Text = user.Email;
                    TwitchTextBox.Text = playerUser.TwitchLink ?? "";
                    DiscordTextBox.Text = playerUser.DiscordLink ?? "";
                    using (var context = new _4lab.BD.DBContext())
                    {
                        var team = context.Teams.FirstOrDefault(t => t.Id == playerUser.TeamId);
                        TeamTextBlock.Text = team?.Name ?? "No team";
                    }
                }

                
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
    }
}