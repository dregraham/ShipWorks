using System.Threading.Tasks;

namespace ShipWorks.OrderLookup.ShipmentHistory
{
    /// <summary>
    /// Locate the last processed shipment for order lookup
    /// </summary>
    public interface IOrderLookupPreviousShipmentLocator
    {
        /// <summary>
        /// Get details of the last processed shipment
        /// </summary>
        Task<PreviousProcessedShipmentDetails> GetLatestShipmentDetails();
    }
}