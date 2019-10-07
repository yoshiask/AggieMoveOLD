using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MTATransit.Shared.API.OTP
{
    public interface IOTPApi
    {
        #region CalculatePlan Overrides
        [Get("/routers/default/plan?fromPlace={from}&toPlace={to}")]
        Task<PlanResponse> CalculatePlan(string from, string to);

        [Get("/routers/default/plan")]
        Task<PlanResponse> CalculatePlan(PlanRequestParameters param);

        [Get("/routers/{routerId}/plan?fromPlace={from}&toPlace={to}")]
        Task<PlanResponse> CalculatePlan(string routerId, string from, string to);

        [Get("/routers/{routerId}/plan")]
        Task<PlanResponse> CalculatePlan(string routerId, PlanRequestParameters param);
        #endregion
    }
}
