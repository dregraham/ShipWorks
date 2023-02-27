using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Warehouse
{
    [Obfuscation]
    public class ShipmentNotification
    {
        public ShipmentNotification(string trackingNumber, string carrierCode, bool useSwatId)
        {
            TrackingNumber = trackingNumber;
            CarrierCode = carrierCode;
            UseSwatId = useSwatId;
        }

        [JsonProperty("trackingNumber")]
        public string TrackingNumber { get; set; }

        [JsonProperty("carrierCode")]
        public string CarrierCode { get; set; }

        [JsonProperty("useSwatId")]
        public bool UseSwatId { get; set; }
    }
}
