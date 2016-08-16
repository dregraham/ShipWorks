using System;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Defines a simple interface for interacting with the shipping dialog
    /// </summary>
    public interface IShippingErrorManager
    {
        /// <summary>
        /// Checks whether the specified shipment has an error associated with it
        /// </summary>
        bool ShipmentHasError(long shipmentId);

        /// <summary>
        /// Sets an error message for the specified shipment id
        /// </summary>
        string SetShipmentErrorMessage(long shipmentID, Exception ex);

        /// <summary>
        /// Get an error for the given shipment, if there is one
        /// </summary>
        Exception GetErrorForShipment(long shipmentID);

        /// <summary>
        /// Sets an error message for the specified shipment id
        /// </summary>
        string SetShipmentErrorMessage(long shipmentId, Exception exception, string action);

        /// <summary>
        /// Clear all shipping errors
        /// </summary>
        void Clear();

        /// <summary>
        /// Remove the specific shipping error
        /// </summary>
        void Remove(long shipmentID);
    }
}