using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShipWorks.Shipping.ShipEngine.DTOs.Registration
{
    public class UpsRegistrationResponse
    {
        [JsonProperty("account_number")]
        public string AccountNumber { get; set; }

        [JsonProperty("ups_id")]
        public string UpsId { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("access_license")]
        public string AccessLicense { get; set; }

        [JsonProperty("carrier_id")]
        public string CarrierId { get; set; }
    }
}
