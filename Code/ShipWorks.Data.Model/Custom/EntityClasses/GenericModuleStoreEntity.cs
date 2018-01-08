using ShipWorks.Stores.Platforms.Amazon;

namespace ShipWorks.Data.Model.EntityClasses
{
    public partial class GenericModuleStoreEntity : IAmazonCredentials
    {
        /// <summary>
        /// Amazon auth token
        /// </summary>
        public string AuthToken
        {
            get => AmazonAuthToken;
            set => AmazonAuthToken = value;
        }

        /// <summary>
        /// Amazon merchant ID
        /// </summary>
        public string MerchantID
        {
            get => AmazonMerchantID;
            set => AmazonMerchantID = value;
        }

        /// <summary>
        /// Amazon store region
        /// </summary>
        public string Region
        {
            get => AmazonApiRegion;
            set => AmazonApiRegion = value;
        }
    }
}
