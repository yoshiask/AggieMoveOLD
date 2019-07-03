using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MTATransit.Shared.API.NextBus
{
    public static class NextBusApiHelper
    {
        public static async Task<Agency> GetAgencyByTitle(string title)
        {
            var agencies = (await Common.NextBusApi.GetAgencies()).Items;
            return agencies.Find(x => x.Title == title);
        }

        public static async Task<Route> GetRouteByTitle(string agency, string title)
        {
            var routes = (await Common.NextBusApi.GetRoutes(agency)).Items;
            return routes.Find(x => x.Title == title);
        }
    }
}
