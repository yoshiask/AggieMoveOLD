using System.Collections.Generic;
using Newtonsoft.Json;

namespace MTATransit.Shared.API.NextBus
{
    public class Vehicle
    {
        /// <summary>
        /// The ID of the vehicle (on busses, it's the number visible to riders)
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "lon")]
        public string Longitude { get; set; }

        [JsonProperty(PropertyName = "lat")]
        public string Latitude { get; set; }

        [JsonProperty(PropertyName = "routeTag")]
        public string RouteTag { get; set; }

        [JsonProperty(PropertyName = "predictable")]
        public bool Predictable { get; set; }

        /// <summary>
        /// The current speed of the vehicle, in Km/hr
        /// </summary>
        [JsonProperty(PropertyName = "speedKmHr")]
        public string Speed { get; set; }

        [JsonProperty(PropertyName = "dirTag")]
        public string Tag { get; set; }

        /// <summary>
        /// The angle(?) that the bus is moving along
        /// </summary>
        [JsonProperty(PropertyName = "heading")]
        public string Heading { get; set; }

        [JsonProperty(PropertyName = "secSinceReport")]
        public string SecondsSinceLastReport { get; set; }
    }

    public class VehicleLocationsResponse
    {
        [JsonProperty(PropertyName = "vehicle")]
        public List<Vehicle> Items { get; set; }

        [JsonProperty(PropertyName = "lastTime")]
        public LocTime LastTime { get; set; }

        [JsonProperty(PropertyName = "copyright")]
        public string Copyright { get; set; }
    }

    public class LocTime
    {
        [JsonProperty(PropertyName = "time")]
        public string Time { get; set; }
    }
}