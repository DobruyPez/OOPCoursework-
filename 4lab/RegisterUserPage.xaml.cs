using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
using Roles;

namespace _4lab
{
    /// <summary>
    /// Логика взаимодействия для RegisterUserPage.xaml
    /// </summary>
    public partial class RegisterUserPage : Page
    {
        public RegisterUserPage()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // Валидация данных
            if (string.IsNullOrWhiteSpace(UsernameBox.Text) ||
                string.IsNullOrWhiteSpace(EmailBox.Text) ||
                string.IsNullOrWhiteSpace(PasswordBox.Password) ||
                PasswordBox.Password != ConfirmPasswordBox.Password)
            {
                MessageBox.Show("Please fill all fields correctly and ensure passwords match.");
                return;
            }

            try
            {
                // Создание нового Player
                var newPlayer = new Player
                {
                    Username = UsernameBox.Text,
                    Email = EmailBox.Text,
                    PasswordHash = HashPassword(PasswordBox.Password),
                    Subscription = SubscriptionType.Light // Значение по умолчанию для нового игрока
                };

                // Сохранение в БД
                using (var context = new DBContext())
                {
                    // Проверка на существующего пользователя
                    if (context.Users.Any(u => u.Username == newPlayer.Username || u.Email == newPlayer.Email))
                    {
                        MessageBox.Show("Username or email already exists.");
                        return;
                    }

                    context.Users.Add(newPlayer);
                    context.SaveChanges();
                }
                NavigationService.Navigate(new UserProfilePage(newPlayer.Username, newPlayer.Email));


                // Возврат на предыдущую страницу или главное меню
                if (NavigationService.CanGoBack)
                {
                    NavigationService.GoBack();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Registration failed: {ex.Message}");
            }
        }

        // Простое хэширование пароля
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

    }
}

