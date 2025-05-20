using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Roles;

namespace _4lab.Pages.Team
{
    public partial class TeamInvitationUserControl : UserControl
    {
        public User User;
        private readonly int _teamId;
        private readonly Action _onInviteSent;

        public TeamInvitationUserControl(User user, int teamId, Action onInviteSent)
        {
            InitializeComponent();
            User = user;
            _teamId = teamId;
            _onInviteSent = onInviteSent;

            UsernameTextBlock.Text = user.Name;
        }

        private void InviteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _4lab.Resources.MessageService.SendMessage(CurrentUser.Instance.GetCurrentUser().Id, User.Id,  $"{CurrentUser.Instance.GetCurrentUser().Name} приглашает вас в команду {CurrentTeam.Team.Name}", _4lab.Resources.MessageType.TeamInvitation);
                // Используем новую логику CurrentTeam для добавления участника
                //CurrentTeam.AddTeamMember(
                //    User.Id,
                //    User.Username,
                //    TeamRole.Member);

                MessageBox.Show("Приглашение отправлено успешно", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                _onInviteSent?.Invoke();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при отправке приглашения: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}