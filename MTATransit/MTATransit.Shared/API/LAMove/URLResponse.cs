using Newtonsoft.Json;
using System.Collections.Generic;

namespace MTATransit.Shared.API.LAMove
{
    public class URLResponse
    {
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
    }
}
