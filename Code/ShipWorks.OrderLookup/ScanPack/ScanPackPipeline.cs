using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Threading;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Common.IO.KeyboardShortcuts;
using ShipWorks.Common.IO.KeyboardShortcuts.Messages;
using ShipWorks.Core.Common.Threading;
using ShipWorks.Core.Messaging;
using ShipWorks.Editions;
using ShipWorks.IO.KeyboardShortcuts;
using ShipWorks.Messaging.Messages.SingleScan;
using ShipWorks.OrderLookup.ScanToShip;
using ShipWorks.Settings;

namespace ShipWorks.OrderLookup.ScanPack
{
    /// <summary>
    /// Pipeline for Scan and Pack
    /// </summary>
    public class ScanPackPipeline : IOrderLookupPipeline
    {
        private readonly IMessenger messenger;
        private readonly IMainForm mainForm;
        private readonly IScanToShipViewModel scanToShipViewModel;
        private readonly ILicenseService licenseService;
        private readonly ISchedulerProvider schedulerProvider;
        private readonly IShortcutManager shortcutManager;
        private readonly ILog log;
        private IDisposable subscriptions;
        private bool processingScan = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public ScanPackPipeline(
            IMessenger messenger,
            IMainForm mainForm,
            IScanToShipViewModel scanToShipViewModel,
            ILicenseService licenseService,
            ISchedulerProvider schedulerProvider,
            IShortcutManager shortcutManager,
            Func<Type, ILog> createLogger)
        {
            this.messenger = messenger;
            this.mainForm = mainForm;
            this.scanToShipViewModel = scanToShipViewModel;
            this.licenseService = licenseService;
            this.schedulerProvider = schedulerProvider;
            this.shortcutManager = shortcutManager;
            this.log = createLogger(GetType());
        }

        /// <summary>
        /// Wire up the scan and pack pipeline
        /// </summary>
        public void InitializeForCurrentScope()
        {
            EndSession();

            EditionRestrictionLevel restrictionLevel = licenseService.CheckRestriction(EditionFeature.Warehouse, null);
            if (restrictionLevel != EditionRestrictionLevel.None)
            {
                scanToShipViewModel.ScanPackViewModel.Enabled = false;
                return;
            }

            scanToShipViewModel.ScanPackViewModel.Enabled = true;

            subscriptions = new CompositeDisposable(
                messenger.OfType<SingleScanMessage>()
                .Where(x => ShouldProcessScan() && scanToShipViewModel.IsPackTabActive)
                .Do(x => processingScan = true)
                .Do(x => OnOrderLookupSearch(x.ScannedText).Forget())
                .CatchAndContinue((Exception ex) => HandleException(ex))
                .Subscribe(),

                messenger.OfType<OrderLookupSearchMessage>()
                .Where(x => ShouldProcessScan() && scanToShipViewModel.IsPackTabActive)
                .Do(x => processingScan = true)
                .Do(x => OnOrderLookupSearch(x.SearchText).Forget())
                .CatchAndContinue((Exception ex) => HandleException(ex))
                .Subscribe(),

                messenger.OfType<OrderLookupLoadOrderMessage>()
                .Do(x => processingScan = true)
                .Do(x => OnOrderLookupLoadOrderMessage(x).Forget())
                .CatchAndContinue((Exception ex) => HandleException(ex))
                .Subscribe(),

                messenger.OfType<OrderLookupClearOrderMessage>()
                .Where(x => ShouldProcessScan())
                .Where(x => x.Reason == OrderClearReason.Reset)
                .Do(x => scanToShipViewModel.ScanPackViewModel.Reset())
                .CatchAndContinue((Exception ex) => HandleException(ex))
                .Subscribe(),

                messenger.OfType<ShortcutMessage>()
                .Where(m => m.AppliesTo(KeyboardShortcutCommand.ClearQuickSearch))
                .ObserveOn(schedulerProvider.WindowsFormsEventLoop)
                .Where(_ => scanToShipViewModel.ScanPackViewModel.CanAcceptFocus())
                .Do(_ => messenger.Send(new OrderLookupClearOrderMessage(this, OrderClearReason.Reset)))
                .Do(shortcutManager.ShowShortcutIndicator)
                .CatchAndContinue((Exception ex) => HandleException(ex))
                .Subscribe()
            );
        }

        /// <summary>
        /// Should we process scans
        /// </summary>
        /// <returns></returns>
        private bool ShouldProcessScan() =>
            !processingScan && 
            !mainForm.AdditionalFormsOpen() && 
            mainForm.UIMode == UIMode.OrderLookup;

        /// <summary>
        /// Handle Exceptions
        /// </summary>
        private void HandleException(Exception ex) => 
            log.Error("Error occurred while handling scan message in ScanPackPipeline.", ex);

        /// <summary>
        /// Handle search
        /// </summary>
        public async Task OnOrderLookupLoadOrderMessage(OrderLookupLoadOrderMessage message)
        {
            try
            {
                await scanToShipViewModel.ScanPackViewModel.LoadOrder(message.Order).ConfigureAwait(true);
            }
            finally
            {
                processingScan = false;
            }
        }

        /// <summary>
        /// Handle search
        /// </summary>
        public async Task OnOrderLookupSearch(string searchText)
        {
            try
            {
                await scanToShipViewModel.ScanPackViewModel.ProcessScan(searchText).ConfigureAwait(true);
            }
            finally
            {
                processingScan = false;
            }
        }

        /// <summary>
        /// End the session
        /// </summary>
        public void EndSession() => subscriptions?.Dispose();

        /// <summary>
        /// End the session
        /// </summary>
        public void Dispose()
        {
            EndSession();
        }
    }
}
