﻿using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Data
{
    /// <summary>
    /// Service for deleting things from the database
    /// </summary>
    public interface IDeletionService
    {
        /// <summary>
        /// Delete the given store
        /// </summary>
        void DeleteStore(StoreEntity store);
    }
}