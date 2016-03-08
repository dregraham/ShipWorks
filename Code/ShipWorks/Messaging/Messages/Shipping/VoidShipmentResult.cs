using System;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Messaging.Messages.Shipping
{
    /// <summary>
    /// Results of voiding a shipment
    /// </summary>
    public struct VoidShipmentResult
    {
        /// <summary>
        /// Constructor for successful result
        /// </summary>
        public VoidShipmentResult(ShipmentEntity shipment)
        {
            Shipment = shipment;
            Error = null;
        }

        /// <summary>
        /// Constructor for failure result
        /// </summary>
        public VoidShipmentResult(ShipmentEntity shipment, Exception error)
        {
            Shipment = shipment;
            Error = error;
        }

        /// <summary>
        /// Shipment that was voided
        /// </summary>
        public ShipmentEntity Shipment { get; }

        /// <summary>
        /// Error generated while voiding, if any
        /// </summary>
        public Exception Error { get; }

        /// <summary>
        /// Was voiding successful
        /// </summary>
        public bool IsSuccessful => Error == null;
    }
}
