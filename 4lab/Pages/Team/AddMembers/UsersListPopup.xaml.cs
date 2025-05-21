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
                        .Where(u => !context.Players.Any(tm => tm.Id == u.Id && tm.TeamId == _teamId) && u.TeamId == null && !u.Banned)
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
                    };
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
            try
            {
                if (string.IsNullOrWhiteSpace(SearchBox.Text))
                {
                    LoadUsers(); // Просто перезагружаем полный список
                    return;
                }

                var searchText = SearchBox.Text.ToLower();

                using (var context = new _4lab.BD.DBContext())
                {
                    // Ищем пользователей, которые не в текущей команде и соответствуют поисковому запросу
                    var filteredUsers = context.Players
                        .Where(u => u.TeamId == null &&
                                   (u.Name != null && u.Name.ToLower().Contains(searchText) ||
                                    u.Email != null && u.Email.ToLower().Contains(searchText)))
                        .ToList();

                    UsersList.Items.Clear();

                    foreach (var player in filteredUsers)
                    {
                        var invitationControl = new TeamInvitationUserControl(
                            player,
                            _teamId,
                            () =>
                            {
                                OnInvitationSent?.Invoke();
                                LoadUsers();
                            });

                        UsersList.Items.Add(invitationControl);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка поиска: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
