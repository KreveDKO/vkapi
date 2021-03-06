﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity
{
    public class FriendsUserToUser
    {
        public long Id { get; set; }

        [ForeignKey("LeftUser")]
        public long LeftUserId { get; set; }

        public virtual VkUser LeftUser { get; set; }

        [ForeignKey("RightUser")]
        public long RightUserId { get; set; }

        public virtual VkUser RightUser { get; set; }
    
    }
}
