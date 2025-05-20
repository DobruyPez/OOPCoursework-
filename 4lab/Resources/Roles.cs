using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using _4lab.BD;
using System.ComponentModel.DataAnnotations.Schema;

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
    public enum TeamRole
    {
        Captain,
        Support,
        Member
    }

    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }  
        public UserRole? Role { get; set; }         
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
    public class Team
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }
        public string Password { get; set; }
        public string Description { get; set; }
        public int OwnerId { get; set; }
        public virtual ICollection<TeamMember> Members { get; set; }

        public Team()
        {
            Members = new List<TeamMember>();
        }
    }

    public class TeamMember
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int TeamId { get; set; }
        public TeamRole Role { get; set; }
        public virtual Team Team { get; set; }
    }

    public static class CurrentTeam
    {
        private static Team _currentTeam;
        private static readonly object _lock = new object();
        public static event EventHandler TeamChanged;
        public static event Action MembersCleared;
        public static event Action TeamChangedAct;

        public static Team Team
        {
            get
            {
                lock (_lock)
                {
                    return _currentTeam;
                }
            }
            private set
            {
                lock (_lock)
                {
                    _currentTeam = value;
                    TeamChanged?.Invoke(null, EventArgs.Empty);
                }
            }
        }

        public static bool ClearAllTeamMembers()
        {
            if (!HasCurrentTeam())
                return false;

            try
            {
                using (var context = new DBContext())
                {
                    // Получаем команду с участниками
                    var team = context.Teams
                        .Include(t => t.Members)
                        .FirstOrDefault(t => t.Id == Team.Id);

                    if (team != null && team.Members != null)
                    {
                        // Фильтруем участников (кроме владельца)
                        var membersToRemove = team.Members;

                        // Получаем ID участников для обновления
                        var memberIds = membersToRemove.Select(m => m.UserId).ToList();

                        // Обновляем TeamId у игроков
                        var playersToUpdate = context.Players
                            .Where(p => memberIds.Contains(p.Id))
                            .ToList();

                        foreach (var player in playersToUpdate)
                        {
                            player.TeamId = null;
                        }

                        context.SaveChanges();
                        CurrentUser.Instance.UpdateBDLink();
                        MembersCleared?.Invoke(); // Уведомление об очистке
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка при очистке участников: {ex.Message}");
                return false;
            }

            return false;
        }

        public static bool HasCurrentTeam()
        {
            return Team != null;
        }

        public static void ClearCurrentTeam()
        {
            Team = null; 
        }

        public static void SetCurrentTeam(int teamId)
        {
            try
            {
                using (var context = new _4lab.BD.DBContext())
                {
                    var team = context.Teams
                        .Include("Members")
                        .FirstOrDefault(t => t.Id == teamId);

                    Team = team;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка при загрузке команды: {ex.Message}");
                Team = null;
            }
        }

        public static void Clear()
        {
            lock (_lock)
            {
                _currentTeam = null;
                TeamChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        public static void UpdateTeam(Team updatedTeam)
        {
            try
            {
                using (var context = new _4lab.BD.DBContext())
                {
                    var dbTeam = context.Teams.Find(updatedTeam.Id);
                    if (dbTeam != null)
                    {
                        dbTeam.Name = updatedTeam.Name;
                        dbTeam.Region = updatedTeam.Region;
                        dbTeam.Password = updatedTeam.Password;
                        dbTeam.Description = updatedTeam.Description;

                        context.SaveChanges();
                        SetCurrentTeam(updatedTeam.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка при обновлении команды: {ex.Message}");
                throw;
            }
        }

        // Добавление участника в команду
        public static void AddTeamMember(int userId, string userName, TeamRole role = TeamRole.Member)
        {
            if (Team == null) return;

            try
            {
                using (var context = new _4lab.BD.DBContext())
                {
                    // Проверяем, не состоит ли уже пользователь в команде
                    if (context.Players.Any(tm => tm.TeamId == Team.Id && tm.Id == userId))
                    {
                        throw new InvalidOperationException("Пользователь уже состоит в команде");
                    }

                    var teamMember = new TeamMember
                    {
                        UserId = userId,
                        UserName = userName,
                        TeamId = Team.Id,
                        Role = role
                    };

                    var team = context.Teams.FirstOrDefault(t => t.Id == _currentTeam.Id);
                    if (team != null)
                    {
                        team.Members.Add(teamMember);
                        context.SaveChanges();
                    }
                    context.SaveChanges();

                    // Обновляем TeamId у пользователя, если это Player
                    var user = context.Users.Find(userId);
                    if (user is Player player)
                    {
                        player.TeamId = Team.Id;
                        context.SaveChanges();
                    }

                    SetCurrentTeam(Team.Id); // Перезагружаем команду
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка при добавлении участника: {ex.Message}");
                throw;
            }
        }

        public static bool ChangeMemberRole(int userId, TeamRole newRole)
        {
            if (Team == null) return false;

            var member = Team.Members.FirstOrDefault(m => m.UserId == userId);
            if (member == null || member.Role == newRole) return false;

            try
            {
                using (var context = new _4lab.BD.DBContext())
                {
                    var dbMember = context.Set<TeamMember>().Find(member.Id);
                    if (dbMember != null)
                    {
                        dbMember.Role = newRole;
                        context.SaveChanges();

                        // Обновляем локальную копию
                        member.Role = newRole;
                        SetCurrentTeam(Team.Id); // Перезагружаем команду
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка при изменении роли: {ex.Message}");
            }
            return false;
        }

        public static bool RemoveTeamMember(int userId)
        {
            if (Team == null) return false;

            var member = Team.Members.FirstOrDefault(m => m.UserId == userId);
            if (member == null) return false;

            try
            {
                using (var context = new _4lab.BD.DBContext())
                {
                    // Удаляем участника из команды
                    var dbMember = context.Set<TeamMember>().Find(member.Id);
                    if (dbMember != null)
                    {
                        context.Set<TeamMember>().Remove(dbMember);
                    }

                    // Сбрасываем TeamId у игрока
                    var player = context.Set<Player>().FirstOrDefault(p => p.Id == userId);
                    if (player != null)
                    {
                        player.TeamId = null;
                        context.Entry(player).State = EntityState.Modified;
                    }

                    context.SaveChanges();

                    // Обновляем локальную копию
                    Team.Members.Remove(member);
                    TeamChangedAct?.Invoke();
                    CurrentUser.Instance.UpdateBDLink();
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка при удалении участника: {ex.Message}");
            }
            return false;
        }
        public static bool SyncTeamMembersWithUsers()
        {
            if (!HasCurrentTeam())
                return false;

            try
            {
                using (var context = new DBContext())
                {
                    // Загружаем команду со всеми участниками
                    var team = context.Teams
                        .Include(t => t.Members)
                        .FirstOrDefault(t => t.Id == Team.Id);

                    if (team == null || team.Members == null)
                        return false;

                    bool changesMade = false;

                    // Получаем всех участников, которые должны быть в команде
                    var validMemberIds = context.Users
                        .OfType<Player>()
                        .Where(u => u.TeamId == team.Id)
                        .Select(u => u.Id)
                        .ToList();

                    // Находим участников для удаления (есть в Members, но нет в Users с правильным TeamId)
                    var membersToRemove = team.Members
                        .Where(m => !validMemberIds.Contains(m.UserId))
                        .ToList();

                    // Удаляем невалидных участников
                    foreach (var member in membersToRemove)
                    {
                        context.Entry(member).State = EntityState.Deleted;
                        team.Members.Remove(member);
                        changesMade = true;
                    }

                    // Добавляем отсутствующих участников (есть в Users с правильным TeamId, но нет в Members)
                    var existingMemberIds = team.Members.Select(m => m.UserId).ToList();
                    var usersToAdd = context.Users
                        .OfType<Player>()
                        .Where(u => u.TeamId == team.Id && !existingMemberIds.Contains(u.Id))
                        .ToList();

                    foreach (var user in usersToAdd)
                    {
                        var newMember = new TeamMember
                        {
                            UserId = user.Id,
                            UserName = user.Name,
                            TeamId = team.Id,
                            Role = TeamRole.Member // Или другая логика определения роли
                        };
                        team.Members.Add(newMember);
                        changesMade = true;
                    }

                    if (changesMade)
                    {
                        context.SaveChanges();
                        SetCurrentTeam(team.Id); // Обновляем текущую команду
                        CurrentUser.Instance.UpdateBDLink();
                    }

                    TeamChangedAct?.Invoke();
                    return changesMade;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка синхронизации участников команды: {ex.Message}");
                return false;
            }
        }
    }

    public class CurrentUser
    {
        private static CurrentUser _instance = new CurrentUser();
        public static CurrentUser Instance => _instance;

        public Admin Admin { get; private set; }
        public Player Player { get; private set; }
        public bool IsLoggedIn => Admin != null || Player != null;

        public int Id { get; internal set; }

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
            Admin = null;
        }

        // Обновление данных текущего пользователя
        public void UpdateCurrentUser(User updatedUser)
        {
            try
            {
                using (var context = new _4lab.BD.DBContext())
                {
                    if (updatedUser is Player updatedPlayer)
                    {
                        // Обновляем данные игрока
                        var dbPlayer = context.Players.Find(updatedPlayer.Id);
                        if (dbPlayer != null)
                        {
                            dbPlayer.Name = updatedPlayer.Name;
                            dbPlayer.Email = updatedPlayer.Email;
                            dbPlayer.PasswordHash = updatedPlayer.PasswordHash;
                            dbPlayer.TwitchLink = updatedPlayer.TwitchLink;
                            dbPlayer.DiscordLink = updatedPlayer.DiscordLink;
                            dbPlayer.Subscription = updatedPlayer.Subscription;
                            dbPlayer.TeamId = updatedPlayer.TeamId;

                            context.SaveChanges();
                            Player = dbPlayer;
                        }
                    }
                    else if (updatedUser is Admin updatedAdmin)
                    {
                        // Обновляем данные админа
                        var dbAdmin = context.Users.OfType<Admin>().FirstOrDefault(a => a.Id == updatedAdmin.Id);
                        if (dbAdmin != null)
                        {
                            dbAdmin.Name = updatedAdmin.Name;
                            dbAdmin.Email = updatedAdmin.Email;
                            dbAdmin.PasswordHash = updatedAdmin.PasswordHash;

                            context.SaveChanges();
                            Admin = dbAdmin;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка при обновлении пользователя: {ex.Message}");
                throw;
            }
        }
        public void UpdatePlayerTeam(int? teamId)
        {
            if (Player == null) return;

            try
            {
                using (var context = new _4lab.BD.DBContext())
                {
                    var dbPlayer = context.Players.Find(Player.Id);
                    if (dbPlayer != null)
                    {
                        dbPlayer.TeamId = teamId;
                        context.SaveChanges();
                        Player.TeamId = teamId;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка при обновлении TeamId: {ex.Message}");
                throw;
            }
        }
        public bool IsTeamIdStillValid()
        {
            if (Player == null || Player.TeamId == null)
                return false;

            try
            {
                using (var context = new _4lab.BD.DBContext())
                {
                    // Проверяем существование команды
                    bool teamExists = context.Teams.Any(t => t.Id == Player.TeamId);

                    if (!teamExists)
                    {
                        // Если команда не существует, сбрасываем TeamId
                        UpdatePlayerTeam(null);
                        return false;
                    }

                    // Проверяем, что пользователь все еще числится в команде
                    bool isStillMember = context.Players
                        .Any(m => m.Id == Player.Id && m.TeamId == Player.TeamId);

                    if (!isStillMember)
                    {
                        UpdatePlayerTeam(null);
                        return false;
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка при проверке TeamId: {ex.Message}");
                return false;
            }
        }

        public void UpdateBDLink()
        {
            if (IsTeamIdStillValid()) return;
            else Player.TeamId = null;
        }
    }
    public enum Offertype
    {
        OneToOne,
        TeamDethMatch
    }

    public class TeamOffer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [ForeignKey("Creator")]
        public int CreatorId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Maps { get; set; }

        [Required]
        [Column(TypeName = "timestamp")]
        public DateTime Date { get; set; }

        public virtual User Creator { get; set; }
    }
}