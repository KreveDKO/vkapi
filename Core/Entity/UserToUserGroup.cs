using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity
{
    public class UserToUserGroup
    {
        public long Id { get; set; }

        [ForeignKey("VkUser")]
        public long VkUserId { get; set; }
        public virtual VkUser VkUser { get; set; }

        [ForeignKey("VkUserGroup")]
        public long VkUserGroupId { get; set; }
        public virtual VkUserGroup VkUserGroup { get; set; }
    }
}
