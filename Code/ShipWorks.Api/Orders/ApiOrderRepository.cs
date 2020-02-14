using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.ComponentRegistration;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Api.Orders
{
    /// <summary>
    /// Repository for retrieving orders for the API
    /// </summary>
    [Component]
    public class ApiOrderRepository : IApiOrderRepository
    {
        private readonly ISqlAdapterFactory sqlAdapterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public ApiOrderRepository(ISqlAdapterFactory sqlAdapterFactory)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
        }

        /// <summary>
        /// Get orders with the given orderNumber or OrderID
        /// </summary>
        public IEnumerable<OrderEntity> GetOrders(string orderNumber)
        {
            using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
            {
                EntityQuery<OrderEntity> query = new QueryFactory().Order;

                query = query.Where(OrderFields.OrderNumberComplete == orderNumber.Trim());

                if (long.TryParse(orderNumber, out long numericOrderNumber))
                {
                    query = query.OrWhere(OrderFields.OrderID == numericOrderNumber);
                }

                EntityCollection<OrderEntity> orderCollection = new EntityCollection<OrderEntity>();
                sqlAdapter.FetchQuery(query, orderCollection);

                return orderCollection;
            }
        }
    }
}
