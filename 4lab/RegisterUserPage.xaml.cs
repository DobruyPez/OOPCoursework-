using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using _4lab.DB;
using _4lab.Migrations;
using Roles;

namespace _4lab
{
    public partial class RegisterUserPage : Page
    {
        public RegisterUserPage()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameBox.Text;
            string email = EmailBox.Text;
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Все поля должны быть заполнены.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Введите корректный email.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Пароли не совпадают.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string hashedPassword = DataBaseInteractor.HashPassword(password);

            try
            {
                using (var context = new _4lab.BD.DBContext())
                {
                    if (context.Users.Any(u => u.Email == email))
                    {
                        MessageBox.Show("Пользователь с таким email уже существует.", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    var user = new Player
                    {
                        Username = username,
                        Email = email,
                        PasswordHash = hashedPassword
                    };

                    context.Users.Add(user);
                    context.SaveChanges();

                    CurrentUser.Instance.SetUser(user);
                    MessageBox.Show("Регистрация прошла успешно!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    NavigationService.Navigate(new MainMenuPage((Window.GetWindow(this) as MainWindow)));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при регистрации: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailBox.Text;
            string password = PasswordBox.Password;
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Email и пароль должны быть заполнены.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var context = new _4lab.BD.DBContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Email == email);
                if (user == null || !DataBaseInteractor.VerifyPassword(hashedPassword, user.PasswordHash))
                {
                    MessageBox.Show("Неверный email или пароль.", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Player player = null;
                Team team = null;
                if (user.Role == UserRole.Player)
                {
                    player = context.Players.FirstOrDefault(p => p.Id == user.Id);
                }

                CurrentUser.Instance.Login(null, player);
                MessageBox.Show("Вход выполнен успешно!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.Navigate(new MainMenuPage((MainWindow)Window.GetWindow(this)));
            }
        }
    }
}