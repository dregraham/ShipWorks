using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content.CombinedOrderSearchProviders;

namespace ShipWorks.Stores.Platforms.NetworkSolutions.OnlineUpdating
{
    /// <summary>
    /// Combined order search provider for NetworkSolutions
    /// </summary>
    [Component]
    public class NetworkSolutionsCombineOrderSearchProvider : CombineOrderSearchBaseProvider<long>, INetworkSolutionsCombineOrderSearchProvider
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public NetworkSolutionsCombineOrderSearchProvider(ISqlAdapterFactory sqlAdapterFactory) :
            base(sqlAdapterFactory)
        {

        }

        /// <summary>
        /// Gets the online store's order identifier
        /// </summary>
        protected override async Task<IEnumerable<long>> GetCombinedOnlineOrderIdentifiers(IOrderEntity order)
        {
            return await GetCombinedOnlineOrderIdentifiers<NetworkSolutionsOrderSearchEntity>(
                NetworkSolutionsOrderSearchFields.OrderID == order.OrderID,
                NetworkSolutionsOrderSearchFields.NetworkSolutionsOrderID).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the NetworkSolutions online order identifier
        /// </summary>
        protected override long GetOnlineOrderIdentifier(IOrderEntity order) =>
            (order as INetworkSolutionsOrderEntity)?.NetworkSolutionsOrderID ?? 0;
    }
}
