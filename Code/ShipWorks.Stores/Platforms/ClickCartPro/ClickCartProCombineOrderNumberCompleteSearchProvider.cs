using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Orders.Combine;
using ShipWorks.Stores.Orders.Combine.SearchProviders;

namespace ShipWorks.Stores.Platforms.ClickCartPro
{
    /// <summary>
    /// Combined order search provider for ClickCartProOrderID
    /// </summary>
    [KeyedComponent(typeof(ICombineOrderSearchProvider<string>), StoreTypeCode.ClickCartPro)]
    public class ClickCartProCombineOrderNumberCompleteSearchProvider : CombineOrderNumberCompleteSearchProvider
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ClickCartProCombineOrderNumberCompleteSearchProvider(ISqlAdapterFactory sqlAdapterFactory) :
            base(sqlAdapterFactory)
        {
        }

        /// <summary>
        /// Gets the online store's order identifier
        /// </summary>
        /// <param name="order">The order for which to find combined order identifiers</param>
        protected override async Task<IEnumerable<string>> GetCombinedOnlineOrderIdentifiers(IOrderEntity order)
        {
            QueryFactory factory = new QueryFactory();

            var from = factory.ClickCartProOrderSearch
                .LeftJoin(factory.OrderSearch)
                .On(ClickCartProOrderSearchFields.OriginalOrderID == OrderSearchFields.OriginalOrderID);

            var query = factory.Create()
                .From(from)
                .Select(() => ClickCartProOrderSearchFields.ClickCartProOrderID.ToValue<string>())
                .Distinct()
                .Where(ClickCartProOrderSearchFields.OrderID == order.OrderID)
                .AndWhere(OrderSearchFields.IsManual == false);

            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
            {
                return await sqlAdapter.FetchQueryAsync(query).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Gets the ClickCartPro online order identifier
        /// </summary>
        protected override string GetOnlineOrderIdentifier(IOrderEntity order)
        {
            return ((IClickCartProOrderEntity) order).ClickCartProOrderID;
        }
    }
}
