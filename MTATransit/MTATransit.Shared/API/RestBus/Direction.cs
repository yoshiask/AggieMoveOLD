using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace MTATransit.Shared.API.RestBus
{
    public class Direction
    {
        [JsonProperty(PropertyName = "id")]
        public object Id { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
    }
}
