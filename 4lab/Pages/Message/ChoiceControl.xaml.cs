using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
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
                Width = 250,
                Height = 150,
                AllowsTransparency = true,
                PopupAnimation = PopupAnimation.Fade
            };

            var popupContent = new Border
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#333333")),
                CornerRadius = new CornerRadius(10),
                BorderBrush = Brushes.White,
                BorderThickness = new Thickness(1),
                Padding = new Thickness(15)
            };

            var mainStack = new StackPanel();

            // Заголовок - используем стиль из ресурсов UserControl
            var roomLabel = new TextBlock
            {
                Text = "Введите номер комнаты:",
                Style = (Style)TryFindResource("TextBlockStyle"),
                FontWeight = FontWeights.Bold,
                FontSize = 16,
                Margin = new Thickness(0, 0, 0, 10),
                Foreground = Brushes.White
            };

            // Поле ввода - кастомный стиль
            var roomInput = new TextBox
            {
                Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4A4A4A")),
                BorderBrush = Brushes.White,
                BorderThickness = new Thickness(1),
                Margin = new Thickness(0, 0, 0, 10),
                Foreground = Brushes.White,
                FontSize = 14,
                Padding = new Thickness(5)
            };

            // Кнопка отправки - используем стиль из ресурсов UserControl
            var sendButton = new Button
            {
                Content = "Отправить",
                Style = (Style)TryFindResource("ButtonStyle"),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(0, 5, 0, 0),
                Width = double.NaN, // Сбрасываем фиксированную ширину
                Height = 30
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

            mainStack.Children.Add(roomLabel);
            mainStack.Children.Add(roomInput);
            mainStack.Children.Add(sendButton);

            popupContent.Child = mainStack;
            _roomPopup.Child = popupContent;
        }

        private void SendRoomNumber(string roomNumber)
        {
            try
            {
                using (var context = new DBContext())
                {
                    // Отправка сообщения с номером комнаты
                    MessageService.SendMessage(
                        CurrentUser.Instance.GetCurrentUser().Id,
                        _message.SenderId,
                        $"Номер комнаты для игры: {roomNumber}",
                        MessageType.Default);

                    // Если сообщение связано с офером, помечаем его как завершенный
                    if (_message.OfferId.HasValue)
                    {
                        var offer = context.TeamOffers.Find(_message.OfferId.Value);
                        if (offer != null)
                        {
                            offer.Resolved = true;
                            context.SaveChanges();
                        }
                    }

                    MessageBox.Show("Номер комнаты отправлен");
                    OnChoiceMade?.Invoke(this, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            if ( _message.MessageType == MessageType.TeamInvitation)
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
            }
            else if (_message.MessageType == MessageType.TeamOffer)
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