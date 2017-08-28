using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Jet.DTO
{
    public class JetOrderItem
    {
        [JsonProperty("order_item_id")]
        public string OrderItemId { get; set; }

        [JsonProperty("merchant_sku")]
        public string MerchantSku { get; set; }

        [JsonProperty("request_order_quantity")]
        public double RequestOrderQuantity { get; set; }

        [JsonProperty("item_tax_code")]
        public string ItemTaxCode { get; set; }

        [JsonProperty("item_price")]
        public JetOrderItemPrice ItemPrice { get; set; }

        [JsonProperty("item_fees")]
        public double ItemFees { get; set; }

        [JsonProperty("fee_adjustments")]
        public IList<JetFeeAdjustment> FeeAdjustments { get; set; }

        [JsonProperty("regulatory_fees")]
        public double RegulatoryFees { get; set; }

        [JsonProperty("product_title")]
        public string ProductTitle { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}