using System;
using System.Text.RegularExpressions;

namespace MTATransit.Shared.API.MTA
{
    public static class MTAHelper
    {
        public class RiderInfo
        {
            /// <summary>
            /// Age of the rider
            /// </summary>
            public int Age;

            /// <summary>
            /// Whether the rider is disabled, senior, or on Medicare
            /// </summary>
            public bool IsDisabled;

            /// <summary>
            /// Whether the rider is in college/vocational
            /// </summary>
            public bool IsCollege;

            /// <summary>
            /// Whether the rider is a K-12 student
            /// </summary>
            public bool IsStudent;
        }

        /// <summary>
        /// Fall back on this when offline. Prefer the LA Move API
        /// </summary>
        public static class FareHelper
        {
            /// <summary>
            /// Calculates the 1-Ride Base Fare for the rider
            /// </summary>
            /// <param name="rider"></param>
            /// <param name="date">Date of the trip</param>
            /// <returns></returns>
            public static double GetBaseFare(RiderInfo rider, DateTime date)
            {
                // If the date is not passed through,
                // then set it to the current date and time
                if (date == null)
                    date = DateTime.Now;

                if (rider.IsStudent)
                    return 1.00;

                if (rider.IsDisabled)
                {
                    if (IsOffPeak(date))
                        return 0.35;
                    else
                        return 0.75;
                }

                // Default fare
                return 1.75;
            }

            /// <summary>
            /// Calculates the 1-Way Trip (on TAP) for the rider
            /// </summary>
            /// <param name="rider"></param>
            /// <param name="date">Date of the trip</param>
            /// <returns></returns>
            public static double Get1WayTripFare(RiderInfo rider, DateTime date)
            {
                // If the date is not passed through,
                // then set it to the current date and time
                if (date == null)
                    date = DateTime.Now;

                if (rider.IsStudent)
                    return 1.00;

                if (rider.IsDisabled)
                {
                    if (IsOffPeak(date))
                        return 0.35;
                    else
                        return 0.75;
                }

                // Default fare
                return 1.75;
            }

            /// <summary>
            /// Calculates the 1-Day Pass (on TAP) for the rider
            /// </summary>
            /// <param name="rider"></param>
            /// <param name="date">Date of the trip</param>
            /// <returns></returns>
            public static double Get1DayPassFare(RiderInfo rider)
            {

                if (rider.IsDisabled)
                    return 2.50;

                // Default fare
                return 7.00;
            }

            public static double Get7DayPassFare(RiderInfo rider)
            {
                /// Default fare
                return 25.00;
            }

            /// <summary>
            /// Calculates the 30 Day Pass (on TAP) for the rider
            /// </summary>
            /// <param name="rider"></param>
            /// <returns></returns>
            public static double Get30DayPassFare(RiderInfo rider)
            {

                if (rider.IsStudent)
                    return 24.00;

                if (rider.IsDisabled)
                    return 20.00;

                if (rider.IsCollege)
                    return 43.00;

                // Default fare
                return 100.00;
            }

            public static double Get30DayZone1PassFare(RiderInfo rider)
            {
                /// Default fare
                return 122.00;
            }

            public static double GetMetroToMuniFare(RiderInfo rider)
            {
                if (rider.IsDisabled)
                    return 0.25;

                // Default fare
                return 0.50;
            }


            public static bool IsOffPeak(DateTime date)
            {
                if (date.DayOfWeek == DayOfWeek.Monday ||
                    date.DayOfWeek == DayOfWeek.Tuesday ||
                    date.DayOfWeek == DayOfWeek.Wednesday ||
                    date.DayOfWeek == DayOfWeek.Thursday ||
                    date.DayOfWeek == DayOfWeek.Friday)
                {
                    if (date.Hour >= 9 && date.Hour < 15)
                        return true;
                }
                else
                {
                    if (date.Hour >= 19 && date.Hour < 24 ||
                        date.Hour >= 0 && date.Hour < 5)
                        return true;
                }

                return false;
            }
        }
    }
}
