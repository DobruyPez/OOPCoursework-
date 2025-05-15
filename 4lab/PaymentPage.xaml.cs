using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace _4lab
{
    public partial class PaymentPage : Page
    {
        public PaymentPage(string selectedPlan)
        {
            InitializeComponent();
            PlanTitle.Text = $"Оплата плана {selectedPlan}";
        }

        private void PayButton_Click(object sender, RoutedEventArgs e)
        {
            // Сбрасываем сообщение и видимость перед новой проверкой
            StatusMessage.Text = "";
            StatusMessage.Visibility = Visibility.Visible;
            StatusMessage.Foreground = new SolidColorBrush(Colors.White); // Белый по умолчанию

            // Проверка номера карты (16 цифр)
            if (!Regex.IsMatch(CardNumber.Text, @"^\d{16}$"))
            {
                StatusMessage.Text = "Неверный номер карты";
                StatusMessage.Foreground = new SolidColorBrush(Colors.Red); // Красный для ошибки
                return;
            }

            // Проверка срока действия (MM/YY)
            if (!Regex.IsMatch(ExpiryDate.Text, @"^(0[1-9]|1[0-2])\/\d{2}$"))
            {
                StatusMessage.Text = "Неверный срок действия";
                StatusMessage.Foreground = new SolidColorBrush(Colors.Red);
                return;
            }

            // Проверка CVV (3 цифры)
            if (!Regex.IsMatch(Cvv.Text, @"^\d{3}$"))
            {
                StatusMessage.Text = "Неверный CVV";
                StatusMessage.Foreground = new SolidColorBrush(Colors.Red);
                return;
            }

            // Проверка имени владельца (не пустое)
            if (string.IsNullOrWhiteSpace(CardHolder.Text))
            {
                StatusMessage.Text = "Введите имя владельца";
                StatusMessage.Foreground = new SolidColorBrush(Colors.Red);
                return;
            }

            // Имитация успешной оплаты
            StatusMessage.Text = "Оплата проведена успешно";
            StatusMessage.Foreground = new SolidColorBrush(Colors.Green); // Зелёный для успеха
        }
    }
}