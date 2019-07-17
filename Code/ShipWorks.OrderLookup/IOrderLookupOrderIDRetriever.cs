using System.Threading.Tasks;
using Interapptive.Shared.Utility;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Represents a resource for getting an orderid
    /// </summary>
    public interface IOrderLookupOrderIDRetriever
    {
        /// <summary>
        /// Get an order id from the given scanned text
        /// </summary>
        Task<TelemetricResult<long?>> GetOrderID(string scannedText, string userInputTelemetryTimeSliceName, string dataLoadingTelemetryTimeSliceName, string orderCountTelemetryPropertyName);
    }
}