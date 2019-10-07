using Newtonsoft.Json;
using System.Collections.Generic;

namespace MTATransit.Shared.API.Yelp
{
    public class Business
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "alias")]
        public string Alias { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "image_url")]
        public string ImageUrl { get; set; }

        [JsonProperty(PropertyName = "is_closed")]
        public bool IsClosed { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "review_count")]
        public int ReviewCount { get; set; }

        [JsonProperty(PropertyName = "rating")]
        public double Rating { get; set; }

        [JsonProperty(PropertyName = "transactions")]
        public List<string> Transactions { get; set; }

        [JsonProperty(PropertyName = "location")]
        public Address Location { get; set; }

        [JsonProperty(PropertyName = "coordinates")]
        public Point Coordinates { get; set; }

        [JsonProperty(PropertyName = "phone")]
        public string Phone { get; set; }

        [JsonProperty(PropertyName = "display_phone")]
        public string DisplayPhone { get; set; }

        [JsonProperty(PropertyName = "distance")]
        public decimal Distance { get; set; }
    }
}
