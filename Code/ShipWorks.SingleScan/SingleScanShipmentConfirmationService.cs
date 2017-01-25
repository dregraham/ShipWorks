using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Interapptive.Shared.UI;
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

        private const string scenarioOneMessage =
            "The scanned Order # has already been processed. To create and print a new label and reprint all existing labels for the selected order, scan the barcode again or click Continue";
        private const string scenarioTwo =
                    "The scanned Order # has been partially processed. To reprint existing labels, and create new labels for all unprocessed shipments in the order, scan the barcode again or click Continue.";
        private const string scenarioThree =
                    "The scanned Order # has multiple unprocessed shipments.  To print labels for each shipment in the order, scan the barcode again or click Continue." ;


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
            if(!securityContextRetriever().HasPermission(PermissionType.ShipmentsCreateEditProcess, orderId))
            {
                throw new ShippingException("Auto printing is not allowed for the scanned order.");
            }

            ShipmentEntity[] shipments =
                (await orderLoader.LoadAsync(new[] {orderId}, ProgressDisplayOptions.NeverShow, true, Timeout.Infinite))
                .Shipments.Where(s => !s.Voided).ToArray();

            bool shouldPrintAndProcess = ShouldPrintAndProcessShipments(shipments, scannedBarcode);

            return shouldPrintAndProcess ? shipments : new ShipmentEntity[0];
        }

        /// <summary>
        /// Check to see if we should print and process the given shipments
        /// </summary>
        /// <param name="shipments">the list of shipments</param>
        /// <param name="scannedBarcode">the barcode that will accept and dismiss the dialog</param>
        /// <returns></returns>
        private bool ShouldPrintAndProcessShipments(ShipmentEntity[] shipments, string scannedBarcode)
        {
            KeyValuePair<string, string> messaging = GetMessaging(shipments);
            IDialog dialog = dlgFactory.Create(scannedBarcode, messaging.Key, messaging.Value);

            bool? result = dialog.ShowDialog();

            return result != null && result.Value;
        }

        /// <summary>
        /// Get the Title/Message text to display to the user based on the Shipments
        /// </summary>
        private KeyValuePair<string, string> GetMessaging(ShipmentEntity[] shipments)
        {
            if (shipments.All(s => s.Processed))
            {
                return new KeyValuePair<string, string>("Order Already Processed", scenarioOneMessage);
            }

            if (shipments.All(s => !s.Processed))
            {
                return new KeyValuePair<string, string>("Multiple Unprocessed Shipments", scenarioThree);
            }

            return new KeyValuePair<string, string>("Order Partially Processed", scenarioTwo);
        }
    }
}