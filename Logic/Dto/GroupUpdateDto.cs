using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Logic.Dto
{
    public class GroupUpdateDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("withUsers")]
        public bool WithUsers { get; set; }
    }
}