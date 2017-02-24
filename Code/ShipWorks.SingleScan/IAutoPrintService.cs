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
        /// Determines if the auto print message should be sent
        /// </summary>
        bool AllowAutoPrint(ScanMessage scanMessage);

        /// <summary>
        /// Handles the request for auto printing an order.
        /// </summary>
        Task<GenericResult<string>> Print(AutoPrintServiceDto autoPrintServiceDto);
    }
}