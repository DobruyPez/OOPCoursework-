using System.Windows.Controls;

namespace _4lab.Pages.TeamMatches
{
    public partial class TeamMatchesPage : Page
    {
        public TeamMatchesPage(MainWindow parent = null)
        {
            InitializeComponent();
            DataContext = new TeamMatchesViewModel(parent);
        }
    }
}