using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MTATransit.Shared.API.LAMove
{
    public interface ILAMoveApi
    {
        [Get("/agencies/")]
        Task<List<Agency>> GetAgencies();

        [Get("/agencies/{agency}/fares/types")]
        Task<List<FareType>> GetFareTypes(string agency);

        [Get("/agencies/{agency}/fares/tables")]
        Task<List<FareTable>> GetFareTables(string agency);

        [Get("/{agency}/fares/{fareType}?isDisabled={isDisabled}&isStudent={isStudent}&isCollege={isCollege}")]
        Task<FareCost> GetFareCost(string agency, int fareType, bool isDisabled = false, bool isStudent = false, bool isCollege = false);

        [Get("/vidurl")]
        Task<URLResponse> GetVIDUrl();
    }
}
