using Newtonsoft.Json;

namespace Logic.Dto
{
    public class FriendsUpdateDto
    {
        [JsonProperty("recursive")]
        public bool Recursive { get; set; }

        [JsonProperty("userId")]
        public long? UserId { get; set; }
        
        
    }
}