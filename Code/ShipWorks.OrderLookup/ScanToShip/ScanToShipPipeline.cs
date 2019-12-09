using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Interapptive.Shared.Collections;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages.Orders;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Settings;

namespace ShipWorks.OrderLookup.ScanToShip
{
    /// <summary>
    /// Pipeline that listens to messages related to Scan To Ship
    /// </summary>
    public class ScanToShipPipeline : IOrderLookupPipeline
    {
        private IDisposable subscriptions;
        private readonly IMessenger messenger;
        private readonly ILog log;
        private readonly IScanToShipViewModel scanToShipViewModel;
        private readonly IMainForm mainForm;

        /// <summary>
        /// Constructor
        /// </summary>
        public ScanToShipPipeline(IMessenger messenger, IScanToShipViewModel scanToShipViewModel, IMainForm mainForm, Func<Type, ILog> createLogger)
        {
            this.messenger = messenger;
            this.scanToShipViewModel = scanToShipViewModel;
            this.mainForm = mainForm;
            log = createLogger(GetType());
        }

        /// <summary>
        /// Interface for initializing order lookup pipelines under a top level lifetime scope
        /// </summary>
        public void InitializeForCurrentScope()
        {
            Dispose();

            subscriptions = new CompositeDisposable(
                messenger.OfType<ShipmentsProcessedMessage>()
                    .Where(_ => mainForm.UIMode == UIMode.OrderLookup)
                    .Where(x => x.Shipments.All(s => s.Shipment.Processed))
                    .Do(HandleShipmentProcessed)
                    .CatchAndContinue((Exception ex) => HandleException(ex))
                    .Subscribe(),

                messenger.OfType<OrderVerifiedMessage>()
                    .Where(_ => mainForm.UIMode == UIMode.OrderLookup)
                    .Where(x => x.Order.Verified)
                    .Do(HandleOrderVerified)
                    .CatchAndContinue((Exception ex) => HandleException(ex))
                    .Subscribe()
            );
        }

        /// <summary>
        /// Handle the shipment processed message
        /// </summary>
        private void HandleShipmentProcessed(ShipmentsProcessedMessage shipmentsProcessedMessage)
        {
            // maybe check if same order
            scanToShipViewModel.IsOrderProcessed = true;
        }

        /// <summary>
        /// Handle the shipment processed message
        /// </summary>
        private void HandleOrderVerified(OrderVerifiedMessage orderVerifiedMessage)
        {
            // maybe check if same order
            scanToShipViewModel.IsOrderVerified = true;
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose() => subscriptions?.Dispose();

        /// <summary>
        /// Handle any exceptions that occur
        /// </summary>
        private void HandleException(Exception ex) =>
            log.Error("Error occurred while handling message in ScanToShipPipeline.", ex);
    }
}
