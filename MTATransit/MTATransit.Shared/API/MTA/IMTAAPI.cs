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
    }
}
