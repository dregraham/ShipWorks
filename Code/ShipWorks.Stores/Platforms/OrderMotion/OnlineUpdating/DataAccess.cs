using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.OrderMotion.OnlineUpdating
{
    /// <summary>
    /// Data access for uploading order details
    /// </summary>
    [Component]
    public class DataAccess : IDataAccess
    {
        private readonly IDataProvider dataProvider;
        private readonly IOrderMotionCombineOrderSearchProvider searchProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public DataAccess(IDataProvider dataProvider, IOrderMotionCombineOrderSearchProvider searchProvider)
        {
            this.searchProvider = searchProvider;
            this.dataProvider = dataProvider;
        }

        /// <summary>
        /// Get order details for uploading
        /// </summary>
        public async Task<IEnumerable<OrderDetail>> GetOrderDetails(long orderID)
        {
            var order = await dataProvider.GetEntityAsync<OrderMotionOrderEntity>(orderID).ConfigureAwait(false);
            return await searchProvider.GetOrderIdentifiers(order).ConfigureAwait(false);
        }
    }
}
