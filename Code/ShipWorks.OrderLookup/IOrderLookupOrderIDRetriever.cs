using System.Threading.Tasks;
using Interapptive.Shared.Utility;

namespace ShipWorks.OrderLookup
{
    public interface IOrderLookupOrderIDRetriever
    {
        Task<TelemetricResult<long?>> GetOrderID(string scannedText, string userInputTelemetryTimeSliceName, string dataLoadingTelemetryTimeSliceName, string orderCountTelemetryPropertyName);
    }
}