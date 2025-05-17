using System;
using System.Windows;
using System.Windows.Controls;
using Roles;
using System.Windows.Controls.Primitives; 
using _4lab.Pages.Team; 

namespace _4lab
{
    public partial class TeamControls : UserControl
    {
        public TeamControls()
        {
            InitializeComponent();
        }

        public void SetRegion(string region)
        {
            EditRegionBox.Text = region;
        }

        public void SetTeamDescription(string description)
        {
            EditDescriptionBox.Text = description;
        }

        private void SaveDescriptionButton_Click(object sender, RoutedEventArgs e)
        {
            var team = CurrentTeam.Team;
            if (team == null)
            {
                MessageBox.Show(
                    Application.Current.Resources["NoTeamSelected"]?.ToString() ?? "Команда не выбрана",
                    Application.Current.Resources["ErrorTitle"]?.ToString() ?? "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                using (var context = new _4lab.BD.DBContext())
                {
                    var teamToUpdate = context.Teams.Find(team.Id);
                    if (teamToUpdate != null)
                    {
                        teamToUpdate.Region = EditRegionBox.Text;
                        teamToUpdate.Description = EditDescriptionBox.Text;
                        context.SaveChanges();

                        // Обновляем CurrentTeam
                        CurrentTeam.SetCurrentTeam(team.Id);

                        MessageBox.Show(
                            Application.Current.Resources["ChangesSaved"]?.ToString() ?? "Изменения сохранены",
                            Application.Current.Resources["SuccessTitle"]?.ToString() ?? "Успех",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show(
                            Application.Current.Resources["TeamNotFound"]?.ToString() ?? "Команда не найдена",
                            Application.Current.Resources["ErrorTitle"]?.ToString() ?? "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"{Application.Current.Resources["SaveError"]?.ToString() ?? "Ошибка при сохранении данных"}: {ex.Message}",
                    Application.Current.Resources["ErrorTitle"]?.ToString() ?? "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void InviteMemberButton_Click(object sender, RoutedEventArgs e)
        {
            var team = CurrentTeam.Team;
            if (team == null)
            {
                MessageBox.Show(
                    Application.Current.Resources["NoTeamSelected"]?.ToString() ?? "Команда не выбрана",
                    Application.Current.Resources["ErrorTitle"]?.ToString() ?? "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Создаем Popup
            var popup = new Popup
            {
                Width = 400,
                Height = 500,
                Placement = PlacementMode.Center,
                PlacementTarget = Application.Current.MainWindow, // Указываем главное окно как цель для позиционирования
                IsOpen = true,
                StaysOpen = false,
                AllowsTransparency = true // Для правильного отображения
            };

            // Создаем UsersListPopup
            var usersListPopup = new UsersListPopup(team.Id);
            usersListPopup.OnInvitationSent += () => popup.IsOpen = false;

            // Устанавливаем контент Popup
            popup.Child = new Border
            {
                CornerRadius = new CornerRadius(5),
                BorderThickness = new Thickness(1),
                Child = usersListPopup,
                Padding = new Thickness(10)
            };

            // Открываем Popup
            popup.IsOpen = true;
        }

        private void ChangeRoleButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Реализовать логику изменения роли
            MessageBox.Show("Функция изменения роли пока не реализована.",
                Application.Current.Resources["InfoTitle"]?.ToString() ?? "Информация",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void RemoveMemberButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Реализовать логику удаления участника
            MessageBox.Show("Функция удаления участника пока не реализована.",
                Application.Current.Resources["InfoTitle"]?.ToString() ?? "Информация",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}