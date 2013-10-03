using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Diagnostics;
using log4net;
using System.Threading;
using System.Globalization;

namespace Interapptive.Shared.Data
{
    /// <summary>
    /// Utility functions for dealing with Sql Server and Sql statements
    /// </summary>
    public static class SqlUtility
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(SqlUtility));

        // Indicates if we've discovered we have to truncate with DELETE due to missing permissions
        static bool truncateWithDelete = false;

        /// <summary>
        /// Executes each batch in the SQL, as separated by the GO keyword, using the given connection.
        /// </summary>
        public static void ExecuteScriptSql(string name, string sql, SqlConnection con)
        {
            if (con == null)
            {
                throw new ArgumentNullException("con");
            }

            SqlScript script = new SqlScript(name, sql);
            script.Execute(con);
        }

        /// <summary>
        /// Enable CLR usage on the given connection
        /// </summary>
        public static void EnableClr(SqlConnection con)
        {
            if (con == null)
            {
                throw new ArgumentNullException("con");
            }

            log.Info("EnableCLR");

            try
            {
                SqlCommandProvider.ExecuteNonQuery(con, "sp_configure 'clr enabled', 1");
                SqlCommandProvider.ExecuteNonQuery(con, "RECONFIGURE");
            }
            catch (Exception ex)
            {
                log.Error("Failed to enable clr.", ex);

                throw;
            }
        }

        /// <summary>
        /// Set the compatibility level of the database
        /// </summary>
        public static void SetSql2008CompatibilityLevel(SqlConnection con)
        {
            if (con == null)
            {
                throw new ArgumentNullException("con");
            }

            SqlCommand cmd = SqlCommandProvider.Create(con);
            cmd.CommandText = string.Format("ALTER DATABASE [{0}] SET COMPATIBILITY_LEVEL = 100", con.Database);

            log.Info(cmd.CommandText);

            SqlCommandProvider.ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// Set the database on the given connection into single-user mode.
        /// </summary>
        public static void SetSingleUser(SqlConnection con)
        {
            if (con == null)
            {
                throw new ArgumentNullException("con");
            }

            log.Info("Altering database to SINGLE_USER");
            SqlCommandProvider.ExecuteNonQuery(con, "ALTER DATABASE [" + con.Database + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
        }

        /// <summary>
        /// Set the database on the given connection into multi-user mode
        /// </summary>
        public static void SetMultiUser(SqlConnection con)
        {
            if (con == null)
            {
                throw new ArgumentNullException("con");
            }

            log.Info("Altering database to MULTI_USER");
            SqlCommandProvider.ExecuteNonQuery(con, "ALTER DATABASE [" + con.Database + "] SET MULTI_USER WITH ROLLBACK IMMEDIATE");
        }
        
        /// <summary>
        /// Determines if the specified database is in SINGLE_USER mode using the given connection
        /// </summary>
        public static bool IsSingleUser(SqlConnection con, string databaseName)
        {
            if (con == null)
            {
                throw new ArgumentNullException("con");
            }

            SqlCommand cmd = SqlCommandProvider.Create(con);
            cmd.CommandText = "SELECT user_access FROM sys.databases WHERE name = @name";
            cmd.Parameters.AddWithValue("@name", databaseName);

            object result = cmd.ExecuteScalar();
            if (result == null || result is DBNull)
            {
                return false;
            }

            return Convert.ToInt32(result) == 1;
        }

        /// <summary>
        /// Set the number of days change tracking should retain data for the database on the given connection
        /// </summary>
        public static void SetChangeTrackingRetention(SqlConnection con, int days)
        {
            if (con == null)
            {
                throw new ArgumentNullException("con");
            }

            log.InfoFormat("Altering database to CHANGE_RETRENTION {0} DAYS", days);

            SqlCommandProvider.ExecuteNonQuery(con, string.Format("ALTER DATABASE [" + con.Database + "] SET CHANGE_TRACKING (CHANGE_RETENTION = {0} DAYS)", days));
        }

        /// <summary>
        /// Start a new transaction on the given connection
        /// </summary>
        public static void BeginTransaction(SqlConnection con)
        {
            SqlCommandProvider.ExecuteNonQuery(con, "BEGIN TRANSACTION");
        }

        /// <summary>
        /// Commit the outstanding transaction on the given connection
        /// </summary>
        public static void CommitTransaction(SqlConnection con)
        {
            SqlCommandProvider.ExecuteNonQuery(con, "COMMIT");
        }

        /// <summary>
        /// Returns true if the database exists on the server from the specified connection
        /// </summary>
        public static bool DoesDatabaseExist(SqlConnection con, string databaseName)
        {
            // query the sys tables to determine if a database exists
            SqlCommand cmd = SqlCommandProvider.Create(con);
            cmd.CommandText = "select count(*) from master..sysdatabases where name = @databaseName";
            cmd.Parameters.AddWithValue("@databaseName", databaseName);

            if ((int) SqlCommandProvider.ExecuteScalar(cmd) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Convert the byte[] to a long
        /// </summary>
        public static long GetTimestampValue(byte[] timestamp)
        {
            if (timestamp == null)
            {
                throw new ArgumentNullException("timestamp");
            }

            if (timestamp.Length != 8)
            {
                throw new ArgumentException("timestamp should be 8 bytes");
            }

            long value = 0;

            for (int i = 7; i >= 0; i--)
            {
                value += timestamp[i] * (long) Math.Pow(256, 7 - i);
            }

            return value;
        }
        
        /// <summary>
        /// Get the effective logged in SQL Server user of the connection
        /// </summary>
        public static string GetUsername(SqlConnection con)
        {
            return (string) SqlCommandProvider.ExecuteScalar(con, "SELECT SUSER_SNAME()");
        }

        /// <summary>
        /// Checks that the given permission name is available for the current user on the current schema
        /// </summary>
        public static bool CheckSchemaPermission(SqlConnection con, string permission)
        {
            int result = (int) SqlCommandProvider.ExecuteScalar(con, string.Format("SELECT HAS_PERMS_BY_NAME(SCHEMA_NAME(), 'SCHEMA', '{0}')", permission));

            return result != 0;
        }

        /// <summary>
        /// Checks that the given user has the given permission in the current database
        /// </summary>
        public static bool CheckDatabasePermission(SqlConnection con, string permission)
        {
            int result = (int) SqlCommandProvider.ExecuteScalar(con, string.Format("SELECT HAS_PERMS_BY_NAME('{0}', 'DATABASE', '{1}')", con.Database, permission));

            return result != 0;
        }

        /// <summary>
        /// Checks that the given user has the given permission on the given object in the current database
        /// </summary>
        public static bool CheckObjectPermission(SqlConnection con, string objectName, string permission)
        {
            int result = (int) SqlCommandProvider.ExecuteScalar(con, string.Format("SELECT HAS_PERMS_BY_NAME('{0}', 'OBJECT', '{1}')", objectName, permission));

            return result != 0;
        }

        /// <summary>
        /// Get the file path to the master database mdf file
        /// </summary>
        public static string GetMasterDataFilePath(SqlConnection con)
        {
            object fileResult = SqlCommandProvider.ExecuteScalar(con, "select filename from master..sysdatabases where name = 'master'");

            if (fileResult == null || fileResult is DBNull)
            {
                throw new SqlScriptException(string.Format("The user you are connecting to SQL Server as ('{0}') does not have permissions to create a database.",
                    SqlUtility.GetUsername(con)));
            }

            // Get the filepath to master
            string masterPath = Path.GetDirectoryName((string) fileResult);

            // We need it to end in a slash
            if (!masterPath.EndsWith("\\"))
            {
                masterPath += "\\";
            }

            return masterPath;
        }

        /// <summary>
        /// Truncate the given table contents
        /// </summary>
        public static void TruncateTable(string table, SqlConnection con)
        {
            if (!truncateWithDelete)
            {
                // Try TRUNCATE for optimal performance
                try
                {
                    SqlCommand truncateCmd = con.CreateCommand();
                    truncateCmd.CommandText = "TRUNCATE TABLE [" + table + "]";
                    truncateCmd.ExecuteNonQuery();

                    return;
                }
                catch (SqlException)
                {

                }
            }

            // Fallback to DELETE in case user doesn't have permission
            SqlCommand deleteCmd = con.CreateCommand();
            deleteCmd.CommandText = "DELETE " + table;
            deleteCmd.ExecuteNonQuery();

            truncateWithDelete = true;
        }
    }
}
