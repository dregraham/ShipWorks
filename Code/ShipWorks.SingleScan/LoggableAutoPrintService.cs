using System;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Messaging.Messages.SingleScan;

namespace ShipWorks.SingleScan
{
    /// <summary>
    /// Log wrapper for AutoPrintService
    /// </summary>
    public class LoggableAutoPrintService : IAutoPrintService
    {
        private readonly IAutoPrintService autoPrintService;
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggableAutoPrintService"/> class.
        /// </summary>
        public LoggableAutoPrintService(IAutoPrintService autoPrintService, Func<Type, ILog> logFactory)
        {
            this.autoPrintService = autoPrintService;

            log = logFactory(typeof(AutoPrintService));
        }

        /// <summary>
        /// Determines if the auto print message should be sent
        /// </summary>
        public bool AllowAutoPrint(ScanMessage scanMessage)
        {
            return autoPrintService.AllowAutoPrint(scanMessage);
        }

        /// <summary>
        /// Handles the request for auto printing an order.
        /// </summary>
        public async Task<GenericResult<AutoPrintResult>> Print(AutoPrintServiceDto autoPrintServiceDto)
        {
            GenericResult<AutoPrintResult> result = await autoPrintService.Print(autoPrintServiceDto);

            if (result.Failure)
            {
                log.Error(result.Message);
            }

            return result;
        }
    }
}