using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MTATransit.Shared.API.RestBus
{
    public interface IRestBusApi
    {
        [Get("/locations/{lat},{lon}/predictions")]
        Task<List<Prediction>> GetPredictions(int lat, int lon);
    }
}
