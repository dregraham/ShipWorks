using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.Messaging;
using ShipWorks.Shipping.Services;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Log wrapper for AutoPrintService
    /// </summary>
    /// <seealso cref="ShipWorks.SingleScan.AutoPrintService" />
    public class LoggableAutoPrintService : AutoPrintService, IInitializeForCurrentUISession
    {
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggableAutoPrintService"/> class.
        /// </summary>
        /// <param name="messenger">The messenger.</param>
        /// <param name="schedulerProvider">The scheduler provider.</param>
        /// <param name="autoPrintPermissions">The automatic print permissions.</param>
        /// <param name="singleScanShipmentConfirmationService">The single scan shipment confirmation service.</param>
        /// <param name="singleScanOrderConfirmationService">The single scan order confirmation service.</param>
        /// <param name="logFactory">The log factory.</param>
        /// <param name="trackedDurationEventFactory">The tracked duration event factory.</param>
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
        /// Disconnect the scan messages observable
        /// </summary>
        protected override void EndScanMessagesObservation()
        {
            log.Info("Ending scan message observation.");
            base.EndScanMessagesObservation();
        }

        /// <summary>
        /// Connect to the scan messages observable
        /// </summary>
        protected override void StartScanMessagesObservation()
        {
            log.Info("Starting scan message observation.");
            base.StartScanMessagesObservation();
        }

        /// <summary>
        /// Logs the exception and reconnect pipeline.
        /// </summary>
        protected override void HandleException(Exception ex)
        {
            log.Error("Error occurred while attempting to auto print.", ex);
            base.HandleException(ex);
        }

        /// <summary>
        /// Handles the request for auto printing an order.
        /// </summary>
        protected override async Task<GenericResult<string>> HandleAutoPrintShipment(AutoPrintServiceDto autoPrintServiceDto)
        {
            GenericResult<string> result = await base.HandleAutoPrintShipment(autoPrintServiceDto);

            if (result.Failure)
            {
                log.Error(result.Message);
            }

            return result;
        }

        /// <summary>
        /// Waits for shipments processed message.
        /// </summary>
        protected override IObservable<GenericResult<string>> WaitForShipmentsProcessedMessage(
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