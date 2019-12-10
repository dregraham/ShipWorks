using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Interapptive.Shared;
using Interapptive.Shared.Collections;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Common.IO.KeyboardShortcuts.Messages;
using ShipWorks.Core.Messaging;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.Messaging.Messages.Orders;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.OrderLookup.Messages;
using ShipWorks.Settings;
using ShipWorks.Users;

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
        private readonly ILicenseService licenseService;
        private readonly IUserSession userSession;

        /// <summary>
        /// Constructor
        /// </summary>
        [NDependIgnoreTooManyParams]
        public ScanToShipPipeline(IMessenger messenger, IScanToShipViewModel scanToShipViewModel, IMainForm mainForm,
                                  ILicenseService licenseService, IUserSession userSession, Func<Type, ILog> createLogger)
        {
            this.messenger = messenger;
            this.scanToShipViewModel = scanToShipViewModel;
            this.mainForm = mainForm;
            this.licenseService = licenseService;
            this.userSession = userSession;
            log = createLogger(GetType());

            scanToShipViewModel.SelectedTab = (int) (licenseService.IsHub ? ScanToShipTab.PackTab : ScanToShipTab.ShipTab);
        }

        /// <summary>
        /// Interface for initializing order lookup pipelines under a top level lifetime scope
        /// </summary>
        public void InitializeForCurrentScope()
        {
            Dispose();

            subscriptions = new CompositeDisposable(

                messenger.OfType<ScanToShipShipmentLoadedMessage>()
                    .Where(_ => mainForm.UIMode == UIMode.OrderLookup)
                    .Do(HandleShipmentLoadedMessage)
                    .CatchAndContinue((Exception ex) => HandleException(ex))
                    .Subscribe(),

                messenger.OfType<ShipmentsProcessedMessage>()
                    .Where(_ => mainForm.UIMode == UIMode.OrderLookup)
                    .Where(x => x.Shipments.All(s => s.Shipment.Processed))
                    .Do(_ => scanToShipViewModel.IsOrderProcessed = true)
                    .CatchAndContinue((Exception ex) => HandleException(ex))
                    .Subscribe(),

                messenger.OfType<OrderVerifiedMessage>()
                    .Where(_ => mainForm.UIMode == UIMode.OrderLookup)
                    .Where(x => x.Order.Verified)
                    .Do(HandleOrderVerifiedMessage)
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
        /// Does the user have the auto advance setting enabled
        /// </summary>
        private bool IsAutoAdvanceEnabled => licenseService.IsHub && userSession?.Settings?.ScanToShipAutoAdvance == true;

        /// <summary>
        /// Handle the shipment loaded message
        /// </summary>
        private void HandleShipmentLoadedMessage(ScanToShipShipmentLoadedMessage shipmentLoadedMessage)
        {
            scanToShipViewModel.ShowVerificationError = false;
            scanToShipViewModel.IsOrderVerified = shipmentLoadedMessage?.Shipment?.Order?.Verified ?? false;
            scanToShipViewModel.IsOrderProcessed = shipmentLoadedMessage?.Shipment?.Processed ?? false;

            if (IsAutoAdvanceEnabled)
            {
                scanToShipViewModel.SelectedTab = (int) (scanToShipViewModel.IsOrderVerified ?
                    ScanToShipTab.ShipTab :
                    ScanToShipTab.PackTab);
            }
        }

        /// <summary>
        /// Handle the order verified message
        /// </summary>
        private void HandleOrderVerifiedMessage(OrderVerifiedMessage orderVerifiedMessage)
        {
            scanToShipViewModel.ShowVerificationError = false;
            scanToShipViewModel.IsOrderVerified = true;

            if (IsAutoAdvanceEnabled)
            {
                scanToShipViewModel.SelectedTab = (int) ScanToShipTab.ShipTab;
            }
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
