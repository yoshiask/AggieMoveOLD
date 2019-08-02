using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace MTATransit.Shared.API.RestBus
{
    public class Path
    {
        [JsonProperty(PropertyName = "points")]
        public List<Point> Points { get; set; }
    }

    public class Point
    {
        [JsonProperty(PropertyName = "lat")]
        public decimal Latitude { get; set; }

        [JsonProperty(PropertyName = "lon")]
        public decimal Longitude { get; set; }
    }
}
