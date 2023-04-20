using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.Platform.OnlineUpdating;

namespace ShipWorks.Stores.Warehouse
{
    [Obfuscation]
    public class ShipmentNotification
    {
        public ShipmentNotification(string trackingNumber, string trackingUrl, string carrierCode, bool useSwatId, List<SalesOrderItem> salesOrderItems, bool? notifyBuyer)
        {
            TrackingNumber = trackingNumber;
            TrackingUrl = trackingUrl;
            CarrierCode = carrierCode;
            UseSwatId = useSwatId;
            SalesOrderItems = salesOrderItems;
            NotifyBuyer = notifyBuyer;
        }

        [JsonProperty("trackingNumber")]
        public string TrackingNumber { get; set; }

        [JsonProperty("trackingUrl")]
        public string TrackingUrl { get; set; }

        [JsonProperty("carrierCode")]
        public string CarrierCode { get; set; }

        [JsonProperty("useSwatId")]
        public bool UseSwatId { get; set; }

        [JsonProperty("salesOrderItems")]
        public List<SalesOrderItem> SalesOrderItems { get; set; }

        [JsonProperty("notifyBuyer")]
        public bool? NotifyBuyer { get; set; }
    }
}
