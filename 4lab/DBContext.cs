using System.Data.Entity;
using Roles;

public class DBContext : DbContext
{
    public DBContext() : base("name=MyAppDbContext")
    {
    }

    public DbSet<User> Users { get; set; }
}