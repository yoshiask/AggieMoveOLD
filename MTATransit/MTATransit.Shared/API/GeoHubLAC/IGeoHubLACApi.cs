using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MTATransit.Shared.API.GeoHubLAC
{
    public interface IGeoHubLACApi
    {
        [Get("/query?where=1%3D1&outFields=source,org_name,Name,hours,url,Shape,post_id,description,latitude,longitude,POINT_Y,POINT_X&outSR=4326&f=json")]
        Task<string> GetBikeways();
    }
}
