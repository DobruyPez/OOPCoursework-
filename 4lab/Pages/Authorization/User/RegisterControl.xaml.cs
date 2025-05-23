﻿using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using _4lab.DB;
using Roles;
using static Roles.CurrentTeam;

namespace _4lab
{
    public partial class RegisterControl : UserControl
    {
        public RegisterControl()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameBox.Text;
            string email = EmailBox.Text;
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            // Проверка на пустые поля
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Все поля должны быть заполнены.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Проверка на отсутствие русских символов
            if (Regex.IsMatch(username, @"[а-яА-ЯёЁ]") || Regex.IsMatch(email, @"[а-яА-ЯёЁ]") || Regex.IsMatch(password, @"[а-яА-ЯёЁ]"))
            {
                MessageBox.Show("Имя пользователя, email и пароль не должны содержать русские символы.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Проверка формата email
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]{2,}$"))
            {
                MessageBox.Show("Введите корректный email.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Проверка совпадения паролей
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
                        Name = username,
                        Email = email,
                        PasswordHash = hashedPassword,
                        Role = UserRole.Player,
                    };

                    context.Users.Add(user);
                    context.SaveChanges();

                    CurrentUser.Instance.SetUser(user);

                    if (user.TeamId.HasValue)
                    {
                        CurrentTeam.SetCurrentTeam(user.TeamId.Value);
                    }

                    MessageBox.Show("Регистрация прошла успешно!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    var page = this.FindAncestor<Page>();
                    page?.NavigationService.Navigate(new MainMenuPage((MainWindow)Window.GetWindow(this)));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при регистрации: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}