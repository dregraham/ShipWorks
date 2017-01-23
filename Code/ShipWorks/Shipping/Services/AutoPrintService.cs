using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.Threading;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Options;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Filters;
using ShipWorks.Messaging.Messages;
using ShipWorks.Messaging.Messages.Filters;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Users;
using ShipWorks.Users.Security;

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
        private readonly Func<ISecurityContext> securityContextRetriever;
        private readonly IOrderLoader orderLoader;
        private IDisposable filterCompletedMessageSubscription;
        private readonly IUserSession userSession;

        /// <summary>
        /// Constructor
        /// </summary>
        public AutoPrintService(IMessenger messenger,
            ISchedulerProvider schedulerProvider,
            Func<ISecurityContext> securityContextRetriever,
            IOrderLoader orderLoader,
            IUserSession userSession)
        {
            this.messenger = messenger;
            this.schedulerProvider = schedulerProvider;
            this.securityContextRetriever = securityContextRetriever;
            this.orderLoader = orderLoader;
            this.userSession = userSession;

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
                .SelectMany(messenger.OfType<FilterSearchCompletedMessage>().Take(1))
                .ObserveOn(schedulerProvider.WindowsFormsEventLoop)
                .Do(m => HandleAutoPrintShipment(m.FilterNodeContent).RunSynchronously())
                .CatchAndContinue((Exception ex) => log.Error("Error occurred while attempting to auto print.", ex))
                .Gate(messenger.OfType<ShipmentsProcessedMessage>())
                .Subscribe(x => log.Fatal($"ShipmentsProcessedMessage received."));
        }

        /// <summary>
        /// Determines if the auto print message should be sent
        /// </summary>
        private bool AllowAutoPrint()
        {
            bool allowed = userSession.Settings?.SingleScanSettings == (int) SingleScanSettings.AutoPrint;

            return allowed;
        }

        /// <summary>
        /// Gets whether a shipment should be auto printed for an order
        /// </summary>
        public bool ShouldAutoPrintShipment(IEnumerable<ShipmentEntity> shipments, long orderId)
        {
            return shipments.IsCountEqualTo(1) && 
                   shipments.All(s => !s.Processed) &&
                   securityContextRetriever().HasPermission(PermissionType.ShipmentsCreateEditProcess, orderId);
        }

        /// <summary>
        /// Handles the request for auto printing an order.
        /// </summary>
        public async Task HandleAutoPrintShipment(IFilterNodeContentEntity filterNodeContent)
        {
            // Only auto print if 1 order was found
            if (filterNodeContent?.Count != 1)
            {
                throw new ShippingException("Auto printing is not allowed for the scanned order.");
            }

            // Get the order associated with the barcode search
            long autoPrintOrderId = FilterContentManager.FetchFirstOrderIdForFilterNodeContent(filterNodeContent.FilterNodeContentID);

            // Load the order, creating shipments if necessary.
            ShipmentsLoadedEventArgs shipmentArgs = await orderLoader.LoadAsync(new[] { autoPrintOrderId }, ProgressDisplayOptions.NeverShow, true, Timeout.Infinite);
            IEnumerable<ShipmentEntity> shipments = shipmentArgs.Shipments;

            // See if we should allow auto print for this order
            if (!ShouldAutoPrintShipment(shipments, autoPrintOrderId))
            {
                throw new ShippingException("Auto printing is not allowed for the scanned order.");
            }

            // All good, process the shipment
            messenger.Send(new ProcessShipmentsMessage(this, shipments, shipments, null));
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
