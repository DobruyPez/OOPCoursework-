using System;
using System.Windows;
using System.Windows.Controls;
using _4lab.BD;
using System.Linq;
using _4lab.Resources;

namespace _4lab.Pages.Admin.Prices
{
    public partial class ManagePricesPage : Page
    {
        public decimal LitePrice { get; set; }
        public decimal SemiProPrice { get; set; }
        public decimal ProPrice { get; set; }

        public ManagePricesPage()
        {
            InitializeComponent();
            LoadCurrentPrices();
            DataContext = this;
        }

        private void LoadCurrentPrices()
        {
            using (var context = new DBContext())
            {
                var latestPrices = context.SubscriptionPrices
                    .OrderByDescending(p => p.CreatedAt)
                    .FirstOrDefault();

                if (latestPrices != null)
                {
                    LitePrice = latestPrices.LitePrice;
                    SemiProPrice = latestPrices.SemiProPrice;
                    ProPrice = latestPrices.ProPrice;
                }
            }
        }

        private void SavePricesButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new DBContext())
                {
                    var newPrices = new SubscriptionPrice
                    {
                        LitePrice = LitePrice,
                        SemiProPrice = SemiProPrice,
                        ProPrice = ProPrice
                    };

                    context.SubscriptionPrices.Add(newPrices);
                    context.SaveChanges();

                    MessageBox.Show("Цены успешно обновлены!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении цен: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}