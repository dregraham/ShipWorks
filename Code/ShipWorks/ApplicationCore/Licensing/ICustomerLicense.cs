﻿using ShipWorks.Stores;
using System.Collections.Generic;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Interface for customer license
    /// </summary>
    public interface ICustomerLicense : ILicense
    {
        /// <summary>
        /// Saves the license
        /// </summary>
        void Save();

        /// <summary>
        /// IEnumerable of ActiveStores for the license
        /// </summary>
        IEnumerable<ActiveStore> GetActiveStores();

        /// <summary>
        /// Deletes the given channel
        /// </summary>
        void DeleteChannel(StoreTypeCode storeType);
    }
}