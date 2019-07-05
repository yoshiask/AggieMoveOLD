using Newtonsoft.Json;
using System.Collections.Generic;

namespace MTATransit.Shared.API.NextBus
{
    public class Message
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "sendToBuses")]
        public bool SendToBuses { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "priority")]
        public PriorityEnum Priority { get; set; }

        /// <summary>
        /// A string representing when the message will stop
        /// </summary>
        [JsonProperty(PropertyName = "endBoundaryStr")]
        public string EndBoundaryString { get; set; }

        /// <summary>
        /// A string representing when the message will start
        /// </summary>
        [JsonProperty(PropertyName = "startBoundaryStr")]
        public string StartBoundaryString { get; set; }

        /// <summary>
        /// A Unix time representing when the message will stop
        /// </summary>
        [JsonProperty(PropertyName = "endBoundary")]
        public long EndBoundary { get; set; }

        /// <summary>
        /// A Unix time representing when the message will start
        /// </summary>
        [JsonProperty(PropertyName = "startBoundary")]
        public long StartBoundary { get; set; }


        public enum PriorityEnum
        {
            Normal,
            High
        }
    }

    public class MessageResponse
    {
        [JsonProperty(PropertyName = "routes")]
        public Route Routes { get; set; }

        [JsonProperty(PropertyName = "copyright")]
        public string Copyright { get; set; }
    }
}
