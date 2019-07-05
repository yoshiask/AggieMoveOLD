using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTATransit.Shared.API.ArcGIS
{
    public class AddressCandidate
    {
        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }

        [JsonProperty(PropertyName = "location")]
        public Location Location { get; set; }

        [JsonProperty(PropertyName = "score")]
        public int Score { get; set; }

        [JsonProperty(PropertyName = "extent")]
        public Extent Extent { get; set; }
    }

    public class GeocodeResponse
    {
        [JsonProperty(PropertyName = "candidates")]
        public List<AddressCandidate> Candidates { get; set; }

        [JsonProperty(PropertyName = "spatialReference")]
        public SpatialReference SpatialReference { get; set; }
    }
}
