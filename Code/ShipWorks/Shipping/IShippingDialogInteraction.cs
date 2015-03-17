using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Defines a simple interface for interacting with the shipping dialog
    /// </summary>
    public interface IShippingDialogInteraction
    {
        /// <summary>
        /// Save the specified list of shipments to the database
        /// </summary>
        IDictionary<ShipmentEntity, Exception> SaveShipmentsToDatabase(IEnumerable<ShipmentEntity> shipmentsToSave, bool forceSave);

        /// <summary>
        /// Get the full list of shipments in the shipment control
        /// </summary>
        IEnumerable<ShipmentEntity> FetchShipmentsFromShipmentControl();

        /// <summary>
        /// Checks whether the specified shipment has an error associated with it
        /// </summary>
        bool ShipmentHasError(long shipmentId);

        /// <summary>
        /// Sets an error message for the specified shipment id
        /// </summary>
        void SetShipmentErrorMessage(long shipmentId, Exception exception, string action);
    }
}