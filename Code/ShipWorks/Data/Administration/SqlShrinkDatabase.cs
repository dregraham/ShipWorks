﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Interapptive.Shared.Utility;
using Microsoft.SqlServer.Server;
using ShipWorks.Data.Connection;
using ShipWorks.SqlServer.Common.Data;
using ShipWorks.SqlServer.General;
using ShipWorks.SqlServer.Purge;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Helper class for shrinking a SQL Server database
    /// </summary>
    public static class SqlShrinkDatabase
    {
        /// <summary>
        /// Connects to the database and attempts to shrink the database.
        /// </summary>
        public static void ShrinkDatabase()
        {
            // Attach to the connection
            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                SqlAppLockUtility.GetLockedCommand(con, "ShrinkDatabaseTaskLock", command =>
                {
                    command.CommandText = ShrinkDbSql;
                    command.CommandTimeout = 0;

                    command.ExecuteNonQuery();
                });
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
