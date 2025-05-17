using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace _4lab
{
    public partial class TeamMatchesPage : Page
    {
        private MainWindow _parentWindow;
        private List<Match> _matches;
        private List<Match> _filteredMatches;

        public ICommand MakeOfferCommand { get; }

        // Беспараметрический конструктор для обновления страницы
        public TeamMatchesPage()
        {
            InitializeComponent();
            MakeOfferCommand = new RelayCommand(MakeOffer);
            DataContext = this;
            LoadMatches();
        }

        public TeamMatchesPage(MainWindow parent) : this()
        {
            _parentWindow = parent;
        }

        private void LoadMatches()
        {
            // Заглушка: тестовые данные
            _matches = new List<Match>
            {
                new Match
                {
                    TeamName = "VIRTUS.PRO",
                    Maps = "Top Tier / Sandstone, Province, Rust",
                    DateTime = "12.04 / 16:00",
                    ImagePath = "Images/virtus.jfif"
                },
                new Match
                {
                    TeamName = "ABSOLUTE",
                    Maps = "Top Tier / Sandstone, Rust",
                    DateTime = "12.04 / 18:00",
                    ImagePath = "Images/ManWithDog.jpg"
                },
                new Match
                {
                    TeamName = "FORZE ESPORTS",
                    Maps = "Top Tier / Sandstone, Rust",
                    DateTime = "14.04 / 16:00",
                    ImagePath = "Images/Бабка.jpg"
                }
            };
            _filteredMatches = _matches;
            MatchesList.ItemsSource = _filteredMatches;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchText = SearchBox.Text.ToLower();
            if (string.IsNullOrEmpty(searchText) || searchText == (string)FindResource("SearchLabel"))
            {
                _filteredMatches = _matches;
            }
            else
            {
                _filteredMatches = _matches
                    .Where(m => m.TeamName.ToLower().Contains(searchText) || m.Maps.ToLower().Contains(searchText))
                    .ToList();
            }
            MatchesList.ItemsSource = null; // Сбрасываем источник
            MatchesList.ItemsSource = _filteredMatches;
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
    }

    // Простая реализация ICommand
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