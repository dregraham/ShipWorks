using System;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Messaging.Messages.Shipping
{
    /// <summary>
    /// Results of processing a shipment
    /// </summary>
    public struct ProcessShipmentResult
    {
        /// <summary>
        /// Constructor for successful result
        /// </summary>
        public ProcessShipmentResult(ShipmentEntity shipment)
        {
            Shipment = shipment;
            Error = null;
        }

        /// <summary>
        /// Constructor for failure result
        /// </summary>
        public ProcessShipmentResult(ShipmentEntity shipment, Exception error)
        {
            Shipment = shipment;
            Error = error;
        }

        /// <summary>
        /// Shipment that was processed
        /// </summary>
        public ShipmentEntity Shipment { get; }

        /// <summary>
        /// Error generated while processing, if any
        /// </summary>
        public Exception Error { get; }

        /// <summary>
        /// Was processing successful
        /// </summary>
        public bool IsSuccessful => Error != null;
    }
}
