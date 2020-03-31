﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Metrics;
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
    [Component]
    public class SingleScanShipmentConfirmationService : ISingleScanShipmentConfirmationService
    {
        private readonly IOrderLoader orderLoader;
        private readonly Func<ISecurityContext> securityContext;
        private readonly IAutoPrintConfirmationDlgFactory dlgFactory;
        private readonly IShipmentFactory shipmentFactory;
        private readonly IMessageHelper messageHelper;
        private readonly Func<string, ITrackedDurationEvent> trackedDurationEventFactory;
        private readonly ISingleScanAutomationSettings singleScanAutomationSettings;
        private readonly ICarrierShipmentAdapterFactory shipmentAdapterFactory;

        private const string AlreadyProcessedMessage = "The scanned order has been previously processed. To create and print a new label, scan the barcode again or click 'Create New Label'.";
        private const string MultipleShipmentsMessage = "The scanned order has multiple shipments. To create a label for each unprocessed shipment in the order, scan the barcode again or click '{0}'.";
        private const string MultiplePackageMessage = "The resulting shipment has multiple packages. To create a label for each package, scan the barcode again or click '{0}'.";
        public const string CannotProcessNoneMessage = "The resulting shipment has a carrier of \"None\".\r\n The carrier \"None\" does not support processing.";

        private const string AutoWeighMessage = "{0}\r\n\r\nNote: ShipWorks will update each {1} with the weight from the scale.";

        /// <summary>
        /// Constructor
        /// </summary>
        public SingleScanShipmentConfirmationService(IOrderLoader orderLoader,
            Func<ISecurityContext> securityContext,
            IAutoPrintConfirmationDlgFactory dlgFactory,
            IShipmentFactory shipmentFactory,
            IMessageHelper messageHelper,
            Func<string, ITrackedDurationEvent> trackedDurationEventFactory,
            ISingleScanAutomationSettings singleScanAutomationSettings,
            ICarrierShipmentAdapterFactory shipmentAdapterFactory)
        {
            this.orderLoader = orderLoader;
            this.securityContext = securityContext;
            this.dlgFactory = dlgFactory;
            this.shipmentFactory = shipmentFactory;
            this.messageHelper = messageHelper;
            this.trackedDurationEventFactory = trackedDurationEventFactory;
            this.singleScanAutomationSettings = singleScanAutomationSettings;
            this.shipmentAdapterFactory = shipmentAdapterFactory;
        }

        /// <summary>
        /// Gets the Shipments that SingleScan should auto print/process
        /// </summary>
        public async Task<IEnumerable<ShipmentEntity>> GetShipments(long orderId, string scannedBarcode)
        {
            if (!securityContext().HasPermission(PermissionType.ShipmentsCreateEditProcess, orderId))
            {
                throw new ShippingException("Auto printing is not allowed for the scanned order.");
            }

            // Get all of the shipments for the order id that are not voided, this will add a new shipment if the order currently has no shipments
            ShipmentsLoadedEventArgs loadedOrders = await orderLoader.LoadAsync(new[] { orderId }, ProgressDisplayOptions.NeverShow, true, Timeout.Infinite);

            ShipmentEntity[] shipments = loadedOrders?.Shipments.Where(s => !s.Voided).ToArray();
            ShipmentEntity[] confirmedShipments = GetConfirmedShipments(orderId, scannedBarcode, shipments);

            if (HasDisqualifyingShipmentTypes(confirmedShipments))
            {
                messageHelper.ShowError(CannotProcessNoneMessage);
                confirmedShipments = new ShipmentEntity[0];
            }

            return confirmedShipments;
        }

        /// <summary>
        /// Gets Confirmed Shipments
        /// </summary>
        /// <remarks>
        /// If the order has no shipments we create and return a shipment
        /// If the order has a single unprocessed shipment, we return that shipment
        /// If the order already has a processed shipment or there are multiple unprocessed shipments we prompt
        /// the user to see if they want to proceed and only return shipments if they do.
        /// If AutoWeigh is on and the above rules result in multiple packages being
        /// processed, we prompt the user to see if they want to proceed and only return shipments if they do.
        /// After this method is called, we send the returned shipments to the AutoWeighService. The AutoWeighService
        /// sets the weigh of each shipment and each package to the weight of the scale if that setting is turned on.
        /// </remarks>
        private ShipmentEntity[] GetConfirmedShipments(long orderId, string scannedBarcode, ShipmentEntity[] shipments)
        {
            ShipmentEntity[] confirmedShipments = new ShipmentEntity[0];
            if (shipments != null)
            {
                if (shipments.IsCountEqualTo(1) && shipments.All(s => !s.Processed))
                {
                    confirmedShipments = shipments;
                }
                else if (shipments.None())
                {
                    // If the order has no non Voided Shipments add one and return it
                    confirmedShipments = new[] { shipmentFactory.Create(orderId) };
                }
                else if (ShouldPrintAndProcessShipments(shipments, scannedBarcode))
                {
                    // If all of the shipments are processed and the user confirms they want to process again add a shipment
                    if (shipments.All(s => s.Processed))
                    {
                        confirmedShipments = new[] { shipmentFactory.Create(orderId) };
                    }

                    // If some of the shipments are not process and the user confirms return only the unprocessed shipments
                    if (shipments.Any(s => !s.Processed))
                    {
                        confirmedShipments = shipments.Where(s => !s.Processed).ToArray();
                    }
                }

                if (singleScanAutomationSettings.IsAutoWeighEnabled && confirmedShipments.IsCountEqualTo(1))
                {
                    ShipmentEntity confirmedShipment = confirmedShipments.SingleOrDefault();
                    int packageCount = shipmentAdapterFactory.Get(confirmedShipment).GetPackageAdaptersAndEnsureShipmentIsLoaded().Count();

                    if (packageCount > 1 && !ShouldPrintAndProcessShipmentWithMultiplePackages(packageCount, scannedBarcode))
                    {
                        confirmedShipments = new ShipmentEntity[0];
                    }
                }
            }

            return confirmedShipments;
        }

        private bool ShouldPrintAndProcessShipmentWithMultiplePackages(int packageCount, string scannedBarcode)
        {
            string buttonText = $"Print {packageCount} Labels";

            MessagingText messaging = new MessagingText()
            {
                Title = "Multiple Packages",
                Body = string.Format(AutoWeighMessage, string.Format(MultiplePackageMessage, buttonText), "package"),
                Continue = buttonText
            };

            using (ITrackedDurationEvent telemetryEvent =
                trackedDurationEventFactory("SingleScan.AutoPrint.Confirmation.MultiplePackages"))
            {
                DialogResult result = messageHelper.ShowDialog(() => dlgFactory.Create(scannedBarcode, messaging));

                bool shouldPrint = result == DialogResult.OK;

                telemetryEvent.AddMetric("SingleScan.AutoPrint.Confirmation.MultiplePackages.Total", packageCount);
                telemetryEvent.AddProperty("SingleScan.AutoPrint.Confirmation.MultiplePackages.Action", shouldPrint ? "Continue" : "Cancel");

                return shouldPrint;
            }
        }

        /// <summary>
        /// Determines whether any shipment has a ShipmentTypeCode of "None".
        /// </summary>
        private static bool HasDisqualifyingShipmentTypes(IEnumerable<ShipmentEntity> shipments) =>
            shipments.Any(shipment => shipment.ShipmentTypeCode == ShipmentTypeCode.None);

        /// <summary>
        /// Check to see if we should print and process the given shipments
        /// </summary>
        /// <param name="shipments">the list of shipments</param>
        /// <param name="scannedBarcode">the barcode that will accept and dismiss the dialog</param>
        /// <returns></returns>
        private bool ShouldPrintAndProcessShipments(ShipmentEntity[] shipments, string scannedBarcode)
        {
            MessagingText messaging = GetMessaging(shipments);

            using (ITrackedDurationEvent telemetryEvent =
                trackedDurationEventFactory("SingleScan.AutoPrint.Confirmation.MultipleShipments"))
            {
                DialogResult result = messageHelper.ShowDialog(
                    () => dlgFactory.Create(scannedBarcode, messaging));

                bool shouldPrint = result == DialogResult.OK;

                telemetryEvent.AddMetric("SingleScan.AutoPrint.Confirmation.MultipleShipments.Total", shipments.Length);
                telemetryEvent.AddMetric("SingleScan.AutoPrint.Confirmation.MultipleShipments.Unprocessed", shipments.Count(s => !s.Processed));
                telemetryEvent.AddMetric("SingleScan.AutoPrint.Confirmation.MultipleShipments.Processed", shipments.Count(s => s.Processed));
                telemetryEvent.AddProperty("SingleScan.AutoPrint.Confirmation.MultipleShipments.Action", shouldPrint ? "Continue" : "Cancel");

                return shouldPrint;
            }
        }

        /// <summary>
        /// Get the Title/Message text to display to the user based on the Shipments
        /// </summary>
        private MessagingText GetMessaging(ShipmentEntity[] shipments)
        {
            if (shipments.All(s => s.Processed))
            {
                return new MessagingText
                {
                    Title = "Order Previously Processed",
                    Body = AlreadyProcessedMessage,
                    Continue = "Create New Label",
                    ContinueOptional = "Reprint Existing Label"
                };
            }

            string labels = shipments.Where(s => !s.Processed).IsCountGreaterThan(1) ? "Labels" : "Label";
            string buttonText = $"Create {shipments.Count(s => !s.Processed)} {labels}";

            string multipleShipmentsMessage = string.Format(MultipleShipmentsMessage, buttonText);
            if (singleScanAutomationSettings.IsAutoWeighEnabled)
            {
                multipleShipmentsMessage = string.Format(AutoWeighMessage, multipleShipmentsMessage, "shipment");
            }

            return new MessagingText
            {
                Title = "Multiple Shipments",
                Body = multipleShipmentsMessage,
                Continue = buttonText
            };
        }
    }
}