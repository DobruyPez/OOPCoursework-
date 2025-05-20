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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace _4lab
{
    public partial class PaymentPage : Page
    {
        private readonly string selectedPlan;

        public PaymentPage(string selectedPlan)
        {
            InitializeComponent(); // Убедитесь, что этот метод вызывается первым
            if (PlanTitle == null || StatusMessage == null || CardNumber == null || ExpiryDate == null ||
                Cvv == null || CardHolder == null || Email == null || PayButton == null)
            {
                MessageBox.Show("Ошибка: Один или несколько элементов интерфейса не найдены в XAML.");
                return;
            }
            this.selectedPlan = selectedPlan;
            PlanTitle.Text = $"Оплата плана {selectedPlan}";
        }

        private void PayButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверка инициализации элементов
            if (StatusMessage == null || CardNumber == null || ExpiryDate == null || Cvv == null ||
                CardHolder == null || Email == null)
            {
                StatusMessage.Text = "Ошибка: Элементы интерфейса не инициализированы.";
                StatusMessage.Foreground = new SolidColorBrush(Colors.Red);
                return;
            }

            // Сбрасываем сообщение и видимость перед новой проверкой
            StatusMessage.Text = "";
            StatusMessage.Visibility = Visibility.Visible;
            StatusMessage.Foreground = new SolidColorBrush(Colors.White);

            // Проверка номера карты (ровно 16 цифр)
            if (string.IsNullOrWhiteSpace(CardNumber.Text) || !Regex.IsMatch(CardNumber.Text.Trim(), @"^\d{16}$"))
            {
                StatusMessage.Text = "Неверный номер карты (требуется 16 цифр)";
                StatusMessage.Foreground = new SolidColorBrush(Colors.Red);
                return;
            }

            // Проверка срока действия (ММ/ГГ, например 05/25)
            if (string.IsNullOrWhiteSpace(ExpiryDate.Text) || !Regex.IsMatch(ExpiryDate.Text.Trim(), @"^(0[1-9]|1[0-2])\/\d{2}$"))
            {
                StatusMessage.Text = "Неверный срок действия (формат ММ/ГГ)";
                StatusMessage.Foreground = new SolidColorBrush(Colors.Red);
                return;
            }

            // Проверка CVV (ровно 3 цифры)
            if (string.IsNullOrWhiteSpace(Cvv.Text) || !Regex.IsMatch(Cvv.Text.Trim(), @"^\d{3}$"))
            {
                StatusMessage.Text = "Неверный CVV (требуется 3 цифры)";
                StatusMessage.Foreground = new SolidColorBrush(Colors.Red);
                return;
            }

            // Проверка имени владельца (не пустое и только буквы/пробелы)
            if (string.IsNullOrWhiteSpace(CardHolder.Text) || !Regex.IsMatch(CardHolder.Text.Trim(), @"^[a-zA-Z\s]+$"))
            {
                StatusMessage.Text = "Введите имя владельца (только буквы и пробелы)";
                StatusMessage.Foreground = new SolidColorBrush(Colors.Red);
                return;
            }

            // Проверка email (строгая валидация)
            if (string.IsNullOrWhiteSpace(Email.Text) || !IsValidEmail(Email.Text.Trim()))
            {
                StatusMessage.Text = "Введите корректный email (например, example@domain.com)";
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
            return true; // Имитация успешного платежа
        }

        // Формирование чека
        private string GenerateReceipt(string plan, string cardHolder, string email)
        {
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
                // Проверка валидности email получателя
                if (string.IsNullOrWhiteSpace(recipientEmail) || !IsValidEmail(recipientEmail))
                {
                    Console.WriteLine("Ошибка: Некорректный email получателя.");
                    return false;
                }

                // Настройки SMTP для mail
                string smtpServer = "smtp.mail.ru";
                int smtpPort = 587; // Или 587 для TLS
                string smtpUsername = "fonksilas@mail.ru";
                string smtpPassword = "mU11M05k2L4YzjiJwfcF";

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
                    mailMessage.To.Add(recipientEmail); // Отправка на любой указанный email (Gmail, Mail.ru, Яндекс и т.д.)

                    client.Send(mailMessage);
                    Console.WriteLine($"Письмо успешно отправлено на {recipientEmail}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка отправки email: {ex.Message}");
                return false;
            }
        
        }
    }
}