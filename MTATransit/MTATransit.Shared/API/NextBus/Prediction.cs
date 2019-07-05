using Newtonsoft.Json;
using System.Collections.Generic;

namespace MTATransit.Shared.API.NextBus
{
    public class PredictionInfo
    {
        [JsonProperty(PropertyName = "isDeparture")]
        public bool IsDeparture { get; set; }

        [JsonProperty(PropertyName = "minutes")]
        public int Minutes { get; set; }

        [JsonProperty(PropertyName = "seconds")]
        public int Seconds { get; set; }

        [JsonProperty(PropertyName = "tripTag")]
        public string TripTag { get; set; }

        [JsonProperty(PropertyName = "vehicle")]
        public string Vehicle { get; set; }

        [JsonProperty(PropertyName = "block")]
        public string Block { get; set; }

        [JsonProperty(PropertyName = "dirTag")]
        public string DirTag { get; set; }

        [JsonProperty(PropertyName = "epochTime")]
        public long UnixTime { get; set; }
    }

    public class Prediction
    {
        [JsonProperty(PropertyName = "agencyTitle")]
        public string AgencyTitle { get; set; }

        [JsonProperty(PropertyName = "routeTag")]
        public string RouteTag { get; set; }

        [JsonProperty(PropertyName = "routeTitle")]
        public string RouteTitle { get; set; }

        [JsonProperty(PropertyName = "direction")]
        public DirectionInfo Direction { get; set; }

        [JsonProperty(PropertyName = "stopTitle")]
        public string StopTitle { get; set; }

        [JsonProperty(PropertyName = "stopTag")]
        public string StopTag { get; set; }

        [JsonProperty(PropertyName = "message")]
        public Message Message { get; set; }


        public class DirectionInfo
        {
            [JsonProperty(PropertyName = "title")]
            public string Title { get; set; }

            [JsonProperty(PropertyName = "prediction")]
            public List<PredictionInfo> Prediction { get; set; }
        }
    }

    public class PredictionResponse
    {
        /// <summary>
        /// Either <see cref="Prediction"/> or a list of <see cref="Prediction"/>
        /// </summary>
        [JsonProperty(PropertyName = "predictions")]
        public object Predictions { get; set; }

        [JsonProperty(PropertyName = "copyright")]
        public string Copyright { get; set; }
    }
}
