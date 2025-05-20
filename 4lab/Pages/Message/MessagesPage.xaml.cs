using System.Linq;
using System.Windows;
using System.Windows.Controls;
using _4lab.BD;

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
                var unreadMessages = context.Messages
                    .Include("Sender")
                    .Where(m => !m.IsRead)
                    .ToList();

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
                    };

                    MessagesContainer.Children.Add(control);
                }
            }
        }
    }
}