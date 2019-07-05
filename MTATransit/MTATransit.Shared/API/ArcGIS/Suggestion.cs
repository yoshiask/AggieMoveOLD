using Newtonsoft.Json;
using System.Collections.Generic;

namespace MTATransit.Shared.API.ArcGIS
{
    public class Suggestion
    {
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        /// <summary>
        /// Effectively a unique ID
        /// </summary>
        [JsonProperty(PropertyName = "magicKey")]
        public string MagicKey { get; set; }

        [JsonProperty(PropertyName = "isCollection")]
        public bool IsCollection { get; set; }
    }

    public class Suggestions
    {
        [JsonProperty(PropertyName = "suggestions")]
        public List<Suggestion> Items { get; set; }
    }
}
