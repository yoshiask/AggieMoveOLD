using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MTATransit.Shared.API.ArcGIS
{
    public interface IArcGISApi
    {
        [Get("/suggest?text={text}&f=json")]
        Task<Suggestions> GetSuggestions(string text);

        [Get("/addressCandidates?SingleLine={text}&magicKey={key}&f=json")]
        Task<GeocodeResponse> Geocode(string text, string key);
    }
}
