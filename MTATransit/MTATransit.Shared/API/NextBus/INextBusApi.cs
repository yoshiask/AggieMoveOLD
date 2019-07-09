using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MTATransit.Shared.API.NextBus
{
    public interface INextBusApi
    {
        [Headers("Accept-Encoding: gzip, deflate")]
        [Get("/publicJSONFeed?command=agencyList")]
        Task<AgencyResponse> GetAgencies();

        [Headers("Accept-Encoding: gzip, deflate")]
        [Get("/publicJSONFeed?command=routeList&a={agency}")]
        Task<RouteResponse> GetRoutes(string agency);

        [Headers("Accept-Encoding: gzip, deflate")]
        [Get("/publicJSONFeed?command=routeConfig&a={agency}&r={route}")]
        Task<RouteInfoResponse> GetRouteInfo(string agency, string route);

        /// <param name="time">Unix Time, as string</param>
        [Headers("Accept-Encoding: gzip, deflate")]
        [Get("/publicJSONFeed?command=vehicleLocations&a={agency}&r={route}&t={time}")]
        Task<VehicleLocationsResponse> GetVehicleLocations(string agency, string route, string time = "");

        [Headers("Accept-Encoding: gzip, deflate")]
        [Get("/publicJSONFeed?command=vehicleLocation&a={agency}&v={vehicleId}")]
        Task<VehicleLocationResponse> GetVehicleLocation(string agency, string vehicleId);

        [Headers("Accept-Encoding: gzip, deflate")]
        [Get("/publicJSONFeed?command=vehicleLocations&a={agency}")]
        Task<VehicleLocationsResponse> GetVehicles(string agency);

        [Headers("Accept-Encoding: gzip, deflate")]
        [Get("/publicJSONFeed?command=predictions&a={agency}&r={route}&stopId={stop}")]
        Task<PredictionResponse> GetStopPredictions(string agency, string route, string stop);
    }
}
