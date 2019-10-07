using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Orders.Split.Hub
{
    /// <summary>
    /// Interface for auditing split orders
    /// </summary>
    public interface IOrderSplitAuditHub
    {
        /// <summary>
        /// Audit the original order.
        /// </summary>
        Task Audit(IOrderEntity originalOrder);
    }
}
