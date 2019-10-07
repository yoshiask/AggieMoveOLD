using Newtonsoft.Json;
using Refit;
using System.Collections.Generic;

namespace MTATransit.Shared.API.Yelp
{
    public class BusinessSearchResponse
    {
        [JsonProperty(PropertyName = "businesses")]
        public List<Business> Businesses { get; set; }

        [JsonProperty(PropertyName = "total")]
        public int Total { get; set; }

        [JsonProperty(PropertyName = "region")]
        public object Region { get; set; }
    }
}
