﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Order Repository for OrderLookup pipeline
    /// </summary>
    [Component]
    public class OrderLookupOrderRepository : IOrderLookupOrderRepository
    {
        private readonly Func<ISqlSession> sqlSession;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly IOrderLoader orderLoader;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLookupOrderRepository(
            Func<ISqlSession> sqlSession,
            ISqlAdapterFactory sqlAdapterFactory,
            IOrderLoader orderLoader)
        {
            this.sqlSession = sqlSession;
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.orderLoader = orderLoader;
        }

        /// <summary>
        /// Get the OrderId matching the order number
        /// </summary>
        public List<long> GetOrderIDs(string orderNumberComplete)
        {
            using (DbConnection conn = sqlSession().OpenConnection())
            {
                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new SqlParameter("orderNumberComplete", orderNumberComplete));

                    string sql = "SELECT OrderID FROM [Order] WHERE OrderNumberComplete = @orderNumberComplete";
                    if(long.TryParse(orderNumberComplete, out long orderNumber))
                    {
                        cmd.Parameters.Add(new SqlParameter("orderNumber", orderNumber));
                        sql += " OR OrderNumber = @orderNumber";
                    }

                    cmd.CommandText = sql;
                    return GetOrders(cmd);
                }
            }
        }

        /// <summary>
        /// Returns all order ids that match
        /// </summary>
        private List<long> GetOrders(DbCommand cmd)
        {
            List<long> orderIds = new List<long>();
            using (DbDataReader reader = cmd.ExecuteReader())
            {
                while(reader.Read())
                {
                    orderIds.Add(reader.GetInt64(0));
                }
            }

            return orderIds;
        }

        /// <summary>
        /// Given scanned text, find the associated order
        /// </summary>
        public async Task<OrderEntity> GetOrder(long orderID)
        {
            ShipmentsLoadedEventArgs result = await orderLoader.LoadAsync(new[] { orderID }, ProgressDisplayOptions.Delay, true, TimeSpan.FromMilliseconds(1000)).ConfigureAwait(true);

            if (result.Shipments.Any())
            {
                return result.Shipments.Last().Order;
            }

            // The OrderId returned no shipments, this could happen if the user has no
            // rights to create a shipment for the OrderID or if the OrderID does not exist in the database
            return null;
        }
    }
}
