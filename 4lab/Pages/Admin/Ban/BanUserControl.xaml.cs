using System.Windows;
using System.Windows.Controls;
using _4lab.BD;
using Roles;

namespace _4lab.Pages.Admin.Ban
{
    public partial class BanUserControl : UserControl
    {
        public Player Player { get; private set; }

        public string PlayerName => $"{Player.Name} ({Player.Email})";

        public BanUserControl(Player player)
        {
            InitializeComponent();
            Player = player;
            UpdateButtonText();
            DataContext = this;
        }

        private void UpdateButtonText()
        {
            BanButton.Content = Player.Banned ? "Unban" : "Ban";
        }

        private void BanButton_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new DBContext())
            {
                var dbPlayer = context.Players.Find(Player.Id);
                if (dbPlayer != null)
                {
                    dbPlayer.Banned = !dbPlayer.Banned;
                    context.SaveChanges();
                    Player.Banned = dbPlayer.Banned;
                    UpdateButtonText();

                    MessageBox.Show($"Player {Player.Name} has been {(Player.Banned ? "banned" : "unbanned")}",
                                  "Success",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Information);
                }
            }
        }
    }
}
