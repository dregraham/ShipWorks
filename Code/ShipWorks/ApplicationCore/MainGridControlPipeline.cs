﻿using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Settings;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages.Filters;
using ShipWorks.Messaging.Messages.SingleScan;
using ShipWorks.Settings;
using ShipWorks.Stores.Communication;
using ShipWorks.Users;

namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// Class for registering observables in the MainGridControl
    /// </summary>
    public class MainGridControlPipeline : IMainGridControlPipeline
    {
        // Logger
        private static readonly ILog log = LogManager.GetLogger(typeof(MainGridControlPipeline));

        private readonly IMainForm mainForm;
        private readonly IMessenger messenger;
        private readonly ISchedulerProvider schedulerProvider;
        private readonly ICurrentUserSettings currentUserSettings;
        private readonly IOnDemandDownloader singleScanOnDemandDownloader;
        private readonly IOnDemandDownloader onDemandDownloader;

        // Debouncing observables for searching
        private readonly IConnectableObservable<SingleScanMessage> scanMessages;
        private IDisposable scanMessagesConnection;

        /// <summary>
        /// Constructor
        /// </summary>
        public MainGridControlPipeline(
            IMessenger messenger,
            IMainForm mainForm,
            ISchedulerProvider schedulerProvider,
            IOnDemandDownloaderFactory onDemandDownloaderFactory,
            ICurrentUserSettings currentUserSettings)
        {
            this.messenger = messenger;
            this.schedulerProvider = schedulerProvider;
            this.currentUserSettings = currentUserSettings;
            singleScanOnDemandDownloader = onDemandDownloaderFactory.CreateSingleScanOnDemandDownloader();
            onDemandDownloader = onDemandDownloaderFactory.CreateOnDemandDownloader();
            this.mainForm = mainForm;

            scanMessages = messenger.OfType<SingleScanMessage>().Publish();
            scanMessagesConnection = scanMessages.Connect();
        }

        /// <summary>
        /// Register the pipeline with the main grid control
        /// </summary>
        public IDisposable Register(IMainGridControl gridControl)
        {
            return new CompositeDisposable(
                // Wire up observable for debouncing quick search text box
                Observable.FromEventPattern(gridControl.SearchTextChangedAdd, gridControl.SearchTextChangedRemove)
                    .Throttle(TimeSpan.FromMilliseconds(450))
                    .ObserveOn(schedulerProvider.WindowsFormsEventLoop)
                    .Where(_ => currentUserSettings.GetUIMode() == UIMode.Batch)
                    .Do(x => PerformDownloadOnDemand(gridControl.GetBasicSearchText()))
                    .Do(x => gridControl.PerformManualSearch())
                    .CatchAndContinue((Exception ex) => log.Error("Error occurred while debouncing quick search.", ex))
                    .Subscribe(),

                // Wire up observable for debouncing advanced search text box
                Observable.FromEventPattern(gridControl.FilterEditorDefinitionEditedAdd, gridControl.FilterEditorDefinitionEditedRemove)
                    .Throttle(TimeSpan.FromMilliseconds(450))
                    .ObserveOn(schedulerProvider.WindowsFormsEventLoop)
                    .Where(_ => currentUserSettings.GetUIMode() == UIMode.Batch)
                    .Do(x => gridControl.PerformManualSearch())
                    .CatchAndContinue((Exception ex) => log.Error("Error occurred while debouncing advanced search.", ex))
                    .Subscribe(),

                // Wire up observable for doing barcode searches
                scanMessages.ObserveOn(schedulerProvider.WindowsFormsEventLoop)
                    .Where(_ => currentUserSettings.GetUIMode() == UIMode.Batch)
                    // This causes the shipping panel to save if there are unsaved changes
                    .Do(_ => mainForm.Focus())
                    .Where(scanMsg => AllowBarcodeSearch(gridControl, scanMsg.ScannedText))
                    .Do(scanMsg => EndScanMessagesObservation())
                    .SelectMany(scanMsg => Observable.FromAsync(() => PerformBarcodeDownloadOnDemand(scanMsg.ScannedText)).Select(_ => scanMsg))
                    .Do(scanMsg => PerformBarcodeSearchAsync(gridControl, scanMsg.ScannedText))
                    // Start listening for FilterCountsUpdatedMessages, and only continue after we receive one or the timeout has passed.
                    .ContinueAfter(messenger.OfType<SingleScanFilterUpdateCompleteMessage>(), TimeSpan.FromSeconds(25), schedulerProvider.WindowsFormsEventLoop)
                    .Do(_ => StartScanMessagesObservation())
                    .CatchAndContinue((Exception ex) =>
                    {
                        log.Error("Error occurred while performing barcode search.", ex);

                        StartScanMessagesObservation();
                    })
                    .Subscribe(),

                // This class doesn't actually get disposed, so we need to include our cleanup here
                Disposable.Create(() =>
                {
                    // This dispose is really just disconnecting the observable, so we'll go ahead and
                    // set it to null as well.
                    scanMessagesConnection?.Dispose();
                    scanMessagesConnection = null;
                })
            );
        }

        /// <summary>
        /// Download order from quicksearch
        /// </summary>
        private Task PerformDownloadOnDemand(string searchText)
        {
            return onDemandDownloader.Download(searchText);
        }

        /// <summary>
        /// Download order from scan
        /// </summary>
        private Task PerformBarcodeDownloadOnDemand(string searchString)
        {
            return singleScanOnDemandDownloader.Download(searchString.Trim());
        }

        /// <summary>
        /// Handles the request for auto printing an order.
        /// </summary>
        /// <remarks>
        /// We need to do the barcode search asynchronously so that the ContinueAfter registration starts immediately,
        /// otherwise we could miss incoming FilterCountsUpdatedMessages and have to fail over to the timeout.
        /// </remarks>
        public void PerformBarcodeSearchAsync(IMainGridControl gridControl, string scannedBarcode)
        {
            gridControl.BeginInvoke((Action<string>) gridControl.PerformBarcodeSearch, scannedBarcode);
        }

        /// <summary>
        /// Disconnect the scan messages observable
        /// </summary>
        private void EndScanMessagesObservation()
        {
            log.Info("Ending scan message observation.");

            scanMessagesConnection?.Dispose();
            scanMessagesConnection = null;

            Cursor.Current = Cursors.WaitCursor;
        }

        /// <summary>
        /// Connect to the scan messages observable
        /// </summary>
        private void StartScanMessagesObservation()
        {
            log.Info("Starting scan message observation.");

            if (scanMessagesConnection == null)
            {
                scanMessagesConnection = scanMessages.Connect();
            }

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Determines if the barcode search message should be sent
        /// </summary>
        private bool AllowBarcodeSearch(IMainGridControl gridControl, string barcode)
        {
            return !barcode.IsNullOrWhiteSpace() &&
                   gridControl.Visible &&
                   gridControl.CanFocus &&
                   currentUserSettings.GetSingleScanSettings() != SingleScanSettings.Disabled &&
                   !mainForm.AdditionalFormsOpen();
        }
    }
}
