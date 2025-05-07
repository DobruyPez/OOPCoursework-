using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;


namespace _4lab
{
    /// <summary>
    /// Логика взаимодействия для PremiumSubscrationPage.xaml
    /// </summary>
    public partial class PremiumSubscrationPage : Page
    {
        public PremiumSubscrationPage()
        {
            InitializeComponent();
            SetupButtonHandlers();
        }

        private void SetupButtonHandlers()
        {
            // Access the Grid directly (assuming it's the root element of the Page)
            var grid = this.Content as Grid;
            if (grid != null)
            {
                foreach (var border in grid.Children)
                {
                    if (border is Border borderElement)
                    {
                        var stackPanel = borderElement.Child as StackPanel;
                        var button = stackPanel?.Children.OfType<Button>().FirstOrDefault();
                        if (button != null)
                        {
                            button.Click += SubscriptionButton_Click;
                        }
                    }
                }
            }
        }

        private void SubscriptionButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                string plan = button.Content.ToString().Replace("Choose ", "");
                MessageBox.Show($"You have selected the {plan} plan!", "Subscription Confirmation",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}