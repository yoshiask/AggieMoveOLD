using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MTATransit.Shared.API.NagerDate
{
    public interface INagerDateApi
    {
        [Get("/PublicHolidays/{year}/{country_code}")]
        Task<List<PublicHoliday>> GetRouteInfo(string country_code, string year);
    }
}
