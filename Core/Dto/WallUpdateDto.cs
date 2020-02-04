using Newtonsoft.Json;

namespace Core.Dto
{
    public class WallUpdateDto
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("limit")]

        public int Limit { get; set; }

        [JsonProperty("offset")]

        public ulong Offset { get; set; }
    }
}