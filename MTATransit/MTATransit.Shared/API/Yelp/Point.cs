using Newtonsoft.Json;
using System.Collections.Generic;

namespace MTATransit.Shared.API.Yelp
{
    public class Point
    {
        [JsonProperty(PropertyName = "latitude")]
        public decimal Latitude { get; set; }

        [JsonProperty(PropertyName = "longitude")]
        public decimal Longitude { get; set; }

        public override string ToString()
        {
            return Longitude.ToString() + ", " + Latitude.ToString();
        }
    }
}
