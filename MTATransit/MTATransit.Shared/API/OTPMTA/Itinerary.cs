using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace MTATransit.Shared.API.OTPMTA
{
    public class Itinerary
    {
        [JsonProperty(PropertyName = "duration")]
        public long Duration { get; set; }

        [JsonProperty(PropertyName = "startTime")]
        public long StartTime { get; set; }

        [JsonProperty(PropertyName = "stopTime")]
        public long StopTime { get; set; }

        [JsonProperty(PropertyName = "walkTime")]
        public long WalkTime { get; set; }

        [JsonProperty(PropertyName = "transitTime")]
        public long TransitTime { get; set; }

        [JsonProperty(PropertyName = "waitingTime")]
        public long WaitingTime { get; set; }

        [JsonProperty(PropertyName = "walkDistance")]
        public double WalkDistance { get; set; }

        [JsonProperty(PropertyName = "walkLimitExceeded")]
        public bool WalkLimitExceeded { get; set; }

        [JsonProperty(PropertyName = "elevationLost")]
        public double ElevationLost { get; set; }

        [JsonProperty(PropertyName = "elevationGained")]
        public double ElevationGained { get; set; }

        [JsonProperty(PropertyName = "transfers")]
        public int Transfers { get; set; }

        [JsonProperty(PropertyName = "tooSloped")]
        public bool TooSloped { get; set; }

        [JsonProperty(PropertyName = "legs")]
        public List<Leg> Legs { get; set; }
    }
}
