using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Jet.DTO
{
    public class JetOrderTotals
    {
        [JsonProperty("item_price")]
        public JetOrderItemPrice ItemPrice { get; set; }

        [JsonProperty("item_fees")]
        public double ItemFees { get; set; }

        [JsonProperty("fee_adjustments")]
        public IList<JetFeeAdjustment> FeeAdjustments { get; set; }

        [JsonProperty("regulatory_fees")]
        public double RegulatoryFees { get; set; }
    }
}