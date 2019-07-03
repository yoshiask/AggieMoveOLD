using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MTATransit.Shared.API.NextBus
{
    public interface INextBusApi
    {
        [Get("/publicJSONFeed?command=agencyList")]
        Task<Agencies> GetAgencies();

        [Get("/publicJSONFeed?command=routeList&a={agency}")]
        Task<Routes> GetRoutes(string agency);

        [Get("/publicJSONFeed?command=routeConfig&a={agency}&r={route}")]
        Task<RouteInfoResponse> GetRouteInfo(string agency, string route);
    }
}
