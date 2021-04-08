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
        public ShipmentNotification(string trackingNumber, string carrierCode)
        {
            TrackingNumber = trackingNumber;
            CarrierCode = carrierCode;
        }

        [JsonProperty("trackingNumber")]
        public string TrackingNumber { get; set; }

        [JsonProperty("carrierCode")]
        public string CarrierCode { get; set; }
    }
}
