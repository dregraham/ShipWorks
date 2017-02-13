using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Options;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages.Filters;
using ShipWorks.Messaging.Messages.SingleScan;
using ShipWorks.Users;

namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// Class for registering observables in the MainGridControl
    /// </summary>
    public class MainGridControlPipeline : IMainGridControlPipeline, IDisposable
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(MainGridControlPipeline));

        private readonly IMainForm mainForm;
        private readonly IMessenger messenger;
        private readonly IUserSession userSession;
        private readonly ISchedulerProvider schedulerProvider;
        private MainGridControl gridControl;

        // Debouncing observables for searching
        private IDisposable quickSearchSubscription;
        private IDisposable advancedSearchSubscription;
        private IDisposable barcodeScannedMessageSubscription;
        private readonly IConnectableObservable<ScanMessage> scanMessages;
        private IDisposable scanMessagesConnection;

        /// <summary>
        /// Constructor
        /// </summary>
        public MainGridControlPipeline(IMessenger messenger, IUserSession userSession, IMainForm mainForm, ISchedulerProvider schedulerProvider)
        {
            this.messenger = messenger;
            this.userSession = userSession;
            this.schedulerProvider = schedulerProvider;
            this.mainForm = mainForm;

            scanMessages = messenger.OfType<ScanMessage>().Publish();
            scanMessagesConnection = scanMessages.Connect();
        }

        /// <summary>
        /// Register the pipeline with the main grid control
        /// </summary>
        public IDisposable Register(MainGridControl mainGridControl)
        {
            gridControl = mainGridControl;

            return new CompositeDisposable(
                // Wire up observable for debouncing quick search text box
                quickSearchSubscription = Observable
                    .FromEventPattern(gridControl.SearchTextChangedAdd, gridControl.SearchTextChangedRemove)
                    .Throttle(TimeSpan.FromMilliseconds(450))
                    .ObserveOn(schedulerProvider.WindowsFormsEventLoop)
                    .CatchAndContinue((Exception ex) => log.Error("Error occurred while debouncing quick search.", ex))
                    .Subscribe(x => gridControl.PerformManualSearch()),

                // Wire up observable for debouncing advanced search text box
                advancedSearchSubscription = Observable
                    .FromEventPattern(gridControl.FilterEditorDefinitionEditedAdd, gridControl.FilterEditorDefinitionEditedRemove)
                    .Throttle(TimeSpan.FromMilliseconds(450))
                    .ObserveOn(schedulerProvider.WindowsFormsEventLoop)
                    .CatchAndContinue((Exception ex) => log.Error("Error occurred while debouncing advanced search.", ex))
                    .Subscribe(x => gridControl.PerformManualSearch()),

                // Wire up observable for doing barcode searches
                barcodeScannedMessageSubscription = scanMessages
                    .ObserveOn(schedulerProvider.WindowsFormsEventLoop)
                    // This causes the shipping panel to save if there are unsaved changes
                    .Do(_ => mainForm.Focus())
                    .Where(scanMsg => AllowBarcodeSearch(scanMsg.ScannedText))
                    .Do(scanMsg => EndScanMessagesObservation())
                    // We need to do the barcode search asynchronously so that the ContinueAfter registration starts immediately, otherwise we could
                    // miss incoming FilterCountsUpdatedMessages and have to fail over to the timeout
                    .Do(scanMsg => PerformBarcodeSearchAsync(scanMsg.ScannedText))
                    // Start listening for FilterCountsUpdatedMessages, and only continue after we receive one or the timeout has passed.
                    .ContinueAfter(messenger.OfType<SingleScanFilterUpdateCompleteMessage>(), TimeSpan.FromSeconds(25), schedulerProvider.WindowsFormsEventLoop)
                    .CatchAndContinue((Exception ex) =>
                    {
                        log.Error("Error occurred while performing barcode search.", ex);

                        StartScanMessagesObservation();
                    })
                    .Subscribe(_ => StartScanMessagesObservation())
            );
        }

        /// <summary>
        /// Handles the request for auto printing an order.
        /// </summary>
        public void PerformBarcodeSearchAsync(string scannedBarcode)
        {
            TaskEx.Run(() =>
            {
                if (gridControl?.InvokeRequired == true)
                {
                    gridControl.Invoke(new Action(() => gridControl?.PerformBarcodeSearch(scannedBarcode)));
                }
                else
                {
                    gridControl?.PerformBarcodeSearch(scannedBarcode);
                }
            });
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
        private bool AllowBarcodeSearch(string barcode)
        {
            return !barcode.IsNullOrWhiteSpace() &&
                   gridControl.Visible &&
                   gridControl.CanFocus &&
                   userSession.Settings?.SingleScanSettings != (int) SingleScanSettings.Disabled &&
                   !mainForm.AdditionalFormsOpen();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        public void Dispose()
        {
            quickSearchSubscription?.Dispose();
            advancedSearchSubscription?.Dispose();
            barcodeScannedMessageSubscription?.Dispose();

            // This dispose is really just disconnecting the observable, so we'll go ahead and
            // set it to null as well.
            scanMessagesConnection?.Dispose();
            scanMessagesConnection = null;
        }
    }
}
