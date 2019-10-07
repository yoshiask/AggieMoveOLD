using Newtonsoft.Json;
using System.Collections.Generic;

namespace MTATransit.Shared.API.Yelp
{
    public class Address
    {
        [JsonProperty(PropertyName = "address1")]
        public string Address1 { get; set; }

        [JsonProperty(PropertyName = "address2")]
        public string Address2 { get; set; }

        [JsonProperty(PropertyName = "address3")]
        public string Address3 { get; set; }

        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }

        [JsonProperty(PropertyName = "zip_code")]
        public string ZipCode { get; set; }

        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "display_address")]
        public string[] DisplayAddress { get; set; }

        public string AddressText()
        {
            string output = "";
            foreach (string str in DisplayAddress)
            {
                output += str;
                output += ", ";
            }
            return output;
        }
    }
}
