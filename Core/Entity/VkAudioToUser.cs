using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity
{
    public class VkAudioToUser
    {
        public long Id { get; set; }

        [ForeignKey("VkAudio")]
        public long VkAudioId { get; set; }

        public virtual VkAudio VkAudio { get; set; }
        
        [ForeignKey("VkUser")]
        public long VkUserId { get; set; }

        public virtual VkUser VkUser { get; set; }
    }
}