using Newtonsoft.Json;

namespace Core.Dto
{
    public class GroupUpdateDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("withUsers")]
        public bool WithUsers { get; set; }
    }
}