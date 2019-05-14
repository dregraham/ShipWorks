using Newtonsoft.Json;

namespace ShipWorks.Stores.Management
{
    /// <summary>
    /// Response to the GenerateDataKeyRequest
    /// </summary>
    public class GenerateDataKeyResponse
    {
        /// <summary>
        /// The encrypted key
        /// </summary>
        [JsonProperty("CiphertextBlob")]
        public string CiphertextBlob { get; set; }

        /// <summary>
        /// The key
        /// </summary>
        [JsonProperty("Plaintext")]
        public string Plaintext { get; set; }
    }
}
