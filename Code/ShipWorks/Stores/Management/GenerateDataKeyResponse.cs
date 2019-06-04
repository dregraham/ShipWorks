using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Management
{
    /// <summary>
    /// Response to the GenerateDataKeyRequest
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
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
