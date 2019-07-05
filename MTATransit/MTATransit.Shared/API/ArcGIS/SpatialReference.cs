using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MTATransit.Shared.API.ArcGIS
{
    public class SpatialReference
    {
        [JsonProperty(PropertyName = "wkid")]
        public float Wkid { get; set; }

        [JsonProperty(PropertyName = "latestWkid")]
        public float LatestWkid { get; set; }
    }
}
