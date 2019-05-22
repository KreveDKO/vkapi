using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logic.Dto
{
    public class Node
    {
		[JsonProperty("id")]
        public long Id { get; set; }

		[JsonProperty("user")]
        public string User { get; set; }

		[JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("userid")]
        public long UserId { get; set; }

        [JsonProperty("isactive")]
        public bool IsActive { get; set; }

    }
}
