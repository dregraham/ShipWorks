using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Content.CombinedOrderSearchProviders
{
    /// <summary>
    /// Interface for combined order search providers
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public interface ICombineOrderSearchProvider<TResult>
    {
        /// <summary>
        /// Search for order identifier(s) based on combine/split status
        /// </summary>
        Task<IEnumerable<TResult>> GetOrderIdentifiers(IOrderEntity order);
    }
}
