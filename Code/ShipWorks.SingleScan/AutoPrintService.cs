using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Messaging.Messages.Filters;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Messaging.Messages.SingleScan;
using ShipWorks.Shipping.Services;
using ShipWorks.Stores.Content.Panels.Selectors;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Handles auto printing
    /// </summary>
    public class AutoPrintService
    {
        private readonly Func<string, ITrackedDurationEvent> trackedDurationEventFactory;
        protected readonly IMessenger Messenger;
        protected readonly ISchedulerProvider SchedulerProvider;
        private readonly IAutoPrintPermissions autoPrintPermissions;
        protected IDisposable FilterCompletedMessageSubscription;
        private readonly ISingleScanShipmentConfirmationService singleScanShipmentConfirmationService;
        private readonly ISingleScanOrderConfirmationService singleScanOrderConfirmationService;
        protected readonly IConnectableObservable<ScanMessage> ScanMessages;
        private IDisposable scanMessagesConnection;
        private const int ShipmentsProcessedMessageTimeoutInMinutes = 5;
        private const int FilterCountsUpdatedMessageTimeoutInSeconds = 25;

        /// <summary>
        /// Constructor
        /// </summary>
        public AutoPrintService(IMessenger messenger,
            ISchedulerProvider schedulerProvider,
            IAutoPrintPermissions autoPrintPermissions,
            ISingleScanShipmentConfirmationService singleScanShipmentConfirmationService,
            ISingleScanOrderConfirmationService singleScanOrderConfirmationService,
            Func<string, ITrackedDurationEvent> trackedDurationEventFactory)
        {
            this.Messenger = messenger;
            this.SchedulerProvider = schedulerProvider;
            this.autoPrintPermissions = autoPrintPermissions;
            this.trackedDurationEventFactory = trackedDurationEventFactory;
            this.singleScanShipmentConfirmationService = singleScanShipmentConfirmationService;
            this.singleScanOrderConfirmationService = singleScanOrderConfirmationService;

            ScanMessages = messenger.OfType<ScanMessage>().Publish();
            scanMessagesConnection = ScanMessages.Connect();
        }

        /// <summary>
        /// Initialize auto print for the current session
        /// </summary>
        public void InitializeForCurrentSession()
        {
            // Wire up observable for auto printing
            // note: One of the first things we do is dispose of scanMessagesConnection.
            // This turns off the pipeline to ensure that another order isn't
            // picked up before we are finished with possessing the current order.
            // All exit points of the pipeline need to call ReconnectPipeline()
            FilterCompletedMessageSubscription = ScanMessages
                .Where(AllowAutoPrint)
                .Do(x => EndScanMessagesObservation())
                .ContinueAfter(Messenger.OfType<SingleScanFilterUpdateCompleteMessage>(),
                    TimeSpan.FromSeconds(FilterCountsUpdatedMessageTimeoutInSeconds),
                    SchedulerProvider.Default,
                    (scanMsg, filterCountsUpdatedMessage) =>
                        new AutoPrintServiceDto(filterCountsUpdatedMessage, scanMsg))
                .ObserveOn(SchedulerProvider.WindowsFormsEventLoop)
                .SelectMany(m => HandleAutoPrintShipment(m).ToObservable())
                .SelectMany(WaitForShipmentsProcessedMessage)
                .CatchAndContinue((Exception ex) => HandleException(ex))
                .Subscribe(x => StartScanMessagesObservation());
        }

        /// <summary>
        /// Disconnect the scan messages observable
        /// </summary>
        protected virtual void EndScanMessagesObservation()
        {
            scanMessagesConnection?.Dispose();
            scanMessagesConnection = null;
        }

        /// <summary>
        /// Connect to the scan messages observable
        /// </summary>
        protected virtual void StartScanMessagesObservation()
        {
            if (scanMessagesConnection == null)
            {
                scanMessagesConnection = ScanMessages.Connect();
            }
        }

        /// <summary>
        /// Determines if the auto print message should be sent
        /// </summary>
        protected bool AllowAutoPrint(ScanMessage scanMessage)
        {
            // they scanned a barcode
            return !scanMessage.ScannedText.IsNullOrWhiteSpace() && autoPrintPermissions.AutoPrintPermitted();
        }

        /// <summary>
        /// Handles the request for auto printing an order.
        /// </summary>
        protected virtual async Task<GenericResult<string>> HandleAutoPrintShipment(AutoPrintServiceDto autoPrintServiceDto)
        {
            GenericResult<string> result;
            string scannedBarcode = autoPrintServiceDto.ScanMessage.ScannedText;
            long? orderID = GetOrderID(autoPrintServiceDto);

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
                    int matchedOrderCount = autoPrintServiceDto.SingleScanFilterUpdateCompleteMessage.FilterNodeContent.Count;

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
                            Messenger.Send(new ProcessShipmentsMessage(this, shipments, shipments, null));

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
        /// Get the order ID of the order we intend to process
        /// </summary>
        protected static long? GetOrderID(AutoPrintServiceDto autoPrintServiceDto)
        {
            bool orderNotFound = autoPrintServiceDto.SingleScanFilterUpdateCompleteMessage.FilterNodeContent == null ||
                   autoPrintServiceDto.SingleScanFilterUpdateCompleteMessage.FilterNodeContent.Count < 1 ||
                   autoPrintServiceDto.SingleScanFilterUpdateCompleteMessage.OrderId == null;

            if (orderNotFound)
            {
                return null;
            }

            return autoPrintServiceDto.SingleScanFilterUpdateCompleteMessage.OrderId.Value;
        }

        /// <summary>
        /// Collects telemetry data
        /// </summary>
        private void CollectTelemetryData(ITrackedDurationEvent autoPrintTrackedDurationEvent, IEnumerable<ShipmentEntity> shipments, int matchedOrderCount, bool printAborted)
        {
            List<string> carrierList = new List<string>();
            foreach (ShipmentEntity shipment in shipments)
            {
                carrierList.Add(EnumHelper.GetDescription(shipment.ShipmentTypeCode));
            }
            string carriers = string.Join(", ", carrierList.Distinct());

            autoPrintTrackedDurationEvent.AddProperty("SingleScan.AutoPrint.ShipmentsProcessed.ShippingProviders", string.IsNullOrWhiteSpace(carriers) ? "N/A" : carriers);
            autoPrintTrackedDurationEvent.AddProperty("SingleScan.AutoPrint.ShipmentsProcessed.RequiredConfirmation", shipments.Count() != 1 || matchedOrderCount != 1 ? "Yes" : "No");
            autoPrintTrackedDurationEvent.AddProperty("SingleScan.AutoPrint.ShipmentsProcessed.PrintAborted", printAborted ? "Yes" : "No");
        }

        /// <summary>
        /// Logs the exception and reconnect pipeline.
        /// </summary>
        protected virtual void HandleException(Exception ex)
        {
            StartScanMessagesObservation();
        }

        /// <summary>
        /// Waits for shipments processed message.
        /// </summary>
        protected virtual IObservable<GenericResult<string>> WaitForShipmentsProcessedMessage(GenericResult<string> genericResult)
        {
            IObservable<GenericResult<string>> returnResult;

            if (genericResult.Success)
            {
                // Listen for ShipmentsProcessedMessages, but timeout if processing takes
                // longer than ShipmentsProcessedMessageTimeoutInMinutes.
                // We don't get an observable to start from, but we need one to use ContinueAfter, so using
                // Observable.Return to get an observable to work with.
                returnResult = Observable.Return(0)
                    .ContinueAfter(Messenger.OfType<ShipmentsProcessedMessage>(),
                        TimeSpan.FromMinutes(ShipmentsProcessedMessageTimeoutInMinutes),
                        SchedulerProvider.Default,
                        ((i, message) => message))
                    .Do(message => Messenger.Send(
                        new OrderSelectionChangingMessage(this,
                        message.Shipments.Select(s => s.Shipment.OrderID).Distinct(),
                        EntityGridRowSelector.SpecificEntities(message.Shipments.Select(s => s.Shipment.ShipmentID).Distinct()))))
                    .Select(f => genericResult);

            }
            else
            {
                returnResult = Observable.Return(genericResult);
            }

            return returnResult;
        }

        /// <summary>
        /// End the current session
        /// </summary>
        public void EndSession()
        {
            FilterCompletedMessageSubscription?.Dispose();
        }

        /// <summary>
        /// Dispose the container
        /// </summary>
        public void Dispose()
        {
            EndSession();
        }
    }
}
