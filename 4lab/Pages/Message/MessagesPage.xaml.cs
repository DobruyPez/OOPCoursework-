using System.Linq;
using System.Windows;
using System.Windows.Controls;
using _4lab.BD;
using Roles;

namespace _4lab
{
    public partial class MessagesPage : Page
    {
        public MessagesPage()
        {
            InitializeComponent();
            LoadMessages();
        }

        private void LoadMessages()
        {
            using (var context = new DBContext())
            {
                var currPlayId = CurrentUser.Instance.GetCurrentUser().Id;
                var unreadMessages = context.Messages
                    .Include("Sender")
                    .Where(m => !m.IsRead)
                    .Where(m => m.ReceiverId == currPlayId)
                    .ToList();

                if (unreadMessages.Count == 0)
                {
                    NoMessagesText.Visibility = Visibility.Visible;
                    return;
                }

                foreach (var message in unreadMessages)
                {
                    var control = new ChoiceControl(message);
                    control.OnChoiceMade += (sender, e) =>
                    {
                        using (var ctx = new DBContext())
                        {
                            var msg = ctx.Messages.Find(message.MessageId);
                            if (msg != null)
                            {
                                msg.IsRead = true;
                                ctx.SaveChanges();
                            }
                        }
                        MessagesContainer.Children.Remove(control);

                        // Проверяем, остались ли еще сообщения
                        if (MessagesContainer.Children.Count == 0)
                        {
                            NoMessagesText.Visibility = Visibility.Visible;
                        }
                    };

                    MessagesContainer.Children.Add(control);
                }
            }
        }
    }
}