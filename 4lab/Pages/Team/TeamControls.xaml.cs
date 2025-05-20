using System;
using System.Windows;
using System.Windows.Controls;
using Roles;
using System.Windows.Controls.Primitives; 
using _4lab.Pages.Team;
using System.Collections.Generic;
using System.Data.Entity;
using static _4lab.TeamPage;
using System.Linq;

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
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
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

            // Подтверждение удаления
            var result = MessageBox.Show(
                Application.Current.Resources["ConfirmTeamDelete"]?.ToString() ?? "Вы уверены, что хотите удалить команду? Это действие нельзя отменить.",
                Application.Current.Resources["ConfirmTitle"]?.ToString() ?? "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                using (var context = new _4lab.BD.DBContext())
                {
                    // Находим команду со всеми связанными данными
                    var teamToDelete = context.Teams
                        .Include(t => t.Members)
                        .FirstOrDefault(t => t.Id == team.Id);

                    if (teamToDelete != null)
                    {                        
                        CurrentTeam.ClearAllTeamMembers();
                        context.Teams.Remove(teamToDelete);
                        context.SaveChanges();

                        CurrentTeam.ClearCurrentTeam();

                        MessageBox.Show(
                            Application.Current.Resources["TeamDeletedSuccess"]?.ToString() ?? "Команда успешно удалена",
                            Application.Current.Resources["SuccessTitle"]?.ToString() ?? "Успех",
                            MessageBoxButton.OK, MessageBoxImage.Information);

                        // Можно вызвать событие для обновления UI
                        // OnTeamDeleted?.Invoke();
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
                    $"{Application.Current.Resources["DeleteError"]?.ToString() ?? "Ошибка при удалении команды"}: {ex.Message}",
                    Application.Current.Resources["ErrorTitle"]?.ToString() ?? "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}