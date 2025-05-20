using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Roles;
using static Roles.CurrentTeam;

namespace _4lab
{
    public partial class RegisterTeamPage : Page
    {
        public RegisterTeamPage()
        {
            InitializeComponent();
        }

        private void CreateTeamButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CurrentUser.Instance.IsLoggedIn)
            {
                MessageBox.Show(Application.Current.Resources["LoginRequiredMessage"]?.ToString() ?? "Пожалуйста, войдите в систему перед созданием команды.",
                    Application.Current.Resources["ErrorTitle"]?.ToString() ?? "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                NavigationService.Navigate(new RegisterUserPage());
                return;
            }
            var role = CurrentUser.Instance.GetCurrentUser();
            if (role is Player)
            {
                var player = CurrentUser.Instance.Player;
                if (player.TeamId != null)
                {
                    MessageBox.Show("Вы не можете создать команду пока есть старая",
                    Application.Current.Resources["ErrorTitle"]?.ToString() ?? "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                    NavigationService.Navigate(new TeamPage());
                    return;
                } 
            } 

            string teamName = TeamNameBox.Text;
            string teamRegion = (RegionComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            string teamPassword = TeamPasswordBox.Password;

            if (string.IsNullOrWhiteSpace(teamName) || string.IsNullOrWhiteSpace(teamRegion) || string.IsNullOrWhiteSpace(teamPassword))
            {
                MessageBox.Show("Все поля должны быть заполнены.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                using (var context = new _4lab.BD.DBContext())
                {
                    var team = new Team
                    {
                        Name = teamName,
                        Region = teamRegion,
                        Password = teamPassword,
                        OwnerId = CurrentUser.Instance.GetCurrentUser().Id
                    };

                    var currUser = CurrentUser.Instance.GetCurrentUser();
                    var captain = new TeamMember
                    {
                        UserId = currUser.Id,
                        UserName = currUser.Name, 
                        TeamId = team.Id, 
                        Role = TeamRole.Captain, 
                        Team = team 
                    };

                    context.Teams.Add(team);
                    team.Members.Add(captain);
                    context.SaveChanges();

                    if (currUser != null && currUser is Player playerUser)
                    {
                        // Обновляем TeamId в объекте CurrentUser
                        playerUser.TeamId = team.Id;

                        // Находим пользователя в базе данных и обновляем TeamId
                        var user = context.Players.FirstOrDefault(u => u.Email == currUser.Email);
                        if (user != null)
                        {
                            user.TeamId = team.Id;
                        }
                        else
                        {
                            MessageBox.Show(Application.Current.Resources["UserNotFound"]?.ToString() ?? "Пользователь не найден в базе данных.",
                                Application.Current.Resources["ErrorTitle"]?.ToString() ?? "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        // Сохраняем изменения для пользователя
                        context.SaveChanges();
                        CurrentTeam.SetCurrentTeam(team.Id);

                        MessageBox.Show(Application.Current.Resources["TeamCreatedSuccess"]?.ToString() ?? "Команда успешно создана!",
                            Application.Current.Resources["SuccessTitle"]?.ToString() ?? "Успех",
                            MessageBoxButton.OK, MessageBoxImage.Information);

                    }
                    else
                    {
                        MessageBox.Show(Application.Current.Resources["UserNotPlayer"]?.ToString() ?? "Текущий пользователь не является игроком.",
                            Application.Current.Resources["ErrorTitle"]?.ToString() ?? "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    MessageBox.Show("Команда успешно создана!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    NavigationService.Navigate(new MainMenuPage((Window.GetWindow(this) as MainWindow)));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"{Application.Current.Resources["TeamCreationError"]?.ToString() ?? "Ошибка при создании команды"}: {ex.Message}",
                    Application.Current.Resources["ErrorTitle"]?.ToString() ?? "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}