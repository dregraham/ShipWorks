using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.SparkPay.DTO;

namespace ShipWorks.Stores.Platforms.Jet.DTO
{
    public class JetShippingTo
    {
        [JsonProperty("recipient")]
        public JetRecipient Recipient { get; set; }

        [JsonProperty("address")]
        public JetAddress Address { get; set; }
    }
}