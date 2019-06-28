using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse.DTO
{
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class VoidShipment
    {
        [JsonProperty("tangoShipmentId")]
        public string TangoShipmentId { get; set; }

        [JsonProperty("shipworksShipmentId")]
        public long ShipworksShipmentId { get; set; }
    }
}
