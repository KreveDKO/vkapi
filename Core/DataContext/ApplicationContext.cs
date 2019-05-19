using Core.Entity;
using Microsoft.EntityFrameworkCore;

namespace Core.DataContext
{
    public class ApplicationContext : DbContext
    {

        public DbSet<User> Users { get; set; }
        public DbSet<FriendsUserToUser> FriendsUserToUsers { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=friends;Username=postgres;Password=123").UseLazyLoadingProxies();
        }

    }
}
