using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Core.Entity
{
    public class FriendsUserToUser
    {
        public long Id { get; set; }

        [ForeignKey("LeftUser")]
        public long LeftUserId { get; set; }

        public virtual User LeftUser { get; set; }

        [ForeignKey("RightUser")]
        public long RightUserId { get; set; }

        public virtual User RightUser { get; set; }
    
    }
}
