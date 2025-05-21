using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using _4lab.DB;
using System.Linq;
using _4lab.Resources;
using Roles;
using static Roles.CurrentTeam;

namespace _4lab.BD
{
    public class DBContext : DbContext
    {
        public DBContext() : base("name=MyAppDbContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DBContext, _4lab.Migrations.Configuration>()); Database.Log = s => System.Diagnostics.Debug.WriteLine(s);

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamOffer> TeamOffers { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<SubscriptionPrice> SubscriptionPrices { get; set; }
        public DbSet<Advertisement> Advertisements { get; set; }

        public void SeedInitialAdmin()
        {
            if (!Users.Any(u => u.Role == UserRole.Admin))
            {
                var admin = new Admin
                {
                    Email = "admin@example.com",
                    Name = "Administrator",
                    PasswordHash = DataBaseInteractor.HashPassword("admin123"), // Реализуйте этот метод
                    Role = UserRole.Admin
                };

                Users.Add(admin);
                SaveChanges();
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            // Отключение PluralizingTableNameConvention для точного соответствия именам таблиц
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // Настройка TPH для User, Player, Admin
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id)
                .Map<User>(m => m.Requires("Discriminator").HasValue("User"))
                .Map<Player>(m => m.Requires("Discriminator").HasValue("Player"))
                .Map<Admin>(m => m.Requires("Discriminator").HasValue("Admin"));

            // Настройка Team
            modelBuilder.Entity<Team>()
                .HasKey(t => t.Id)
                .Property(t => t.OwnerId)
                .IsRequired(); // OwnerId обязателен (один создатель)

            // Внешние ключи
            modelBuilder.Entity<Player>()
                .Property(p => p.TeamId)
                .IsOptional(); // TeamId опционален

            modelBuilder.Entity<Team>()
                .HasIndex(t => t.OwnerId); // Индекс для OwnerId

            modelBuilder.Entity<TeamMember>()
                .HasRequired(m => m.Team)
                .WithMany(t => t.Members)
                .HasForeignKey(m => m.TeamId);

            modelBuilder.Entity<Message>()
                .HasRequired(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Message>()
                .HasRequired(m => m.Receiver)
                .WithMany()
                .HasForeignKey(m => m.ReceiverId)
                .WillCascadeOnDelete(false);

            // Конфигурация для enum
            modelBuilder.Entity<Message>()
                 .Property(m => m.MessageTypeString)
                 .HasColumnName("message_type") // Должно совпадать с [Column]
                 .IsRequired()
                 .HasMaxLength(20)
                 .IsUnicode(false)
                 .HasColumnType("varchar");
            
            // Настройка TeamOffer с новой структурой
            modelBuilder.Entity<TeamOffer>()
                .HasKey(to => to.Id);

            modelBuilder.Entity<TeamOffer>()
                .Property(to => to.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<TeamOffer>()
                .Property(to => to.CreatorId)
                .IsRequired();

            modelBuilder.Entity<TeamOffer>()
                .Property(to => to.Maps)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<TeamOffer>()
                .Property(to => to.Date)
                .HasColumnType("timestamp")
                .IsRequired();

            // Внешний ключ для CreatorId
            modelBuilder.Entity<TeamOffer>()
                .HasRequired(to => to.Creator)
                .WithMany()
                .HasForeignKey(to => to.CreatorId)
                .WillCascadeOnDelete(false);

            // Индексы для быстрого поиска
            modelBuilder.Entity<TeamOffer>()
                .HasIndex(to => to.CreatorId);

            modelBuilder.Entity<TeamOffer>()
                .HasIndex(to => to.Date);

            modelBuilder.Entity<TeamOffer>()
                .HasIndex(to => to.Resolved);

            modelBuilder.Entity<Message>()
                .HasOptional(m => m.TeamOffer)
                .WithMany()
                .HasForeignKey(m => m.OfferId)
                .WillCascadeOnDelete(false); 
            modelBuilder.Entity<SubscriptionPrice>()
                .Property(p => p.LitePrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<SubscriptionPrice>()
                .Property(p => p.SemiProPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<SubscriptionPrice>()
                .Property(p => p.ProPrice)
                .HasPrecision(18, 2);
            modelBuilder.Entity<Advertisement>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<Advertisement>()
                .Property(a => a.Image)
                .IsRequired()
                .HasMaxLength(500);

            modelBuilder.Entity<Advertisement>()
                .Property(a => a.Link)
                .IsRequired()
                .HasMaxLength(500);

        }
    }
}