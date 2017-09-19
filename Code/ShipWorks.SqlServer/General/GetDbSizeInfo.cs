using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

/// <summary>
/// CLR stored procedures
/// </summary>
public partial class StoredProcedures
{
    /// <summary>
    /// Gets misc db size info
    /// </summary>
    [SqlProcedure]
    public static void GetDbSizeInfo(out SqlInt64 totalOrderCount, out SqlInt64 totalStanderdCombinedOrderCount,
        out SqlInt64 totalStanderdLegacyOrderCount, out SqlDouble orderTableSize, out SqlDouble combinedOrderSize)
    {
        // Attach to the connection
        using (SqlConnection con = new SqlConnection("Context connection = true"))
        {
            con.Open();

            totalOrderCount = 0;
            totalStanderdCombinedOrderCount = 0;
            totalStanderdLegacyOrderCount = 0;
            orderTableSize = 0;
            combinedOrderSize = 0;

            try
            {
                // Perform the count
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = Query;

                // See if there is a count to take
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        totalOrderCount = reader.GetInt64(0);
                        totalStanderdCombinedOrderCount = reader.GetInt64(1);
                        totalStanderdLegacyOrderCount = reader.GetInt64(2);
                        orderTableSize = reader.GetDouble(3);
                        combinedOrderSize = reader.GetDouble(4);
                    }
                }
            }
            catch
            {
            }
        }

        return;
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
		        @CombinedOrderSizeInMb AS 'CombinedOrderSizeInMb'
        ";
};
