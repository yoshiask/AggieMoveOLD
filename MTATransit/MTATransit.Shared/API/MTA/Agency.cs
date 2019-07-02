using Newtonsoft.Json;

namespace MTATransit.Shared.API.MTA
{
    public class Agency
    {
        [JsonProperty(PropertyName = "display_name")]
        public string DisplayName { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
    }
}
