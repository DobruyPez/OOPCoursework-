using System;
using System.Collections.Generic;
using System.Linq;
using Roles;
using System.Windows;

namespace _4lab.Resources
{
    public static class MessageService
    {
        public static void SendMessage(int senderId, int receiverId, string content, MessageType messageType)
        {
            using (var context = new _4lab.BD.DBContext())
            {
                var message = new Message
                {
                    SenderId = senderId,
                    ReceiverId = receiverId,
                    Content = content,
                    IsRead = false,
                    MessageType = messageType
                };

                context.Messages.Add(message);
                context.SaveChanges();
            }
        }

        public static bool CanSendMessage(int senderId, int receiverId)
        {
            using (var context = new _4lab.BD.DBContext())
            {
                return context.Users.Any(u => u.Id == senderId) &&
                       context.Users.Any(u => u.Id == receiverId);
            }
        }

        public static List<Message> GetMessagesBetweenUsers(int userId1, int userId2)
        {
            using (var context = new _4lab.BD.DBContext())
            {
                return context.Messages
                    .Where(m => (m.SenderId == userId1 && m.ReceiverId == userId2) ||
                                (m.SenderId == userId2 && m.ReceiverId == userId1))
                    .OrderBy(m => m.MessageId)
                    .ToList();
            }
        }

        public static List<Message> GetUnreadMessages(int userId)
        {
            using (var context = new _4lab.BD.DBContext())
            {
                return context.Messages
                    .Where(m => m.ReceiverId == userId && !m.IsRead)
                    .ToList();
            }
        }

        public static List<Message> GetMessagesByType(int userId, MessageType messageType)
        {
            using (var context = new _4lab.BD.DBContext())
            {
                return context.Messages
                    .Where(m => m.ReceiverId == userId && m.MessageType == messageType)
                    .ToList();
            }
        }

        public static List<Message> GetAllMessages(int userId)
        {
            using (var context = new _4lab.BD.DBContext())
            {
                return context.Messages
                    .Where(m => m.ReceiverId == userId || m.SenderId == userId)
                    .ToList();
            }
        }

        public static void MarkAsRead(int messageId)
        {
            using (var context = new _4lab.BD.DBContext())
            {
                var message = context.Messages.Find(messageId);
                if (message != null)
                {
                    message.IsRead = true;
                    context.SaveChanges();
                }
            }
        }

        public static void DeleteMessage(int messageId)
        {
            using (var context = new _4lab.BD.DBContext())
            {
                var message = context.Messages.Find(messageId);
                if (message != null)
                {
                    context.Messages.Remove(message);
                    context.SaveChanges();
                }
            }
        }
    }
}