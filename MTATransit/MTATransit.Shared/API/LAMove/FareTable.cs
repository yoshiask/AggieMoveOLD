using Newtonsoft.Json;

namespace MTATransit.Shared.API.LAMove
{
    public class FareTable
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "regular")]
        public int Regular { get; set; }

        [JsonProperty(PropertyName = "disabled-peak")]
        public int Disabled_Peak { get; set; }

        [JsonProperty(PropertyName = "disabled-offpeak")]
        public int Disabled_OffPeak { get; set; }

        [JsonProperty(PropertyName = "college")]
        public int College { get; set; }

        [JsonProperty(PropertyName = "student")]
        public int Student { get; set; }
    }
}
