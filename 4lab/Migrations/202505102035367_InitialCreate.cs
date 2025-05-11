namespace _4lab.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            // Переименование таблицы Teams в Team (если Teams существует)
            RenameTable(name: "dbo.Teams", newName: "Team");

            // Создание таблицы User с нуля
            CreateTable(
                "dbo.User",
                c => new
                {
                    Id = c.Int(nullable: false),
                    Email = c.String(),
                    Username = c.String(),
                    PasswordHash = c.String(),
                    Role = c.Int(),
                    Subscription = c.Int(),
                    TwitchLink = c.String(),
                    DiscordLink = c.String(),
                    TeamId = c.Int(),
                    Discriminator = c.String(nullable: false, maxLength: 128),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.TeamId);

            // Установка SERIAL для Id в User
            Sql("ALTER TABLE dbo.User ALTER COLUMN Id SET DATA TYPE serial;");

            // Создание таблицы TeamOffer
            CreateTable(
                "dbo.TeamOffer",
                c => new
                {
                    Id = c.Int(nullable: false),
                    Name = c.String(),
                    Team1Id = c.Int(nullable: false),
                    Team2Id = c.Int(),
                    Date = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Team1Id)
                .Index(t => t.Team2Id);

            // Установка SERIAL для Id в TeamOffer
            Sql("ALTER TABLE dbo.TeamOffer ALTER COLUMN Id SET DATA TYPE serial;");

            // Добавление столбцов в таблицу Team
            AddColumn("dbo.Team", "Password", c => c.String());
            AddColumn("dbo.Team", "OwnerId", c => c.Int(nullable: false));

            // Установка SERIAL для Id в Team (если не настроено ранее)
            Sql("ALTER TABLE dbo.Team ALTER COLUMN Id SET DATA TYPE serial;");

            // Создание индекса для OwnerId
            CreateIndex("dbo.Team", "OwnerId");

            // Удаление столбца PasswordHash из Team
            DropColumn("dbo.Team", "PasswordHash");

            // Удаление таблицы Users, если она существует
            Sql("DROP TABLE IF EXISTS dbo.Users;");
        }

        public override void Down()
        {
            // Восстановление столбца PasswordHash в Team
            AddColumn("dbo.Team", "PasswordHash", c => c.String());

            // Удаление индексов
            DropIndex("dbo.Team", new[] { "OwnerId" });
            DropIndex("dbo.User", new[] { "TeamId" });
            DropIndex("dbo.TeamOffer", new[] { "Team2Id" });
            DropIndex("dbo.TeamOffer", new[] { "Team1Id" });

            // Удаление столбцов из Team
            DropColumn("dbo.Team", "OwnerId");
            DropColumn("dbo.Team", "Password");

            // Удаление таблиц TeamOffer и User
            DropTable("dbo.TeamOffer");
            DropTable("dbo.User");

            // Восстановление таблицы Users
            CreateTable(
                "dbo.Users",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Username = c.String(),
                    Email = c.String(),
                    PasswordHash = c.String(),
                })
                .PrimaryKey(t => t.Id);

            // Переименование Team обратно в Teams
            RenameTable(name: "dbo.Team", newName: "Teams");

            // Откат SERIAL для Team
            Sql("ALTER TABLE dbo.Teams ALTER COLUMN Id SET DATA TYPE integer;");
        }
    }
}