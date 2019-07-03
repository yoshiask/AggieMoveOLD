using System.Collections.Generic;
using Newtonsoft.Json;

namespace MTATransit.Shared.API.NextBus
{
    public class Stop
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "tag")]
        public string Tag { get; set; }

        [JsonProperty(PropertyName = "stopId")]
        public string StopId { get; set; }

        [JsonProperty(PropertyName = "lat")]
        public string Latitude { get; set; }

        [JsonProperty(PropertyName = "lon")]
        public string Longitude { get; set; }
    }
}
