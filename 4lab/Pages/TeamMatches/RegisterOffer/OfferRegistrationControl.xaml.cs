using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using _4lab.DB;
using Roles;
using static Roles.CurrentTeam;

namespace _4lab.Pages.TeamMatches.RegisterOffer
{
    /// <summary>
    /// Логика взаимодействия для OfferRegistrationControl.xaml
    /// </summary>
    public partial class OfferRegistrationControl : UserControl
    {
        public OfferRegistrationControl()
        {
            InitializeComponent();
            DatePicker.SelectedDate = DateTime.Now;
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

            if (DatePicker.SelectedDate == null)
            {
                MessageBox.Show("Выберите дату.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (TimePicker.SelectedItem == null)
            {
                MessageBox.Show("Выберите время.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var selectedDate = DatePicker.SelectedDate.Value;
            var selectedTime = TimeSpan.Parse(((ComboBoxItem)TimePicker.SelectedItem).Content.ToString());
            var selectedDateTime = selectedDate.Date + selectedTime;

            if (selectedDateTime < DateTime.Now)
            {
                MessageBox.Show("Дата и время не могут быть в прошлом.", "Ошибка",
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
                    string maps = string.Join(", ", MapsComboBox.SelectedItems.Cast<object>().Select(x => x.ToString().Split(':').Last().Trim()));

                    if (offerType == "TeamDeathMatch")
                    {
                        var user = CurrentUser.Instance.GetCurrentUser();
                        var team = context.Teams.FirstOrDefault(t => t.OwnerId == user.Id);

                        if (team == null)
                        {
                            MessageBox.Show("Для создания TeamDeathMatch офера вы должны быть владельцем команды.", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        newOffer.Name = offerType + " " + team.Name;
                        newOffer.CreatorId = user.Id;
                        newOffer.Maps = maps;
                        newOffer.Date = selectedDateTime;
                        newOffer.Offertype = Offertype.TeamDethMatch;
                    }
                    else // OneToOne
                    {
                        var user = CurrentUser.Instance.GetCurrentUser();
                        newOffer.Name = offerType + " " + user.Name;
                        newOffer.CreatorId = user.Id;
                        newOffer.Maps = maps;
                        newOffer.Date = selectedDateTime;
                        newOffer.Offertype = Offertype.OneToOne;
                    }

                    context.TeamOffers.Add(newOffer);
                    context.SaveChanges();

                    MessageBox.Show("Офер успешно зарегистрирован!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    Frame frame = Application.Current.MainWindow?.FindName("MainFrame") as Frame;
                    if (frame != null)
                    {
                        frame.NavigationService.Navigate(new TeamMatchesPage());
                    }
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
