using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTATransit.Shared.API.ArcGIS
{
    public class Location
    {
        [JsonProperty(PropertyName = "x")]
        public double Latitude { get; set; }

        [JsonProperty(PropertyName = "y")]
        public double Longitude { get; set; }
    }

    public class Extent
    {
        [JsonProperty(PropertyName = "xmin")]
        public float XMin { get; set; }

        [JsonProperty(PropertyName = "ymin")]
        public float YMin { get; set; }

        [JsonProperty(PropertyName = "xmax")]
        public float XMax { get; set; }

        [JsonProperty(PropertyName = "ymax")]
        public float YMax { get; set; }
    }
}
