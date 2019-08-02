using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace MTATransit.Shared.API.LAMove
{
    public class FareCost
    {
        [JsonProperty(PropertyName = "fare")]
        public decimal Fare { get; set; }
    }
}
