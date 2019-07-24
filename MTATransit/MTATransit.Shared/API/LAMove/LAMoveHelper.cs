using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MTATransit.Shared.API.LAMove
{
    public static class LAMoveHelper
    {
        public static async Task<string> GetVIDFromBus()
        {
            string url = (await Common.LAMoveApi.GetVIDUrl()).Url;
            string js = await new HttpClient().GetAsync(url).Result.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine(js);

            Regex r = new Regex(@"/(var vid = )(?<vid>[0-9]*)(;)+/g",
                RegexOptions.None, TimeSpan.FromMilliseconds(150));
            Match m = r.Match(js);
            if (m.Success)
                return r.Match(url).Result("${vid}");
            else
                return null;
        }
    }
}
