using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MTATransit.Shared.API.OTPMTA
{
    public class Itinerary
    {
        #region Properties
        /// <summary>
        /// Total duration of the trip, in seconds
        /// </summary>
        [JsonProperty(PropertyName = "duration")]
        public long Duration { get; set; }

        /// <summary>
        /// Unix Time, but in milliseconds
        /// </summary>
        [JsonProperty(PropertyName = "startTime")]
        public long StartTime { get; set; }

        /// <summary>
        /// Unix Time, but in milliseconds
        /// </summary>
        [JsonProperty(PropertyName = "stopTime")]
        public long StopTime { get; set; }

        /// <summary>
        /// Total time spent walking, in seconds
        /// </summary>
        [JsonProperty(PropertyName = "walkTime")]
        public long WalkTime { get; set; }

        /// <summary>
        /// Total time spent on transit, in seconds
        /// </summary>
        [JsonProperty(PropertyName = "transitTime")]
        public long TransitTime { get; set; }

        /// <summary>
        /// Total time spent waiting, in seconds
        /// </summary>
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
        #endregion

        #region Strings
        public override string ToString()
        {
            string output = "";

            DateTime startTime = Common.NumberHelper.UnixTimeStampToDateTime(StartTime / 1000);
            DateTime stopTime = Common.NumberHelper.UnixTimeStampToDateTime(StopTime / 1000);

            output += ToShortLegString();
            output += ": ";
            output += ToDurationString();
            output += ", ";
            output += "~$" + GetTotalCost().Result.ToString();

            return output;
        }

        public string ToDurationString()
        {
            return Common.NumberHelper.ToShortTimeString(Duration);
        }

        public string ToShortLegString()
        {
            string output = "";

            for (int i = 0; i < Legs.Count; i++)
            {
                var l = Legs[i];
                if (l.Mode == "WALK")
                {
                    output += "Walk, ";
                    double miles = Common.NumberHelper.MetersToMiles(l.Distance);
                    output += Math.Round(miles, 2).ToString() + " mi";
                }
                else
                    output += Legs[i].RouteShortName;
                if (i != Legs.Count - 1)
                    output += " > ";
            }

            return output;
        }
        #endregion

        #region Helpers
        public async Task<decimal> GetTotalCost()
        {
            decimal cost = 0;
            foreach (Leg leg in Legs)
            {
                switch (leg.Mode)
                {
                    case "WALK":
                        cost += 0;
                        break;

                    case "RAIL":
                        cost += new decimal(5.25);
                        break;

                    case "BUS":
                        if (leg.RouteId.StartsWith("uscalacmtabus"))
                            cost += (await Common.LAMoveApi.GetFareCost("lametro", 0)).Fare;
                        else
                            // TODO: Add more agencies to the LA Move API
                            cost += new decimal(2.00);//await Common.LAMoveApi.GetFareCost(leg.AgencyId, 0);
                        break;

                    case "TRAM":

                        break;

                    case "SUBWAY":

                        break;
                }
            }
            return cost;
        }
        #endregion
    }
}
