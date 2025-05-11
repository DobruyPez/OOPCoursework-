using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Roles;

namespace _4lab
{
    public partial class RegisterTeamPage : Page
    {
        public RegisterTeamPage()
        {
            InitializeComponent();
        }

        private void CreateTeamButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CurrentUser.Instance.IsLoggedIn)
            {
                MessageBox.Show("Пожалуйста, войдите в систему перед созданием команды.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                NavigationService.Navigate(new RegisterUserPage());
                return;
            }

            string teamName = TeamNameBox.Text;
            string teamRegion = (RegionComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            string teamPassword = TeamPasswordBox.Password;

            if (string.IsNullOrWhiteSpace(teamName) || string.IsNullOrWhiteSpace(teamRegion) || string.IsNullOrWhiteSpace(teamPassword))
            {
                MessageBox.Show("Все поля должны быть заполнены.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                using (var context = new _4lab.BD.DBContext())
                {
                    var team = new Team
                    {
                        Name = teamName,
                        Region = teamRegion,
                        Password = teamPassword,
                        OwnerId = CurrentUser.Instance.GetCurrentUser().Id
                    };

                    context.Teams.Add(team);
                    context.SaveChanges();

                    MessageBox.Show("Команда успешно создана!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    NavigationService.Navigate(new MainMenuPage((Window.GetWindow(this) as MainWindow)));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании команды: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}