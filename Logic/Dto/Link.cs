using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logic.Dto
{
    public class Link
    {
        [JsonProperty("source")]
        public long Source { get; set; }

        [JsonProperty("target")]
        public long Target { get; set; }
    }
}
