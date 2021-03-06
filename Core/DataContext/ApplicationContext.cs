﻿using Core.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Core.DataContext
{
    public class ApplicationContext : DbContext
    {


        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }
        
        
        public DbSet<VkWallMessage> VkWallMessages { get; set; }
        public DbSet<VkUser> VkUsers { get; set; }
        public DbSet<FriendsUserToUser> FriendsUserToUsers { get; set; }

        public DbSet<VkMessage> VkMessages { get; set; }

        public DbSet<VkUserGroup> UserGroups { get; set; }
        public DbSet<UserToUserGroup> UserToUserGroup { get; set; }

        public DbSet<VkAudio> VkAudio { get; set; }

        public DbSet<VkAudioArtist> VkAudioArtist { get; set; }

        public DbSet<VkAudioToArtist> VkAudioToArtist { get; set; }
        
        public DbSet<VkAudioToUser> VkAudioToUser { get; set; }
        
        

    }
}
