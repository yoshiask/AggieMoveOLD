using Newtonsoft.Json;
using System.Collections.Generic;

namespace MTATransit.Shared.API.MTA
{
    public class Run
    {
        [JsonProperty(PropertyName = "display_name")]
        public string DisplayName { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "direction_name")]
        public string Direction { get; set; }

        [JsonProperty(PropertyName = "route_id")]
        public string RouteId { get; set; }

        [JsonProperty(PropertyName = "display_in_ui")]
        public bool DisplayInUI { get; set; }
    }

    public class Runs
    {
        [JsonProperty(PropertyName = "items")]
        public List<Run> Items { get; set; }
    }
}
