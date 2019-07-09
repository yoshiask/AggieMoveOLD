using Newtonsoft.Json;
using System.Collections.Generic;

namespace MTATransit.Shared.API.NagerDate
{
    public class PublicHoliday
    {
        [JsonProperty(PropertyName = "date")]
        public string Date { get; set; }

        [JsonProperty(PropertyName = "localName")]
        public string LocalName { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "countryCode")]
        public string CountryCode { get; set; }

        [JsonProperty(PropertyName = "fixed")]
        public bool IsDateFixed { get; set; }

        [JsonProperty(PropertyName = "global")]
        public bool IsGlobal { get; set; }

        [JsonProperty(PropertyName = "counties")]
        public List<string> Counties { get; set; }

        [JsonProperty(PropertyName = "launchYear")]
        public int LaunchYear { get; set; }

        [JsonProperty(PropertyName = "type")]
        public int Type { get; set; }
    }
}
