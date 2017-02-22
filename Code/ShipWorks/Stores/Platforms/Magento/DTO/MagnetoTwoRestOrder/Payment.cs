using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagnetoTwoRestOrder
{
    public class Payment
    {
        [JsonProperty("method")]
        public string Method { get; set; }
    }
}