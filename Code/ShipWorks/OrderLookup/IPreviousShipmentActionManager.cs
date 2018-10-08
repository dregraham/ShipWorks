using System;
using System.Threading.Tasks;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Interface to delegate tasks on previous shipments (reprint, void, etc)
    /// </summary>
    public interface IPreviousShipmentActionManager : IDisposable
    {
        /// <summary>
        /// Reprint the last shipment
        /// </summary>
        Task ReprintLastShipment();
    }
}