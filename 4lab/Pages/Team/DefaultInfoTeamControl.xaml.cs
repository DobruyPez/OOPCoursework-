using System;
using System.Collections.Generic;
using System.Linq;
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

namespace _4lab
{
    public partial class DefaultInfoTeamControl : UserControl
    {
        public DefaultInfoTeamControl()
        {
            InitializeComponent();
            CurrentTeam.TeamChanged += OnTeamChanged;
            Loaded += DefaultInfoTeamControl_Loaded;
        }

        public void SetRegion(string region)
        {
            RegionTextBlock.Text = region;
        }

        public void SetTeamDescription(string description)
        {
            TeamDescriptionTextBlock.Text = description;
        }
        private void DefaultInfoTeamControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateLeaveButtonVisibility();
        }

        private void OnTeamChanged(object sender, EventArgs e)
        {
            UpdateLeaveButtonVisibility();
        }

        private void UpdateLeaveButtonVisibility()
        {
            if (CurrentTeam.HasCurrentTeam() &&
                CurrentUser.Instance.IsLoggedIn &&
                CurrentUser.Instance.GetCurrentUser() is Player player)
            {
                // Показываем кнопку только если пользователь не владелец
                LeaveTeamButton.Visibility =
                    player.Id != CurrentTeam.Team.OwnerId ?
                    Visibility.Visible :
                    Visibility.Collapsed;
            }
            else
            {
                LeaveTeamButton.Visibility = Visibility.Collapsed;
            }
        }

        private void LeaveTeamButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(
                Application.Current.Resources["ConfirmLeaveTeam"]?.ToString() ?? "Вы уверены, что хотите покинуть команду?",
                Application.Current.Resources["ConfirmTitle"]?.ToString() ?? "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    if (CurrentUser.Instance.GetCurrentUser() is Player player)
                    {
                        using (var context = new _4lab.BD.DBContext())
                        {
                            // Удаляем пользователя из членов команды
                            var member = context.Set<TeamMember>()
                                .FirstOrDefault(m => m.UserId == player.Id && m.TeamId == CurrentTeam.Team.Id);

                            if (member != null)
                            {
                                context.Set<TeamMember>().Remove(member);
                            }

                            // Обновляем TeamId у игрока
                            var dbPlayer = context.Players.Find(player.Id);
                            if (dbPlayer != null)
                            {
                                dbPlayer.TeamId = null;
                            }

                            context.SaveChanges();

                            // Обновляем текущего пользователя
                            CurrentUser.Instance.UpdatePlayerTeam(null);

                            // Обновляем текущую команду
                            CurrentTeam.SyncTeamMembersWithUsers();
                            //CurrentTeam.SetCurrentTeam(CurrentTeam.Team.Id);
                            CurrentTeam.ClearCurrentTeam();

                            MessageBox.Show(
                                Application.Current.Resources["LeftTeamSuccess"]?.ToString() ?? "Вы успешно покинули команду",
                                Application.Current.Resources["SuccessTitle"]?.ToString() ?? "Успех",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"{Application.Current.Resources["LeaveTeamError"]?.ToString() ?? "Ошибка при выходе из команды"}: {ex.Message}",
                        Application.Current.Resources["ErrorTitle"]?.ToString() ?? "Ошибка",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }
    }
}
