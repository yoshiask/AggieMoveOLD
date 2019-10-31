using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace MTATransit.Shared.API.OTP
{
    public class Leg
    {
        [JsonProperty(PropertyName = "startTime")]
        public long StartTime { get; set; }

        [JsonProperty(PropertyName = "endTime")]
        public long EndTime { get; set; }

        [JsonProperty(PropertyName = "departureDelay")]
        public long DepartureDelay { get; set; }

        [JsonProperty(PropertyName = "arrivalDelay")]
        public long ArrivalDelay { get; set; }

        [JsonProperty(PropertyName = "realTime")]
        public bool RealTime { get; set; }

        /// <summary>
        /// Distance of the leg, in meters
        /// </summary>
        [JsonProperty(PropertyName = "distance")]
        public double Distance { get; set; }

        [JsonProperty(PropertyName = "pathway")]
        public bool Pathway { get; set; }

        [JsonProperty(PropertyName = "mode")]
        public string Mode { get; set; }

        [JsonProperty(PropertyName = "route")]
        public string Route { get; set; }

        [JsonProperty(PropertyName = "agencyName")]
        public string AgencyName { get; set; }

        [JsonProperty(PropertyName = "agencyUrl")]
        public string AgencyUrl { get; set; }

        [JsonProperty(PropertyName = "agencyTimeZoneOffset")]
        public long AgencyTimeZoneOffset { get; set; }

        [JsonProperty(PropertyName = "routeType")]
        public int RouteType { get; set; }

        [JsonProperty(PropertyName = "routeId")]
        public string RouteId { get; set; }

        [JsonProperty(PropertyName = "interlineWithPreviousLeg")]
        public bool InterlineWithPreviousLeg { get; set; }

        [JsonProperty(PropertyName = "headsign")]
        public string Headsign { get; set; }

        [JsonProperty(PropertyName = "agencyId")]
        public string AgencyId { get; set; }

        [JsonProperty(PropertyName = "tripId")]
        public string TripId { get; set; }

        [JsonProperty(PropertyName = "serviceDate")]
        public string ServiceDate { get; set; }

        [JsonProperty(PropertyName = "from")]
        public LegPoint From { get; set; }

        [JsonProperty(PropertyName = "to")]
        public LegPoint To { get; set; }

        [JsonProperty(PropertyName = "routeShortName")]
        public string RouteShortName { get; set; }

        [JsonProperty(PropertyName = "routeLongName")]
        public string RouteLongName { get; set; }

        [JsonProperty(PropertyName = "rentedBike")]
        public bool RentedBike { get; set; }

        [JsonProperty(PropertyName = "transitLeg")]
        public bool TransitLeg { get; set; }

        [JsonProperty(PropertyName = "legGeometry")]
        public LegGeometry Geometry { get; set; }

        /// <summary>
        /// How long it takes to complete the leg, in seconds
        /// </summary>
        [JsonProperty(PropertyName = "duration")]
        public long Duration { get; set; }

        [JsonProperty(PropertyName = "steps")]
        public List<Step> Steps { get; set; }

        public string ToLegString()
        {
            string output = "";
            if (Mode == "WALK")
            {
                output += "Walk, ";
                output += ToDistanceString();
            }
            else
                output += Route;
            return output;
        }

        public string ToDistanceString()
        {
            return System.Math.Round(Common.NumberHelper.MetersToMiles(Distance), 2).ToString() + " mi";
        }

        public string ToShortDisplayString()
        {
            switch (Mode)
            {
                case "WALK":
                    return ToDistanceString();

                case "BIKE":
                    return ToDistanceString();

                default:
                    return RouteShortName;
            }
        }
    }

    public class LegPoint
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "stopId")]
        public string StopId { get; set; }

        [JsonProperty(PropertyName = "lon")]
        public long Longitude { get; set; }

        [JsonProperty(PropertyName = "lat")]
        public long Latitude { get; set; }

        [JsonProperty(PropertyName = "arrival")]
        public long Arrival { get; set; }

        [JsonProperty(PropertyName = "departure")]
        public long Departure { get; set; }

        [JsonProperty(PropertyName = "stopIndex")]
        public int StopIndex { get; set; }

        [JsonProperty(PropertyName = "stopSequence")]
        public int StopSequence { get; set; }

        [JsonProperty(PropertyName = "vertexType")]
        public string VertexType { get; set; }
    }

    public class LegGeometry
    {
        /// <summary>
        /// Use <see cref="GooglePolylineConverter"/> to get a useable list of points
        /// </summary>
        [JsonProperty(PropertyName = "points")]
        public string Points { get; set; }

        [JsonProperty(PropertyName = "length")]
        public int Length { get; set; }
    }
}
