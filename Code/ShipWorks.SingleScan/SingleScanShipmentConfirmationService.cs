using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Services;
using ShipWorks.Users.Security;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Confirm with the user what they want to do when auto printing shipments
    /// </summary>
    public class SingleScanShipmentConfirmationService : ISingleScanShipmentConfirmationService
    {
        private readonly IOrderLoader orderLoader;
        private readonly Func<ISecurityContext> securityContextRetriever;
        private readonly IAutoPrintConfirmationDlgFactory dlgFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public SingleScanShipmentConfirmationService(IOrderLoader orderLoader, Func<ISecurityContext> securityContextRetriever, IAutoPrintConfirmationDlgFactory dlgFactory)
        {
            this.orderLoader = orderLoader;
            this.securityContextRetriever = securityContextRetriever;
            this.dlgFactory = dlgFactory;
        }

        /// <summary>
        /// Gets the Shipments that SingleScan should auto print/process
        /// </summary>
        public async Task<IEnumerable<ShipmentEntity>> GetShipments(long orderId, string scannedBarcode)
        {
            securityContextRetriever().HasPermission(PermissionType.ShipmentsCreateEditProcess, orderId);

            ShipmentsLoadedEventArgs shipmentArgs = await orderLoader.LoadAsync(new[] { orderId }, ProgressDisplayOptions.NeverShow, true, Timeout.Infinite);




            return shipmentArgs.Shipments;
        }
    }
}