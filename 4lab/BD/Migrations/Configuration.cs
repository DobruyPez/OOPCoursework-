namespace _4lab.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Roles;

    internal sealed class Configuration : DbMigrationsConfiguration<_4lab.BD.DBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "DBContext";
        }

        protected override void Seed(_4lab.BD.DBContext context)
        {
            // Здесь можно добавить начальные данные, если нужно
            if (!context.Users.Any())
            {
                context.Users.Add(new User { Name = "John Doe", Email = "john@example.com", PasswordHash = "hashedpassword" });
                context.SaveChanges();
            }
        }
    }
}
