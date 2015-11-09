using Interapptive.Shared.Utility;
using Newtonsoft.Json;
using ShipWorks.Data.Model.EntityClasses;

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
        public static AmazonShippingToken GetShippingToken(this AmazonStoreEntity store)
        {
            if (string.IsNullOrWhiteSpace(store.AmazonShippingToken))
            {
                return new AmazonShippingToken();
            }

            string decryptedToken = SecureText.Decrypt(store.AmazonShippingToken, amazonShippingTokenEncryptionKey);
            return JsonConvert.DeserializeObject<AmazonShippingToken>(decryptedToken) ?? new AmazonShippingToken();
        }

        /// <summary>
        /// Set the shipping token on the store
        /// </summary>
        public static void SetShippingToken(this AmazonStoreEntity store, AmazonShippingToken shippingToken) =>
            store.AmazonShippingToken = SecureText.Encrypt(JsonConvert.SerializeObject(shippingToken), amazonShippingTokenEncryptionKey);
    }
}
