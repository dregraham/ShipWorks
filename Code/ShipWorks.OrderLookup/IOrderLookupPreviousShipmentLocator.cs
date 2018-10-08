using System.Threading.Tasks;

namespace ShipWorks.OrderLookup.ShipmentHistory
{
    /// <summary>
    /// Class to delegate tasks on previous shipments (reprint, void, etc)
    /// </summary>
    public interface IOrderLookupPreviousShipmentLocator
    {
        /// <summary>
        /// Get the last shipment ID to reprint.
        /// </summary>
        Task<PreviousProcessedShipmentDetails> GetLatestShipmentID();
    }
}