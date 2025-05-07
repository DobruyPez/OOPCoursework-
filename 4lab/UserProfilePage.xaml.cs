using System.Windows.Controls;
using System.Windows;

namespace _4lab
{
    public partial class UserProfilePage : Page
    {
        public UserProfilePage(string username, string email)
        {
            InitializeComponent();
            UsernameTextBlock.Text = $"Username: {username}";
            EmailTextBlock.Text = $"Email: {email}";
            // Team info can be set here if available; for now, it defaults to "Not in a team"
        }

        private void SaveLinksButton_Click(object sender, RoutedEventArgs e)
        {
            string twitch = TwitchTextBox.Text;
            string discord = DiscordTextBox.Text;

            // For now, just show a confirmation message
            MessageBox.Show($"Links saved!\nTwitch: {twitch}\nDiscord: {discord}", "Success",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}