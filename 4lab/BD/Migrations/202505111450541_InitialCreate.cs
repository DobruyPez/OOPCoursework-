namespace _4lab.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "public.User",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
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
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "public.TeamOffer",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Team1Id = c.Int(nullable: false),
                        Team2Id = c.Int(),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Team1Id);
            
            CreateTable(
                "public.Team",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Region = c.String(),
                        Password = c.String(),
                        OwnerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.OwnerId);
            
        }
        
        public override void Down()
        {
            DropIndex("public.Team", new[] { "OwnerId" });
            DropIndex("public.TeamOffer", new[] { "Team1Id" });
            DropTable("public.Team");
            DropTable("public.TeamOffer");
            DropTable("public.User");
        }
    }
}
