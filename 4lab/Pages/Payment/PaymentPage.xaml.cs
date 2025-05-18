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
using System.Net.Mail;
using System.Net;

namespace _4lab
{
    public partial class PaymentPage : Page
    {
        private readonly string selectedPlan;

        public PaymentPage(string selectedPlan)
        {
            InitializeComponent();
            this.selectedPlan = selectedPlan;
            PlanTitle.Text = $"Оплата плана {selectedPlan}";
        }

        private void PayButton_Click(object sender, RoutedEventArgs e)
        {
            // Сбрасываем сообщение и видимость перед новой проверкой
            StatusMessage.Text = "";
            StatusMessage.Visibility = Visibility.Visible;
            StatusMessage.Foreground = new SolidColorBrush(Colors.White);

            // Проверка номера карты (16 цифр)
            if (!Regex.IsMatch(CardNumber.Text, @"^\d{16}$"))
            {
                StatusMessage.Text = "Неверный номер карты";
                StatusMessage.Foreground = new SolidColorBrush(Colors.Red);
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

            // Проверка email
            if (string.IsNullOrWhiteSpace(Email.Text) || !IsValidEmail(Email.Text))
            {
                StatusMessage.Text = "Введите корректный email";
                StatusMessage.Foreground = new SolidColorBrush(Colors.Red);
                return;
            }

            // Имитация успешной оплаты
            bool paymentSuccess = ProcessPayment(selectedPlan);

            if (paymentSuccess)
            {
                // Формируем чек
                string receipt = GenerateReceipt(selectedPlan, CardHolder.Text, Email.Text);

                // Отправляем чек на email
                bool emailSent = SendReceiptEmail(Email.Text, receipt);

                if (emailSent)
                {
                    StatusMessage.Text = "Оплата проведена успешно. Чек отправлен на email.";
                    StatusMessage.Foreground = new SolidColorBrush(Colors.Green);
                }
                else
                {
                    StatusMessage.Text = "Оплата успешна, но ошибка при отправке чека.";
                    StatusMessage.Foreground = new SolidColorBrush(Colors.Orange);
                }
            }
            else
            {
                StatusMessage.Text = "Ошибка при обработке платежа.";
                StatusMessage.Foreground = new SolidColorBrush(Colors.Red);
            }
        }

        // Проверка корректности email
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        // Имитация обработки платежа
        private bool ProcessPayment(string plan)
        {
            // Здесь можно интегрировать реальный платежный шлюз (например, Stripe)
            // Для имитации возвращаем true
            return true;
        }

        // Формирование чека
        private string GenerateReceipt(string plan, string cardHolder, string email)
        {
            // Примерные цены для плана
            decimal amount = plan.ToLower() switch
            {
                "basic" => 10.00m,
                "pro" => 20.00m,
                "enterprise" => 50.00m,
                _ => 0.00m
            };

            return $@"Чек об оплате
-------------------
План: {plan}
Сумма: {amount:C}
Владелец карты: {cardHolder}
Email: {email}
Дата: {DateTime.Now:dd.MM.yyyy HH:mm}
-------------------
Спасибо за покупку!";
        }

        // Отправка чека по email
        private bool SendReceiptEmail(string recipientEmail, string receipt)
        {
            try
            {
                // Настройки SMTP (пример для Gmail)
                string smtpServer = "smtp.gmail.com";
                int smtpPort = 587;
                string smtpUsername = "your-email@gmail.com"; // Замените на ваш email
                string smtpPassword = "your-app-password"; // Используйте App Password от Gmail

                using (var client = new SmtpClient(smtpServer, smtpPort))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(smtpUsername, "Payment Service"),
                        Subject = "Чек за оплату плана",
                        Body = receipt,
                        IsBodyHtml = false
                    };
                    mailMessage.To.Add(recipientEmail);

                    client.Send(mailMessage);
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Логирование ошибки (в реальном приложении)
                Console.WriteLine($"Ошибка отправки email: {ex.Message}");
                return false;
            }
        }
    }
}