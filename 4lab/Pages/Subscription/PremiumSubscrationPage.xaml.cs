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
        }

        private void ChooseLite_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PaymentPage("Lite"));
        }

        private void ChooseStandard_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PaymentPage("Standard"));
        }

        private void ChoosePro_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PaymentPage("Pro"));
        }
    }
}