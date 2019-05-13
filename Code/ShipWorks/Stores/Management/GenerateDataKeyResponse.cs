using System.IO;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Management
{
    public class GenerateDataKeyResponse
    {
        [JsonProperty("CiphertextBlob")]
        public string CiphertextBlob { get; set; }

        [JsonProperty("Plaintext")]
        public string Plaintext { get; set; }
    }
}    