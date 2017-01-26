using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.Collections;
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
        private readonly IShipmentFactory shipmentFactory;
        private readonly Func<IWin32Window> ownerFactory;

        private const string AlreadyProcessedMessage = "The scanned Order # has already been processed. To create and print a new label scan the barcode again or click Continue";
        private const string PartiallyProcessedMessage = "The scanned Order # has been partially processed. To create a label for each unprocessed shipment in the order, scan the barcode again or click Continue.";
        private const string MultipleUnprocessedMessage = "The scanned Order # has multiple unprocessed shipments.  To print labels for each shipment in the order, scan the barcode again or click Continue." ;

        /// <summary>
        /// Constructor
        /// </summary>
        public SingleScanShipmentConfirmationService(IOrderLoader orderLoader, Func<ISecurityContext> securityContextRetriever, IAutoPrintConfirmationDlgFactory dlgFactory, IShipmentFactory shipmentFactory, Func<IWin32Window> ownerFactory)
        {
            this.orderLoader = orderLoader;
            this.securityContextRetriever = securityContextRetriever;
            this.dlgFactory = dlgFactory;
            this.shipmentFactory = shipmentFactory;
            this.ownerFactory = ownerFactory;
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

            // Get all of the shipments for the order id that are not voided, this will add a new shipment if the order currently has no shipments
            ShipmentEntity[] shipments =
                (await orderLoader.LoadAsync(new[] {orderId}, ProgressDisplayOptions.NeverShow, true, Timeout.Infinite))
                .Shipments.Where(s => !s.Voided).ToArray();

            if (shipments.IsCountEqualTo(1) && shipments.All(s => !s.Processed))
            {
                return shipments;
            }

            if (ShouldPrintAndProcessShipments(shipments, scannedBarcode))
            {
                // If all of the shipments are processed and the user confirms they want to process again add a shipment
                if (shipments.All(s => s.Processed))
                {
                    return new[] { shipmentFactory.Create(shipments.First().Order) };
                }

                // If some of the shipments are not process and the user confirms return only the unprocessed shipments
                if (shipments.Any(s => !s.Processed))
                {
                    return shipments.Where(s => !s.Processed);
                }
            }

            return new ShipmentEntity[0];
        }

        /// <summary>
        /// Check to see if we should print and process the given shipments
        /// </summary>
        /// <param name="shipments">the list of shipments</param>
        /// <param name="scannedBarcode">the barcode that will accept and dismiss the dialog</param>
        /// <returns></returns>
        private bool ShouldPrintAndProcessShipments(ShipmentEntity[] shipments, string scannedBarcode)
        {
            // If we have a single unprocessed shipment always return true
            if (shipments.IsCountEqualTo(1) && shipments.All(s => !s.Processed))
            {
                return true;
            }

            KeyValuePair<string, string> messaging = GetMessaging(shipments);

            using (IFormsDialog dialog = dlgFactory.Create(scannedBarcode, messaging.Key, messaging.Value))
            {
                return dialog.ShowDialog(ownerFactory()) == DialogResult.OK;
            }
        }

        /// <summary>
        /// Get the Title/Message text to display to the user based on the Shipments
        /// </summary>
        private KeyValuePair<string, string> GetMessaging(ShipmentEntity[] shipments)
        {
            if (shipments.All(s => s.Processed))
            {
                return new KeyValuePair<string, string>("Order Already Processed", AlreadyProcessedMessage);
            }

            if (shipments.Any(s => !s.Processed) && shipments.Any(s => s.Processed))
            {
                return new KeyValuePair<string, string>("Order Partially Processed", PartiallyProcessedMessage);
            }

            return new KeyValuePair<string, string>("Multiple Unprocessed Shipments", MultipleUnprocessedMessage);
        }
    }
}