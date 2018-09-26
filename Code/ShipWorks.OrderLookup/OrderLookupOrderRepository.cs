﻿using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Orders;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Order Repository for OrderLookup pipeline
    /// </summary>
    [Component]
    public class OrderLookupOrderRepository : IOrderLookupOrderRepository
    {
        private readonly IOrderRepository orderRepository;
        private readonly ISqlSession sqlSession;
        private readonly ISqlAdapterFactory sqlAdapterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupOrderRepository(
            IOrderRepository orderRepository, 
            ISqlSession sqlSession, 
            ISqlAdapterFactory sqlAdapterFactory
           )
        {
            this.orderRepository = orderRepository;
            this.sqlSession = sqlSession;
            this.sqlAdapterFactory = sqlAdapterFactory;
        }

        /// <summary>
        /// Get the OrderId matching the search text
        /// </summary>
        public long? GetOrderID(string searchText)
        {
            using (DbConnection conn = sqlSession.OpenConnection())
            {
                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new SqlParameter("searchText", searchText));

                    // If the searchtext is numeric add orderId tot he query
                    if (long.TryParse(searchText, out long orderId))
                    {
                        cmd.CommandText = "SELECT OrderID FROM [Order] WHERE OrderID = @searchOrderId OR OrderNumberComplete = @searchText";
                        cmd.Parameters.Add(new SqlParameter("searchOrderId", orderId));
                    }
                    else
                    {
                        cmd.CommandText = "SELECT OrderID FROM [Order] WHERE OrderNumberComplete = @searchText";
                    }
                    
                    return (long?) cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// Given scanned text, find the associated order
        /// </summary>
        public async Task<OrderEntity> GetOrder(long orderID)
        {
            using (ISqlAdapter adapter = sqlAdapterFactory.Create())
            {
                EntityQuery<OrderEntity> orderQuery = new QueryFactory().Order.Where(OrderFields.OrderID == orderID);
                return await adapter.FetchFirstAsync(orderQuery);
            }
        }
    }
}
