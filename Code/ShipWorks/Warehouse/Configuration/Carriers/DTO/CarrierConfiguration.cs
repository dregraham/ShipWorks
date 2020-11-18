using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Shipping;
using ShipWorks.Warehouse.Configuration.DTO;

namespace ShipWorks.Warehouse.Configuration.Carriers.DTO
{
    /// <summary>
    /// DTO for importing a carrier confiugration from hub
    /// </summary>
    [Obfuscation]
    public class CarrierConfiguration
    {
        /// <summary>
        /// The Hub ID for this carrier
        /// </summary>
        [JsonProperty("id")]
        public Guid HubCarrierID { get; set; }

        /// <summary>
        /// The version of the configuration that is in hub
        /// </summary>
        public int HubVersion { get; set; }

        /// <summary>
        /// The type code of the configured carrier
        /// </summary>
        public ShipmentTypeCode CarrierType { get; set; }

        /// <summary>
        /// The ShipEngine Carrier ID
        /// </summary>
        [JsonProperty("shipEngineCarrierId")]
        public string ShipEngineCarrierID { get; set; }

        /// <summary>
        /// The requested label format
        /// </summary>
        public ThermalLanguage RequestedLabelFormat { get; set; }

        /// <summary>
        /// The address for the carrier
        /// </summary>
        public ConfigurationAddress Address { get; set; }

        /// <summary>
        /// Additional carrier specific configuration data
        /// </summary>
        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; set; }
    }
}
