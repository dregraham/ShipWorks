namespace ShipWorks.Stores.Platforms.Shopify
{
    /// <summary>
    /// Shopify shipment has already uploaded tracking information Exception 
    /// </summary>
    public class ShopifyAlreadyUploadedException : ShopifyException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyAlreadyUploadedException()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyAlreadyUploadedException(string message) 
            : base(message)
        {
        }
    }
}
