using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShipWorks.Shipping.ShipEngine.DTOs.Registration
{
    [Obfuscation(Exclude = true)]
    public class UpsRegistrationRequest
    {
        [JsonProperty("nickname")]
        public string Nickname { get; set; }

        [JsonProperty("ups_id")]
        public string UpsId { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("access_license")]
        public string AccessLicense { get; set; }

        [JsonProperty("developer_key")]
        public string DeveloperKey { get; set; }

        //[JsonProperty("promo_code")]
        //public string PromoCode { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("address")]
        public Address Address { get; set; }

        [JsonProperty("weight_units")]
        public string WeightUnits { get; set; }

        [JsonProperty("end_user_ip_address")]
        public string EndUserIpAddress { get; set; }

        //[JsonProperty("device_identity")]
        //public string DeviceIdentity { get; set; }

        [JsonProperty("software_provider")]
        public string SoftwareProvider { get; set; }

        [JsonProperty("software_product_name")]
        public string SoftwareProductName { get; set; }
    }
}
