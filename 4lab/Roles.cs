using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roles
{
    public enum SubscriptionType
    {
        Light,
        Semi,
        Pro
    }

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }

    public class Team {
        private List<Player> PlayerList;

        public Player GetPlayerByRole(string role) { 
            Player player = null;
            return player;
        }
    }

    public class Player : User
    {
        public SubscriptionType Subscription { get; set; }
    }
}
