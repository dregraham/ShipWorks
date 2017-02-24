using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages.SingleScan;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Log wrapper for AutoPrintService
    /// </summary>
    /// <seealso cref="ShipWorks.SingleScan.AutoPrintService" />
    [KeyedComponent(typeof(IAutoPrintService), AutoPrintServiceType.Loggable)]
    public class LoggableAutoPrintService : IAutoPrintService
    {
        private readonly IAutoPrintService autoPrintService;
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggableAutoPrintService"/> class.
        /// </summary>
        public LoggableAutoPrintService(IIndex<AutoPrintServiceType, IAutoPrintService> autoPrintServiceFactory, Func<Type, ILog> logFactory)
        {
            this.autoPrintService = autoPrintServiceFactory[AutoPrintServiceType.Default];
            ScanMessages = autoPrintService.ScanMessages;
            Messenger = autoPrintService.Messenger;
            SchedulerProvider = autoPrintService.SchedulerProvider;
            log = logFactory(typeof(AutoPrintService));
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
        /// Disconnect the scan messages observable
        /// </summary>
        public void EndScanMessagesObservation()
        {
            log.Info("Ending scan message observation.");
            autoPrintService.EndScanMessagesObservation();
        }

        /// <summary>
        /// Connect to the scan messages observable
        /// </summary>
        public void StartScanMessagesObservation()
        {
            log.Info("Starting scan message observation.");
            autoPrintService.StartScanMessagesObservation();
        }

        /// <summary>
        /// Determines if the auto print message should be sent
        /// </summary>
        public bool AllowAutoPrint(ScanMessage scanMessage)
        {
            return autoPrintService.AllowAutoPrint(scanMessage);
        }

        /// <summary>
        /// Logs the exception and reconnect pipeline.
        /// </summary>
        public void HandleException(Exception ex)
        {
            log.Error("Error occurred while attempting to auto print.", ex);
            autoPrintService.HandleException(ex);
        }

        /// <summary>
        /// Handles the request for auto printing an order.
        /// </summary>
        public async Task<GenericResult<string>> HandleAutoPrintShipment(AutoPrintServiceDto autoPrintServiceDto)
        {
            GenericResult<string> result = await autoPrintService.HandleAutoPrintShipment(autoPrintServiceDto);

            if (result.Failure)
            {
                log.Error(result.Message);
            }

            return result;
        }

        /// <summary>
        /// Waits for shipments processed message.
        /// </summary>
        public IObservable<GenericResult<string>> WaitForShipmentsProcessedMessage(GenericResult<string> genericResult)
        {
            IObservable<GenericResult<string>> returnResult = autoPrintService.WaitForShipmentsProcessedMessage(genericResult);

            log.Info(genericResult.Failure ?
                "No Shipments, not waiting for ShipmentsProcessMessageScan" :
                $"ShipmentsProcessedMessage received from scan {genericResult.Value}");

            return returnResult;
        }
    }
}