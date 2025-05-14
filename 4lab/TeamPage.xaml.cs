using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace _4lab
{
    public partial class TeamPage : Page
    {
        public bool IsCreator { get; set; } = true; // Замените на реальную логику
        private bool HasSelection => MembersList.SelectedItem != null;

        public TeamPage()
        {
            InitializeComponent();
            InitializeButtons();
            UpdateButtonStates();
        }

        private void InitializeButtons()
        {
            // Очищаем панели кнопок
            ButtonsPanel.Children.Clear();
            AdminControlsPanel.Visibility = Visibility.Collapsed;

            // Добавляем соответствующие кнопки в зависимости от роли
            if (IsCreator)
            {
                var inviteButton = new Button
                {
                    Content = "Пригласить",
                    Style = (Style)FindResource("ButtonStyle")
                };
                inviteButton.Click += InviteButton_Click;
                ButtonsPanel.Children.Add(inviteButton);

                AdminControlsPanel.Visibility = Visibility.Visible;
            }
            else
            {
                var leaveButton = new Button
                {
                    Content = "Покинуть",
                    Style = (Style)FindResource("ButtonStyle"),
                    Margin = new Thickness(10, 0, 0, 0)
                };
                leaveButton.Click += LeaveButton_Click;
                ButtonsPanel.Children.Add(leaveButton);
            }
        }

        private void UpdateButtonStates()
        {
            ChangeRoleButton.IsEnabled = HasSelection;
            RemoveMemberButton.IsEnabled = HasSelection;
        }

        private void MembersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonStates();
        }

        private void InviteButton_Click(object sender, RoutedEventArgs e)
        {
            // Логика приглашения участника
            MessageBox.Show("Функция приглашения участника");
        }

        private void LeaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Логика выхода из команды
            var result = MessageBox.Show("Вы уверены, что хотите покинуть команду?", "Подтверждение", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                NavigationService?.GoBack();
            }
        }

        private void ChangeRoleButton_Click(object sender, RoutedEventArgs e)
        {
            if (MembersList.SelectedItem is TeamMember member)
            {
                MessageBox.Show($"Изменение роли для {member.Name}");
            }
        }

        private void RemoveMemberButton_Click(object sender, RoutedEventArgs e)
        {
            if (MembersList.SelectedItem is TeamMember member)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить {member.Name} из команды?", "Подтверждение", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    // Логика удаления участника
                }
            }
        }
    }

    public class TeamMember
    {
        public string Name { get; set; }
        public string Role { get; set; }
    }
}