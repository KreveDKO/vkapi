using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entity
{
    public class VkWallMessage
    {
        public long Id { get; set; }

        public string Text { get; set; }


        public int Views { get; set; }

        public int Likes { get; set; }

        [ForeignKey("VkUser")]
        public long VkUserId { get; set; }

        public VkUser VkUser { get; set; }


        public long ExternalId { get; set; }
    }
}
