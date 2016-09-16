using System.Data.Common;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Connection;
using ShipWorks.SqlServer.Common.Data;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Helper class for shrinking a SQL Server database
    /// </summary>
    public static class SqlShrinkDatabase
    {
        static readonly ILog log = LogManager.GetLogger(typeof(SqlShrinkDatabase));

        /// <summary>
        /// Connects to the database and attempts to shrink the database.
        /// </summary>
        public static void ShrinkDatabase()
        {
            using (new LoggedStopwatch(log, "Shrink Database"))
            {

                // Attach to the connection
                using (DbConnection con = SqlSession.Current.OpenConnection())
                {
                    SqlAppLockUtility.RunLockedCommand(con, "ShrinkDatabaseTaskLock", command =>
                        {
                            command.CommandText = ShrinkDbSql;
                            command.CommandTimeout = 0;

                            command.ExecuteNonQuery();
                        });
                }
            }
        }

        /// <summary>
        /// The TSQL to shrink the db
        /// </summary>
        private static string ShrinkDbSql
        {
            get
            {
                return ResourceUtility.ReadString("ShipWorks.Data.Administration.Scripts.Maintenance.ShrinkDatabase.sql");
            }
        }
    }
}
