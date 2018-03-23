﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Utility;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Messaging.Messages.SingleScan;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Handles auto printing
    /// </summary>
    [NamedComponent(nameof(AutoPrintService), typeof(IAutoPrintService))]
    public class AutoPrintService : IAutoPrintService
    {
        private readonly Func<string, ITrackedDurationEvent> trackedDurationEventFactory;
        private readonly ISingleScanAutomationSettings singleScanAutomationSettings;
        private readonly ISingleScanConfirmationService confirmationService;
        private readonly IAutoWeighService autoWeighService;
        private readonly IMessenger messenger;

        /// <summary>
        /// Constructor
        /// </summary>
        public AutoPrintService(IMessenger messenger,
            ISingleScanAutomationSettings singleScanAutomationSettings,
            ISingleScanConfirmationService confirmationService,
            IAutoWeighService autoWeighService,
            Func<string, ITrackedDurationEvent> trackedDurationEventFactory)
        {
            this.messenger = messenger;
            this.singleScanAutomationSettings = singleScanAutomationSettings;
            this.confirmationService = confirmationService;
            this.autoWeighService = autoWeighService;
            this.trackedDurationEventFactory = trackedDurationEventFactory;
        }

        /// <summary>
        /// Determines if the auto print message should be sent
        /// </summary>
        public bool AllowAutoPrint(SingleScanMessage scanMessage)
        {
            // they scanned a barcode
            return !scanMessage.ScannedText.IsNullOrWhiteSpace() && singleScanAutomationSettings.IsAutoPrintEnabled();
        }

        /// <summary>
        /// Handles the request for auto printing an order.
        /// </summary>
        public async Task<GenericResult<AutoPrintResult>> Print(AutoPrintServiceDto autoPrintServiceDto)
        {
            string errorMessage = null;
            string scannedBarcode = autoPrintServiceDto.ScannedBarcode;
            long? orderID = autoPrintServiceDto.OrderID;
            bool processShipmentsMessageSent = false;

            // Only auto print if an order was found
            if (!orderID.HasValue)
            {
                errorMessage = "Order not found for scanned order.";
            }
            else
            {
                using (ITrackedDurationEvent autoPrintTrackedDurationEvent =
                    trackedDurationEventFactory("SingleScan.AutoPrint.ShipmentsProcessed"))
                {
                    int matchedOrderCount = autoPrintServiceDto.MatchedOrderCount;

                    // Confirm that the order found is the one the user wants to print
                    bool userCanceledPrint = !confirmationService.ConfirmOrder(orderID.Value, matchedOrderCount, scannedBarcode);

                    IEnumerable<ShipmentEntity> shipments = Enumerable.Empty<ShipmentEntity>();

                    if (userCanceledPrint)
                    {
                        errorMessage = "Multiple orders selected, user chose not to process";
                    }
                    else
                    {
                        // Get shipments to process (assumes GetShipments will not return voided shipments)
                        shipments = await confirmationService.GetShipments(orderID.Value, scannedBarcode);

                        userCanceledPrint = !shipments.Any();

                        if (!userCanceledPrint)
                        {
                            bool weighSuccessful = autoWeighService.ApplyWeight(shipments, autoPrintTrackedDurationEvent);
                            if (!weighSuccessful)
                            {
                                errorMessage = "Error reading scale";
                            }
                            else
                            {
                                // All good, process the shipment
                                messenger.Send(new ProcessShipmentsMessage(this, shipments, shipments, null));

                                processShipmentsMessageSent = true;
                            }
                        }
                        else
                        {
                            errorMessage = "No shipments processed";
                        }
                    }

                    CollectTelemetryData(autoPrintTrackedDurationEvent, shipments, matchedOrderCount, userCanceledPrint);
                }
            }

            AutoPrintResult autoPrintResult = new AutoPrintResult(scannedBarcode, orderID);

            return processShipmentsMessageSent ?
                GenericResult.FromSuccess(autoPrintResult) :
                GenericResult.FromError(errorMessage, autoPrintResult);
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
