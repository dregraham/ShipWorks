using System;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Services.ShipmentProcessorPhases
{
    /// <summary>
    /// Results of the save label phase
    /// </summary>
    public class SaveLabelResult : ISaveLabelResult
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SaveLabelResult(IGetLabelResult result)
        {
            EntityLock = result.EntityLock;
            Exception = result.Exception;
            Canceled = result.Canceled;
            OriginalShipment = result.OriginalShipment;
            Store = result.Store;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SaveLabelResult(IGetLabelResult result, Exception exception) : this(result)
        {
            Exception = exception;
        }

        /// <summary>
        /// Entity lock for the current shipment
        /// </summary>
        public IDisposable EntityLock { get; }

        /// <summary>
        /// Shipment that is being processed
        /// </summary>
        public ShipmentEntity OriginalShipment { get; }

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