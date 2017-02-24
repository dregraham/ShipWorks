using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using Autofac.Features.Indexed;
using Interapptive.Shared.Collections;
using ShipWorks.ApplicationCore;
using ShipWorks.Messaging.Messages.Filters;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Subscription for AutoPrint
    /// </summary>
    /// <seealso cref="ShipWorks.ApplicationCore.IInitializeForCurrentUISession" />
    public class AutoPrintSubscription : IInitializeForCurrentUISession
    {
        private readonly IAutoPrintService autoPrintService;
        public IDisposable FilterCompletedMessageSubscription;
        private const int FilterCountsUpdatedMessageTimeoutInSeconds = 25;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoPrintSubscription"/> class.
        /// </summary>
        /// <param name="autoPrintServiceFactory">The automatic print service factory.</param>
        public AutoPrintSubscription(IIndex<AutoPrintServiceType, IAutoPrintService> autoPrintServiceFactory)
        {
            autoPrintService = autoPrintServiceFactory[AutoPrintServiceType.Loggable];
        }

        /// <summary>
        /// Initialize auto print for the current session
        /// </summary>
        public void InitializeForCurrentSession()
        {
            // Wire up observable for auto printing
            // note: One of the first things we do is dispose of scanMessagesConnection.
            // This turns off the pipeline to ensure that another order isn't
            // picked up before we are finished with possessing the current order.
            // All exit points of the pipeline need to call ReconnectPipeline()
            FilterCompletedMessageSubscription = autoPrintService.ScanMessages
                .Where(autoPrintService.AllowAutoPrint)
                .Do(x => autoPrintService.EndScanMessagesObservation())
                .ContinueAfter(autoPrintService.Messenger.OfType<SingleScanFilterUpdateCompleteMessage>(),
                    TimeSpan.FromSeconds(FilterCountsUpdatedMessageTimeoutInSeconds),
                    autoPrintService.SchedulerProvider.Default,
                    (scanMsg, filterCountsUpdatedMessage) =>
                        new AutoPrintServiceDto(filterCountsUpdatedMessage, scanMsg))
                .ObserveOn(autoPrintService.SchedulerProvider.WindowsFormsEventLoop)
                .SelectMany(m => autoPrintService.HandleAutoPrintShipment(m).ToObservable())
                .SelectMany(autoPrintService.WaitForShipmentsProcessedMessage)
                .CatchAndContinue((Exception ex) => autoPrintService.HandleException(ex))
                .Subscribe(x => autoPrintService.StartScanMessagesObservation());
        }

        /// <summary>
        /// End the current session
        /// </summary>
        public void EndSession()
        {
            FilterCompletedMessageSubscription?.Dispose();
        }

        /// <summary>
        /// Dispose the container
        /// </summary>
        public void Dispose()
        {
            EndSession();
        }
    }
}