using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Roles;

namespace _4lab
{
    internal class DataBaseInteractor
    {
        public static void CreateDatabase()
        {
            using (var context = new DBContext())
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

        // Метод для удаления базы данных
        public static void DeleteDatabase()
        {
            using (var context = new DBContext())
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
            using (var context = new DBContext())
            {
                if (context.Database.Exists())
                {
                    Seed(context);
                    context.SaveChanges();
                }
            }
        }

        // Вспомогательный метод для начального заполнения (Seed)
        private static void Seed(DBContext context)
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