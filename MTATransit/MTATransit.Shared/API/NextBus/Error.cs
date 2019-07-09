using Newtonsoft.Json;
using System.Collections.Generic;

namespace MTATransit.Shared.API.NextBus
{
    public class Error
    {
        [JsonProperty(PropertyName = "content")]
        public string Content { get; set; }

        [JsonProperty(PropertyName = "shouldRetry")]
        public bool ShouldRetry { get; set; }
    }
}
