using Microsoft.EntityFrameworkCore;

namespace ActivityCenter.Models
{
    public class MyContext : DbContext
    {
       public MyContext(DbContextOptions options) : base(options) {}
       public DbSet<User> users {get;set;}
       public DbSet<Plan> plans {get;set;}
       public DbSet<Association> associations {get;set;}
    }
}