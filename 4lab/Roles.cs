using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using static Roles.CurrentUser;

namespace Roles
{
    public enum UserRole
    {
        Admin,
        Player
    }
    public enum SubscriptionType  // Заглушка, можно расширить
    {
        Lite,
        Semiro,
        Pro
    }

    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }  
        public UserRole? Role { get; set; }         
    }

    public class Team
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }
        public string Password { get; set; }
        public int OwnerId { get; set; } 
    }


    public class Player : User
    {
        public SubscriptionType? Subscription { get; set; }
        public string TwitchLink { get; set; }   // Добавлено
        public string DiscordLink { get; set; }  // Добавлено
        public int? TeamId { get; set; }          // Добавлено
    }

    public class Admin : User
    {
        // Пока без дополнительных возможностей
    }

    public class CurrentUser
    {
        private static CurrentUser _instance = new CurrentUser();
        public static CurrentUser Instance => _instance;

        public Admin Admin { get; private set; }
        public Player Player { get; private set; }
        public bool IsLoggedIn => Admin != null || Player != null;

        private CurrentUser() { }

        public void SetUser(User user)
        {
            Admin = null;
            Player = null;

            if (user is Admin admin)
            {
                Admin = admin;
            }
            else if (user is Player player)
            {
                Player = player;
            }
        }

        public void Login(Admin admin, Player player)
        {
            Admin = admin;
            Player = player;
        }

        public User GetCurrentUser()
        {
            if (Player == null) return Admin;
            else return Player;
        }

        internal void Logout()
        {
            Player = null;
        }
    }
    public class TeamOffer
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Team1Id { get; set; }
        public int? Team2Id { get; set; }
        public DateTime Date { get; set; }
    }
}