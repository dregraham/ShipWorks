﻿using System;

namespace ShipWorks.Data
{
    /// <summary>
    /// Interface for identifying a database
    /// </summary>
    public interface IDatabaseIdentifier
    {
        /// <summary>
        /// Gets the database identifier
        /// </summary>
        Guid Get();

        /// <summary>
        /// Reset the cached database identifier
        /// </summary>
        void Reset();
    }
}