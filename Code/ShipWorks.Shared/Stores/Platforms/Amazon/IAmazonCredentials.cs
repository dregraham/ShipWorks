namespace ShipWorks.Stores.Platforms.Amazon
{
    /// <summary>
    /// Implemented by store types that pull amazon orders
    /// </summary>
    public interface IAmazonCredentials
    {
        /// <summary>
        /// Amazon merchant ID
        /// </summary>
        string MerchantID { get; }

        /// <summary>
        /// Amazon auth token
        /// </summary>
        string AuthToken { get; }

        /// <summary>
        /// Amazon shipping token
        /// </summary>
        string ShippingToken { get; set; }

        /// <summary>
        /// Amazon store region
        /// </summary>
        string Region { get; }
    }
}
