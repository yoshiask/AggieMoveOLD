using System.Collections.Generic;
using Newtonsoft.Json;

namespace MTATransit.Shared.API.NextBus
{
    public class Direction
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        /// <summary>
        /// Unique ID of the direction
        /// </summary>
        [JsonProperty(PropertyName = "tag")]
        public string Tag { get; set; }

        /// <summary>
        /// One of the four cardinal directions
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "useForUI")]
        public bool UseForUI { get; set; }

        [JsonProperty(PropertyName = "stop")]
        public List<Stop> Stops { get; set; }
    }
}
