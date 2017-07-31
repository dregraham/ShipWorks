using System;
using System.Data;
using System.Threading.Tasks;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;

namespace ShipWorks.Stores.Content
{
    /// <summary>
    /// Implementation of IOrderUtility
    /// </summary>
    public class OrderUtilityWrapper : IOrderUtility
    {
        private readonly ISqlAdapterFactory sqlAdapterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sqlAdapterFactory"></param>
        public OrderUtilityWrapper(ISqlAdapterFactory sqlAdapterFactory)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
        }

        /// <summary>
        /// Gets the next OrderNumber that an order should use.  This is useful for store types that don't supply their own
        /// order numbers for ShipWorks, such as Amazon and eBay.
        /// </summary>
        public async Task<long> GetNextOrderNumberAsync(long storeID)
        {
            ParameterValue maxOrderNumberParam = new ParameterValue(ParameterDirection.InputOutput, dbType: DbType.Int64);
            await sqlAdapterFactory.Create().ExecuteSQLAsync(@"
                    ;with MaxOrderNumber as
                    (
	                    SELECT coalesce(max(OrderNumber), 0) as 'OrderNumber' FROM [Order] WHERE [StoreID] = @StoreID and IsManual = 0
	                    UNION ALL
	                    SELECT coalesce(max(OrderNumber), 0) as 'OrderNumber' FROM [OrderSearch] WHERE [StoreID] = @StoreID and IsManual = 0
                    )
                    SELECT @MaxOrderNumber = max(OrderNumber) from MaxOrderNumber;", 
                    new { MaxOrderNumber = maxOrderNumberParam, StoreID = storeID })
                .ConfigureAwait(false);

            return Convert.ToInt64(maxOrderNumberParam.Value) + 1;
        }
    }
}
