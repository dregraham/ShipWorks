using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Jet.DTO.Requests
{
    public class JetShipmentRequest
    {
        [JsonProperty("alt_order_id")]
        public string AltOrderId{ get; set; }

        [JsonProperty("shipments")]
        public List<Shipment> Shipments { get; set; }
    }

    public class ShipmentItem
    {
        [JsonProperty("alt_shipment_item_id")]
        public string AltShipmentItemId { get; set; }

        [JsonProperty("merchant_sku")]
        public string MerchantSku { get; set; }

        [JsonProperty("response_shipment_sku_quantity")]
        public int ResponseShipmentSkuQuantity { get; set; }
    }

    public class Shipment
    {
        [JsonProperty("alt_shipment_id")]
        public string AltShipmentId { get; set; }

        [JsonProperty("shipment_tracking_number")]
        public string ShipmentTrackingNumber { get; set; }

        [JsonProperty("response_shipment_date")]
        public DateTime ResponseShipmentDate { get; set; }

        [JsonProperty("response_shipment_method")]
        public string ResponseShipmentMethod { get; set; }
        
        [JsonProperty("ship_from_zip_code")]
        public string ShipFromZipCode { get; set; }

        [JsonProperty("carrier")]
        public string Carrier { get; set; }

        [JsonProperty("shipment_items")]
        public List<ShipmentItem> ShipmentItems { get; set; }
    }
}