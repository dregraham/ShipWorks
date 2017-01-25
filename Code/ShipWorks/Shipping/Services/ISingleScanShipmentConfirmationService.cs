﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Services
{
    /// <summary>
    /// Interface for getting the shipment to auto print with single scan
    /// </summary>
    [Service]
    public interface ISingleScanShipmentConfirmationService
    {
        /// <summary>
        /// Gets the Shipments that SingleScan should auto print/process
        /// </summary>
        /// <param name="OrderId">the OrderID</param>
        /// <param name="scannedBarcode">the barcode that was scanned, used to dismiss any dialogs that we need to show</param>
        /// <returns></returns>
        Task<IEnumerable<ShipmentEntity>> GetShipments(long OrderId, string scannedBarcode);
    }
}