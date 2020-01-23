using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime;
using System.Text;

namespace Core.Entity
{
    public class VkWallMessage
    {
        public long Id { get; set; }

        public string Text { get; set; }

        public int AttachmentsCount { get; set; }
        
        public int ViewsCount { get; set; }

        public int LikesCount { get; set; }
        
        public int CommentsCount { get; set; }

        public int RepostsCount { get; set; }

        [ForeignKey("VkUser")]
        public long? VkUserId { get; set; }

        public virtual VkUser? VkUser { get; set; }
        
        [ForeignKey("UserGroup")]
        public long? VkGroupId { get; set; }

        public virtual VkUserGroup? UserGroup { get; set; }

        [ForeignKey("AuthorUser")]
        public long? AuthorUserId { get; set; }

        public virtual VkUser? AuthorUser { get; set; }

        public long ExternalId { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
