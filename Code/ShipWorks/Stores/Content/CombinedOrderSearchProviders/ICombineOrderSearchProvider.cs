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
        ///// <summary>
        ///// Search for combined order identifiers from the database
        ///// </summary>
        //Task<IEnumerable<TResult>> GetCombinedOnlineOrderIdentifiers(IOrderEntity order);

        /// <summary>
        /// Search for order identifier(s) based on combine/split status
        /// </summary>
        Task<IEnumerable<TResult>> GetOrderIdentifiers(IOrderEntity order);
        
        ///// <summary>
        ///// Get the order's non-combined order identifier
        ///// </summary>
        //TResult GetOnlineOrderIdentifier(IOrderEntity order);
    }
}
