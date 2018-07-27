using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Interapptive.Shared.Data;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Utility class for determining disk usages for the current database
    /// </summary>
    public static class SqlDiskUsage
    {
        static List<EntityType> auditList = new List<EntityType>
                    {
                        EntityType.AuditEntity,
                        EntityType.AuditChangeEntity,
                        EntityType.AuditChangeDetailEntity
                    };

        static List<EntityType> downloadList = new List<EntityType>
                    {
                        EntityType.DownloadDetailEntity,
                        EntityType.DownloadEntity
                    };

        // We're getting the list of order related tables dynamically so that we don't have to remember to add
        // to the list when we add new store types
        static Lazy<List<EntityType>> orderList = new Lazy<List<EntityType>>(() =>
            Enum.GetValues(typeof(EntityType))
                .OfType<EntityType>()
                .Where(IsOrderType)
                .ToList());

        private static List<EntityType> shipSenseList = new List<EntityType> { EntityType.ShipSenseKnowledgebaseEntity };
        
        /// <summary>
        /// Indicates the amount of space remaining in the database.  -1 if there is no limit.
        /// </summary>
        public static long SpaceRemaining
        {
            get
            {
                int gbLimit = SizeLimitGB;

                if (gbLimit > 0)
                {
                    return (gbLimit * 1073741824L) - GetDatabaseSpaceUsed();
                }
                else
                {
                    return -1;
                }
            }
        }

        /// <summary>
        /// Gets the number of GB that the database instance is limited to.  -1 if there is no limit.
        /// </summary>
        public static int SizeLimitGB
        {
            get
            {
                if (IsExpress)
                {
                    // 10.5 is R2 which increased the limit to 10GB
                    if (SqlSession.Current.GetServerVersion() >= new Version(10, 5))
                    {
                        return 10;
                    }
                    else
                    {
                        return 4;
                    }
                }
                else
                {
                    return -1;
                }
            }
        }

        /// <summary>
        /// Is the entity type associated with orders
        /// </summary>
        private static bool IsOrderType(EntityType entityType)
        {
            var entityName = entityType.ToString();

            return entityName.EndsWith(EntityType.OrderEntity.ToString()) ||
                entityName.EndsWith(EntityType.OrderItemEntity.ToString()) ||
                entityName.EndsWith(EntityType.OrderSearchEntity.ToString()) ||
                entityType == EntityType.OrderItemAttributeEntity ||
                entityType == EntityType.OrderChargeEntity ||
                entityType == EntityType.OrderPaymentDetailEntity ||
                entityType == EntityType.EbayCombinedOrderRelationEntity;
        }

        /// <summary>
        /// Indicates if the running version of SQL server is the express version
        /// </summary>
        private static bool IsExpress
        {
            get
            {
                using (DbConnection con = SqlSession.Current.OpenConnection())
                {
                    DbCommand cmd = DbCommandProvider.Create(con);
                    cmd.CommandText = "SELECT SERVERPROPERTY ('edition')";

                    return DbCommandProvider.ExecuteScalar(cmd).ToString().Contains("Express");
                }
            }
        }

        /// <summary>
        /// Indicates the remaining space used by the database
        /// </summary>
        public async static Task<long> GetOtherUsage()
        {
            return GetDatabaseSpaceUsed()
                - await GetOrdersUsage() 
                - await GetAuditUsage() 
                - await GetResourceEmailData() 
                - await GetResourcePrintResultData() 
                - await GetResourceLabelData() 
                - await GetShipSenseUsage() 
                - await GetDownloadUsage();
        }

        /// <summary>
        /// Indicates how much space the order/order item tables use
        /// </summary>
        public async static Task<long> GetOrdersUsage() => 
            await GetTableSpaceUsed(orderList);

        /// <summary>
        /// Indicates how much space the download download details tables use
        /// </summary>
        public async static Task<long> GetDownloadUsage() => 
            await GetTableSpaceUsed(downloadList);

        /// <summary>
        /// Indicates how much space the audit logs take
        /// </summary>
        public async static Task<long> GetAuditUsage() =>
            await GetTableSpaceUsed(auditList);

        /// <summary>
        /// Indicates how much space ShipSense takes
        /// </summary>
        public async static Task<long> GetShipSenseUsage() =>
            await GetTableSpaceUsed(shipSenseList); 

        /// <summary>
        /// Get the total space used by the database
        /// </summary>
        public static long GetDatabaseSpaceUsed(string databaseName = "")
        {
            if (databaseName.IsNullOrWhiteSpace())
            {
                databaseName = SqlSession.Current.Configuration.DatabaseName;
            }

            using (DbConnection con = SqlSession.Current.OpenConnection())
            {
                string sql = $@"IF OBJECT_ID('tempdb..#RowCountsAndSizes') IS NOT NULL
                                DROP TABLE #RowCountsAndSizes

                              CREATE TABLE #RowCountsAndSizes (TableName NVARCHAR(128), [rows] CHAR(11), [reserved] VARCHAR(18), [data] VARCHAR(18), [index_size] VARCHAR(18), [unused] VARCHAR(18))
                              EXEC       sp_MSForEachTable 'INSERT INTO #RowCountsAndSizes EXEC {databaseName}..sp_spaceused ''?'' '
                              select sum(CONVERT(bigint,left(reserved,len(reserved)-3))) from #RowCountsAndSizes";

                long size = (long) DbCommandProvider.ExecuteScalar(con, sql);

                return size * 1024L;
            }
        }

        /// <summary>
        /// Get the space used by the given list of entities.
        /// </summary>
        private async static Task<long> GetTableSpaceUsed(Lazy<List<EntityType>> entityList) =>
            await GetTableSpaceUsed(entityList.Value);

        /// <summary>
        /// Get the space used by the given list of entities.
        /// </summary>
        private async static Task<long> GetTableSpaceUsed(List<EntityType> entityList)
        {
            using (DbConnection con = SqlSession.Current.OpenConnection())
            {
                long total = 0;

                foreach (string table in entityList.Select(e => SqlAdapter.GetTableName(e)))
                {
                    DbCommand cmd = DbCommandProvider.Create(con);
                    cmd.CommandText = string.Format("EXEC sp_spaceused '{0}'", table);

                    using (DbDataReader reader = await DbCommandProvider.ExecuteReaderAsync(cmd))
                    {
                        await reader.ReadAsync();

                        total += 1024L * Convert.ToInt64(reader["reserved"].ToString().Split(' ')[0]);
                    }
                }

                return total;
            }
        }

        /// <summary>
        /// Get the amount of email resource data in bytes
        /// </summary>
        /// <returns></returns>
        public async static Task<long> GetResourceEmailData()
        {
            using (DbConnection con = SqlSession.Current.OpenConnection())
            {
                DbCommand cmd = DbCommandProvider.Create(con);
                cmd.CommandText = @"SELECT SUM(DATALENGTH(r.data)) as EmailDataInBytes
                                        FROM Resource r
                                        INNER JOIN ObjectReference o ON o.ObjectID = r.ResourceID
                                        INNER JOIN EmailOutbound e on e.PlainPartResourceID = o.ObjectReferenceID
                                            OR e.HtmlPartResourceID = o.ObjectReferenceID";

                using (DbDataReader reader = await DbCommandProvider.ExecuteReaderAsync(cmd))
                {
                    await reader.ReadAsync();
                    return Convert.ToInt64(reader["EmailDataInBytes"].ToString());
                }
            }
        }

        /// <summary>
        /// Get the amount of print result resource data in bytes
        /// </summary>
        /// <returns></returns>
        public async static Task<long> GetResourcePrintResultData()
        {
            using (DbConnection con = SqlSession.Current.OpenConnection())
            {
                DbCommand cmd = DbCommandProvider.Create(con);
                cmd.CommandText = @"SELECT SUM(DATALENGTH(r.data)) as PrintResultDataInBytes
	                                    FROM Resource r
	                                    INNER JOIN ObjectReference o ON o.ObjectID = r.ResourceID
	                                    INNER JOIN PrintResult p on p.ContentResourceID = o.ObjectReferenceID";

                using (DbDataReader reader = await DbCommandProvider.ExecuteReaderAsync(cmd))
                {
                    await reader.ReadAsync();
                    return Convert.ToInt64(reader["PrintResultDataInBytes"].ToString());
                }
            }
        }

        /// <summary>
        /// Get the amount of label resource data in bytes
        /// </summary>
        public async static Task<long> GetResourceLabelData()
        {
            long result = 0;
            using (DbConnection con = SqlSession.Current.OpenConnection())
            {
                string baseQuery = @"SELECT SUM(DATALENGTH(r.data)) as LabelData
                                        FROM Resource r
                                        INNER JOIN ObjectReference o ON o.ObjectID = r.ResourceID";
                
                result += await GetLabelDataSize(con, $"{baseQuery} INNER JOIN UpsPackage up ON up.UpsPackageID = o.ConsumerID");
                result += await GetLabelDataSize(con, $"{baseQuery} INNER JOIN FedExPackage fp ON fp.FedExPackageID = o.ConsumerID");
                result += await GetLabelDataSize(con, $"{baseQuery} INNER JOIN iParcelPackage ip ON ip.iParcelPackageID = o.ConsumerID");
                result += await GetLabelDataSize(con, $"{baseQuery} INNER JOIN DhlExpressPackage dp ON dp.DhlExpressPackageID = o.ConsumerID");
                result += await GetLabelDataSize(con, $"{baseQuery} INNER JOIN Shipment s ON s.ShipmentID = o.ConsumerID");
            }
            return result;
        }

        /// <summary>
        /// Get the label data size from the given query
        /// </summary>
        private async static Task<long> GetLabelDataSize(DbConnection con, string query)
        {
            DbCommand cmd = DbCommandProvider.Create(con);
            cmd.CommandText = query;

            using (DbDataReader reader = await DbCommandProvider.ExecuteReaderAsync(cmd))
            {
                await reader.ReadAsync();
                if (long.TryParse(reader["LabelData"].ToString(), out long size))
                {
                    return size;
                }
            }
            return 0;
        }
    }
}
