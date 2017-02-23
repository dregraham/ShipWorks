using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages.Filters;
using ShipWorks.Messaging.Messages.SingleScan;
using ShipWorks.Shipping.Services;

namespace ShipWorks.SingleScan
{
    public class LoggableAutoPrintService : AutoPrintService, IInitializeForCurrentUISession
    {
        private readonly ILog log;
        private const int FilterCountsUpdatedMessageTimeoutInSeconds = 25;

        public LoggableAutoPrintService(IMessenger messenger, ISchedulerProvider schedulerProvider,
            IAutoPrintPermissions autoPrintPermissions,
            ISingleScanShipmentConfirmationService singleScanShipmentConfirmationService,
            ISingleScanOrderConfirmationService singleScanOrderConfirmationService, Func<Type, ILog> logFactory,
            Func<string, ITrackedDurationEvent> trackedDurationEventFactory) :
                base(messenger, schedulerProvider, autoPrintPermissions, singleScanShipmentConfirmationService,
                    singleScanOrderConfirmationService, trackedDurationEventFactory)
        {
            log = logFactory(typeof(AutoPrintService));
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
            FilterCompletedMessageSubscription = ScanMessages
                .Where(AllowAutoPrint)
                .Do(x => EndScanMessagesObservation())
                .ContinueAfter(Messenger.OfType<SingleScanFilterUpdateCompleteMessage>(),
                    TimeSpan.FromSeconds(FilterCountsUpdatedMessageTimeoutInSeconds),
                    SchedulerProvider.Default,
                    (scanMsg, filterCountsUpdatedMessage) =>
                        new AutoPrintServiceDto(filterCountsUpdatedMessage, scanMsg))
                .ObserveOn(SchedulerProvider.WindowsFormsEventLoop)
                .SelectMany(m => HandleAutoPrintShipment(m).ToObservable())
                .SelectMany(WaitForShipmentsProcessedMessage)
                .CatchAndContinue((Exception ex) => HandleException(ex))
                .Subscribe(x => StartScanMessagesObservation());
        }

        protected new void EndScanMessagesObservation()
        {
            log.Info("Ending scan message observation.");
            base.EndScanMessagesObservation();
        }

        /// <summary>
        /// Connect to the scan messages observable
        /// </summary>
        protected new void StartScanMessagesObservation()
        {
            log.Info("Starting scan message observation.");
            base.StartScanMessagesObservation();
        }

        /// <summary>
        /// Logs the exception and reconnect pipeline.
        /// </summary>
        protected new void HandleException(Exception ex)
        {
            log.Error("Error occurred while attempting to auto print.", ex);
            base.HandleException(ex);
        }

        protected new Task<GenericResult<string>> HandleAutoPrintShipment(AutoPrintServiceDto autoPrintServiceDto)
        {
            long? orderID = GetOrderID(autoPrintServiceDto);

            // Only auto print if an order was found
            if (!orderID.HasValue)
            {
                log.Error("Order not found for scanned order.");
            }

            return base.HandleAutoPrintShipment(autoPrintServiceDto);
        }

        protected new IObservable<GenericResult<string>> WaitForShipmentsProcessedMessage(
            GenericResult<string> genericResult)
        {
            IObservable<GenericResult<string>> returnResult;

            if (genericResult.Success)
            {
                log.Info($"Waiting for ShipmentsProcessedMessage from scan {genericResult.Value}");
                returnResult = base.WaitForShipmentsProcessedMessage(genericResult);
                log.Info($"ShipmentsProcessedMessage received from scan {genericResult.Value}");
            }
            else
            {
                log.Info("No Shipments, not waiting for ShipmentsProcessMessageScan");
                returnResult = Observable.Return(genericResult);
            }

            return returnResult;
        }
    }
}