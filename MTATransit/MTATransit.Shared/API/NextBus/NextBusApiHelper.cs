using NextBus.NET.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTATransit.Shared.API
{
    public static class NextBusApiHelper
    {
        public static async Task<Agency> GetAgencyByTitle(string title)
        {
            List<Agency> agencies = (await Common.NextBusApi.GetAgencies()).ToList();
            return agencies.Find(x => x.Title == title);
        }

        public static async Task<Route> GetRouteByTitle(string agency, string title)
        {
            var routes = (await Common.NextBusApi.GetRoutesForAgency(agency)).ToList();
            return routes.Find(x => x.Title == title);
        }
    }
}
