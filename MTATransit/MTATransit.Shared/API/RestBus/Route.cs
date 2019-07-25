using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace MTATransit.Shared.API.RestBus
{
    public class Route
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "shortTitle")]
        public string ShortTitle { get; set; }

        [JsonProperty(PropertyName = "color")]
        public string Color { get; set; }

        [JsonProperty(PropertyName = "textColor")]
        public string TextColor { get; set; }

        [JsonProperty(PropertyName = "bounds")]
        public object Bounds { get; set; }

        [JsonProperty(PropertyName = "stops")]
        public List<Stop> Stops { get; set; }

        [JsonProperty(PropertyName = "directions")]
        public List<Direction> Directions { get; set; }

        [JsonProperty(PropertyName = "paths")]
        public List<Path> Paths { get; set; }
    }
}
