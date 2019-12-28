using Core.Entity;
using Microsoft.EntityFrameworkCore;

namespace Core.DataContext
{
    public class ApplicationContext : DbContext
    {
        public DbSet<VkUser> VkUsers { get; set; }
        public DbSet<FriendsUserToUser> FriendsUserToUsers { get; set; }

        public DbSet<VkMessage> VkMessages { get; set; }

        public DbSet<VkUserGroup> UserGroups { get; set; }
        public DbSet<UserToUserGroup> UserToUserGroup { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            optionsBuilder
                .UseNpgsql("Host=localhost;Port=5432;Database=myfriends;Username=postgres;Password=123").UseLazyLoadingProxies();
        }

    }
}
