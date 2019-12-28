using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

    }
}
