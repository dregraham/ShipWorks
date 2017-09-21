using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Data.Connection
{
    /// <summary>
    /// Gets misc db size info
    /// </summary>
    public static class DbSizeInfo
    {
        /// <summary>
        /// Fetch misc db size info
        /// </summary>
        public static Dictionary<string, string> Fetch(SqlConnection connection)
        {
            Dictionary<string, string> results = new Dictionary<string, string>();
            
            try
            {
                // Perform the count
                SqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = Query;

                // See if there is a count to take
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        results.Add("Orders.Quantity.Total", reader.GetInt64(0).ToString());
                        results.Add("Orders.Quantity.Combined.Standard", reader.GetInt64(1).ToString());
                        results.Add("Orders.Quantity.Combined.Legacy", reader.GetInt64(2).ToString());
                        results.Add("Database.SizeInMB.Orders", reader.GetDouble(3).ToString());
                        results.Add("Database.SizeInMB.Orders.Combined", reader.GetDouble(4).ToString());
                        results.Add("Database.SizeInMB.Total", reader.GetDouble(5).ToString());
                    }
                }
            }
            catch
            {
            }

            return results;
        }

        private static readonly string Query = @"
        declare @TotalOrderCount bigint
        declare @TotalStandardCombinedOrderCount bigint
        declare @TotalLegacyCombinedOrderCount bigint
        declare @TotalDbSizeInMb float
        declare @OrderTableSizeInMb float
        declare @CombinedOrderSizeInMb float
        declare @AvgOrderSizeInMb float

        CREATE TABLE #DbSizeInfo (TableName NVARCHAR(128),rows CHAR(11),      
               reserved VARCHAR(18),data VARCHAR(18),index_size VARCHAR(18), 
               unused VARCHAR(18))
	   
        EXEC sp_MSForEachTable 'INSERT INTO #DbSizeInfo EXEC sp_spaceused ''?'' '

        SELECT  @TotalOrderCount  = CONVERT(bigint, [rows])
        FROM    #DbSizeInfo 
        WHERE   TableName = '[dbo].[Order]' or TableName = 'Order' or TableName = '[Order]'

        SELECT  @OrderTableSizeInMb = sum(CONVERT(bigint,left(reserved,len([reserved])-3))) * 1.0 / 1024.0
        FROM    #DbSizeInfo 
        WHERE   TableName LIKE '%Order%'
	        and TableName not like '%Filter%' and TableName not like '%OrderSearch%'

        SELECT @AvgOrderSizeInMb = @OrderTableSizeInMb * 1.0 / @TotalOrderCount

        SELECT  @TotalDbSizeInMb = sum(CONVERT(float,left(reserved,len([reserved])-3))) / 1024.0
        FROM    #DbSizeInfo 

        SELECT  @TotalStandardCombinedOrderCount = count(OrderID)
        FROM [Order]
        WHERE CombineSplitStatus = 1

        SELECT  @TotalLegacyCombinedOrderCount = count(OrderID)
        FROM [EbayOrder]
        WHERE CombinedLocally = 1

        SELECT @OrderTableSizeInMb = @TotalOrderCount * 1.0 * @AvgOrderSizeInMb

        SELECT @CombinedOrderSizeInMb = (@TotalStandardCombinedOrderCount + @TotalLegacyCombinedOrderCount ) * @AvgOrderSizeInMb

        DROP TABLE #DbSizeInfo

        SELECT  @TotalOrderCount AS 'NumberOfOrders', 
		        @TotalStandardCombinedOrderCount as 'TotalStandardCombinedOrderCount',
		        @TotalLegacyCombinedOrderCount as 'TotalLegacyCombinedOrderCount',
		        @OrderTableSizeInMb AS 'OrderTableSizeInMb',
		        @CombinedOrderSizeInMb AS 'CombinedOrderSizeInMb',
				@TotalDbSizeInMb AS 'TotalDatabaseSizeInMb'
        ";
    }
}
