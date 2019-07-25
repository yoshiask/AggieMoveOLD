using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace MTATransit.Shared.API.OTPMTA
{
    public class Step
    {
        [JsonProperty(PropertyName = "distance")]
        public double Distance { get; set; }

        [JsonProperty(PropertyName = "relativeDirection")]
        public RelativeDirection RelativeDirection { get; set; }

        [JsonProperty(PropertyName = "streetName")]
        public string StreetName { get; set; }

        [JsonProperty(PropertyName = "absoluteDirection")]
        public AbsoluteDirection AbsoluteDirection { get; set; }

        [JsonProperty(PropertyName = "stayOn")]
        public bool StayOn { get; set; }

        [JsonProperty(PropertyName = "area")]
        public bool Area { get; set; }

        [JsonProperty(PropertyName = "bogusName")]
        public bool BogusName { get; set; }

        [JsonProperty(PropertyName = "lon")]
        public long Longitude { get; set; }

        [JsonProperty(PropertyName = "lat")]
        public long Latitude { get; set; }

        [JsonProperty(PropertyName = "elevation")]
        public List<object> Elevation { get; set; }
    }

    public enum RelativeDirection
    {
        DEPART,
        HARD_LEFT,
        LEFT,
        SLIGHTLY_LEFT,
        CONTINUE,
        SLIGHTLY_RIGHT,
        RIGHT,
        HARD_RIGHT,
        CIRCLE_CLOCKWISE,
        CIRCLE_COUNTERCLOCKWISE,
        ELEVATOR,
        UTURN_LEFT,
        UTURN_RIGHT
    }

    public enum AbsoluteDirection
    {
        NORTH,
        NORTHEAST,
        EAST,
        SOUTHEAST,
        SOUTH,
        SOUTHWEST,
        WEST,
        NORTHWEST
    }
}
