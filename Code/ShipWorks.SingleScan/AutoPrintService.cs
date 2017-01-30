﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
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
        private readonly IMessenger messenger;
        private readonly ISchedulerProvider schedulerProvider;
        private IDisposable filterCompletedMessageSubscription;
        private readonly IUserSession userSession;
        private readonly ISingleScanShipmentConfirmationService singleScanShipmentConfirmationService;
        private readonly IConnectableObservable<ScanMessage> scanMessages;
        private IDisposable scanMessagesConnection;

        /// <summary>
        /// Constructor
        /// </summary>
        public AutoPrintService(IMessenger messenger,
            ISchedulerProvider schedulerProvider,
            IUserSession userSession,
            ISingleScanShipmentConfirmationService singleScanShipmentConfirmationService,
            Func<Type, ILog> logFactory,
            IMainForm mainForm)
        {
            this.messenger = messenger;
            this.schedulerProvider = schedulerProvider;
            this.userSession = userSession;
            this.mainForm = mainForm;
            this.singleScanShipmentConfirmationService = singleScanShipmentConfirmationService;

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
                .Do(x => scanMessagesConnection.Dispose())
                .SelectMany(WaitForFilterCountsUpdatedMessage)
                .ObserveOn(schedulerProvider.WindowsFormsEventLoop)
                .SelectMany(m => HandleAutoPrintShipment(m).ToObservable())
                .SelectMany(WaitForShipmentsProcessedMessage)
                .CatchAndContinue((Exception ex) => HandleException(ex))
                .Subscribe(x => ReconnectPipeline());
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
        /// Waits for filter counts updated message.
        /// </summary>
        private IObservable<FilterCountsUpdatedAndScanMessages> WaitForFilterCountsUpdatedMessage(ScanMessage scanMessage)
        {
            return messenger.OfType<FilterCountsUpdatedMessage>().Take(1)
                    .Select(filterCountsUpdatedMessage => 
                        new FilterCountsUpdatedAndScanMessages(filterCountsUpdatedMessage, scanMessage));
        }


        /// <summary>
        /// Handles the request for auto printing an order.
        /// </summary>
        private async Task<GenericResult<string>> HandleAutoPrintShipment(FilterCountsUpdatedAndScanMessages messages)
        {
            // Only auto print if 1 order was found
            if (messages.FilterCountsUpdatedMessage.FilterNodeContent?.Count != 1)
            {
                throw new ShippingException("Auto printing is not allowed for the scanned order.");
            }

            if (messages.FilterCountsUpdatedMessage.OrderId == null)
            {
                throw new ShippingException("Order not found for scanned order.");
            }

            string scannedBarcode = messages.ScanMessage.ScannedText;
            long orderId = messages.FilterCountsUpdatedMessage.OrderId.Value;

            // Get shipments to process (assumes GetShipments will not return voided shipments)
            IEnumerable<ShipmentEntity> shipments = await singleScanShipmentConfirmationService.GetShipments(orderId, scannedBarcode);

            if (shipments.Any())
            {
                // All good, process the shipment
                messenger.Send(new ProcessShipmentsMessage(this, shipments, shipments, null));

                return GenericResult.FromSuccess(scannedBarcode);
            }

            return GenericResult.FromError("No shipments processed", scannedBarcode);
        }

        /// <summary>
        /// Logs the exception and reconnect pipeline.
        /// </summary>
        private void HandleException(Exception ex)
        {
            log.Error("Error occurred while attempting to auto print.", ex);
            ReconnectPipeline();
        }

        /// <summary>
        /// Waits for shipments processed message.
        /// </summary>
        private IObservable<GenericResult<string>> WaitForShipmentsProcessedMessage(GenericResult<string> genericResult)
        {
            IObservable<GenericResult<string>> returnResult;

            if (genericResult.Success)
            {
                log.Info("Waiting for ShipmentsProcessedMessage from scan  {genericResult.Value}");
                returnResult = messenger.OfType<ShipmentsProcessedMessage>().Take(1).Select(f => genericResult);
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
        /// Reconnects the pipeline.
        /// </summary>
        private void ReconnectPipeline()
        {
            if (scanMessagesConnection != null)
            {
                scanMessagesConnection = scanMessages.Connect();
            }
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
