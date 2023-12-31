﻿using System.Reactive;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using System.Threading.Tasks;

namespace ShipWorks.Stores.Platforms.BigCommerce.AccountSettings
{
    /// <summary>
    /// Verify a connection to BigCommerce
    /// </summary>
    public interface IBigCommerceConnectionVerifier
    {
        /// <summary>
        /// Verify the connection
        /// </summary>
        Task<IResult> Verify(BigCommerceStoreEntity store, IBigCommerceAuthenticationPersistenceStrategy persistenceStrategy);
    }
}