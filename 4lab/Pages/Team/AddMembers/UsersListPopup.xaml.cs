using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Roles;

namespace _4lab.Pages.Team
{
    public partial class UsersListPopup : UserControl
    {
        public event Action OnInvitationSent;
        public event Action<User> UserSelected;

        private List<Player> _allUsers;
        private int _teamId;

        public UsersListPopup(int teamId)
        {
            InitializeComponent();
            _teamId = teamId;
            LoadUsers();
        }

        private void LoadUsers()
        {
            try
            {
                using (var context = new _4lab.BD.DBContext())
                {
                    // Получаем всех пользователей, которые еще не в команде
                    var availablePlayers = context.Players
                        .Where(u => !context.Players.Any(tm => tm.Id == u.Id && tm.TeamId == _teamId))
                        .ToList();

                    UsersList.Items.Clear();

                    foreach (var player in availablePlayers)
                    {
                        // Создаем и добавляем UserControl для каждого игрока
                        var invitationControl = new TeamInvitationUserControl(
                            player,
                            _teamId,
                            () => OnInvitationSent?.Invoke());

                        UsersList.Items.Add(invitationControl);
                    }
                    ;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки пользователей: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchBox.Text))
            {
                UsersList.ItemsSource = _allUsers;
                return;
            }

            var searchText = SearchBox.Text.ToLower();
            UsersList.ItemsSource = _allUsers
                .Where(u => u.Username.ToLower().Contains(searchText) ||
                             u.Email.ToLower().Contains(searchText))
                .ToList();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            var parent = Parent as Popup;
            if (parent != null)
            {
                parent.IsOpen = false;
            }
        }
    }
}