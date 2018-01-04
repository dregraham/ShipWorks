using System.Threading.Tasks;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Orders.Split
{
    /// <summary>
    /// Interface for auditing split orders
    /// </summary>
    public interface IOrderSplitAudit
    {
        /// <summary>
        /// Audit the original order and split order.
        /// </summary>
        Task Audit(OrderEntity originalOrder, OrderEntity splitOrder);
    }
}
