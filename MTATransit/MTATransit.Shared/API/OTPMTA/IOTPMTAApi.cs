using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MTATransit.Shared.API.OTPMTA
{
    public interface IOTPMTAApi
    {
        #region CalculatePlan Overrides
        [Get("/routers/default/plan?fromPlace={from}&toPlace={to}")]
        Task<PlanResponse> CalculatePlan(string from, string to);

        [Get("/routers/default/plan")]
        Task<PlanResponse> CalculatePlan(PlanRequestParameters param);
        #endregion
    }
}
