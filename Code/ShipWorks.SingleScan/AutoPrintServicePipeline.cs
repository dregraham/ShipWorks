﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Messaging.Messages.Filters;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Messaging.Messages.SingleScan;
using ShipWorks.Settings;
using ShipWorks.Stores.Content.Panels.Selectors;
using ShipWorks.Users;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Subscription for AutoPrint
    /// </summary>
    /// <seealso cref="ShipWorks.ApplicationCore.IInitializeForCurrentUISession" />
    public class AutoPrintServicePipeline : IInitializeForCurrentUISession
    {
        private const int FilterCountsUpdatedMessageTimeoutInSeconds = 25;
        private const int ShipmentsProcessedMessageTimeoutInMinutes = 5;

        private readonly ILog log;
        private readonly IConnectableObservable<SingleScanMessage> scanMessages;
        private readonly IMessenger messenger;
        private readonly ISchedulerProvider schedulerProvider;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly IMainForm mainForm;
        private readonly ICurrentUserSettings currentUserSettings;
        private readonly IAutoPrintService autoPrintService;

        private IDisposable scanMessagesConnection;
        private IDisposable filterCompletedMessageSubscription;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoPrintServicePipeline"/> class.
        /// </summary>
        public AutoPrintServicePipeline(
            IAutoPrintService autoPrintService,
            IMessenger messenger,
            ISchedulerProvider schedulerProvider,
            Func<Type, ILog> logFactory,
            ISqlAdapterFactory sqlAdapterFactory,
            IMainForm mainForm,
            ICurrentUserSettings currentUserSettings)
        {
            this.messenger = messenger;
            this.schedulerProvider = schedulerProvider;
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.mainForm = mainForm;
            this.currentUserSettings = currentUserSettings;
            scanMessages = messenger.OfType<SingleScanMessage>().Publish();
            scanMessagesConnection = scanMessages.Connect();
            this.autoPrintService = autoPrintService;
            log = logFactory(typeof(AutoPrintServicePipeline));
        }

        /// <summary>
        /// Gets a value indicating whether this instance is listening for scans.
        /// </summary>
        public bool IsListeningForScans => scanMessagesConnection != null;

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
                .Where(_ => currentUserSettings.GetUIMode() == UIMode.Batch)
                .Where(x => autoPrintService.AllowAutoPrint(x.ScannedText))
                .Do(x => EndScanMessagesObservation())
                .ContinueAfter(messenger.OfType<SingleScanFilterUpdateCompleteMessage>(),
                    TimeSpan.FromSeconds(FilterCountsUpdatedMessageTimeoutInSeconds),
                    schedulerProvider.Default,
                    (scanMsg, filterCountsUpdatedMessage) =>
                        new AutoPrintServiceDto(filterCountsUpdatedMessage, scanMsg))
                .Where(x => x.OrderID != null)
                .ObserveOn(schedulerProvider.WindowsFormsEventLoop)
                .SelectMany(m => autoPrintService.Print(m).ToObservable())
                .SelectMany(WaitForShipmentsProcessedMessage)
                .Do(SaveUnprocessedShipments)
                // Currently, MapPanel (and possibly other consumers of this message) assumes OrderSelectionChangingMessage to
                // be sent on the UI thread. If the message isn't sent on the UI, the map panel throws a Cross Threaded Exception
                // as it doesn't attempt to Invoke. This should be addressed in the future, but putting the message on the
                // WindowsFormsEventLoop is a safe workaround.
                .ObserveOn(schedulerProvider.WindowsFormsEventLoop)
                .Do(SendOrderSelectionChangingMessage)
                .Do(x => StartScanMessagesObservation())
                .CatchAndContinue((Exception ex) => HandleException(ex))
                .Subscribe();
        }

        /// <summary>
        /// Sends the order selection changing message for processed orders
        /// </summary>
        private void SendOrderSelectionChangingMessage(AutoPrintCompletionResult shipmentsProcessedResult)
        {
            if (shipmentsProcessedResult.OrderID.HasValue)
            {
                IEntityGridRowSelector shipmentRowSelector = null;

                if (shipmentsProcessedResult.ProcessShipmentResults.None())
                {
                    shipmentRowSelector = EntityGridRowSelector
                        .SpecificEntities(shipmentsProcessedResult.ProcessShipmentResults.Select(shipment => shipment.Shipment.ShipmentID).Distinct());
                }

                messenger.Send(new OrderSelectionChangingMessage(this, new[] { shipmentsProcessedResult.OrderID.Value }, shipmentRowSelector));
            }
            else
            {
                log.Info("No shipments found in SendOrderSelectionChangingMessage.");
            }
        }

        /// <summary>
        /// Saves any changed weights for shipments that failed to process
        /// </summary>
        private void SaveUnprocessedShipments(AutoPrintCompletionResult shipmentsProcessedResult)
        {
            if (shipmentsProcessedResult.ProcessShipmentResults.Any())
            {
                List<ShipmentEntity> unprocessedShipments =
                    shipmentsProcessedResult.ProcessShipmentResults.Where(s => !s.IsSuccessful)
                        .Select(s => s.Shipment)
                        .ToList();

                if (unprocessedShipments.Any())
                {
                    log.Info($"Error processing {unprocessedShipments.Count}. Saving unprocessed shipments.");
                    using (ISqlAdapter sqlAdapter = sqlAdapterFactory.CreateTransacted())
                    {
                        foreach (ShipmentEntity unprocessedShipment in unprocessedShipments)
                        {
                            sqlAdapter.SaveAndRefetch(unprocessedShipment);
                        }
                        sqlAdapter.Commit();
                    }
                }
                else
                {
                    log.Info("All shipments successfully processed.");
                }
            }
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
        private IObservable<AutoPrintCompletionResult> WaitForShipmentsProcessedMessage(GenericResult<AutoPrintResult> autoPrintResult)
        {
            IObservable<AutoPrintCompletionResult> returnResult;
            long? orderId = autoPrintResult.Value.OrderId;

            if (autoPrintResult.Success)
            {
                log.Info("Waiting for ShipmentsProcessedMessage");
                // Listen for ShipmentsProcessedMessages, but timeout if processing takes
                // longer than ShipmentsProcessedMessageTimeoutInMinutes.
                // We don't get an observable to start from, but we need one to use ContinueAfter, so using
                // Observable.Return to get an observable to work with.
                returnResult = Observable.Return(0)
                    .ContinueAfter(messenger.OfType<ShipmentsProcessedMessage>(),
                        TimeSpan.FromMinutes(ShipmentsProcessedMessageTimeoutInMinutes),
                        schedulerProvider.Default,
                        (i, shipmentsProcessedMessage) => shipmentsProcessedMessage)
                    .Select(shipmentsProcessedMessage =>
                    {
                        if (EqualityComparer<ShipmentsProcessedMessage>.Default.Equals(shipmentsProcessedMessage, default(ShipmentsProcessedMessage)))
                        {
                            log.Info("Timeout waiting for ShipmentsProcessedMessage");
                            return new AutoPrintCompletionResult(orderId);
                        }

                        log.Info($"ShipmentsProcessedMessage received from scan {autoPrintResult.Value.ScannedBarcode}");
                        return new AutoPrintCompletionResult(orderId, shipmentsProcessedMessage.Shipments);
                    });
            }
            else
            {
                log.Info("No Shipments, not waiting for ShipmentsProcessMessageScan");
                returnResult = Observable.Return(new AutoPrintCompletionResult(orderId));
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