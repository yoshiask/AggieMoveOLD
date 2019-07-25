using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace MTATransit.Shared.API.OTPMTA
{
    public class Plan
    {
        [JsonProperty(PropertyName = "date")]
        public long Date { get; set; }

        [JsonProperty(PropertyName = "from")]
        public PlanPoint From { get; set; }

        [JsonProperty(PropertyName = "to")]
        public PlanPoint To { get; set; }

        [JsonProperty(PropertyName = "itineraries")]
        public List<Itinerary> Itineraries { get; set; }
    }

    public class PlanPoint
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "lon")]
        public long Longitude { get; set; }

        [JsonProperty(PropertyName = "lat")]
        public long Latitude { get; set; }

        [JsonProperty(PropertyName = "orig")]
        public string Origin { get; set; }

        [JsonProperty(PropertyName = "vertexType")]
        public string VertexType { get; set; }
    }
}
