using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Interapptive.Shared.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Utility class for determining disk usages for the current datababase
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

        static List<EntityType> orderList = new List<EntityType>
                    {
                        EntityType.AmazonOrderEntity,
                        EntityType.AmazonOrderItemEntity,
                        EntityType.BigCommerceOrderItemEntity,
                        EntityType.BuyDotComOrderItemEntity,
                        EntityType.ChannelAdvisorOrderEntity,
                        EntityType.ChannelAdvisorOrderItemEntity,
                        EntityType.ClickCartProOrderEntity,
                        EntityType.CommerceInterfaceOrderEntity,
                        EntityType.EbayOrderEntity,
                        EntityType.EbayOrderItemEntity,
                        EntityType.EtsyOrderEntity,
                        EntityType.InfopiaOrderItemEntity,
                        EntityType.MagentoOrderEntity,
                        EntityType.MarketplaceAdvisorOrderEntity,
                        EntityType.MivaOrderItemAttributeEntity,
                        EntityType.NetworkSolutionsOrderEntity,
                        EntityType.NeweggOrderEntity,
                        EntityType.NeweggOrderItemEntity,
                        EntityType.OrderEntity,
                        EntityType.OrderItemEntity,
                        EntityType.OrderItemAttributeEntity,
                        EntityType.OrderChargeEntity,
                        EntityType.OrderMotionOrderEntity,
                        EntityType.OrderPaymentDetailEntity,
                        EntityType.PayPalOrderEntity,
                        EntityType.ProStoresOrderEntity,
                        EntityType.SearsOrderEntity,
                        EntityType.SearsOrderItemEntity,
                        EntityType.ShopifyOrderEntity,
                        EntityType.ShopifyOrderItemEntity,
                        EntityType.ThreeDCartOrderItemEntity,
                        EntityType.YahooOrderEntity,
                        EntityType.YahooOrderItemEntity
                    };

        private static List<EntityType> shipSenseList = new List<EntityType> { EntityType.ShipSenseKnowledgebaseEntity };

        /// <summary>
        /// Indicates how much space the order/order item tables use
        /// </summary>
        public static long OrdersUsage
        {
            get { return GetTableSpaceUsed(orderList); }
        }

        /// <summary>
        /// Indicates how much space the audit logs take
        /// </summary>
        public static long AuditUsage
        {
            get { return GetTableSpaceUsed(auditList); }
        }

        /// <summary>
        /// Indicates how much space resources take
        /// </summary>
        public static long ResourceUsage
        {
            get { return GetTableSpaceUsed(resourceList); }
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
            get { return TotalUsage - OrdersUsage - AuditUsage - ResourceUsage - ShipSenseUsage; }
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
        private static long GetDatabaseSpaceUsed()
        {
            using (DbConnection con = SqlSession.Current.OpenConnection())
            {
                string sql = @"IF OBJECT_ID('tempdb..#RowCountsAndSizes') IS NOT NULL
                                DROP TABLE #RowCountsAndSizes

                              CREATE TABLE #RowCountsAndSizes (TableName NVARCHAR(128), [rows] CHAR(11), [reserved] VARCHAR(18), [data] VARCHAR(18), [index_size] VARCHAR(18), [unused] VARCHAR(18))
                              EXEC       sp_MSForEachTable 'INSERT INTO #RowCountsAndSizes EXEC sp_spaceused ''?'' '
                              select sum(CONVERT(bigint,left(reserved,len(reserved)-3))) from #RowCountsAndSizes";

                long size = (long) DbCommandProvider.ExecuteScalar(con, sql);

                return size * 1024L;
            }
        }

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
    }
}
