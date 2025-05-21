using System.Linq;
using System.Windows;
using System.Windows.Controls;
using _4lab.BD;
using Roles;

namespace _4lab.Pages.Admin.Ban
{
    public partial class BanPlayersPage : Page
    {
        public BanPlayersPage()
        {
            InitializeComponent();
            LoadPlayers();
        }

        private void LoadPlayers()
        {
            using (var context = new DBContext())
            {
                var players = context.Users.OfType<Player>().ToList();

                foreach (var player in players)
                {
                    var playerControl = new BanUserControl(player);
                    PlayersListPanel.Children.Add(playerControl);
                }
            }
        }
    }
}
