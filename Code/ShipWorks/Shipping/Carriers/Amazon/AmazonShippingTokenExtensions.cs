using Interapptive.Shared.Security;
using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.Amazon;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Extension methods to help dealing with the shipping token
    /// </summary>
    public static class AmazonShippingTokenExtensions
    {
        private const string amazonShippingTokenEncryptionKey = "AmazonShippingToken";

        /// <summary>
        /// Get the shipping token from the store
        /// </summary>
        public static AmazonShippingToken GetShippingToken(this IAmazonCredentials store)
        {
            if (string.IsNullOrWhiteSpace(store.ShippingToken))
            {
                return new AmazonShippingToken();
            }

            string decryptedToken = SecureText.Decrypt(store.ShippingToken, amazonShippingTokenEncryptionKey);
            return JsonConvert.DeserializeObject<AmazonShippingToken>(decryptedToken) ?? new AmazonShippingToken();
        }

        /// <summary>
        /// Set the shipping token on the store
        /// </summary>
        public static void SetShippingToken(this IAmazonCredentials store, AmazonShippingToken shippingToken)
        {
            store.ShippingToken = SecureText.Encrypt(JsonConvert.SerializeObject(shippingToken),
                amazonShippingTokenEncryptionKey);
        }
    }
}
