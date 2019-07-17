using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.Common.Threading;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages.SingleScan;
using ShipWorks.OrderLookup;
using ShipWorks.Settings;

namespace ShipWorks.SingleScan.ScanPack
{
    /// <summary>
    /// Pipeline for Scan and Pack
    /// </summary>
    class SacnPackPipeline : IOrderLookupPipeline
    {
        private readonly IMessenger messenger;
        private readonly IMainForm mainForm;
        private readonly IScanPackViewModel scanPackViewModel;
        private IDisposable subscriptions;
        private bool processingScan = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public SacnPackPipeline(
            IMessenger messenger,
            IMainForm mainForm,
            IScanPackViewModel scanPackViewModel)
        {
            this.messenger = messenger;
            this.mainForm = mainForm;
            this.scanPackViewModel = scanPackViewModel;
        }

        /// <summary>
        /// Wire up the scan and pack pipeline
        /// </summary>
        public void InitializeForCurrentScope()
        {
            EndSession();

            subscriptions = new CompositeDisposable(
                messenger.OfType<SingleScanMessage>()
                .Where(x => !processingScan && !mainForm.AdditionalFormsOpen() && mainForm.UIMode == UIMode.OrderLookup && !mainForm.IsShipmentHistoryActive())
                .Do(_ => processingScan = true)
                .Do(x => OnSingleScanMessage(x).Forget())
                .CatchAndContinue((Exception ex) => HandleException(ex))
                .Subscribe(),

                messenger.OfType<OrderLookupSearchMessage>()
                .Where(x => !processingScan && !mainForm.AdditionalFormsOpen() && mainForm.UIMode == UIMode.OrderLookup && !mainForm.IsShipmentHistoryActive())
                .Do(_ => processingScan = true)
                .Do(x => OnOrderLookupSearchMessage(x).Forget())
                .CatchAndContinue((Exception ex) => HandleException(ex))
                .Subscribe()
            );
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
        public async Task OnOrderLookupSearchMessage(OrderLookupSearchMessage message)
        {
            scanPackViewModel.Load(message.SearchText);
            processingScan = false;
        }

        /// <summary>
        /// Handle barcode scans
        /// </summary>
        public async Task OnSingleScanMessage(SingleScanMessage message)
        {
            scanPackViewModel.Load(message.ScannedText);
            processingScan = false;
        }

        /// <summary>
        /// End the session
        /// </summary>
        public void EndSession() => subscriptions?.Dispose();
    }
}
