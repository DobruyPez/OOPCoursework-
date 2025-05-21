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
using _4lab.BD;


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
            LoadPricesFromDatabase();
        }

        private void LoadPricesFromDatabase()
        {
            try
            {
                using (var context = new DBContext())
                {
                    var latestPrices = context.SubscriptionPrices
                        .OrderByDescending(p => p.CreatedAt)
                        .FirstOrDefault();

                    if (latestPrices != null)
                    {
                        // Получаем доступ к ресурсам приложения
                        var appResources = Application.Current.Resources;

                        // Обновляем цену, сохраняя текстовую часть из ресурсов
                        UpdatePriceText(appResources, "LitePrice", latestPrices.LitePrice);
                        UpdatePriceText(appResources, "StandardPrice", latestPrices.SemiProPrice);
                        UpdatePriceText(appResources, "ProPrice", latestPrices.ProPrice);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке цен: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdatePriceText(ResourceDictionary resources, string resourceKey, decimal price)
        {
            if (resources.Contains(resourceKey))
            {
                // Получаем текущую строку (например, "$10/month" или "$15.50/month")
                string baseText = resources[resourceKey] as string;

                if (!string.IsNullOrEmpty(baseText))
                {
                    // Находим позицию, где начинается текст после цифр, знака доллара и десятичной точки
                    int textStartIndex = 0;
                    bool foundDollarSign = false;
                    for (; textStartIndex < baseText.Length; textStartIndex++)
                    {
                        char currentChar = baseText[textStartIndex];
                        // Пропускаем знак доллара (один раз)
                        if (currentChar == '$' && !foundDollarSign)
                        {
                            foundDollarSign = true;
                            continue;
                        }
                        // Пропускаем цифры и десятичную точку
                        if (char.IsDigit(currentChar) || currentChar == '.' || currentChar == ',')
                        {
                            continue;
                        }
                        // Прерываем, если встретился другой символ
                        break;
                    }

                    // Копируем часть строки, начиная с textStartIndex, в выходную строку
                    string suffixText = textStartIndex < baseText.Length ? baseText.Substring(textStartIndex) : string.Empty;

                    // Форматируем новую строку с ценой, добавляя знак доллара
                    string newText = $"${price:F2}{suffixText}";

                    // Обновляем ресурс
                    resources[resourceKey] = newText;

                    // Логируем для отладки
                    Console.WriteLine($"Updating resource '{resourceKey}': '{baseText}' -> '{newText}', Suffix: '{suffixText}'");
                }
            }
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