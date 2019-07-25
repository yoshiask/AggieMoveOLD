using MTATransit.Shared.API.RestBus;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTATransit.Shared.API
{
    public static class NextBusApiHelper
    {
        public static async Task<Agency> GetAgencyByTitle(string title, List<Agency> agencies = null)
        {
            if (agencies == null)
                agencies = await Common.NextBusApi.GetAgencies();
            return agencies.Find(x => x.Title == title);
        }

        public static async Task<Route> GetRouteByTitle(string agency, string title, List<Route> routes = null)
        {
            if (routes == null)
                routes = await Common.NextBusApi.GetAgencyRoutes(agency);
            return routes.Find(x => x.Title == title);
        }
    }
}
