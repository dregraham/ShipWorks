using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Messaging.Messages.SingleScan;
using ShipWorks.Shipping.Services;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Handles auto printing
    /// </summary>
    [NamedComponent(nameof(AutoPrintService), typeof(IAutoPrintService))]
    public class AutoPrintService : IAutoPrintService
    {
        private readonly Func<string, ITrackedDurationEvent> trackedDurationEventFactory;
        private readonly IAutoPrintPermissions autoPrintPermissions;
        private readonly ISingleScanShipmentConfirmationService singleScanShipmentConfirmationService;
        private readonly ISingleScanOrderConfirmationService singleScanOrderConfirmationService;
        private readonly IMessenger messenger;

        /// <summary>
        /// Constructor
        /// </summary>
        public AutoPrintService(IMessenger messenger,
            IAutoPrintPermissions autoPrintPermissions,
            ISingleScanShipmentConfirmationService singleScanShipmentConfirmationService,
            ISingleScanOrderConfirmationService singleScanOrderConfirmationService,
            Func<string, ITrackedDurationEvent> trackedDurationEventFactory)
        {
            this.messenger = messenger;
            this.autoPrintPermissions = autoPrintPermissions;
            this.trackedDurationEventFactory = trackedDurationEventFactory;
            this.singleScanShipmentConfirmationService = singleScanShipmentConfirmationService;
            this.singleScanOrderConfirmationService = singleScanOrderConfirmationService;
        }

        /// <summary>
        /// Determines if the auto print message should be sent
        /// </summary>
        public bool AllowAutoPrint(ScanMessage scanMessage)
        {
            // they scanned a barcode
            return !scanMessage.ScannedText.IsNullOrWhiteSpace() && autoPrintPermissions.AutoPrintPermitted();
        }

        /// <summary>
        /// Handles the request for auto printing an order.
        /// </summary>
        public async Task<GenericResult<string>> Print(AutoPrintServiceDto autoPrintServiceDto)
        {
            GenericResult<string> result;
            string scannedBarcode = autoPrintServiceDto.ScannedBarcode;
            long? orderID = autoPrintServiceDto.OrderID;

            // Only auto print if an order was found
            if (!orderID.HasValue)
            {
                result = GenericResult.FromError("Order not found for scanned order.", scannedBarcode);
            }
            else
            {
                using (ITrackedDurationEvent autoPrintTrackedDurationEvent =
                    trackedDurationEventFactory("SingleScan.AutoPrint.ShipmentsProcessed"))
                {
                    int matchedOrderCount = autoPrintServiceDto.MatchedOrderCount;

                    // Confirm that the order found is the one the user wants to print
                    bool userCanceledPrint = !singleScanOrderConfirmationService.Confirm(orderID.Value, matchedOrderCount, scannedBarcode);

                    IEnumerable<ShipmentEntity> shipments = Enumerable.Empty<ShipmentEntity>();

                    if (userCanceledPrint)
                    {
                        result = GenericResult.FromError("Multiple orders selected, user chose not to process",
                            scannedBarcode);
                    }
                    else
                    {
                        // Get shipments to process (assumes GetShipments will not return voided shipments)
                        shipments = await singleScanShipmentConfirmationService.GetShipments(orderID.Value, scannedBarcode);

                        userCanceledPrint = !shipments.Any();

                        if (!userCanceledPrint)
                        {
                            // All good, process the shipment
                            messenger.Send(new ProcessShipmentsMessage(this, shipments, shipments, null));

                            result = GenericResult.FromSuccess(scannedBarcode);
                        }
                        else
                        {
                            result = GenericResult.FromError("No shipments processed", scannedBarcode);
                        }
                    }

                    CollectTelemetryData(autoPrintTrackedDurationEvent, shipments, matchedOrderCount, userCanceledPrint);
                }
            }

            return result;
        }

        /// <summary>
        /// Collects telemetry data
        /// </summary>
        private void CollectTelemetryData(ITrackedDurationEvent autoPrintTrackedDurationEvent,
            IEnumerable<ShipmentEntity> shipments, int matchedOrderCount, bool printAborted)
        {
            List<string> carrierList = new List<string>();
            foreach (ShipmentEntity shipment in shipments)
            {
                carrierList.Add(EnumHelper.GetDescription(shipment.ShipmentTypeCode));
            }
            string carriers = string.Join(", ", carrierList.Distinct());

            autoPrintTrackedDurationEvent.AddProperty("SingleScan.AutoPrint.ShipmentsProcessed.ShippingProviders",
                string.IsNullOrWhiteSpace(carriers) ? "N/A" : carriers);
            autoPrintTrackedDurationEvent.AddProperty("SingleScan.AutoPrint.ShipmentsProcessed.RequiredConfirmation",
                shipments.Count() != 1 || matchedOrderCount != 1 ? "Yes" : "No");
            autoPrintTrackedDurationEvent.AddProperty("SingleScan.AutoPrint.ShipmentsProcessed.PrintAborted",
                printAborted ? "Yes" : "No");
        }
    }
}
