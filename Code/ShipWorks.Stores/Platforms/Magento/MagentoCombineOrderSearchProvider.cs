using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Orders.Combine;
using ShipWorks.Stores.Orders.Combine.SearchProviders;

namespace ShipWorks.Stores.Platforms.Magento
{
    /// <summary>
    /// Constructor
    /// </summary>
    [KeyedComponent(typeof(ICombineOrderSearchProvider<MagentoOrderSearchEntity>), StoreTypeCode.Magento)]
    public class MagentoCombineOrderSearchProvider : CombineOrderSearchBaseProvider<MagentoOrderSearchEntity>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MagentoCombineOrderSearchProvider(ISqlAdapterFactory sqlAdapterFactory) : base(sqlAdapterFactory)
        {
        }

        /// <summary>
        /// Gets the online store's order identifier
        /// </summary>
        /// <param name="order">The order for which to find combined order identifiers</param>
        protected override async Task<IEnumerable<MagentoOrderSearchEntity>> GetCombinedOnlineOrderIdentifiers(IOrderEntity order)
        {
            QueryFactory factory = new QueryFactory();

            var from = factory.MagentoOrderSearch
                .LeftJoin(factory.OrderSearch)
                .On(MagentoOrderSearchFields.OriginalOrderID == OrderSearchFields.OriginalOrderID);

            var query = factory.Create()
                .From(from)
                .Select(() => new MagentoOrderSearchEntity()
                {
                    MagentoOrderID = MagentoOrderSearchFields.MagentoOrderID.ToValue<long>(),
                    OrderID = MagentoOrderSearchFields.OrderID.As("MOSFOrderID").ToValue<long>(),
                    OriginalOrderID = MagentoOrderSearchFields.OriginalOrderID.ToValue<long>(),
                })
                .Distinct()
                .Where(MagentoOrderSearchFields.OrderID == order.OrderID)
                .AndWhere(OrderSearchFields.IsManual == false);

            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
            {
                IEnumerable<MagentoOrderSearchEntity> results = await sqlAdapter.FetchQueryAsync(query).ConfigureAwait(false);
                return results
                    .Distinct(new MagentoCombineOrderSearchProviderComparer())
                    .OrderBy(o => o.MagentoOrderID);
            }
        }

        /// <summary>
        /// Gets the Magento online order identifier
        /// </summary>
        protected override MagentoOrderSearchEntity GetOnlineOrderIdentifier(IOrderEntity order)
        {
            IMagentoOrderEntity magentoOrder = (IMagentoOrderEntity) order;
            return new MagentoOrderSearchEntity
            {
                OrderID = order.OrderID,
                OriginalOrderID = magentoOrder.OrderID,
                MagentoOrderID = magentoOrder.MagentoOrderID
            };
        }
    }
}
