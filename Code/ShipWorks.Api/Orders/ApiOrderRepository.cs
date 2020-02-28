using System;
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
        private readonly ISqlSession sqlSession;

        /// <summary>
        /// Constructor
        /// </summary>
        public ApiOrderRepository(ISqlAdapterFactory sqlAdapterFactory, ISqlSession sqlSession)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.sqlSession = sqlSession;
        }

        /// <summary>
        /// Get orders with the given orderNumber or OrderID
        /// </summary>
        public IEnumerable<OrderEntity> GetOrders(string orderNumber)
        {
            if (!sqlSession.CanConnect())
            {
                throw new Exception("Unable to connect to ShipWorks database.");
            }

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
