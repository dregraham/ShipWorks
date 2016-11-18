using System;
using System.Collections.Generic;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Services.ShipmentProcessorSteps
{
    public interface IGetLabelResult
    {
        /// <summary>
        /// Clone made of the original shipment
        /// </summary>
        ShipmentEntity Clone { get; }

        /// <summary>
        /// Fields that may need to be restored on the original shipment
        /// </summary>
        List<ShipmentFieldIndex> FieldsToRestore { get; }

        /// <summary>
        /// Label data that was retrieved for the shipment
        /// </summary>
        IDownloadedLabelData LabelData { get; }

        /// <summary>
        /// Original shipment used for processing
        /// </summary>
        ShipmentEntity OriginalShipment { get; }

        /// <summary>
        /// Store associated with the shipment
        /// </summary>
        StoreEntity Store { get; }

        /// <summary>
        /// Existing entity lock
        /// </summary>
        IDisposable EntityLock { get; }

        /// <summary>
        /// Was the process canceled
        /// </summary>
        bool Canceled { get; }

        /// <summary>
        /// Exception generated during the get label phase or earlier
        /// </summary>
        ShippingException Exception { get; }

        /// <summary>
        /// Was the get label phase successful
        /// </summary>
        bool Success { get; }
    }
}