using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using _4lab.DB;
using Roles;
using static Roles.CurrentTeam;

namespace _4lab.Pages.TeamMatches.RegisterOffer
{
    public partial class OfferRegistrationControl : UserControl
    {
        public OfferRegistrationControl()
        {
            InitializeComponent();
            DateTimePicker.SelectedDate = DateTime.Now;
        }

        private void RegisterOfferButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CurrentUser.Instance.IsLoggedIn)
            {
                MessageBox.Show("Вы должны быть авторизованы для создания офера.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var selectedMaps = MapsComboBox.SelectedItems;
            if (selectedMaps.Count == 0)
            {
                MessageBox.Show("Выберите хотя бы одну карту.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (DateTimePicker.SelectedDate == null)
            {
                MessageBox.Show("Выберите дату и время.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var selectedDateTime = DateTimePicker.SelectedDate.Value;
            if (selectedDateTime < DateTime.Now)
            {
                MessageBox.Show("Дата не может быть в прошлом.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var offerType = (OfferTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            if (string.IsNullOrEmpty(offerType))
            {
                MessageBox.Show("Выберите тип офера.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                using (var context = new _4lab.BD.DBContext())
                {
                    TeamOffer newOffer = new TeamOffer();
                    string maps = string.Join(", ", selectedMaps.Cast<ComboBoxItem>().Select(x => x.Content.ToString()));

                    if (offerType == "TeamDeathMatch")
                    {
                        // Проверяем, есть ли у пользователя команда
                        var user = CurrentUser.Instance.GetCurrentUser();
                        var team = context.Teams.FirstOrDefault(t => t.OwnerId == user.Id);

                        if (team == null)
                        {
                            MessageBox.Show("Для создания TeamDeathMatch офера вы должны быть владельцем команды.", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        newOffer.Name = team.Name;
                        newOffer.CreatorId = team.Id;
                        newOffer.Maps = maps;
                        newOffer.Date = selectedDateTime;
                    }
                    else // OneToOne
                    {
                        var user = CurrentUser.Instance.GetCurrentUser();
                        newOffer.Name = user.Name;
                        newOffer.CreatorId = user.Id;
                        newOffer.Maps = maps;
                        newOffer.Date = selectedDateTime;
                    }

                    context.TeamOffers.Add(newOffer);
                    context.SaveChanges();

                    MessageBox.Show("Офер успешно зарегистрирован!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при регистрации офера: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OfferTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Можно добавить дополнительную логику при изменении типа офера
        }
    }
}