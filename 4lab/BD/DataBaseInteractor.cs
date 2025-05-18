using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Roles;
using System.Security.Cryptography;

namespace _4lab.DB
{
    internal class DataBaseInteractor
    {
        public static void CreateDatabase()
        {
            using (var context = new _4lab.BD.DBContext())
            {
                // Убедимся, что база данных создана
                if (!context.Database.Exists())
                {
                    context.Database.Create();
                    Seed(context);
                    context.SaveChanges();
                }
            }
        }

        public static string HashPassword(string password)
        {

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            return hashedPassword;
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        // Метод для удаления базы данных
        public static void DeleteDatabase()
        {
            using (var context = new _4lab.BD.DBContext())
            {
                if (context.Database.Exists())
                {
                    context.Database.Delete();
                }
            }
        }

        // Метод для изменения структуры базы данных
        public static void ModifyDatabase()
        {
            using (var context = new _4lab.BD.DBContext())
            {
                if (context.Database.Exists())
                {
                    Seed(context);
                    context.SaveChanges();
                }
            }
        }

        // Вспомогательный метод для начального заполнения (Seed)
        private static void Seed(_4lab.BD.DBContext context)
        {
            if (!context.Users.Any())
            {
                // Добавляем базовых User
                context.Users.Add(new User { Username = "John Doe", Email = "john@example.com", PasswordHash = "hashedpassword" });
                context.Users.Add(new User { Username = "Jane Smith", Email = "jane@example.com", PasswordHash = "hashedpassword" });

                // Добавляем Player с подпиской
                context.Users.Add(new Player { Username = "Alex Player", Email = "alex@example.com", PasswordHash = "hashedpassword", Subscription = SubscriptionType.Pro });
            }
        }
    }
}