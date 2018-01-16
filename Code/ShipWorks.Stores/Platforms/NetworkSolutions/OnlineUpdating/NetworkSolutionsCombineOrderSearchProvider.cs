using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Orders.Combine.SearchProviders;

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
            QueryFactory factory = new QueryFactory();

            var from = factory.NetworkSolutionsOrderSearch
                .LeftJoin(factory.OrderSearch)
                .On(NetworkSolutionsOrderSearchFields.OriginalOrderID == OrderSearchFields.OriginalOrderID);

            var query = factory.Create()
                .From(from)
                .Select(() => NetworkSolutionsOrderSearchFields.NetworkSolutionsOrderID.ToValue<long>())
                .Distinct()
                .Where(NetworkSolutionsOrderSearchFields.OrderID == order.OrderID)
                .AndWhere(OrderSearchFields.IsManual == false);

            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
            {
                return await sqlAdapter.FetchQueryAsync(query).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Gets the NetworkSolutions online order identifier
        /// </summary>
        protected override long GetOnlineOrderIdentifier(IOrderEntity order) =>
            (order as INetworkSolutionsOrderEntity)?.NetworkSolutionsOrderID ?? 0;
    }
}
