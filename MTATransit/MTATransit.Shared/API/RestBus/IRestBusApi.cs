using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MTATransit.Shared.API.RestBus
{
    public interface IRestBusApi
    {
        [Get("/locations/{lon},{lat}/predictions")]
        Task<List<LocationPrediction>> GetPredictions(decimal lat, decimal lon);

        [Get("/agencies")]
        Task<List<Agency>> GetAgencies();

        [Get("/agencies/{agencyId}")]
        Task<Agency> GetAgency(string agencyId);

        [Get("/agencies/{agencyId}/routes")]
        Task<List<Route>> GetAgencyRoutes(string agencyId);

        [Get("/agencies/{agencyId}/routes/{routeId}")]
        Task<Route> GetRoute(string agencyId, string routeId);

        [Get("/agencies/{agencyId}/vehicles")]
        Task<List<Vehicle>> GetAgencyVehicles(string agencyId);

        [Get("/agencies/{agencyId}/vehicles/{vehicleId}")]
        Task<Vehicle> GetVehicle(string agencyId, string vehicleId);

        [Get("/agencies/{agencyId}/routes/{routeId}/vehicles")]
        Task<List<Vehicle>> GetRouteVehicles(string agencyId, string routeId);

        [Get("/agencies/{agencyId}/routes/{routeId}/stops/{stopId}/predictions")]
        Task<List<Prediction>> GetStopPredictions(string agencyId, string routeId, string stopId);

        /// <param name="stopCode">Not every agency uses stop codes</param>
        [Get("/agencies/{agencyId}/stops/{stopCode}/predictions")]
        Task<List<Prediction>> GetStopPredictions(string agencyId, string stopCode);
    }
}
