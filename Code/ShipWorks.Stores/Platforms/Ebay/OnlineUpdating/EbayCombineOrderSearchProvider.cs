using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content.CombinedOrderSearchProviders;

namespace ShipWorks.Stores.Platforms.Ebay
{
    /// <summary>
    /// Constructor
    /// </summary>
    [KeyedComponent(typeof(ICombineOrderSearchProvider<EbayOrderSearchEntity>), StoreTypeCode.Ebay)]
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
            return await GetCombinedOnlineOrderIdentifiers<EbayOrderSearchEntity>(EbayOrderSearchFields.OrderID == order.OrderID)
                .ConfigureAwait(false);
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
