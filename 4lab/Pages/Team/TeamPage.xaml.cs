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

                    if (team.Members != null && team.Members.Any())
                    {
                        var membersData = team.Members.Select(m => new
                        {
                            Name = m.UserName ?? "Unknown",
                            Role = Application.Current.Resources[$"{m.Role}"]?.ToString() ?? m.Role.ToString(),
                            IsOwner = m.UserId == team.OwnerId
                        }).ToList();

                        MembersList.ItemsSource = membersData;
                    }
                    else
                    {
                        MembersList.ItemsSource = new List<object>
                        {
                            new { Name = "Нет участников", Role = "", IsOwner = false }
                        };
                    }


                    TeamTitle.Text = team.Name ?? "No Name";

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