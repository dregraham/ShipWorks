using ShipWorks.Stores.Platforms.Amazon;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Implemented by OrderEntities that could be Amazon Orders (Orders from Amazon or CA maybe others in the future)
    /// </summary>
    public partial class ChannelAdvisorStoreEntity : IAmazonCredentials
    {
        /// <summary>
        /// Amazon auth token
        /// </summary>
        string IAmazonCredentials.AuthToken
        {
            get { return AmazonAuthToken; }
        }

        /// <summary>
        /// Amazon merchant ID
        /// </summary>
        string IAmazonCredentials.MerchantID
        {
            get { return AmazonMerchantID; }
        }

        /// <summary>
        /// Amazon store region
        /// </summary>
        string IAmazonCredentials.Region
        {
            get { return AmazonApiRegion; }
        }

        /// <summary>
        /// Amazon shipping token
        /// </summary>
        string IAmazonCredentials.ShippingToken
        {
            get { return AmazonShippingToken; }
            set { AmazonShippingToken = value; }
        }
    }
}
