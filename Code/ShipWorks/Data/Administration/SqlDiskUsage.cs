using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
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

        static List<EntityType> resourceList = new List<EntityType>
                    {
                        EntityType.ResourceEntity
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
        /// Indicates how much space the order/order item tables use
        /// </summary>
        public static long OrdersUsage => GetTableSpaceUsed(orderList);

        /// <summary>
        /// Indicates how much space the audit logs take
        /// </summary>
        public static long AuditUsage
        {
            get { return GetTableSpaceUsed(auditList); }
        }

        /// <summary>
        /// Indicates how much space ShipSense takes
        /// </summary>
        public static long ShipSenseUsage
        {
            get { return GetTableSpaceUsed(shipSenseList); }
        }

        /// <summary>
        /// Indicates the total space used by the database
        /// </summary>
        public static long TotalUsage
        {
            get { return GetDatabaseSpaceUsed(); }
        }

        /// <summary>
        /// Indicates the remaining space used by the database
        /// </summary>
        public static long OtherUsage
        {
            get { return TotalUsage - OrdersUsage - AuditUsage - GetResourceEmailData() - GetResourcePrintResultData() - ShipSenseUsage; }
        }

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
                    return (gbLimit * 1073741824L) - TotalUsage;
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
        private static long GetTableSpaceUsed(Lazy<List<EntityType>> entityList) =>
            GetTableSpaceUsed(entityList.Value);

        /// <summary>
        /// Get the space used by the given list of entities.
        /// </summary>
        private static long GetTableSpaceUsed(List<EntityType> entityList)
        {
            using (DbConnection con = SqlSession.Current.OpenConnection())
            {
                long total = 0;

                foreach (string table in entityList.Select(e => SqlAdapter.GetTableName(e)))
                {
                    DbCommand cmd = DbCommandProvider.Create(con);
                    cmd.CommandText = string.Format("EXEC sp_spaceused '{0}'", table);

                    using (DbDataReader reader = DbCommandProvider.ExecuteReader(cmd))
                    {
                        reader.Read();

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
        public static long GetResourceEmailData()
        {
            using (DbConnection con = SqlSession.Current.OpenConnection())
            {
                DbCommand cmd = DbCommandProvider.Create(con);
                cmd.CommandText = @"SELECT SUM(DATALENGTH(r.data)) as EmailDataInBytes
                                        FROM Resource r
                                        INNER JOIN ObjectReference o ON o.ObjectID = r.ResourceID
                                        INNER JOIN EmailOutbound e on e.PlainPartResourceID = o.ObjectReferenceID
                                            OR e.HtmlPartResourceID = o.ObjectReferenceID
                                        WHERE
                                            e.SendStatus = 1";

                using (DbDataReader reader = DbCommandProvider.ExecuteReader(cmd))
                {
                    reader.Read();
                    return Convert.ToInt64(reader["EmailDataInBytes"].ToString());
                }
            }
        }

        /// <summary>
        /// Get the amount of print result resource data in bytes
        /// </summary>
        /// <returns></returns>
        public static long GetResourcePrintResultData()
        {
            using (DbConnection con = SqlSession.Current.OpenConnection())
            {
                DbCommand cmd = DbCommandProvider.Create(con);
                cmd.CommandText = @"SELECT SUM(DATALENGTH(r.data)) as PrintResultDataInBytes
	                                    FROM Resource r
	                                    INNER JOIN ObjectReference o ON o.ObjectID = r.ResourceID
	                                    INNER JOIN PrintResult p on p.ContentResourceID = o.ObjectReferenceID";

                using (DbDataReader reader = DbCommandProvider.ExecuteReader(cmd))
                {
                    reader.Read();
                    return Convert.ToInt64(reader["PrintResultDataInBytes"].ToString());
                }
            }
        }
    }
}
