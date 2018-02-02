using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Orders.Combine
{
    /// <summary>
    /// Interface for combined order search providers
    /// </summary>
    public interface ICombineOrderSearchProvider<TResult>
    {
        /// <summary>
        /// Search for order identifier(s) based on combine/split status
        /// </summary>
        Task<IEnumerable<TResult>> GetOrderIdentifiers(IOrderEntity order);
    }
}
