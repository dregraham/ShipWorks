using System.Data.Common;
using System.Reactive;
using Interapptive.Shared.Utility;

namespace Interapptive.Shared.Data
{
    /// <summary>
    /// Utility functions for dealing with Sql Server and Sql statements
    /// </summary>
    public interface ISqlUtility
    {
        /// <summary>
        /// Executes each batch in the SQL, as separated by the GO keyword, using the given connection.
        /// </summary>
        void ExecuteScriptSql(string name, string sql, DbConnection con);

        /// <summary>
        /// Executes each batch in the SQL, as separated by the GO keyword, using the given connection.
        /// </summary>
        void ExecuteScriptSql(string name, string sql, DbConnection con, DbTransaction transaction);

        /// <summary>
        /// Enable CLR usage on the given connection
        /// </summary>
        void EnableClr(DbConnection con);

        /// <summary>
        /// Set the database on the given connection into single-user mode.
        /// </summary>
        void SetSingleUser(DbConnection connection, string dbName);

        /// <summary>
        /// Set the database on the given connection into multi-user mode
        /// </summary>
        void SetMultiUser(DbConnection con);

        /// <summary>
        /// Set the database on the given connection into multi-user mode
        /// </summary>
        void SetMultiUser(string connectionString, string databaseName);

        /// <summary>
        /// Gets an open connection to the master database
        /// </summary>
        string GetMasterDatabaseConnectionString(string connectionString);

        /// <summary>
        /// Determines if the specified database is in SINGLE_USER mode using the given connection
        /// </summary>
        bool IsSingleUser(string connectionString, string databaseName);

        /// <summary>
        /// Wraps a sql statement in a check for single user mode, set to single user if not in single user,
        /// executes the given sql, then sets to multi user mode.
        /// </summary>
        string WrapSqlInSingleUserMode(string sql, string dbName);

        /// <summary>
        /// Checks to see if there is an archiving database that didn't get renamed, possibly due to a crash,
        /// and renames it to what is expected.
        /// </summary>
        bool RenameArchvingDbIfNeeded(DbConnection con, string databaseName);

        /// <summary>
        /// Get the archiving database name
        /// </summary>
        string GetArchivingDatabasename(string databaseName);

        /// <summary>
        /// Set the number of days change tracking should retain data for the database on the given connection
        /// </summary>
        void SetChangeTrackingRetention(DbConnection con, int days);

        /// <summary>
        /// Start a new transaction on the given connection
        /// </summary>
        void BeginTransaction(DbConnection con);

        /// <summary>
        /// Commit the outstanding transaction on the given connection
        /// </summary>
        void CommitTransaction(DbConnection con);

        /// <summary>
        /// Returns true if the database exists on the server from the specified connection
        /// </summary>
        bool DoesDatabaseExist(DbConnection con, string databaseName);

        /// <summary>
        /// Convert the byte[] to a long
        /// </summary>
        long GetTimestampValue(byte[] timestamp);

        /// <summary>
        /// Get the effective logged in SQL Server user of the connection
        /// </summary>
        string GetUsername(DbConnection con);

        /// <summary>
        /// Checks that the given permission name is available for the current user on the current schema
        /// </summary>
        bool CheckSchemaPermission(DbConnection con, string permission);

        /// <summary>
        /// Checks that the given user has the given permission in the current database
        /// </summary>
        bool CheckDatabasePermission(DbConnection con, string permission);

        /// <summary>
        /// Checks that the given user has the given permission on the given object in the current database
        /// </summary>
        bool CheckObjectPermission(DbConnection con, string objectName, string permission);

        /// <summary>
        /// Get the file path to the master database mdf file
        /// </summary>
        string GetMasterDataFilePath(DbConnection con);

        /// <summary>
        /// Configure SQL Server 2017 to work with our assemblies
        /// </summary>
        /// <param name="con">The connection to use</param>
        /// <param name="databaseName">The database to configure</param>
        /// <param name="databaseOwner">The database owner</param>
        void ConfigureSqlServerForClr(DbConnection con, string databaseName, string databaseOwner);

        /// <summary>
        /// Configure SQL Server 2017 to work with our assemblies
        /// </summary>
        /// <param name="databaseName">The database to configure</param>
        /// <param name="databaseOwner">The database owner</param>
        string ConfigureSqlServerForClrSql(string databaseName, string databaseOwner);

        /// <summary>
        /// Truncate the given table contents
        /// </summary>
        void TruncateTable(string filterNodeContentDirty, DbConnection sqlConnection);

        /// <summary>
        /// Truncate the given table contents
        /// </summary>
        void TruncateTable(string table, DbConnection con, DbTransaction transaction);

        /// <summary>
        /// Update telemetry with information about the sql server.
        /// </summary>
        void RecordDatabaseTelemetry(DbConnection con, TelemetricResult<Unit> databaseUpdateResult);

        /// <summary>
        /// Queries the database for running queries, and returns a pipe delimited CSV of the results.
        /// </summary>
        /// <returns>Pipe delimited CSV of running queries.</returns>
        string GetRunningSqlCommands(string connectionString);
    }
}
