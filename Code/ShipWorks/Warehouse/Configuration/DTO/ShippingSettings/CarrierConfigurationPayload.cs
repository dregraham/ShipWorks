using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShipWorks.Common.IO.Hardware.Printers;

namespace ShipWorks.Warehouse.Configuration.DTO.ShippingSettings
{
    /// <summary>
    /// DTO for importing a carrier confiugration from hub
    /// </summary>
    [Obfuscation]
    public class CarrierConfigurationPayload
    {
        /// <summary>
        /// The address for the carrier
        /// </summary>
        public ConfigurationAddress Address { get; set; }

        /// <summary>
        /// The version of the configuration that is in hub
        /// </summary>
        public int HubVersion { get; set; }

        ///<summary>
        /// The hub carrier id
        ///</summary>
        public Guid HubCarrierId { get; set; }

        /// <summary>
        /// The requested label format
        /// </summary>
        public ThermalLanguage RequestedLabelFormat { get; set; }

        /// <summary>
        /// Additional carrier specific configuration data
        /// </summary>
        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; set; }
    }
}
