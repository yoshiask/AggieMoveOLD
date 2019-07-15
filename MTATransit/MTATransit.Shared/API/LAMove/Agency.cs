using Newtonsoft.Json;
using System.Collections.Generic;

namespace MTATransit.Shared.API.LAMove
{
    public class Agency
    {
        [JsonProperty(PropertyName = "tag")]
        public string Tag { get; set; }

        [JsonProperty(PropertyName = "fareTypes")]
        public List<FareType> FareTypes { get; set; }

        [JsonProperty(PropertyName = "fareTables")]
        public List<FareTable> FareTables { get; set; }
    }
}
