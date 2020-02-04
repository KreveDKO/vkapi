using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity
{
    public class VkAudioToArtist
    {
        public long Id { get; set; }
        
        
        [ForeignKey("VkAudio")]
        public long VkAudioId { get; set; }

        public virtual VkAudio VkAudio { get; set; }

        [ForeignKey("VkAudioArtist")]
        public long VkAudioArtistsId { get; set; }

        public virtual VkAudioArtist VkAudioArtist { get; set; }
    }
}