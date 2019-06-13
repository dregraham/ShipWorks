﻿using System;
using System.Threading.Tasks;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Communication
{
    /// <summary>
    /// Get download starting points
    /// </summary>
    public interface IDownloadStartingPoint
    {
        /// <summary>
        /// Obtains the most recent order date.  If there is none, and the store has an InitialDaysBack policy, it
        /// will be used to calculate the initial number of days back to.
        /// </summary>
        Task<DateTime?> OnlineLastModified(IStoreEntity store);

        /// <summary>
        /// Obtains the most recent order date.  If there is none, and the store has an InitialDaysBack policy, it
        /// will be used to calculate the initial number of days back to.
        /// </summary>
        Task<DateTime?> OrderDate(IStoreEntity store);

        /// <summary>
        /// Obtains the most recent order date.  If there is none, and the store has an InitialDaysBack policy, it
        /// will be used to calculate the initial number of days back to.
        /// </summary>
        Task<DateTime?> CustomDate(IStoreEntity store, EntityField2 field);

        /// <summary>
        /// Gets the largest OrderNumber we have in our database for non-manual orders for this store.  If no
        /// such orders exist, then if there is an InitialDownloadPolicy it is applied.  Otherwise, 0 is returned.
        /// </summary>
        Task<long> OrderNumber(IStoreEntity store);

        /// <summary>
        /// Gets the largest HubSequence we have in our database for non-manual orders for this store.  If no
        /// such orders exist, then 0 is returned.
        /// </summary>
        Task<long> HubSequence(IStoreEntity store);
    }
}
