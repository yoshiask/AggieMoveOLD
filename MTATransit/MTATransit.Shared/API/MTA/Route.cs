using Newtonsoft.Json;
using System.Collections.Generic;

namespace MTATransit.Shared.API.MTA
{
    public class Route
    {
        [JsonProperty(PropertyName = "display_name")]
        public string DisplayName { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
    }

    public class Routes
    {
        [JsonProperty(PropertyName = "items")]
        public List<Route> Items { get; set; }
    }

    public class RouteInfo
    {
        [JsonProperty(PropertyName = "display_name")]
        public string DisplayName { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "fg_color")]
        public string Foreground { get; set; }

        [JsonProperty(PropertyName = "bg_color")]
        public string Background { get; set; }
    }
}
