using System.Collections.Generic;
using Newtonsoft.Json;

namespace MTATransit.Shared.API.NextBus
{
    public class Route
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "tag")]
        public string Tag { get; set; }
    }

    public class RouteInfo
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "tag")]
        public string Tag { get; set; }

        [JsonProperty(PropertyName = "latMax")]
        public string LatitudeMax { get; set; }

        [JsonProperty(PropertyName = "latMin")]
        public string LatitudeMin { get; set; }

        [JsonProperty(PropertyName = "lonMax")]
        public string LongitudeMax { get; set; }

        [JsonProperty(PropertyName = "lonMin")]
        public string LongitudeMin { get; set; }

        [JsonProperty(PropertyName = "color")]
        public string Color { get; set; }

        [JsonProperty(PropertyName = "oppositeColor")]
        public string ForegroundColor { get; set; }
    }

    public class Routes
    {
        [JsonProperty(PropertyName = "route")]
        public List<Route> Items { get; set; }
    }

    public class RouteInfoResponse
    {
        [JsonProperty(PropertyName = "route")]
        public RouteInfo Route { get; set; }

        [JsonProperty(PropertyName = "copyright")]
        public string Copyright { get; set; }
    }
}
