using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;

namespace Interapptive.Shared.Data
{
    /// <summary>
    /// Utility functions for dealing with Sql Server and Sql statements
    /// </summary>
    public class SqlUtilityWrapper : ISqlUtility
    {
        /// <summary>
        /// Executes each batch in the SQL, as separated by the GO keyword, using the given connection.
        /// </summary>
        public void ExecuteScriptSql(string name, string sql, DbConnection con)
        {
            SqlUtility.ExecuteScriptSql(name, sql, con);
        }

        /// <summary>
        /// Executes each batch in the SQL, as separated by the GO keyword, using the given connection.
        /// </summary>
        public void ExecuteScriptSql(string name, string sql, DbConnection con, DbTransaction transaction)
        {
            SqlUtility.ExecuteScriptSql(name, sql, con, transaction);
        }

        /// <summary>
        /// Enable CLR usage on the given connection
        /// </summary>
        public void EnableClr(DbConnection con)
        {
            SqlUtility.EnableClr(con);
        }

        /// <summary>
        /// Set the database on the given connection into single-user mode.
        /// </summary>
        public void SetSingleUser(DbConnection connection, string dbName)
        {
            SqlUtility.SetSingleUser(connection, dbName);
        }

        /// <summary>
        /// Set the database on the given connection into multi-user mode
        /// </summary>
        public void SetMultiUser(DbConnection con)
        {
            SqlUtility.SetMultiUser(con);
        }

        /// <summary>
        /// Set the database on the given connection into multi-user mode
        /// </summary>
        public void SetMultiUser(string connectionString, string databaseName)
        {
            SqlUtility.SetMultiUser(connectionString, databaseName);
        }

        /// <summary>
        /// Gets an open connection to the master database
        /// </summary>
        public string GetMasterDatabaseConnectionString(string connectionString)
        {
            return SqlUtility.GetMasterDatabaseConnectionString(connectionString);
        }

        /// <summary>
        /// Determines if the specified database is in SINGLE_USER mode using the given connection
        /// </summary>
        public bool IsSingleUser(string connectionString, string databaseName)
        {
            return SqlUtility.IsSingleUser(connectionString, databaseName);
        }

        /// <summary>
        /// Wraps a sql statement in a check for single user mode, set to single user if not in single user,
        /// executes the given sql, then sets to multi user mode.
        /// </summary>
        public string WrapSqlInSingleUserMode(string sql, string dbName)
        {
            return SqlUtility.WrapSqlInSingleUserMode(sql, dbName);
        }

        /// <summary>
        /// Checks to see if there is an archiving database that didn't get renamed, possibly due to a crash,
        /// and renames it to what is expected.
        /// </summary>
        public bool RenameArchvingDbIfNeeded(DbConnection con, string databaseName)
        {
            return SqlUtility.RenameArchvingDbIfNeeded(con, databaseName);
        }

        /// <summary>
        /// Get the archiving database name
        /// </summary>
        public string GetArchivingDatabasename(string databaseName)
        {
            return SqlUtility.GetArchivingDatabasename(databaseName);
        }

        /// <summary>
        /// Set the number of days change tracking should retain data for the database on the given connection
        /// </summary>
        public void SetChangeTrackingRetention(DbConnection con, int days)
        {
            SqlUtility.SetChangeTrackingRetention(con, days);
        }

        /// <summary>
        /// Start a new transaction on the given connection
        /// </summary>
        public void BeginTransaction(DbConnection con)
        {
            SqlUtility.BeginTransaction(con);
        }

        /// <summary>
        /// Commit the outstanding transaction on the given connection
        /// </summary>
        public void CommitTransaction(DbConnection con)
        {
            SqlUtility.CommitTransaction(con);
        }

        /// <summary>
        /// Returns true if the database exists on the server from the specified connection
        /// </summary>
        public bool DoesDatabaseExist(DbConnection con, string databaseName)
        {
            return SqlUtility.DoesDatabaseExist(con, databaseName);
        }

        /// <summary>
        /// Convert the byte[] to a long
        /// </summary>
        public long GetTimestampValue(byte[] timestamp)
        {
            return SqlUtility.GetTimestampValue(timestamp);
        }

        /// <summary>
        /// Get the effective logged in SQL Server user of the connection
        /// </summary>
        public string GetUsername(DbConnection con)
        {
            return SqlUtility.GetUsername(con);
        }

        /// <summary>
        /// Checks that the given permission name is available for the current user on the current schema
        /// </summary>
        public bool CheckSchemaPermission(DbConnection con, string permission)
        {
            return SqlUtility.CheckSchemaPermission(con, permission);
        }

        /// <summary>
        /// Checks that the given user has the given permission in the current database
        /// </summary>
        public bool CheckDatabasePermission(DbConnection con, string permission)
        {
            return SqlUtility.CheckDatabasePermission(con, permission);
        }

        /// <summary>
        /// Checks that the given user has the given permission on the given object in the current database
        /// </summary>
        public bool CheckObjectPermission(DbConnection con, string objectName, string permission)
        {
            return SqlUtility.CheckObjectPermission(con, objectName, permission);
        }

        /// <summary>
        /// Get the file path to the master database mdf file
        /// </summary>
        public string GetMasterDataFilePath(DbConnection con)
        {
            return SqlUtility.GetMasterDataFilePath(con);
        }

        /// <summary>
        /// Configure SQL Server 2017 to work with our assemblies
        /// </summary>
        /// <param name="con">The connection to use</param>
        /// <param name="databaseName">The database to configure</param>
        /// <param name="databaseOwner">The database owner</param>
        public void ConfigureSqlServerForClr(DbConnection con, string databaseName, string databaseOwner)
        {
            SqlUtility.ConfigureSqlServerForClr(con, databaseName, databaseOwner);
        }

        /// <summary>
        /// Configure SQL Server 2017 to work with our assemblies
        /// </summary>
        /// <param name="databaseName">The database to configure</param>
        /// <param name="databaseOwner">The database owner</param>
        public string ConfigureSqlServerForClrSql(string databaseName, string databaseOwner)
        {
            return SqlUtility.ConfigureSqlServerForClrSql(databaseName, databaseOwner);
        }

        /// <summary>
        /// Truncate the given table contents
        /// </summary>
        public void TruncateTable(string filterNodeContentDirty, DbConnection sqlConnection)
        {
            SqlUtility.TruncateTable(filterNodeContentDirty, sqlConnection);
        }

        /// <summary>
        /// Truncate the given table contents
        /// </summary>
        public void TruncateTable(string table, DbConnection con, DbTransaction transaction)
        {
            SqlUtility.TruncateTable(table, con, transaction);
        }

        /// <summary>
        /// Update telemetry with information about the sql server.
        /// </summary>
        public void RecordDatabaseTelemetry(DbConnection con, TelemetricResult<Unit> databaseUpdateResult)
        {
            SqlUtility.RecordDatabaseTelemetry(con, databaseUpdateResult);
        }


        /// <summary>
        /// Queries the database for running queries, and returns a pipe delimited CSV of the results.
        /// </summary>
        /// <returns>Pipe delimited CSV of running queries.</returns>
        public string GetRunningSqlCommands(string connectionString)
        {
            return SqlUtility.GetRunningSqlCommands(connectionString);
        }
    }
}
