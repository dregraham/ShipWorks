using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Content
{
    /// <summary>
    /// Interface for auditing combined orders
    /// </summary>
    public interface ICombineOrderAudit
    {
        /// <summary>
        /// Audit the combined order.
        /// </summary>
        Task Audit(long survivingOrderID, IEnumerable<IOrderEntity> orders);
    }
}
