using Newtonsoft.Json;
using System.Collections.Generic;

namespace MTATransit.Shared.API.RestBus
{
    public class Prediction
    {
        [JsonProperty(PropertyName = "agency")]
        public Agency Agency { get; set; }

        [JsonProperty(PropertyName = "route")]
        public Route Route { get; set; }

        [JsonProperty(PropertyName = "stop")]
        public Stop Stop { get; set; }

        [JsonProperty(PropertyName = "messages")]
        public List<string> Messages { get; set; }

        [JsonProperty(PropertyName = "values")]
        public List<PredictionData> Values { get; set; }
    }

    public class PredictionData
    {
        [JsonProperty(PropertyName = "epochTime")]
        public long EpochTime { get; set; }

        [JsonProperty(PropertyName = "seconds")]
        public int Seconds { get; set; }

        [JsonProperty(PropertyName = "minutes")]
        public int Minutes { get; set; }

        [JsonProperty(PropertyName = "branch")]
        public object Branch { get; set; }

        [JsonProperty(PropertyName = "isDeparture")]
        public bool IsDeparture { get; set; }

        [JsonProperty(PropertyName = "affectedByLayover")]
        public bool AffectedByLayover { get; set; }

        [JsonProperty(PropertyName = "isScheduleBased")]
        public bool IsScheduleBased { get; set; }

        [JsonProperty(PropertyName = "vehicle")]
        public object Vehicle { get; set; }

        [JsonProperty(PropertyName = "direction")]
        public Direction Direction { get; set; }
    }
}
