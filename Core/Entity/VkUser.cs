using System;

namespace Core.Entity
{
    public class VkUser
    {
        public long Id { get; set; }
        public bool IsDeactivated { get; set; }
        public string? FullName { get; set; }
        public string? PhotoUrl { get; set; }
        public long ExternalId { get; set; }
        public DateTime LastCheck { get; set; }

        public int? BirthDay { get; set; }

        public int? BirthMonth { get; set; }

        public int? BirthYear { get; set; }



    }
}
