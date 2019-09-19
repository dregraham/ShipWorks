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
        public void ExecuteScriptSql(string name, string sql, DbConnection con)
        {
            SqlUtility.ExecuteScriptSql(name, sql, con);
        }

        public void ExecuteScriptSql(string name, string sql, DbConnection con, DbTransaction transaction)
        {
            SqlUtility.ExecuteScriptSql(name, sql, con, transaction);
        }

        public void EnableClr(DbConnection con)
        {
            SqlUtility.EnableClr(con);
        }

        public void SetSingleUser(DbConnection connection, string dbName)
        {
            SqlUtility.SetSingleUser(connection, dbName);
        }

        public void SetMultiUser(DbConnection con)
        {
            SqlUtility.SetMultiUser(con);
        }

        public void SetMultiUser(string connectionString, string databaseName)
        {
            SqlUtility.SetMultiUser(connectionString, databaseName);
        }

        public string GetMasterDatabaseConnectionString(string connectionString)
        {
            return SqlUtility.GetMasterDatabaseConnectionString(connectionString);
        }

        public bool IsSingleUser(string connectionString, string databaseName)
        {
            return SqlUtility.IsSingleUser(connectionString, databaseName);
        }

        public string WrapSqlInSingleUserMode(string sql, string dbName)
        {
            return SqlUtility.WrapSqlInSingleUserMode(sql, dbName);
        }

        public bool RenameArchvingDbIfNeeded(DbConnection con, string databaseName)
        {
            return SqlUtility.RenameArchvingDbIfNeeded(con, databaseName);
        }

        public string GetArchivingDatabasename(string databaseName)
        {
            return SqlUtility.GetArchivingDatabasename(databaseName);
        }

        public void SetChangeTrackingRetention(DbConnection con, int days)
        {
            SqlUtility.SetChangeTrackingRetention(con, days);
        }

        public void BeginTransaction(DbConnection con)
        {
            SqlUtility.BeginTransaction(con);
        }

        public void CommitTransaction(DbConnection con)
        {
            SqlUtility.CommitTransaction(con);
        }

        public bool DoesDatabaseExist(DbConnection con, string databaseName)
        {
            return SqlUtility.DoesDatabaseExist(con, databaseName);
        }

        public long GetTimestampValue(byte[] timestamp)
        {
            return SqlUtility.GetTimestampValue(timestamp);
        }

        public string GetUsername(DbConnection con)
        {
            return SqlUtility.GetUsername(con);
        }

        public bool CheckSchemaPermission(DbConnection con, string permission)
        {
            return SqlUtility.CheckSchemaPermission(con, permission);
        }

        public bool CheckDatabasePermission(DbConnection con, string permission)
        {
            return SqlUtility.CheckDatabasePermission(con, permission);
        }

        public bool CheckObjectPermission(DbConnection con, string objectName, string permission)
        {
            return SqlUtility.CheckObjectPermission(con, objectName, permission);
        }

        public string GetMasterDataFilePath(DbConnection con)
        {
            return SqlUtility.GetMasterDataFilePath(con);
        }

        public void ConfigureSqlServerForClr(DbConnection con, string databaseName, string databaseOwner)
        {
            SqlUtility.ConfigureSqlServerForClr(con, databaseName, databaseOwner);
        }

        public string ConfigureSqlServerForClrSql(string databaseName, string databaseOwner)
        {
            return SqlUtility.ConfigureSqlServerForClrSql(databaseName, databaseOwner);
        }

        public void TruncateTable(string filterNodeContentDirty, DbConnection sqlConnection)
        {
            SqlUtility.TruncateTable(filterNodeContentDirty, sqlConnection);
        }

        public void TruncateTable(string table, DbConnection con, DbTransaction transaction)
        {
            SqlUtility.TruncateTable(table, con, transaction);
        }

        public void RecordDatabaseTelemetry(DbConnection con, TelemetricResult<Unit> databaseUpdateResult)
        {
            SqlUtility.RecordDatabaseTelemetry(con, databaseUpdateResult);
        }

        public string GetRunningSqlCommands(string connectionString)
        {
            return SqlUtility.GetRunningSqlCommands(connectionString);
        }
    }
}
