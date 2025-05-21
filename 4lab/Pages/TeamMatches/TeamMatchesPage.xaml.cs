using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using _4lab.BD;
using _4lab.Resources;
using Roles;

namespace _4lab.Pages.TeamMatches
{
    public partial class TeamMatchesPage : Page
    {
        private MainWindow _parentWindow;
        private List<Match> _matches;
        private List<Match> _filteredOtherMatches;
        private List<Match> _filteredMyMatches;
        private string _selectedOfferType;
        private string _searchText = "";

        public ICommand MakeOfferCommand { get; }
        public ICommand DeleteOfferCommand { get; }

        public TeamMatchesPage()
        {
            InitializeComponent();
            MakeOfferCommand = new RelayCommand(MakeOffer);
            DeleteOfferCommand = new RelayCommand(DeleteOffer);
            DataContext = this;
            _selectedOfferType = "All";
            LoadMatches();
            OfferTypeFilter.SelectedIndex = 0;
            SearchBox.TextChanged += SearchBox_TextChanged;
        }

        public TeamMatchesPage(MainWindow parent) : this()
        {
            _parentWindow = parent;
        }

        private void OfferTypeFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OfferTypeFilter.SelectedItem is ComboBoxItem selectedItem)
            {
                _selectedOfferType = selectedItem.Content.ToString();
                UpdateFilteredMatches();
            }
        }

        private string DetermineImagePath(TeamOffer offer)
        {
            var imagePaths = new Dictionary<string, string>
            {
                { "VIRTUS.PRO", "Images/virtus.jfif" },
                { "ABSOLUTE", "Images/ManWithDog.jpg" },
                { "FORZE ESPORTS", "Images/Бабка.jpg" }
            };
            return imagePaths.ContainsKey(offer.Name) ? imagePaths[offer.Name] : "Images/default.jpg";
        }

        private void DeleteOffer(object parameter)
        {
            if (parameter is Match selectedMatch)
            {
                var currentUser = CurrentUser.Instance.GetCurrentUser();
                if (currentUser == null || currentUser.Id != selectedMatch.CreatorId) return;

                try
                {
                    using (var context = new DBContext())
                    {
                        var offerToDelete = context.TeamOffers.FirstOrDefault(to => to.Id == selectedMatch.OfferId);
                        if (offerToDelete != null)
                        {
                            context.TeamOffers.Remove(offerToDelete);
                            context.SaveChanges();
                            LoadMatches();
                            MessageBox.Show("Offer deleted successfully!", "Success",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting offer: {ex.Message}", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _searchText = SearchBox.Text.ToLower();
            UpdateFilteredMatches();
        }

        private void LoadMatches()
        {
            try
            {
                using (var context = new DBContext())
                {
                    var currentUser = CurrentUser.Instance.GetCurrentUser();
                    if (currentUser == null)
                    {
                        MessageBox.Show("Пользователь не авторизован.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        NavigationService.Navigate(new RegisterUserPage());
                        return;
                    }

                    var teamOffers = context.TeamOffers
                        .Include("Creator")
                        .Where(to => !to.Resolved)
                        .ToList();

                    _matches = teamOffers.Select(to => new Match
                    {
                        TeamName = to.Name,
                        Maps = to.Maps,
                        DateTime = to.Date.ToString("dd.MM / HH:mm"),
                        ImagePath = DetermineImagePath(to),
                        OfferId = to.Id,
                        CreatorId = to.CreatorId,
                        IsTeamDeathMatch = to.Offertype == Offertype.TeamDethMatch,
                        OfferType = to.Offertype
                    }).ToList();

                    UpdateFilteredMatches();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки матчей: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateFilteredMatches()
        {
            var currentUser = CurrentUser.Instance.GetCurrentUser();
            if (currentUser == null) return;

            var filteredMatches = _matches;
            if (_selectedOfferType == "OneToOne")
            {
                filteredMatches = _matches.Where(m => m.OfferType == Offertype.OneToOne).ToList();
            }
            else if (_selectedOfferType == "TeamDeathMatch")
            {
                filteredMatches = _matches.Where(m => m.OfferType == Offertype.TeamDethMatch).ToList();
            }

            if (!string.IsNullOrWhiteSpace(_searchText) && _searchText != (string)FindResource("SearchLabel"))
            {
                filteredMatches = filteredMatches
                    .Where(m => m.TeamName.ToLower().Contains(_searchText) ||
                               m.Maps.ToLower().Contains(_searchText))
                    .ToList();
            }

            _filteredOtherMatches = filteredMatches
                .Where(m => m.CreatorId != currentUser.Id)
                .ToList();
            _filteredMyMatches = filteredMatches
                .Where(m => m.CreatorId == currentUser.Id)
                .ToList();

            OtherMatchesList.ItemsSource = _filteredOtherMatches;
            MyMatchesList.ItemsSource = _filteredMyMatches;
        }

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchBox.Text == (string)FindResource("SearchLabel"))
            {
                SearchBox.Text = "";
            }
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            if (_parentWindow != null)
            {
                _parentWindow.ShowContent(new UserProfilePage());
            }
            else
            {
                MessageBox.Show("Parent window is not set.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CreateOfferButton_Click(object sender, RoutedEventArgs e)
        {
            if (_parentWindow != null)
            {
                _parentWindow.ShowContent(new _4lab.Pages.TeamMatches.RegisterOffer.OfferRegistrationPage());
            }
            else
            {
                var mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
                if (mainWindow != null)
                {
                    _parentWindow = mainWindow;
                    _parentWindow.ShowContent(new _4lab.Pages.TeamMatches.RegisterOffer.OfferRegistrationPage());
                }
                else
                {
                    MessageBox.Show("Не удалось найти главное окно приложения.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void MakeOffer(object parameter)
        {
            if (parameter is Match selectedMatch)
            {
                var currentUser = CurrentUser.Instance.GetCurrentUser();
                if (currentUser == null) return;

                try
                {
                    using (var context = new DBContext())
                    {
                        var teamOffer = context.TeamOffers
                            .FirstOrDefault(to => to.Id == selectedMatch.OfferId);

                        if (teamOffer == null)
                        {
                            MessageBox.Show("Офер не найден.", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        MessageService.SendMessage(currentUser.Id, selectedMatch.CreatorId,
                            $"{currentUser.Name} хочет присоединиться к игре {teamOffer.Date}",
                            MessageType.TeamOffer, teamOffer.Id);

                        MessageBox.Show($"Предложение отправлено для {selectedMatch.TeamName}!", "Успех",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при отправке предложения: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }

    public class Match
    {
        public string TeamName { get; set; }
        public string Maps { get; set; }
        public string DateTime { get; set; }
        public string ImagePath { get; set; }
        public int OfferId { get; set; }
        public int CreatorId { get; set; }
        public bool IsTeamDeathMatch { get; set; }
        public Offertype OfferType { get; set; }

        public bool CanJoin => CurrentUser.Instance.GetCurrentUser()?.Id != CreatorId;
        public bool IsMyOffer => CurrentUser.Instance.GetCurrentUser()?.Id == CreatorId;
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute?.Invoke(parameter) ?? true;
        public void Execute(object parameter) => _execute(parameter);

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}