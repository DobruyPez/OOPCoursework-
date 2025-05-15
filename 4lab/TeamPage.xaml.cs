using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

namespace _4lab
{
    public partial class TeamPage : Page
    {
        // Переменная для хранения описания команды (замените на вашу модель данных, если есть)
        private string teamDescription = "Описание команды будет здесь...";

        public TeamPage()
        {
            InitializeComponent();
            // Устанавливаем начальное описание в TextBox
            TeamDescriptionBox.Text = teamDescription;
        }

        // Обработчик выбора участника в списке
        private void MembersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Логика при выборе участника (можно оставить пустым, если не используется)
        }

        // Обработчик для кнопки "Сохранить описание"
        private void SaveDescriptionButton_Click(object sender, RoutedEventArgs e)
        {
            // Сохраняем описание из TextBox
            teamDescription = TeamDescriptionBox.Text;
            MessageBox.Show("Описание команды успешно сохранено!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Обработчик для кнопки "Пригласить участника"
        private void InviteMemberButton_Click(object sender, RoutedEventArgs e)
        {
            // Здесь можно добавить логику приглашения участника, например, открыть диалоговое окно
            MessageBox.Show("Открыть форму приглашения участника?", "Пригласить участника", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Обработчик для кнопки "Изменить роль"
        private void ChangeRoleButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, выбран ли участник
            if (MembersList.SelectedItem != null)
            {
                // Здесь можно добавить логику изменения роли, например, открыть диалоговое окно
                MessageBox.Show($"Изменение роли для участника: {MembersList.SelectedItem}", "Изменить роль", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите участника из списка.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Обработчик для кнопки "Удалить"
        private void RemoveMemberButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, выбран ли участник
            if (MembersList.SelectedItem != null)
            {
                // Здесь можно добавить логику удаления участника
                MessageBox.Show($"Удаление участника: {MembersList.SelectedItem}", "Удалить участника", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите участника из списка.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}