using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using Autofac.Features.Indexed;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages;
using ShipWorks.Messaging.Messages.Filters;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Messaging.Messages.SingleScan;
using ShipWorks.Stores.Content.Panels.Selectors;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Subscription for AutoPrint
    /// </summary>
    /// <seealso cref="ShipWorks.ApplicationCore.IInitializeForCurrentUISession" />
    public class AutoPrintServicePipeline : IInitializeForCurrentUISession
    {
        private readonly IAutoPrintService autoPrintService;
        public IDisposable FilterCompletedMessageSubscription;
        private const int FilterCountsUpdatedMessageTimeoutInSeconds = 25;
        private const int ShipmentsProcessedMessageTimeoutInMinutes = 5;
        private IDisposable scanMessagesConnection;
        private ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoPrintServicePipeline"/> class.
        /// </summary>
        public AutoPrintServicePipeline(IAutoPrintService autoPrintService, IMessenger messenger,
            ISchedulerProvider schedulerProvider, Func<Type, ILog> logFactory)
        {
            Messenger = messenger;
            SchedulerProvider = schedulerProvider;

            ScanMessages = messenger.OfType<ScanMessage>().Publish();
            scanMessagesConnection = ScanMessages.Connect();
            this.autoPrintService = autoPrintService;
            log = logFactory(typeof(AutoPrintServicePipeline));
        }

        /// <summary>
        /// Scan messages received by the auto print service
        /// </summary>
        public IConnectableObservable<ScanMessage> ScanMessages { get; set; }

        /// <summary>
        /// The messenger.
        /// </summary>
        public IMessenger Messenger { get; set; }

        /// <summary>
        /// The scheduler provider.
        /// </summary>
        public ISchedulerProvider SchedulerProvider { get; set; }

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
            FilterCompletedMessageSubscription = ScanMessages
                .Where(autoPrintService.AllowAutoPrint)
                .Do(x => EndScanMessagesObservation())
                .ContinueAfter(Messenger.OfType<SingleScanFilterUpdateCompleteMessage>(),
                    TimeSpan.FromSeconds(FilterCountsUpdatedMessageTimeoutInSeconds),
                    SchedulerProvider.Default,
                    (scanMsg, filterCountsUpdatedMessage) =>
                        new AutoPrintServiceDto(filterCountsUpdatedMessage, scanMsg))
                .ObserveOn(SchedulerProvider.WindowsFormsEventLoop)
                .SelectMany(m => autoPrintService.HandleAutoPrintShipment(m).ToObservable())
                .SelectMany(WaitForShipmentsProcessedMessage)
                .CatchAndContinue((Exception ex) => HandleException(ex))
                .Subscribe(x => StartScanMessagesObservation());
        }

        /// <summary>
        /// Disconnect the scan messages observable
        /// </summary>
        public void EndScanMessagesObservation()
        {
            log.Info("Ending scan message observation.");
            scanMessagesConnection?.Dispose();
            scanMessagesConnection = null;
        }

        /// <summary>
        /// Connect to the scan messages observable
        /// </summary>
        public void StartScanMessagesObservation()
        {
            log.Info("Starting scan message observation.");
            if (scanMessagesConnection == null)
            {
                scanMessagesConnection = ScanMessages.Connect();
            }
        }

        /// <summary>
        /// Logs the exception and reconnect pipeline.
        /// </summary>
        public void HandleException(Exception ex)
        {
            log.Error("Error occurred while attempting to auto print.", ex);
            StartScanMessagesObservation();
        }

        /// <summary>
        /// Waits for shipments processed message.
        /// </summary>
        public IObservable<GenericResult<string>> WaitForShipmentsProcessedMessage(GenericResult<string> genericResult)
        {
            IObservable<GenericResult<string>> returnResult;

            if (genericResult.Success)
            {
                // Listen for ShipmentsProcessedMessages, but timeout if processing takes
                // longer than ShipmentsProcessedMessageTimeoutInMinutes.
                // We don't get an observable to start from, but we need one to use ContinueAfter, so using
                // Observable.Return to get an observable to work with.
                returnResult = Observable.Return(0)
                    .ContinueAfter(Messenger.OfType<ShipmentsProcessedMessage>(),
                        TimeSpan.FromMinutes(ShipmentsProcessedMessageTimeoutInMinutes),
                        SchedulerProvider.Default,
                        ((i, message) => message))
                    .Do(message => Messenger.Send(
                        new OrderSelectionChangingMessage(this,
                        message.Shipments.Select(s => s.Shipment.OrderID).Distinct(),
                        EntityGridRowSelector.SpecificEntities(message.Shipments.Select(s => s.Shipment.ShipmentID).Distinct()))))
                    .Select(f => genericResult);

            }
            else
            {
                returnResult = Observable.Return(genericResult);
            }

            log.Info(genericResult.Failure ?
                "No Shipments, not waiting for ShipmentsProcessMessageScan" :
                $"ShipmentsProcessedMessage received from scan {genericResult.Value}");

            return returnResult;
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