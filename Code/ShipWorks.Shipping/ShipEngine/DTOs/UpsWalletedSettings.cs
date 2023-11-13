using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShipWorks.Shipping.ShipEngine.DTOs
{
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class UpsWalletedSettings
    {
        //[JsonProperty("nickname", NullValueHandling = NullValueHandling.Ignore)]
        //public string Nickname { get; set; }

        //[JsonProperty("is_primary_account", NullValueHandling = NullValueHandling.Ignore)]
        //public bool IsPrimaryAccount { get; set; }

        //[JsonProperty("use_ground_freight_pricing", NullValueHandling = NullValueHandling.Ignore)]
        //public bool UseGroundFreightPricing { get; set; }

        [JsonProperty("enable_ground_saver_service", NullValueHandling = NullValueHandling.Ignore)]
        public bool EnableGroundSaverService { get; set; }
    }
}
