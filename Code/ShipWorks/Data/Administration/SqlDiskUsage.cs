using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using ShipWorks.Data.Model;
using ShipWorks.Data.Connection;
using Interapptive.Shared.Data;

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

        static  List<EntityType> resourceList = new List<EntityType>
                    {
                        EntityType.ResourceEntity
                    };

        /// <summary>
        /// Indicates how much space the stores, customers, orders, etc. take
        /// </summary>
        public static long OrdersUsage
        {
            get { return TotalUsage - ResourceUsage - AuditUsage; }
        }

        /// <summary>
        /// Indicates how much space the audit logs take
        /// </summary>
        public static long AuditUsage
        {
            get { return GetTableSpaceUsed(auditList); }
        }

        /// <summary>
        /// Indicates howmuch space resources take
        /// </summary>
        public static long ResourceUsage
        {
            get { return GetTableSpaceUsed(resourceList); }
        }

        /// <summary>
        /// Indicates the total space used by the database
        /// </summary>
        public static long TotalUsage
        {
            get { return GetDatabaseSpaceUsed(); }
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
                using (SqlConnection con = SqlSession.Current.OpenConnection())
                {
                    SqlCommand cmd = SqlCommandProvider.Create(con);
                    cmd.CommandText = "SELECT SERVERPROPERTY ('edition')";

                    return SqlCommandProvider.ExecuteScalar(cmd).ToString().Contains("Express");
                }
            }
        }

        /// <summary>
        /// Get the total space used by the database
        /// </summary>
        private static long GetDatabaseSpaceUsed()
        {
            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                long size = (int) SqlCommandProvider.ExecuteScalar(con, "SELECT size FROM sys.database_files WHERE type = 0");

                return (size * 8) * 1024L;
            }
        }

        /// <summary>
        /// Get the space used by the given list of entities.
        /// </summary>
        private static long GetTableSpaceUsed(List<EntityType> entityList)
        {
            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                long total = 0;

                foreach (string table in entityList.Select(e => SqlAdapter.GetTableName(e)))
                {
                    SqlCommand cmd = SqlCommandProvider.Create(con);
                    cmd.CommandText = string.Format("EXEC sp_spaceused '{0}'", table);

                    using (SqlDataReader reader = SqlCommandProvider.ExecuteReader(cmd))
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
