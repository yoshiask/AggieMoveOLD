using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace MTATransit.Shared.API.RestBus
{
    public class Vehicle
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "routeId")]
        public string RouteId { get; set; }

        [JsonProperty(PropertyName = "directionId")]
        public string DirectionId { get; set; }

        [JsonProperty(PropertyName = "predictable")]
        public bool IsPredictable { get; set; }

        [JsonProperty(PropertyName = "secsSinceReport")]
        public int SecondsSinceReport { get; set; }

        /// <summary>
        /// Speed of the vehicle, in kilometers per hour
        /// </summary>
        [JsonProperty(PropertyName = "kph")]
        public int Kph { get; set; }

        /// <summary>
        /// Direction the speed is heading, in degrees
        /// </summary>
        [JsonProperty(PropertyName = "heading")]
        public int Heading { get; set; }

        [JsonProperty(PropertyName = "lat")]
        public double Latitude { get; set; }

        [JsonProperty(PropertyName = "lon")]
        public double Longitude { get; set; }

        [JsonProperty(PropertyName = "leadingVehicleId")]
        public string LEadingVehicleId { get; set; }
    }
}
