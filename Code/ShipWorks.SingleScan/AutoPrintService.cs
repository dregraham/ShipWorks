﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Options;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Filters;
using ShipWorks.Messaging.Messages.Filters;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Messaging.Messages.SingleScan;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Services;
using ShipWorks.Users;
using ShipWorks.Users.Security;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Handles auto printing
    /// </summary>
    public class AutoPrintService : IInitializeForCurrentUISession
    {
        private readonly ILog log;
        private readonly IMainForm mainForm;
        private readonly Func<string, ITrackedDurationEvent> trackedDurationEventFactory;
        private readonly IMessenger messenger;
        private readonly ISchedulerProvider schedulerProvider;
        private IDisposable filterCompletedMessageSubscription;
        private readonly IUserSession userSession;
        private readonly ISingleScanShipmentConfirmationService singleScanShipmentConfirmationService;
        private readonly ISingleScanOrderConfirmationService singleScanOrderConfirmationService;
        private readonly IConnectableObservable<ScanMessage> scanMessages;
        private IDisposable scanMessagesConnection;
        private const int FilterCountsUpdatedMessageTimeoutInSeconds = 25;
        private const int ShipmentsProcessedMessageTimeoutInMinutes = 5;

        /// <summary>
        /// Constructor
        /// </summary>
        public AutoPrintService(IMessenger messenger,
            ISchedulerProvider schedulerProvider,
            IUserSession userSession,
            ISingleScanShipmentConfirmationService singleScanShipmentConfirmationService,
            ISingleScanOrderConfirmationService singleScanOrderConfirmationService,
            Func<Type, ILog> logFactory,
            IMainForm mainForm,
            Func<string, ITrackedDurationEvent> trackedDurationEventFactory)
        {
            this.messenger = messenger;
            this.schedulerProvider = schedulerProvider;
            this.userSession = userSession;
            this.mainForm = mainForm;
            this.trackedDurationEventFactory = trackedDurationEventFactory;
            this.singleScanShipmentConfirmationService = singleScanShipmentConfirmationService;
            this.singleScanOrderConfirmationService = singleScanOrderConfirmationService;

            scanMessages = messenger.OfType<ScanMessage>().Publish();
            scanMessagesConnection = scanMessages.Connect();

            log = logFactory(typeof(AutoPrintService));
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
            filterCompletedMessageSubscription = scanMessages
                .Where(AllowAutoPrint)
                .Do(x => EndScanMessagesObservation())
                .ContinueAfter(messenger.OfType<SingleScanFilterUpdateCompleteMessage>(),
                               TimeSpan.FromSeconds(FilterCountsUpdatedMessageTimeoutInSeconds),
                               schedulerProvider.Default,
                               (scanMsg, filterCountsUpdatedMessage) => new AutoPrintServiceDto(filterCountsUpdatedMessage, scanMsg))
                .ObserveOn(schedulerProvider.WindowsFormsEventLoop)
                .SelectMany(m => HandleAutoPrintShipment(m).ToObservable())
                .SelectMany(WaitForShipmentsProcessedMessage)
                .CatchAndContinue((Exception ex) => HandleException(ex))
                .Subscribe(x => StartScanMessagesObservation());
        }

        /// <summary>
        /// Disconnect the scan messages observable
        /// </summary>
        private void EndScanMessagesObservation()
        {
            log.Info("Ending scan message observation.");

            scanMessagesConnection?.Dispose();
            scanMessagesConnection = null;
        }

        /// <summary>
        /// Connect to the scan messages observable
        /// </summary>
        private void StartScanMessagesObservation()
        {
            log.Info("Starting scan message observation.");

            if (scanMessagesConnection == null)
            {
                scanMessagesConnection = scanMessages.Connect();
            }
        }

        /// <summary>
        /// Determines if the auto print message should be sent
        /// </summary>
        private bool AllowAutoPrint(ScanMessage scanMessage)
        {
            // they scanned a barcode
            return !scanMessage.ScannedText.IsNullOrWhiteSpace() &&
                userSession.Settings?.SingleScanSettings == (int) SingleScanSettings.AutoPrint &&
                           !mainForm.AdditionalFormsOpen();
        }

        /// <summary>
        /// Handles the request for auto printing an order.
        /// </summary>
        private async Task<GenericResult<string>> HandleAutoPrintShipment(AutoPrintServiceDto autoPrintServiceDto)
        {
            GenericResult<string> result;
            string scannedBarcode = autoPrintServiceDto.ScanMessage.ScannedText;
            long? orderID = GetOrderID(autoPrintServiceDto);

            // Only auto print if an order was found
            if (!orderID.HasValue)
            {
                log.Error("Order not found for scanned order.");
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
        /// Get the order ID of the order we intend to process
        /// </summary>
        private static long? GetOrderID(AutoPrintServiceDto autoPrintServiceDto)
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
        private void HandleException(Exception ex)
        {
            log.Error("Error occurred while attempting to auto print.", ex);
            StartScanMessagesObservation();
        }

        /// <summary>
        /// Waits for shipments processed message.
        /// </summary>
        private IObservable<GenericResult<string>> WaitForShipmentsProcessedMessage(GenericResult<string> genericResult)
        {
            IObservable<GenericResult<string>> returnResult;

            if (genericResult.Success)
            {
                log.Info($"Waiting for ShipmentsProcessedMessage from scan  {genericResult.Value}");

                // Listen for ShipmentsProcessedMessages, but timeout if processing takes
                // longer than ShipmentsProcessedMessageTimeoutInMinutes.
                // We don't get an observable to start from, but we need one to use ContinueAfter, so using
                // Observable.Range to get an observable to work with.
                returnResult = Observable.Range(0, 1)
                                         .ContinueAfter(messenger.OfType<ShipmentsProcessedMessage>(),
                                                        TimeSpan.FromMinutes(ShipmentsProcessedMessageTimeoutInMinutes),
                                                        schedulerProvider.Default)
                                         .Select(f => genericResult);

                log.Info($"ShipmentsProcessedMessage received from scan {genericResult.Value}");
            }
            else
            {
                log.Info("No Shipments, not waiting for ShipmentsProcessMessageScan");
                returnResult = Observable.Return(genericResult);
            }

            return returnResult;
        }

        /// <summary>
        /// End the current session
        /// </summary>
        public void EndSession()
        {
            filterCompletedMessageSubscription?.Dispose();
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
