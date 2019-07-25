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

        [JsonProperty(PropertyName = "shortTitle")]
        public string ShortTitle { get; set; }

        [JsonProperty(PropertyName = "useForUi")]
        public bool UseForUi { get; set; }

        /// <summary>
        /// List of stop IDs, not codes
        /// </summary>
        [JsonProperty(PropertyName = "stops")]
        public List<string> Stops { get; set; }
    }
}
