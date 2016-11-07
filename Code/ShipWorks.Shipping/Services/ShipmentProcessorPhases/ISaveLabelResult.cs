﻿using System;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Services.ShipmentProcessorPhases
{
    /// <summary>
    /// Results of the save label phase
    /// </summary>
    public interface ISaveLabelResult
    {
        /// <summary>
        /// Entity lock for the current shipment
        /// </summary>
        IDisposable EntityLock { get; }

        /// <summary>
        /// Shipment that is being processed
        /// </summary>
        ShipmentEntity OriginalShipment { get; }

        /// <summary>
        /// The store associated with the shipment
        /// </summary>
        StoreEntity Store { get; }

        /// <summary>
        /// Is the process canceled
        /// </summary>
        bool Canceled { get; }

        /// <summary>
        /// Exception raised while processing
        /// </summary>
        Exception Exception { get; }

        /// <summary>
        /// Was the phase successful
        /// </summary>
        bool Success { get; }
    }
}