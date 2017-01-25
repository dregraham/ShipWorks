using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Threading;
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
using ShipWorks.Users;

namespace ShipWorks.Shipping.Services
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

        /// <summary>
        /// Constructor
        /// </summary>
        public AutoPrintService(IMessenger messenger,
            ISchedulerProvider schedulerProvider,
            IUserSession userSession,
            ISingleScanShipmentConfirmationService singleScanShipmentConfirmationService)
        {
            this.messenger = messenger;
            this.schedulerProvider = schedulerProvider;
            this.userSession = userSession;
            this.singleScanShipmentConfirmationService = singleScanShipmentConfirmationService;

            log = LogManager.GetLogger(typeof(AutoPrintService));
        }

        /// <summary>
        /// Initialize auto print for the current session
        /// </summary>
        public void InitializeForCurrentSession()
        {
            // Wire up observable for auto printing
            filterCompletedMessageSubscription = messenger.OfType<ScanMessage>()
                .Where(x => AllowAutoPrint())
                .SelectMany(m => messenger.OfType<FilterCountsUpdatedMessage>().Take(1).Select(f => new {FilterCountsUpdateMessage = f, ScanMessage = m}))
                .ObserveOn(schedulerProvider.WindowsFormsEventLoop)
                .Do(m => HandleAutoPrintShipment(m.FilterCountsUpdateMessage.FilterNodeContent, m.ScanMessage.ScannedText).RunSynchronously())
                .CatchAndContinue((Exception ex) => log.Error("Error occurred while attempting to auto print.", ex))
                .Gate(messenger.OfType<ShipmentsProcessedMessage>())
                .Subscribe(x => log.Debug("ShipmentsProcessedMessage received."));
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
        public async Task HandleAutoPrintShipment(IFilterNodeContentEntity filterNodeContent, string scannedBarcode)
        {
            // Only auto print if 1 order was found
            if (filterNodeContent?.Count != 1)
            {
                throw new ShippingException("Auto printing is not allowed for the scanned order.");
            }

            // Get the order associated with the barcode search
            long autoPrintOrderId = FilterContentManager.FetchFirstOrderIdForFilterNodeContent(filterNodeContent.FilterNodeContentID);

            // Get shipments to process (assumes GetShipments will not return voided shipments)
            List<ShipmentEntity> shipments = (await singleScanShipmentConfirmationService.GetShipments(autoPrintOrderId, scannedBarcode)).ToList();

            Debug.Assert(shipments != null);

            ProcessUnprocessedShipments(shipments);
        }

        /// <summary>
        /// Given a collection of shipments, process the unprocessed shipments
        /// </summary>
        private void ProcessUnprocessedShipments(List<ShipmentEntity> shipments)
        {
            Debug.Assert(shipments.TrueForAll(s => s.Processed == false));

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
