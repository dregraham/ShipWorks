using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Interapptive.Shared.Collections;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.Common.IO.KeyboardShortcuts.Messages;
using ShipWorks.Core.Messaging;
using ShipWorks.IO.KeyboardShortcuts;
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
                    .Do(_ => scanToShipViewModel.IsOrderProcessed = true)
                    .CatchAndContinue((Exception ex) => HandleException(ex))
                    .Subscribe(),

                messenger.OfType<OrderVerifiedMessage>()
                    .Where(_ => mainForm.UIMode == UIMode.OrderLookup)
                    .Where(x => x.Order.Verified)
                    .Do(_ => scanToShipViewModel.IsOrderVerified = true)
                    .CatchAndContinue((Exception ex) => HandleException(ex))
                    .Subscribe(),

                messenger.OfType<ShortcutMessage>()
                    .Where(_ => mainForm.UIMode == UIMode.OrderLookup)
                    .Do(HandleNavigateMessage)
                    .CatchAndContinue((Exception ex) => HandleException(ex))
                    .Subscribe()
            );
        }
        
        /// <summary>
        /// Navigate to a specific tab
        /// </summary>
        private void HandleNavigateMessage(ShortcutMessage shortcutMessage)
        {
            if (shortcutMessage.AppliesTo(KeyboardShortcutCommand.NavigateToPackTab))
            {
                scanToShipViewModel.SelectedTab = (int) ScanToShipTab.PackTab;
            }

            if (shortcutMessage.AppliesTo(KeyboardShortcutCommand.NavigateToShipTab))
            {
                scanToShipViewModel.SelectedTab = (int) ScanToShipTab.ShipTab;
            }
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
