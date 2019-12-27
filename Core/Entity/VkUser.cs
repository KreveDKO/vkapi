using System;
using System.Collections.Generic;

namespace Core.Entity
{
    public class VkUser
    {
        public long Id { get; set; }
        public bool IsDeactivated { get; set; }
        public string FullName { get; set; }
        public string PhotoUrl { get; set; }
        public virtual  List<VkUser> MuturalFriend { get; set; }
        public long UserId { get; set; }
        public DateTime LastCheck { get; set; }

    }
}
