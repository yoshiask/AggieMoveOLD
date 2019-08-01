using System;

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

        #region Derived Properties
        public bool HasArrivalTime {
            get {
                return ArrivalTime != 0 ? true : false;
            }
        }
        public string ArrivalTimeString {
            get {
                if (!HasArrivalTime)
                    return "N/A";
                return Common.NumberHelper.ToShortDayTimeString(ArrivalTime);
            }
        }
        public DateTime? ArrivalDateTime {
            get {
                if (HasArrivalTime)
                {
                    return Common.NumberHelper.UnixTimeStampToDateTime(ArrivalTime);
                }
                else
                {
                    return null;
                }
            }
        }
        public TimeSpan ArrivalTimeSpan {
            get {
                var dt = Common.NumberHelper.UnixTimeStampToDateTime(ArrivalTime);
                return new TimeSpan(((DateTimeOffset)new DateTime(dt.Year, dt.Month, dt.Day)).ToUnixTimeSeconds());
            }
        }

        public bool HasDepartureTime {
            get {
                return ArrivalTime != 0 ? true : false;
            }
        }
        public string DepartureTimeString {
            get {
                if (!HasDepartureTime)
                    return "N/A";
                return Common.NumberHelper.ToShortDayTimeString(DepartureTime);
            }
        }
        public DateTime? DepartureDateTime {
            get {
                if (HasDepartureTime)
                {
                    return Common.NumberHelper.UnixTimeStampToDateTime(DepartureTime);
                }
                else
                {
                    return null;
                }
            }
        }
        public TimeSpan DepartureTimeSpan {
            get {
                var dt = Common.NumberHelper.UnixTimeStampToDateTime(DepartureTime);
                return new TimeSpan(((DateTimeOffset)new DateTime(dt.Year, dt.Month, dt.Day)).ToUnixTimeSeconds());
            }
        }
        #endregion
    }
}
