using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using _4lab.BD;
using _4lab.Resources;
using Roles;

namespace _4lab
{
    public partial class ChoiceControl : UserControl
    {
        public event EventHandler OnChoiceMade;
        private readonly Message _message;
        private Popup _roomPopup;

        public ChoiceControl(Message message)
        {
            InitializeComponent();
            _message = message;
            LoadData();
            InitializePopup();
        }

        private void LoadData()
        {
            using (var context = new DBContext())
            {
                var sender = context.Users.Find(_message.SenderId);
                SenderName.Text = sender?.Name ?? "Unknown";
                MessageContent.Text = _message.Content;

                if (_message.MessageType == MessageType.Default)
                {
                    AcceptButton.Visibility = Visibility.Collapsed;
                    DeclineButton.Margin = new Thickness(0, 0, 0, 0); // Убираем отступ
                }
            }
        }

        private void InitializePopup()
        {
            _roomPopup = new Popup
            {
                Placement = PlacementMode.Bottom,
                PlacementTarget = AcceptButton,
                StaysOpen = false,
                Width = 200,
                AllowsTransparency = true
            };

            var popupContent = new StackPanel
            {
                Background = System.Windows.Media.Brushes.White,
            };

            var roomLabel = new TextBlock { Text = "Введите номер комнаты:" };
            var roomInput = new TextBox { Margin = new Thickness(0, 5, 0, 5) };
            var sendButton = new Button
            {
                Content = "➤",
                HorizontalAlignment = HorizontalAlignment.Right,
                Style = (Style)FindResource("ButtonStyle")
            };

            sendButton.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(roomInput.Text))
                {
                    MessageBox.Show("Пожалуйста, введите номер комнаты");
                    return;
                }

                SendRoomNumber(roomInput.Text);
                _roomPopup.IsOpen = false;
            };

            popupContent.Children.Add(roomLabel);
            popupContent.Children.Add(roomInput);
            popupContent.Children.Add(sendButton);

            _roomPopup.Child = popupContent;
        }

        private void SendRoomNumber(string roomNumber)
        {
            try
            {
                var responseMessage = $"Номер комнаты для переговоров: {roomNumber}";
                MessageService.SendMessage(
                    CurrentUser.Instance.GetCurrentUser().Id,
                    _message.SenderId,
                    responseMessage,
                    MessageType.Default);

                MessageBox.Show("Номер комнаты отправлен", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                OnChoiceMade?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при отправке номера комнаты: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            if(CurrentUser.Instance.GetCurrentUser() is Player player && player.TeamId == null)
            {
                try
                {
                    using (var context = new DBContext())
                    {
                        // Получаем капитана команды (отправителя приглашения)
                        var team = context.Teams
                            .FirstOrDefault(p => p.OwnerId == _message.SenderId);

                        CurrentTeam.SetCurrentTeam(team.Id);
                        if (CurrentTeam.Team != null)
                        {
                            // Добавляем игрока в команду через CurrentTeam
                            CurrentTeam.AddTeamMember(
                                player.Id,
                                player.Name,
                                TeamRole.Member); // Или другая роль по умолчанию

                            // Обновляем данные текущего пользователя
                            CurrentUser.Instance.UpdatePlayerTeam(team.Id);
                            CurrentUser.Instance.UpdateBDLink();

                            // Помечаем сообщение как прочитанное
                            MessageService.MarkAsRead(_message.MessageId);

                            MessageBox.Show("Вы успешно присоединились к команде!", "Успех",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Не удалось найти команду отправителя", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при принятии приглашения: {ex.Message}", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Вы уже состоите в другой команде", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            if (_message.MessageType == MessageType.TeamOffer)
            {
                _roomPopup.IsOpen = true;
                return;
            }

            OnChoiceMade?.Invoke(this, EventArgs.Empty);
        }

        private void DeclineButton_Click(object sender, RoutedEventArgs e)
        {
            MessageService.MarkAsRead(_message.MessageId);
            OnChoiceMade?.Invoke(this, EventArgs.Empty);
        }
    }
}