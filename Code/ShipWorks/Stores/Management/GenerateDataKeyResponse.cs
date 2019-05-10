using System.IO;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Management
{
    public class GenerateDataKeyResponse
    {
        [JsonProperty("CiphertextBlob")]
        public MemoryStream CiphertextBlob { get; set; }

        [JsonProperty("Plaintext")]
        public MemoryStream Plaintext { get; set; }
    }
}    