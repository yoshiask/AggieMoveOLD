using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MTATransit.Shared.API.Yelp
{
    public interface IYelpApi
    {
        [Headers("Authorization: Bearer dBrl_wjFTZ2mbtvSobrwDyMOeCvui486ccXpfnM0-J-vJoLuB4oSEiARKQGb6QvugENi7oouIj0JCs_dkcZcl99mzz1SHHzIaHTA0NqMw04evYHa3OZYppRmPruWXXYx")]
        [Get("/businesses/search?latitude={latitude}&longitude={longitude}&term={term}")]
        Task<BusinessSearchResponse> BusinessSearch(decimal latitude, decimal longitude, string term);
    }
}
