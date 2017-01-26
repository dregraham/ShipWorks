using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Threading;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Options;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages.Filters;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Messaging.Messages.SingleScan;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Services;
using ShipWorks.Users;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Handles auto printing
    /// </summary>
    public class AutoPrintService : IInitializeForCurrentUISession
    {
        private readonly ILog log;
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
            Func<Type, ILog> logFactory)
        {
            this.messenger = messenger;
            this.schedulerProvider = schedulerProvider;
            this.userSession = userSession;
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
            filterCompletedMessageSubscription = messenger.OfType<ScanMessage>()
                .Where(x => AllowAutoPrint())
                .Do(x => scanMessagesConnection.Dispose())
                .SelectMany(m => messenger.OfType<FilterCountsUpdatedMessage>().Take(1).Select(f => new {FilterCountsUpdateMessage = f, ScanMessage = m}))
                .ObserveOn(schedulerProvider.WindowsFormsEventLoop)
                .SelectMany(m => Observable.FromAsync(() => HandleAutoPrintShipment(m.FilterCountsUpdateMessage, m.ScanMessage.ScannedText)))
                 .CatchAndContinue((Exception ex) =>
                 {
                     scanMessagesConnection = scanMessages.Connect();
                     log.Error("Error occurred while attempting to auto print.", ex);
                 })
                .Gate(messenger.OfType<ShipmentsProcessedMessage>())
                .Do(x =>
                {
                    scanMessagesConnection = scanMessages.Connect();
                    log.Debug("ShipmentsProcessedMessage received.");
                })
                .Subscribe();
        }

        /// <summary>
        /// Determines if the auto print message should be sent
        /// </summary>
        private bool AllowAutoPrint()
        {
            return userSession.Settings?.SingleScanSettings == (int) SingleScanSettings.AutoPrint;
        }

        /// <summary>
        /// Handles the request for auto printing an order.
        /// </summary>
        private async Task HandleAutoPrintShipment(FilterCountsUpdatedMessage filterCountsUpdateMessage, string scannedBarcode)
        {
            // Only auto print if 1 order was found
            if (filterCountsUpdateMessage.FilterNodeContent?.Count != 1)
            {
                throw new ShippingException("Auto printing is not allowed for the scanned order.");
            }

            long orderId = filterCountsUpdateMessage.OrderIds.First();

            // Get shipments to process (assumes GetShipments will not return voided shipments)
            List<ShipmentEntity> shipments = (await singleScanShipmentConfirmationService.GetShipments(orderId, scannedBarcode)).ToList();

            Debug.Assert(shipments != null);

            ProcessUnprocessedShipments(shipments);
        }

        /// <summary>
        /// Given a collection of shipments, process the unprocessed shipments
        /// </summary>
        private void ProcessUnprocessedShipments(List<ShipmentEntity> shipments)
        {
            Debug.Assert(shipments.TrueForAll(s => !s.Processed));

            if (shipments.Any())
            {
                // All good, process the shipment
                messenger.Send(new ProcessShipmentsMessage(this, shipments, shipments, null));
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
