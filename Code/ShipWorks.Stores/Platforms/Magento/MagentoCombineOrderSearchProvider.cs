using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content.CombinedOrderSearchProviders;

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
            return await GetCombinedOnlineOrderIdentifiers<MagentoOrderSearchEntity>(MagentoOrderSearchFields.OrderID == order.OrderID)
                .ConfigureAwait(false);
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
