using MTATransit.Shared.API.OTP;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MTATransit.Shared.Models
{
    public class ItineraryModel
    {
        public ItineraryModel() { }

        public ItineraryModel(Itinerary otpI)
        {
            Duration = otpI.Duration;
            ElevationGained = otpI.ElevationGained;
            ElevationLost = otpI.ElevationLost;
            Legs = otpI.Legs;
            StartTime = otpI.StartTime;
            StopTime = otpI.StopTime;
            TooSloped = otpI.TooSloped;
            Transfers = otpI.Transfers;
            TransitTime = otpI.TransitTime;
            WaitingTime = otpI.WaitingTime;
            WalkDistance = otpI.WalkDistance;
            WalkLimitExceeded = otpI.WalkLimitExceeded;
            WalkTime = otpI.WalkTime;

            LoadTotalCost();
        }

        #region Properties
        /// <summary>
        /// Total duration of the trip, in seconds
        /// </summary>
        public long Duration { get; set; }

        /// <summary>
        /// Unix Time, but in milliseconds
        /// </summary>
        public long StartTime { get; set; }

        /// <summary>
        /// Unix Time, but in milliseconds
        /// </summary>
        public long StopTime { get; set; }

        /// <summary>
        /// Total time spent walking, in seconds
        /// </summary>
        public long WalkTime { get; set; }

        /// <summary>
        /// Total time spent on transit, in seconds
        /// </summary>
        public long TransitTime { get; set; }

        /// <summary>
        /// Total time spent waiting, in seconds
        /// </summary>
        public long WaitingTime { get; set; }

        public double WalkDistance { get; set; }

        public bool WalkLimitExceeded { get; set; }

        public double ElevationLost { get; set; }

        public double ElevationGained { get; set; }

        public int Transfers { get; set; }

        public bool TooSloped { get; set; }

        public List<Leg> Legs { get; set; }

        public decimal TotalCost { get; set; }
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

        public string ToDurationRangeString()
        {
            if (StopTime == 0)
                StopTime = StartTime + (Duration * 1000);

            return Common.NumberHelper.ToShortDayTimeString(StartTime / 1000) + " - "
                + Common.NumberHelper.ToShortDayTimeString(StopTime / 1000);
        }

        public string ToShortLegString()
        {
            string output = "";

            for (int i = 0; i < Legs.Count; i++)
            {
                var l = Legs[i];
                output += l.ToLegString();
                if (i != Legs.Count - 1)
                    output += " > ";
            }

            return output;
        }

        public string ToShortCostString()
        {
            return Common.NumberHelper.ToShortCurrencyString(TotalCost, true);
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

        private async void LoadTotalCost()
        {
            TotalCost = await GetTotalCost();
        }
        #endregion
    }
}
