using System.Collections.Generic;
using Newtonsoft.Json;

namespace MTATransit.Shared.API.NextBus
{
    public class Agency
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "regionTitle")]
        public string RegionTitle { get; set; }

        [JsonProperty(PropertyName = "tag")]
        public string Tag { get; set; }
    }

    public class Agencies
    {
        [JsonProperty(PropertyName = "agency")]
        public List<Agency> Items { get; set; }
    }
}
