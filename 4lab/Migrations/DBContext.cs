using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Roles;

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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
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

            // Настройка TeamOffer
            modelBuilder.Entity<TeamOffer>()
                .HasKey(to => to.Id)
                .Property(to => to.Team1Id)
                .IsRequired(); // Team1Id обязателен

            // Внешние ключи
            modelBuilder.Entity<Player>()
                .Property(p => p.TeamId)
                .IsOptional(); // TeamId опционален

            modelBuilder.Entity<Team>()
                .HasIndex(t => t.OwnerId); // Индекс для OwnerId

            modelBuilder.Entity<TeamOffer>()
                .HasIndex(to => to.Team1Id);
                //.HasIndex(to => to.Team2Id); // Индексы для Team1Id и Team2Id
        }
    }
}