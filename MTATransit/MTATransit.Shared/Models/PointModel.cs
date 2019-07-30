using System;
using System.Collections.Generic;
using System.Text;

namespace MTATransit.Shared.Models
{
    public class PointModel
    {
        public string Title { get; set; }
        public string Address { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public long ArrivalTime { get; set; }
        public long DepartureTime { get; set; }

        public string ArrivalTimeString {
            get {
                if (ArrivalTime == 0)
                    return "N/A";
                return Common.NumberHelper.ToShortTimeString(ArrivalTime);
            }
        }
        public string DepartureTimeString {
            get {
                if (DepartureTime == 0)
                    return "N/A";
                return Common.NumberHelper.ToShortTimeString(ArrivalTime);
            }
        }
    }
}
