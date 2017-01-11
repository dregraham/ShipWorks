using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Threading;
using log4net;
using ShipWorks.ApplicationCore.Options;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages;
using ShipWorks.Users;
using Interapptive.Shared.Utility;

namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// Class for registering observables in the MainGridControl
    /// </summary>
    public class MainGridControlPipeline : IMainGridControlPipeline, IDisposable
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(MainGridControlPipeline));

        private readonly IMessenger messenger;
        private readonly IUserSession userSession;
        private readonly ISchedulerProvider schedulerProvider;

        // Debouncing observables for searching
        IDisposable quickSearchSubscription;
        IDisposable advancedSearchSubscription;
        IDisposable barcodeScannedMessageSubscription;

        /// <summary>
        /// Constructor
        /// </summary>
        public MainGridControlPipeline(IMessenger messenger, IUserSession userSession, ISchedulerProvider schedulerProvider)
        {
            this.messenger = messenger;
            this.userSession = userSession;
            this.schedulerProvider = schedulerProvider;
        }

        /// <summary>
        /// Register the pipeline with the main grid control
        /// </summary>
        public IDisposable Register(MainGridControl mainGridControl)
        {
            return new CompositeDisposable(
                // Wire up observable for debouncing quick search text box
                quickSearchSubscription = Observable
                    .FromEventPattern(mainGridControl.SearchTextChangedAdd, mainGridControl.SearchTextChangedRemove)
                    .Throttle(TimeSpan.FromMilliseconds(450))
                    .ObserveOn(schedulerProvider.WindowsFormsEventLoop)
                    .CatchAndContinue((Exception ex) => log.Error("Error occurred while debouncing quick search.", ex))
                    .Subscribe(x => mainGridControl.PerformSearch()),

                // Wire up observable for debouncing advanced search text box
                advancedSearchSubscription = Observable
                    .FromEventPattern(mainGridControl.FilterEditorDefinitionEditedAdd, mainGridControl.FilterEditorDefinitionEditedRemove)
                    .Throttle(TimeSpan.FromMilliseconds(450))
                    .ObserveOn(schedulerProvider.WindowsFormsEventLoop)
                    .CatchAndContinue((Exception ex) => log.Error("Error occurred while debouncing advanced search.", ex))
                    .Subscribe(x => mainGridControl.PerformSearch()),

                // Wire up observable for doing barcode searches
                barcodeScannedMessageSubscription = messenger.OfType<ScanMessage>()
                    .Where(x => AllowBarcodeSearch(mainGridControl))
                    .ObserveOn(schedulerProvider.WindowsFormsEventLoop)
                    .CatchAndContinue((Exception ex) => log.Error("Error occurred while performing barcode search search.", ex))
                    .Subscribe(m => mainGridControl.PerformBarcodeSearch(m.ScannedText))
            );
        }

        /// <summary>
        /// Determines if the barcode search message should be sent
        /// </summary>
        private bool AllowBarcodeSearch(MainGridControl mainGridControl)
        {
            return mainGridControl.Visible && 
                   mainGridControl.CanFocus &&
                   !ApplicationUtility.AnyModalDialogs() &&
                   userSession.Settings?.SingleScanSettings != (int) SingleScanSettings.Disabled;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        public void Dispose()
        {
            quickSearchSubscription?.Dispose();
            advancedSearchSubscription?.Dispose();
            barcodeScannedMessageSubscription?.Dispose();
        }
    }
}
