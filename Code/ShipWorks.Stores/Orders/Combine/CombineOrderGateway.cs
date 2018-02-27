using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Data;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.Orders.Combine
{
    /// <summary>
    /// Order gateway for combining orders
    /// </summary>
    [Component]
    public class CombineOrderGateway : ICombineOrderGateway
    {
        private readonly Dictionary<StoreTypeCode, string> storeSpecificSearches =
            new Dictionary<StoreTypeCode, string>
            {
                { StoreTypeCode.Amazon, canCombineAmazonSql },
                { StoreTypeCode.Ebay, canCombineEbaySql },
                { StoreTypeCode.ChannelAdvisor, canCombineChannelAdvisorSql }
            };

        private readonly IOrderManager orderManager;
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly IStoreTypeManager storeTypeManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public CombineOrderGateway(IOrderManager orderManager, ISqlAdapterFactory sqlAdapterFactory, IStoreTypeManager storeTypeManager)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.storeTypeManager = storeTypeManager;
            this.orderManager = orderManager;
        }

        /// <summary>
        /// Load information needed for combining the orders
        /// </summary>
        /// <remarks>
        /// We should change the return type from IOrderEntity to an
        /// actual projection class, depending on our needs
        /// </remarks>
        public async Task<GenericResult<IEnumerable<IOrderEntity>>> LoadOrders(IEnumerable<long> orderIDs)
        {
            try
            {
                using (ISqlAdapter sqlAdapter = sqlAdapterFactory.Create())
                {
                    IEnumerable<IOrderEntity> orders = await orderManager.LoadOrdersAsync(orderIDs, sqlAdapter);

                    // Force the correct sort order.
                    IEnumerable<IOrderEntity> orderedByIDList = from i in orderIDs
                                                                join o in orders
                                                                  on i equals o.OrderID
                                                                select o;

                    return GenericResult.FromSuccess(orderedByIDList);
                }
            }
            catch (ORMException ex)
            {
                return GenericResult.FromError<IEnumerable<IOrderEntity>>(ex);
            }
        }

        /// <summary>
        /// Can the given orders be combined
        /// </summary>
        /// <remarks>
        /// This cannot be async because it is called from existing UI machinery that cannot be async
        /// </remarks>
        public bool CanCombine(IStoreEntity store, IEnumerable<long> orderIDs)
        {
            // The store can be null when deleting all stores
            if (store == null)
            {
                return false;
            }

            using (var conn = SqlSession.Current.OpenConnection())
            {
                using (var comm = conn.CreateCommand())
                {
                    comm.CommandText = CreateCanCombineQuery(store);
                    comm.AddParameterWithValue("@StoreID", store.StoreID);
                    comm.Parameters.Add(CreateOrderIDParameter(orderIDs));

                    var resultParameter = new SqlParameter("@result", SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };
                    comm.Parameters.Add(resultParameter);

                    comm.ExecuteNonQuery();

                    return (bool) resultParameter.Value;
                }
            }
        }

        /// <summary>
        /// Create a table parameter for the order ID list
        /// </summary>
        /// <param name="orderIDs">List of order IDs</param>
        private SqlParameter CreateOrderIDParameter(IEnumerable<long> orderIDs)
        {
            var table = new DataTable();
            table.Columns.Add("item", typeof(long));
            foreach (var value in orderIDs)
            {
                table.Rows.Add(value);
            }

            return new SqlParameter("@OrderIDs", SqlDbType.Structured)
            {
                TypeName = "LongList",
                Value = table
            };
        }

        /// <summary>
        /// Create the CanCombine query
        /// </summary>
        private string CreateCanCombineQuery(IStoreEntity store)
        {
            return canCombineSql + Environment.NewLine +
                CreateStoreSpecificQuery(store) + Environment.NewLine +
                canCombineSuccess;
        }

        /// <summary>
        /// Create the store specific query fragment
        /// </summary>
        private string CreateStoreSpecificQuery(IStoreEntity store)
        {
            // we can override store specific behavior here
            if (storeSpecificSearches.TryGetValue((StoreTypeCode) store.TypeCode, out string getStoreSpecificSearch))
            {
                return getStoreSpecificSearch;
            }

            // If it is a Generic Module based store type use the GenericModule behavior 
            if (storeTypeManager.IsStoreTypeCodeGenericModuleBased((StoreTypeCode) store.TypeCode))
            {
                return canCombineGenericModuleSql;
            }

            return string.Empty;
        }

        private static string canCombineSuccess = "SELECT @result = 1";

        private static string canCombineSql = @"
IF EXISTS(SELECT TOP 1 StoreID
	FROM [Order]
	WHERE OrderID IN (SELECT item FROM @OrderIDS) AND StoreID <> @StoreID)
BEGIN
	PRINT 'Too many stores'
	SELECT @result = 0
	RETURN
END

IF EXISTS(SELECT ShipmentID FROM Shipment WHERE Processed = 1 AND OrderID IN (SELECT item FROM @OrderIDS))
BEGIN
	PRINT 'Has processed shipments'
	SELECT @result = 0
	RETURN
END
";

        private static string canCombineAmazonSql = @"
IF EXISTS(SELECT OrderID 
	FROM AmazonOrder 
	WHERE OrderID IN (SELECT item FROM @OrderIDS) 
		AND (IsPrime IN (0, 1) OR FulfillmentChannel IN (0, 1)))
BEGIN
	PRINT 'Amazon details are bad'
	SELECT @result = 0
	RETURN
END";

        private static string canCombineEbaySql = @"
IF EXISTS(SELECT OrderID
	FROM EbayOrder
	WHERE OrderID IN (SELECT item FROM @OrderIDS) AND GspEligible = 1)
BEGIN
	PRINT 'Ebay details are bad'
	SELECT @result = 0
	RETURN
END

SELECT TOP 2 RollupEffectiveCheckoutStatus
	FROM EbayOrder
	WHERE OrderID IN (SELECT item FROM @OrderIDS)
	GROUP BY RollupEffectiveCheckoutStatus
IF @@ROWCOUNT > 1
BEGIN
	PRINT 'Ebay details are bad'
	SELECT @result = 0
	RETURN
END";

        private static string canCombineChannelAdvisorSql = @"
IF EXISTS(SELECT OrderID 
	FROM ChannelAdvisorOrder 
	WHERE OrderID IN (SELECT item FROM @OrderIDS) 
		AND IsPrime IN (0, 1))
BEGIN
	PRINT 'ChannelAdvisor details are bad'
	SELECT @result = 0
	RETURN
END

IF EXISTS (SELECT OrderID 
	FROM ChannelAdvisorOrderItem
		LEFT JOIN OrderItem
			ON ChannelAdvisorOrderItem.OrderItemID = OrderItem.OrderItemID
	WHERE OrderID IN (SELECT item FROM @OrderIDS) 
		AND IsFBA = 1)
BEGIN
	PRINT 'ChannelAdvisor details are bad'
	SELECT @result = 0
	RETURN
END";

        private static string canCombineGenericModuleSql = @"
IF EXISTS(SELECT OrderID 
	FROM GenericModuleOrder 
	WHERE OrderID IN (SELECT item FROM @OrderIDS) 
		AND (IsPrime IN (0, 1) OR IsFBA = 1))
BEGIN
	PRINT 'GenericModule details are bad'
	SELECT @result = 0
	RETURN
END";
    }
}