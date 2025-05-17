using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Roles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static Roles.CurrentTeam;

namespace _4lab
{
    public partial class TeamPage : System.Windows.Controls.Page
    {
        public event Action NavigateToRegisterRequested;
        private DefaultInfoTeamControl defaultInfoControl;
        private TeamControls teamControls; 
        private bool _isOwner;

        public bool IsOwner
        {
            get => _isOwner;
            private set
            {
                _isOwner = value;
                OnPropertyChanged();
            }
        }

        public TeamPage()
        {
            if (!CurrentUser.Instance.IsLoggedIn || CurrentTeam.Team == null)
            {
                MessageBox.Show("Пожалуйста, войдите в систему и выберите команду.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                NavigationService?.Navigate(new Uri("Pages/Authorization/User/RegisterUserPage.xaml", UriKind.Relative));

                NavigateToRegisterRequested?.Invoke();
                var mainWindow = System.Windows.Window.GetWindow(this) as MainWindow;
                mainWindow?.MainFrame.Navigate(new RegisterUserPage());

                return;
            }

            InitializeComponent();

            defaultInfoControl = new DefaultInfoTeamControl();
            teamControls = new TeamControls(); 
            DataContext = this;
            UpdateOwnerStatus();
            LoadTeamData();
            UpdateView();

            // Подписка на изменение CurrentTeam
            CurrentTeam.TeamChanged += (s, e) =>
            {
                UpdateOwnerStatus();
                LoadTeamData();
                UpdateView();
            };
        }
        private void UpdateOwnerStatus()
        {
            IsOwner = CurrentTeam.Team?.OwnerId == CurrentUser.Instance.GetCurrentUser()?.Id;
        }

        private void LoadTeamData()
        {
            var team = CurrentTeam.Team;
            if (team == null)
            {
                defaultInfoControl.SetTeamDescription(
                    Application.Current.Resources["NoTeamSelected"]?.ToString() ?? "Команда не выбрана");
                return;
            }

            try
            {
                using (var context = new _4lab.BD.DBContext())
                {

                    if (team.Members == null)
                    {
                        team.Members = new List<TeamMember>();
                    }

                    var teamMembers = team?.Members?
                    .Select(m => new
                    {
                        Name = m.UserName ?? "Unknown",
                        Role = Application.Current.Resources[$"{m.Role}"]?.ToString() ?? m.Role.ToString()
                    })
                    .ToList();

                    string membersList = teamMembers.Any()
                        ? string.Join(", ", teamMembers)
                        : Application.Current.Resources["NoMembers"]?.ToString() ?? "Нет участников";

                    defaultInfoControl.SetRegion(team.Region ?? (Application.Current.Resources["NoRegion"]?.ToString() ?? "Регион не указан"));
                    defaultInfoControl.SetTeamDescription(team.Description ?? (Application.Current.Resources["NoDescription"]?.ToString() ?? "Описание отсутствует"));

                    // Устанавливаем данные для редактирования
                    teamControls.SetRegion(team.Region);
                    teamControls.SetTeamDescription(team.Description);
                    TeamTitle.Text = team.Name ?? "No Name";

                    var members = new List<object>();

                    // Добавляем владельца первым
                    var owner = team.Members?.FirstOrDefault(m => m.UserId == team.OwnerId);
                    if (owner != null)
                    {
                        members.Add(new MemberViewModel
                        {
                            UserId = owner.UserId,
                            Name = owner.UserName ?? "Owner",
                            Role = TeamRole.Captain,
                            IsOwner = true
                        });
                    }

                    // Добавляем остальных участников
                    if (team.Members != null)
                    {
                        foreach (var member in team.Members.Where(m => m.UserId != team.OwnerId))
                        {
                            members.Add(new MemberViewModel
                            {
                                UserId = member.UserId,
                                Name = member.UserName ?? "Unknown",
                                Role = member.Role,
                                IsOwner = false
                            });
                        }
                    }

                    MembersList.ItemsSource = members.Any() ? members : new List<object> { new { Name = "Нет участников" } };



                    defaultInfoControl.HorizontalAlignment = HorizontalAlignment.Stretch;
                    defaultInfoControl.VerticalAlignment = VerticalAlignment.Stretch;
                    teamControls.HorizontalAlignment = HorizontalAlignment.Stretch;
                    teamControls.VerticalAlignment = VerticalAlignment.Stretch;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"{Application.Current.Resources["TeamDataLoadError"]?.ToString() ?? "Ошибка загрузки данных команды"}: {ex.Message}",
                    Application.Current.Resources["ErrorTitle"]?.ToString() ?? "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                defaultInfoControl.SetTeamDescription(
                    Application.Current.Resources["TeamDataLoadError"]?.ToString() ?? "Ошибка загрузки данных команды");
            }
        }

        public class MemberViewModel : INotifyPropertyChanged
        {
            public int UserId { get; set; }
            public string Name { get; set; }
            public bool IsOwner { get; set; }
            public IEnumerable<KeyValuePair<TeamRole, string>> AvailableRoles
            {
                get
                {
                    return new[]
                    {
                        new KeyValuePair<TeamRole, string>(TeamRole.Member, "Member"),
                        new KeyValuePair<TeamRole, string>(TeamRole.Support, "Support")
                    };
                }
            }

            private TeamRole _role;
            public TeamRole Role
            {
                get => _role;
                set
                {
                    if (_role != value)
                    {
                        _role = value;
                        OnPropertyChanged();
                        RoleChanged?.Invoke(this, EventArgs.Empty);
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            public event EventHandler RoleChanged;

            protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public class MemberTemplateSelector : DataTemplateSelector
        {
            public DataTemplate OwnerTemplate { get; set; }
            public DataTemplate EditableTemplate { get; set; }
            public DataTemplate ReadOnlyTemplate { get; set; }

            public override DataTemplate SelectTemplate(object item, DependencyObject container)
            {
                if (item is MemberViewModel member)
                {
                    if (member.IsOwner)
                        return OwnerTemplate;

                    var currentUser = CurrentUser.Instance.GetCurrentUser();
                    var isCurrentUserOwner = CurrentTeam.Team?.OwnerId == currentUser?.Id;

                    return isCurrentUserOwner ? EditableTemplate : ReadOnlyTemplate;
                }
                return ReadOnlyTemplate;
            }
        }

        private void UpdateMemberRole(int userId, TeamRole newRole)
        {
            if (CurrentTeam.ChangeMemberRole(userId, newRole))
            {
                MessageBox.Show("Роль успешно изменена", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Не удалось изменить роль", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RemoveMember_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;
            if (button?.CommandParameter is int userId)
            {
                var result = MessageBox.Show("Вы уверены, что хотите удалить участника?",
                    "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    if (CurrentTeam.RemoveTeamMember(userId))
                    {
                        MessageBox.Show("Участник успешно удален", "Успех",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Не удалось удалить участника", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void UpdateView()
        {
            // Устанавливаем начальное отображение в зависимости от IsOwner
            if(IsOwner) TeamDescriptionContent.Content = teamControls;
            else TeamDescriptionContent.Content = defaultInfoControl;
        }

        private void ShowDefaultInfo_Click(object sender, RoutedEventArgs e)
        {
            TeamDescriptionContent.Content = defaultInfoControl;
        }

        private void ShowEditControls_Click(object sender, RoutedEventArgs e)
        {
            TeamDescriptionContent.Content = teamControls;
        }
        
        // Реализация INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}