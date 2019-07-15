using Newtonsoft.Json;

namespace MTATransit.Shared.API.LAMove
{
    public class FareType
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "desc")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "isTAP")]
        public bool IsTAP { get; set; }
    }
}
