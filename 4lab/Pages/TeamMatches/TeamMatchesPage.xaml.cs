using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using _4lab.BD;
using _4lab.Pages.TeamMatches.RegisterOffer;
using Roles;

namespace _4lab
{
    public partial class TeamMatchesPage : Page
    {
        private MainWindow _parentWindow;
        private List<Match> _matches;
        private List<Match> _filteredOtherMatches;
        private List<Match> _filteredMyMatches;
        private string _selectedOfferType;

        public ICommand MakeOfferCommand { get; }

        public TeamMatchesPage()
        {
            InitializeComponent();
            MakeOfferCommand = new RelayCommand(MakeOffer);
            DataContext = this;
            _selectedOfferType = "All"; 
            LoadMatches();
            OfferTypeFilter.SelectedIndex = 0;
        }

        public TeamMatchesPage(MainWindow parent) : this()
        {
            _parentWindow = parent;
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
                    .ToList();

                    _matches = teamOffers.Select(to => new Match
                    {
                        TeamName = to.Name,
                        Maps = to.Maps,
                        DateTime = to.Date.ToString("dd.MM / HH:mm"),
                        ImagePath = DetermineImagePath(to),
                        OfferId = to.Id,
                        CreatorId = to.CreatorId,
                        IsTeamDeathMatch = context.Teams.Any(t => t.Id == to.CreatorId)
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
                filteredMatches = _matches.Where(m => !m.IsTeamDeathMatch).ToList();
            }
            else if (_selectedOfferType == "TeamDeathMatch")
            {
                filteredMatches = _matches.Where(m => m.IsTeamDeathMatch).ToList();
            }

            _filteredOtherMatches = filteredMatches
                .Where(m => m.CreatorId != currentUser.Id)
                .ToList();
            _filteredMyMatches = filteredMatches
                .Where(m => m.CreatorId == currentUser.Id)
                .ToList();

            OtherMatchesList.ItemsSource = null;
            OtherMatchesList.ItemsSource = _filteredOtherMatches;
            MyMatchesList.ItemsSource = null;
            MyMatchesList.ItemsSource = _filteredMyMatches;
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

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchText = SearchBox.Text.ToLower();
            if (string.IsNullOrEmpty(searchText) || searchText == (string)FindResource("SearchLabel"))
            {
                UpdateFilteredMatches();
            }
            else
            {
                _filteredOtherMatches = _filteredOtherMatches
                    .Where(m => m.TeamName.ToLower().Contains(searchText) || m.Maps.ToLower().Contains(searchText))
                    .ToList();
                _filteredMyMatches = _filteredMyMatches
                    .Where(m => m.TeamName.ToLower().Contains(searchText) || m.Maps.ToLower().Contains(searchText))
                    .ToList();

                OtherMatchesList.ItemsSource = null;
                OtherMatchesList.ItemsSource = _filteredOtherMatches;
                MyMatchesList.ItemsSource = null;
                MyMatchesList.ItemsSource = _filteredMyMatches;
            }
        }

        private void OfferTypeFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OfferTypeFilter.SelectedItem is ComboBoxItem selectedItem)
            {
                _selectedOfferType = selectedItem.Content.ToString();
                UpdateFilteredMatches();
            }
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
                _parentWindow.ShowContent(new OfferRegistrationPage());
            }
            else
            {
                MessageBox.Show("Parent window is not set.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MakeOffer(object parameter)
        {
            if (parameter is Match selectedMatch)
            {
                MessageBox.Show($"Offer made for {selectedMatch.TeamName}!", "Offer Sent",
                    MessageBoxButton.OK, MessageBoxImage.Information);
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