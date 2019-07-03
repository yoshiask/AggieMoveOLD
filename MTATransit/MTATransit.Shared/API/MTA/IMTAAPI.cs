using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MTATransit.Shared.API.MTA
{
    public interface IMTAApi
    {
        [Get("/agencies/")]
        Task<List<Agency>> GetAgencies();

        [Get("/agencies/{agency}/routes/")]
        Task<Routes> GetRoutes(string agency);

        [Get("/agencies/{agency}/routes/{route}")]
        Task<RouteInfo> GetRouteInfo(string agency, string route);


        // TODO: Add the correct return objects
        // TODO: Test the API implementations beyond this point
        [Get("/agencies/{agency}/routes/{route}/runs")]
        Task<Runs> GetRuns(string agency, string route);

        [Get("/agencies/{agency}/routes/{route}/runs/{run}")]
        Task<Runs> GetRunInfo(string agency, string route, string run);

        [Get("/agencies/{agency}/routes/{route}/runs/{run}/stops")]
        Task<Runs> GetStops(string agency, string route, string run);

        [Get("/agencies/{agency}/routes/{route}/stops")]
        Task<Runs> GetStops(string agency, string route);

        [Get("/agencies/{agency}/routes/{route}/sequence")]
        Task<Runs> GetStopSequence(string agency, string route);

        [Get("/agencies/{agency}/routes/{route}/stops/{stop}")]
        Task<Runs> GetStop(string agency, string route, string stop);

        [Get("/agencies/{agency}/routes/{route}/stops/{stop}/predictions")]
        Task<Runs> GetStopPredictions(string agency, string route, string stop);

        [Get("/agencies/{agency}/routes/{route}/vehicles")]
        Task<Runs> GetRouteVehicles(string agency, string route);

        [Get("/agencies/{agency}/routes/{route}/vehicles/{vehicle}")]
        Task<Runs> GetVehicle(string agency, string route, string vehicle);

        [Get("/agencies/{agency}/stops/")]
        Task<Routes> GetStops(string agency);

        [Get("/agencies/{agency}/stops/{stop}/info")]
        Task<RouteInfo> GetStop(string agency, string stop);

        [Get("/agencies/{agency}/stops/{stop}/messages")]
        Task<RouteInfo> GetStopMessages(string agency, string stop);

        [Get("/agencies/{agency}/stops/{stop}/predictions")]
        Task<RouteInfo> GetStopPredictions(string agency, string stop);

        [Get("/agencies/{agency}/stops/{stop}/routes")]
        Task<RouteInfo> GetStopRoutes(string agency, string stop);

        [Get("/agencies/{agency}/vehicles/")]
        Task<Routes> GetVehicles(string agency);

        [Get("/agencies/{agency}/vehicles/{vehicle}")]
        Task<Routes> GetVehicle(string agency, string vehicle);
    }
}
