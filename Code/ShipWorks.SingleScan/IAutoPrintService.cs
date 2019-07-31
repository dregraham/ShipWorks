using System.Threading.Tasks;
using Interapptive.Shared.Utility;

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
        bool AllowAutoPrint(string scanText);

        /// <summary>
        /// Handles the request for auto printing an order.
        /// </summary>
        Task<GenericResult<AutoPrintResult>> Print(AutoPrintServiceDto autoPrintServiceDto);
    }
}