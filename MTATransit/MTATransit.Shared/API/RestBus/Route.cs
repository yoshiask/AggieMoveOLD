using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace MTATransit.Shared.API.RestBus
{
    public class Route
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
    }
}
