using Newtonsoft.Json;
using Refit;
using System.Collections.Generic;

namespace MTATransit.Shared.API.OTPMTA
{
    public class PlanResponse
    {
        [JsonProperty(PropertyName = "requestParameters")]
        public PlanRequestParameters RequestParameters { get; set; }

        [JsonProperty(PropertyName = "plan")]
        public Plan Plan { get; set; }
    }

    public class PlanRequestParameters
    {
        [AliasAs("fromPlace")]
        [JsonProperty(PropertyName = "fromPlace")]
        public string FromPlace { get; set; }

        [AliasAs("toPlace")]
        [JsonProperty(PropertyName = "toPlace")]
        public string ToPlace { get; set; }

        [AliasAs("date")]
        [JsonProperty(PropertyName = "date")]
        public string ArriveDate { get; set; }

        [AliasAs("time")]
        [JsonProperty(PropertyName = "time")]
        public long ArriveTime { get; set; }
    }
}
