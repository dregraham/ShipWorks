using System;
using System.Collections.Generic;
using Interapptive.Shared.Collections;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Services.ShipmentProcessorSteps
{
    /// <summary>
    /// Data resulting from preparing a shipment for processing
    /// </summary>
    public class ShipmentPreparationResult : IShipmentPreparationResult
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentPreparationResult(IDisposable entityLock, ProcessShipmentState state, bool canceled)
        {
            Index = state.Index;
            OriginalShipment = state.OriginalShipment;
            EntityLock = entityLock;
            Canceled = canceled;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentPreparationResult(IDisposable entityLock, ProcessShipmentState state, Exception exception)
        {
            Index = state.Index;
            OriginalShipment = state.OriginalShipment;
            EntityLock = entityLock;
            Exception = exception as ShippingException ?? new ShippingException(exception.Message, exception);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentPreparationResult(IDisposable entityLock, ProcessShipmentState state,
            IEnumerable<ShipmentEntity> shipmentsToTryToProcess,
            StoreEntity store)
        {
            Index = state.Index;
            OriginalShipment = state.OriginalShipment;
            EntityLock = entityLock;
            Shipments = shipmentsToTryToProcess.ToReadOnly();
            Store = store;
        }

        /// <summary>
        /// Index of the shipment being processed
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Was processing canceled
        /// </summary>
        public bool Canceled { get; }

        /// <summary>
        /// Entity lock for the shipment
        /// </summary>
        public IDisposable EntityLock { get; }

        /// <summary>
        /// Exception from the preparation phase, if any
        /// </summary>
        public ShippingException Exception { get; }

        /// <summary>
        /// Original shipment
        /// </summary>
        public ShipmentEntity OriginalShipment { get; }

        /// <summary>
        /// Ordered collection of shipments to try and process
        /// </summary>
        public IEnumerable<ShipmentEntity> Shipments { get; }

        /// <summary>
        /// Store associated with the shipment
        /// </summary>
        public StoreEntity Store { get; }

        /// <summary>
        /// Was the preparation phase successful
        /// </summary>
        public bool Success => Exception == null && !Canceled;
    }
}