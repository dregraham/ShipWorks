using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Interapptive.Shared.Collections;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.Messaging;
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
        private IDisposable subscriptions;
        private bool processingScan = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public ScanPackPipeline(
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
                .Where(x => !processingScan && !mainForm.AdditionalFormsOpen() && mainForm.UIMode == UIMode.OrderLookup && mainForm.IsScanPackActive())
                .Do(x => OnSingleScanMessage(x))
                .CatchAndContinue((Exception ex) => HandleException(ex))
                .Subscribe(),

                messenger.OfType<OrderLookupSearchMessage>()
                .Where(x => !processingScan && !mainForm.AdditionalFormsOpen() && mainForm.UIMode == UIMode.OrderLookup && mainForm.IsScanPackActive())
                .Do(x => OnOrderLookupSearchMessage(x))
                .CatchAndContinue((Exception ex) => HandleException(ex))
                .Subscribe(),

                messenger.OfType<OrderLookupLoadOrderMessage>()
                .Where(x => !processingScan && !mainForm.AdditionalFormsOpen() && mainForm.UIMode == UIMode.OrderLookup && !mainForm.IsScanPackActive())
                .Do(x => OnOrderLookupLoadOrderMessage(x))
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
        public void OnOrderLookupLoadOrderMessage(OrderLookupLoadOrderMessage message)
        {
            if (processingScan)
            {
                return;
            }

            processingScan = true;

            try
            {
                scanPackViewModel.Load(message.Order);
            }
            finally
            {
                processingScan = false;
            }
        }

        /// <summary>
        /// Handle search
        /// </summary>
        public void OnOrderLookupSearchMessage(OrderLookupSearchMessage message)
        {
            if (processingScan)
            {
                return;
            }

            processingScan = true;

            try
            {
                scanPackViewModel.Load(message.SearchText);
            }
            finally
            {
                processingScan = false;
            }
        }

        /// <summary>
        /// Handle barcode scans
        /// </summary>
        public void OnSingleScanMessage(SingleScanMessage message)
        {
            if (processingScan)
            {
                return;
            }

            processingScan = true;

            try
            {
                scanPackViewModel.Load(message.ScannedText);
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
    }
}
