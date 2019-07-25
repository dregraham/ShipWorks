using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Core.Common.Threading;
using ShipWorks.Core.Messaging;
using ShipWorks.Editions;
using ShipWorks.Messaging.Messages.SingleScan;
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
        private readonly IScanPackViewModel scanPackViewModel;
        private readonly ILicenseService licenseService;
        private IDisposable subscriptions;
        private bool processingScan = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public ScanPackPipeline(
            IMessenger messenger,
            IMainForm mainForm,
            IScanPackViewModel scanPackViewModel,
            ILicenseService licenseService)
        {
            this.messenger = messenger;
            this.mainForm = mainForm;
            this.scanPackViewModel = scanPackViewModel;
            this.licenseService = licenseService;
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
                scanPackViewModel.Enabled = false;
                return;
            }

            scanPackViewModel.Enabled = true;

            subscriptions = new CompositeDisposable(
                messenger.OfType<SingleScanMessage>()
                .Where(x => !processingScan && !mainForm.AdditionalFormsOpen() && mainForm.UIMode == UIMode.OrderLookup && mainForm.IsScanPackActive())
                .Do(x => processingScan = true)
                .Do(x => OnOrderLookupSearch(x.ScannedText).Forget())
                .CatchAndContinue((Exception ex) => HandleException(ex))
                .Subscribe(),

            messenger.OfType<OrderLookupSearchMessage>()
                .Where(x => !processingScan && !mainForm.AdditionalFormsOpen() && mainForm.UIMode == UIMode.OrderLookup && mainForm.IsScanPackActive())
                .Do(x => processingScan = true)
                .Do(x => OnOrderLookupSearch(x.SearchText).Forget())
                .CatchAndContinue((Exception ex) => HandleException(ex))
                .Subscribe(),

            messenger.OfType<OrderLookupLoadOrderMessage>()
                .Where(x => !processingScan && !mainForm.AdditionalFormsOpen() && mainForm.UIMode == UIMode.OrderLookup && !mainForm.IsScanPackActive())
                .Do(x => processingScan = true)
                .Do(x => OnOrderLookupLoadOrderMessage(x).Forget())
                .CatchAndContinue((Exception ex) => HandleException(ex))
                .Subscribe(),

            messenger.OfType<OrderLookupClearOrderMessage>()
                .Do(OnOrderLookupClearOrderMessage)
                .CatchAndContinue((Exception ex) => HandleException(ex))
                .Subscribe()
            );
        }

        /// <summary>
        /// Handle clear message
        /// </summary>
        private void OnOrderLookupClearOrderMessage(OrderLookupClearOrderMessage message)
        {
            if (message.Reason == OrderClearReason.Reset)
            {
                scanPackViewModel.Reset();
            }
        }

        /// <summary>
        /// Handle Exceptions
        /// </summary>
        private void HandleException(Exception ex)
        {
            throw ex;
        }

        /// <summary>
        /// Handle search
        /// </summary>
        public async Task OnOrderLookupLoadOrderMessage(OrderLookupLoadOrderMessage message)
        {
            try
            {
                await scanPackViewModel.Load(message.Order).ConfigureAwait(true);
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
                await scanPackViewModel.Load(searchText).ConfigureAwait(true);
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
