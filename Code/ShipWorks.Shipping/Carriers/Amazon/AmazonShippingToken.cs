using Interapptive.Shared.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Object used to store, encrypt and decrypt an Amazon shipping token
    /// </summary>
    public class AmazonShippingToken
    {
        [JsonProperty("ErrorDate")]
        public string ErrorDate { get; set; }

        [JsonProperty("ErrorReason")]
        public string ErrorReason { get; set; }

        /// <summary>
        /// Decrypts the specified encrypted text.
        /// </summary>
        /// <param name="encryptedText">The encrypted text.</param>
        /// <returns>The decrypted AmazonShippingToken</returns>
        public string Decrypt(string encryptedText)
        {
            JToken decryptedText = JToken.Parse(SecureText.Decrypt(encryptedText, "AmazonShippingToken"));

            ErrorDate = decryptedText.SelectToken("ErrorDate").ToString();
            ErrorReason = decryptedText.SelectToken("ErrorReason").ToString();

            return decryptedText.ToString();
        }

        /// <summary>
        /// Encrypts and returns the current instance of this object
        /// </summary>
        /// <returns>The encrypted AmazonShippingToken</returns>
        public string Encrypt()
        {
            return SecureText.Encrypt(JsonConvert.SerializeObject(this), "AmazonShippingToken");
        }
    }
}
