﻿using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagnetoTwoRestOrder
{
    public class CustomOption
    {
        [JsonProperty("option_id")]
        public string OptionID { get; set; }

        [JsonProperty("option_value")]
        public string OptionValue { get; set; }

        [JsonProperty("extension_attributes")]
        public ExtensionAttributes ExtensionAttributes { get; set; }
    }
}