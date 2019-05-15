using System;
using System.Collections.Generic;

namespace Core.Entity
{
    public class User
    {
        public long Id { get; set; }
        public bool IsDeactivated { get; set; }
        public string FullName { get; set; }
        public string PhotoUrl { get; set; }
        public int MuturalCount { get; set; }
        public virtual  List<User> MuturalFriend { get; set; }
        public long UserId { get; set; }
        public DateTime LastCheck { get; set; }

    }
}
