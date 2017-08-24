using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content.CombinedOrderSearchProviders;

namespace ShipWorks.Stores.Platforms.LemonStand
{
    /// <summary>
    /// Constructor
    /// </summary>
    [Component(RegistrationType.Self)]
    public class LemonStandCombineOrderIdSearchProvider : CombineOrderNumberCompleteSearchProvider
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public LemonStandCombineOrderIdSearchProvider(ISqlAdapterFactory sqlAdapterFactory) :
            base(sqlAdapterFactory)
        {

        }

        /// <summary>
        /// Gets the online store's order identifier
        /// </summary>
        /// <param name="order">The order for which to find combined order identifiers</param>
        protected override async Task<IEnumerable<string>> GetCombinedOnlineOrderIdentifiers(IOrderEntity order)
        {
            return await GetCombinedOnlineOrderIdentifiers<LemonStandOrderSearchEntity>(
                LemonStandOrderSearchFields.OrderID == order.OrderID,
                LemonStandOrderSearchFields.LemonStandOrderID).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the LemonStand online order identifier
        /// </summary>
        protected override string GetOnlineOrderIdentifier(IOrderEntity order)
        {
            return ((ILemonStandOrderEntity) order).LemonStandOrderID;
        }
    }
}
