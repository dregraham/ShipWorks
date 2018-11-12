using System.Threading.Tasks;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Interface to reprint last shipment
    /// </summary>
    public interface IPreviousShipmentReprintActionHandler
    {
        /// <summary>
        /// Reprint the last shipment
        /// </summary>
        Task ReprintLastShipment();
    }
}