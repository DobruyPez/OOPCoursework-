using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using _4lab.BD;
using _4lab.Resources;
using Roles;

namespace _4lab.Pages.TeamMatches
{
    public class TeamMatchesViewModel : INotifyPropertyChanged
    {
        private string _searchText = "";
        private string _selectedOfferType = "All";
        private readonly MainWindow _parentWindow;
        private readonly string _searchPlaceholder;

        public ObservableCollection<Match> OtherMatches { get; } = new ObservableCollection<Match>();
        public ObservableCollection<Match> MyMatches { get; } = new ObservableCollection<Match>();

        public ICommand MakeOfferCommand { get; }
        public ICommand DeleteOfferCommand { get; }
        public ICommand CreateOfferCommand { get; }
        public ICommand ProfileCommand { get; }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                    LoadMatches();
                }
            }
        }

        public string SelectedOfferType
        {
            get => _selectedOfferType;
            set
            {
                if (_selectedOfferType != value)
                {
                    _selectedOfferType = value;
                    OnPropertyChanged(nameof(SelectedOfferType));
                    LoadMatches();
                }
            }
        }

        public TeamMatchesViewModel(MainWindow parent = null)
        {
            _parentWindow = parent;
            _searchPlaceholder = (string)Application.Current.FindResource("SearchLabel");
            MakeOfferCommand = new RelayCommand(MakeOffer);
            DeleteOfferCommand = new RelayCommand(DeleteOffer);
            CreateOfferCommand = new RelayCommand(_ => CreateOffer());
            ProfileCommand = new RelayCommand(_ => OpenProfile());

            // Инициализация SearchText placeholder'ом
            _searchText = _searchPlaceholder;
            LoadMatches();
        }

        private void LoadMatches()
        {
            try
            {
                var currentUser = CurrentUser.Instance.GetCurrentUser();
                if (currentUser == null)
                {
                    MessageBox.Show("Пользователь не авторизован.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    _parentWindow?.ShowContent(new RegisterUserPage());
                    return;
                }

                using (var context = new DBContext())
                {
                    var teamOffers = context.TeamOffers
                        .Include("Creator")
                        .Where(to => !to.Resolved)
                        .ToList();

                    var matches = teamOffers.Select(to => new Match
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

                    ApplyFilters(matches, currentUser.Id);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки матчей: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ApplyFilters(List<Match> matches, int currentUserId)
        {
            var filteredMatches = matches.AsEnumerable();

            if (SelectedOfferType == "OneToOne")
            {
                filteredMatches = filteredMatches.Where(m => m.OfferType == Offertype.OneToOne);
            }
            else if (SelectedOfferType == "TeamDeathMatch")
            {
                filteredMatches = filteredMatches.Where(m => m.OfferType == Offertype.TeamDethMatch);
            }

            if (!string.IsNullOrWhiteSpace(SearchText) && SearchText != _searchPlaceholder)
            {
                filteredMatches = filteredMatches
                    .Where(m => m.TeamName.ToLower().Contains(SearchText.ToLower()) ||
                               m.Maps.ToLower().Contains(SearchText.ToLower()));
            }

            var filteredList = filteredMatches.ToList();

            OtherMatches.Clear();
            foreach (var match in filteredList.Where(m => m.CreatorId != currentUserId))
            {
                OtherMatches.Add(match);
            }

            MyMatches.Clear();
            foreach (var match in filteredList.Where(m => m.CreatorId == currentUserId))
            {
                MyMatches.Add(match);
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
                        var teamOffer = context.TeamOffers.FirstOrDefault(to => to.Id == selectedMatch.OfferId);

                        if (teamOffer == null)
                        {
                            MessageBox.Show("Офер не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void CreateOffer()
        {
            _parentWindow?.ShowContent(new RegisterOffer.OfferRegistrationPage());
        }

        private void OpenProfile()
        {
            _parentWindow?.ShowContent(new UserProfilePage());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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