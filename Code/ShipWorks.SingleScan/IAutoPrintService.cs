using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Interapptive.Shared.Threading;
using Interapptive.Shared.Utility;
using ShipWorks.Core.Messaging;
using ShipWorks.Messaging.Messages.SingleScan;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Handles auto printing
    /// </summary>
    public interface IAutoPrintService
    {
        /// <summary>
        /// Scan messages received by the auto print service
        /// </summary>
        IConnectableObservable<ScanMessage> ScanMessages { get; set; }

        /// <summary>
        /// The messenger.
        /// </summary>
        IMessenger Messenger { get; set; }

        /// <summary>
        /// The scheduler provider.
        /// </summary>
        ISchedulerProvider SchedulerProvider { get; set; }

        /// <summary>
        /// Disconnect the scan messages observable
        /// </summary>
        void EndScanMessagesObservation();

        /// <summary>
        /// Connect to the scan messages observable
        /// </summary>
        void StartScanMessagesObservation();

        /// <summary>
        /// Determines if the auto print message should be sent
        /// </summary>
        bool AllowAutoPrint(ScanMessage scanMessage);

        /// <summary>
        /// Handles the request for auto printing an order.
        /// </summary>
        Task<GenericResult<string>> HandleAutoPrintShipment(AutoPrintServiceDto autoPrintServiceDto);

        /// <summary>
        /// Logs the exception and reconnect pipeline.
        /// </summary>
        void HandleException(Exception ex);

        /// <summary>
        /// Waits for shipments processed message.
        /// </summary>
        IObservable<GenericResult<string>> WaitForShipmentsProcessedMessage(GenericResult<string> genericResult);
    }
}