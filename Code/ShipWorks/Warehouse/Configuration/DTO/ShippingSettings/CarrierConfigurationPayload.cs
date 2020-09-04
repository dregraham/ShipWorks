using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ShipWorks.Warehouse.Configuration.DTO.ShippingSettings
{
    [Obfuscation]
    public class CarrierConfigurationPayload
    {
        public ConfigurationAddress Address { get; set; }

        public int HubVersion { get; set; }

        public Guid HubCarrierId { get; set; }

        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; set; }
    }
}
