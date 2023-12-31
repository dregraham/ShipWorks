﻿using ShipWorks.Stores.Platforms.Amazon;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Implemented by OrderEntities that could be Amazon Orders (Orders from Amazon or CA maybe others in the future)
    /// </summary>
    public partial class AmazonStoreEntity : IAmazonCredentials
    {
        /// <summary>
        /// Amazon auth token
        /// </summary>
        string IAmazonCredentials.AuthToken
        {
            get { return AuthToken; }
            set
            {
                // No setter needed for this implementation
            }
        }

        /// <summary>
        /// Amazon merchant ID
        /// </summary>
        string IAmazonCredentials.MerchantID
        {
            get { return MerchantID; }
            set
            {
                // No setter needed for this implementation
            }
        }

        /// <summary>
        /// Amazon store region
        /// </summary>
        string IAmazonCredentials.Region
        {
            get { return AmazonApiRegion; }
            set
            {
                // No setter needed for this implementation
            }
        }
    }
}
