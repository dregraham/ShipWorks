using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Services.ShipmentProcessorSteps
{
    /// <summary>
    /// Data resulting from preparing a shipment for processing
    /// </summary>
    public interface IShipmentPreparationResult
    {
        /// <summary>
        /// Index of the shipment being processed
        /// </summary>
        int Index { get; }

        /// <summary>
        /// Was processing canceled
        /// </summary>
        bool Canceled { get; }

        /// <summary>
        /// Entity lock for the shipment
        /// </summary>
        IDisposable EntityLock { get; }

        /// <summary>
        /// Exception from the preparation phase, if any
        /// </summary>
        ShippingException Exception { get; }

        /// <summary>
        /// Original shipment
        /// </summary>
        ShipmentEntity OriginalShipment { get; }

        /// <summary>
        /// Ordered collection of shipments to try and process
        /// </summary>
        IEnumerable<ShipmentEntity> Shipments { get; }

        /// <summary>
        /// Store associated with the shipment
        /// </summary>
        StoreEntity Store { get; }

        /// <summary>
        /// Was the preparation phase successful
        /// </summary>
        bool Success { get; }
    }
}