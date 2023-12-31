﻿using System;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Services.ShipmentProcessorSteps
{
    /// <summary>
    /// Results of the label persistence phase
    /// </summary>
    public class LabelPersistenceResult : ILabelPersistenceResult
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public LabelPersistenceResult(ILabelRetrievalResult result) : this(result, result.OriginalShipment)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public LabelPersistenceResult(ILabelRetrievalResult result, ShipmentEntity shipmentForTango)
        {
            Index = result.Index;
            EntityLock = result.EntityLock;
            Exception = result.Exception;
            Canceled = result.Canceled;
            OriginalShipment = result.OriginalShipment;
            ShipmentForTango = shipmentForTango;
            Store = result.Store;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public LabelPersistenceResult(ILabelRetrievalResult result, ShipmentEntity shipmentForTango, Exception exception)
        {
            Index = result.Index;
            EntityLock = result.EntityLock;
            Exception = exception;
            Canceled = result.Canceled;
            OriginalShipment = result.OriginalShipment;
            ShipmentForTango = shipmentForTango;
            Store = result.Store;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public LabelPersistenceResult(ILabelRetrievalResult result, Exception exception) : this(result, result.OriginalShipment)
        {
            Index = result.Index;
            Exception = exception;
        }

        /// <summary>
        /// Index of the shipment being processed
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Entity lock for the current shipment
        /// </summary>
        public IDisposable EntityLock { get; }

        /// <summary>
        /// Shipment that is being processed
        /// </summary>
        public ShipmentEntity OriginalShipment { get; }

        /// <summary>
        /// Shipment that should be sent to Tango
        /// </summary>
        public ShipmentEntity ShipmentForTango { get; }

        /// <summary>
        /// The store associated with the shipment
        /// </summary>
        public StoreEntity Store { get; }

        /// <summary>
        /// Is the process canceled
        /// </summary>
        public bool Canceled { get; }

        /// <summary>
        /// Exception raised while processing
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Was the phase successful
        /// </summary>
        public bool Success => Exception == null && !Canceled;
    }
}