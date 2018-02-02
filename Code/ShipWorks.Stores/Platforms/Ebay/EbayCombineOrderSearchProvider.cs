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
using ShipWorks.Stores.Orders.Combine.SearchProviders;

namespace ShipWorks.Stores.Platforms.Ebay
{
    /// <summary>
    /// Constructor
    /// </summary>
    [Component(RegistrationType.Self)]
    public class EbayCombineOrderSearchProvider : CombineOrderSearchBaseProvider<EbayOrderSearchEntity>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EbayCombineOrderSearchProvider(ISqlAdapterFactory sqlAdapterFactory) : base(sqlAdapterFactory)
        {
        }

        /// <summary>
        /// Gets the online store's order identifier
        /// </summary>
        /// <param name="order">The order for which to find combined order identifiers</param>
        protected override async Task<IEnumerable<EbayOrderSearchEntity>> GetCombinedOnlineOrderIdentifiers(IOrderEntity order)
        {
            QueryFactory factory = new QueryFactory();

            var from = factory.EbayOrderSearch
                .LeftJoin(factory.OrderSearch)
                .On(EbayOrderSearchFields.OriginalOrderID == OrderSearchFields.OriginalOrderID);

            var query = factory.Create()
                .From(from)
                .Select(() => new EbayOrderSearchEntity()
                    {
                        EbayOrderID = EbayOrderSearchFields.EbayOrderID.ToValue<long>(),
                        OrderID = EbayOrderSearchFields.OrderID.As("EOSFOrderID").ToValue<long>(),
                        OriginalOrderID = EbayOrderSearchFields.OriginalOrderID.ToValue<long>(),
                        EbayBuyerID = EbayOrderSearchFields.EbayBuyerID.ToValue<string>(),
                        SellingManagerRecord = EbayOrderSearchFields.SellingManagerRecord.ToValue<int>()
                    })
                .Distinct()
                .Where(EbayOrderSearchFields.OrderID == order.OrderID)
                .AndWhere(OrderSearchFields.IsManual == false);

            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
            {
                IEnumerable<EbayOrderSearchEntity> results = await sqlAdapter.FetchQueryAsync(query).ConfigureAwait(false);
                return results.Distinct(new EbayOrderSearchEntityComparer())
                    .OrderBy(o => o.EbayOrderID);
            }
        }

        /// <summary>
        /// Gets the Ebay online order identifier
        /// </summary>
        protected override EbayOrderSearchEntity GetOnlineOrderIdentifier(IOrderEntity order)
        {
            IEbayOrderEntity ebayOrder = (IEbayOrderEntity) order;
            return new EbayOrderSearchEntity
            {
                OrderID = order.OrderID,
                OriginalOrderID = ebayOrder.OrderID,
                EbayOrderID = ebayOrder.EbayOrderID,
                EbayBuyerID = ebayOrder.EbayBuyerID,
                SellingManagerRecord = ebayOrder.SellingManagerRecord
            };
        }
    }
}
