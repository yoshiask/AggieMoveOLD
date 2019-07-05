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


        /// <param name="time">Unix Time, as string</param>
        [Get("/publicJSONFeed?command=vehicleLocations&a={agency}&r={route}&t={time}")]
        Task<VehicleLocationsResponse> GetVehicleLocations(string agency, string route, string time);

        [Get("/publicJSONFeed?command=predictions&a={agency}&r={route}&stopId={stop}")]
        Task<PredictionResponse> GetStopPredictions(string agency, string route, string stop);
    }
}
