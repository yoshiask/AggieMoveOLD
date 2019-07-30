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
            //string js = "var vid = 8365;\n";
            System.Diagnostics.Debug.WriteLine(js);

            try
            {
                return js.Substring(10, 4);
            }
            catch
            {
                return "";
            }
        }
    }
}
