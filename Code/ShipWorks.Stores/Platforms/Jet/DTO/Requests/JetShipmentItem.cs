using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Jet.DTO.Requests
{
    public class JetShipmentItem
    {
        [JsonProperty("alt_shipment_item_id")]
        public string AltShipmentItemId { get; set; }

        [JsonProperty("merchant_sku")]
        public string MerchantSku { get; set; }

        [JsonProperty("response_shipment_sku_quantity")]
        public int ResponseShipmentSkuQuantity { get; set; }
    }
}