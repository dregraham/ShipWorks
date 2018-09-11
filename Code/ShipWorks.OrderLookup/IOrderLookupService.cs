using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Represents a service for finding orders based on scan message text
    /// </summary>
    public interface IOrderLookupService
    {
        /// <summary>
        /// Find an order based on scanned message text
        /// </summary>
        Task<OrderEntity> FindOrder(string scanMsgScannedText);
    }
}