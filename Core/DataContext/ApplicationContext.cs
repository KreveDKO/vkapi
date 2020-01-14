using Core.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Core.DataContext
{
    public class ApplicationContext : DbContext
    {


        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }
        
        
        
        public DbSet<VkUser> VkUsers { get; set; }
        public DbSet<FriendsUserToUser> FriendsUserToUsers { get; set; }

        public DbSet<VkMessage> VkMessages { get; set; }

        public DbSet<VkUserGroup> UserGroups { get; set; }
        public DbSet<UserToUserGroup> UserToUserGroup { get; set; }

    }
}
