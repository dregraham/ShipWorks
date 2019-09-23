using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Data;
using ShipWorks.ApplicationCore.CommandLineOptions;
using ShipWorks.Data.Connection;
using ShipWorks.Startup;
using ShipWorks.Tests.Integration.Shared;
using ShipWorks.Tests.Shared.Database;
using Xunit;

namespace ShipWorks.Core.Tests.Integration.Data
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class SqlUtilityTest : IDisposable
    {
        private readonly DataContext context;
        private readonly AutoMock mock;
        private readonly DatabaseFixture db;

        public SqlUtilityTest(DatabaseFixture db)
        {
            this.db = db;
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));
            mock = context.Mock;
        }

        [Fact]
        public void DbIsSetToMultiUser_WhenSqlExceptionOccurrs()
        {
            string connectionString = string.Empty;
            string dbName = string.Empty;

            using (IDbConnection connection = SqlSession.Current.OpenConnection())
            {
                connectionString = connection.ConnectionString;
                dbName = connection.Database;

                string sql = "select 1/0;";

                DbCommand cmd = DbCommandProvider.Create(connection as DbConnection);
                cmd.CommandText = SqlUtility.WrapSqlInSingleUserMode(sql, dbName);
                cmd.CommandTimeout = 1800;

                DbCommandProvider.ExecuteScalar(cmd);
            }

            Assert.False(SqlUtility.IsSingleUser(connectionString, dbName));
        }

        [Fact]
        public void DbIsSetToMultiUser_WhenCreatingBackup()
        {
            string connectionString = string.Empty;
            string dbName = string.Empty;
            string backupPathAndfilename = string.Empty;

            using (DbConnection connection = SqlSession.Current.OpenConnection())
            {
                connectionString = connection.ConnectionString;
                dbName = connection.Database;
                backupPathAndfilename = Path.Combine(System.IO.Path.GetTempPath(), $"{dbName}_{Guid.NewGuid().ToString("N")}.bak");

                DbUtils.BackupDb(connection, backupPathAndfilename);
            }

            Assert.False(SqlUtility.IsSingleUser(connectionString, dbName));
            DbUtils.DeleteBackupFile(backupPathAndfilename);
        }

        [Fact]
        public void DbIsSetToMultiUser_WhenCreatingBackupAndRestoring()
        {
            string connectionString = string.Empty;
            string dbName = string.Empty;
            string backupPathAndfilename = string.Empty;

            using (DbConnection connection = SqlSession.Current.OpenConnection())
            {
                connectionString = connection.ConnectionString;
                dbName = connection.Database;
                backupPathAndfilename = Path.Combine(System.IO.Path.GetTempPath(), $"{dbName}_{Guid.NewGuid().ToString("N")}.bak");

                DbUtils.BackupDb(connection, backupPathAndfilename);
                DbUtils.RestoreDb(connection, backupPathAndfilename);
            }

            Assert.False(SqlUtility.IsSingleUser(connectionString, dbName));
            DbUtils.DeleteBackupFile(backupPathAndfilename);

            db.ResetDatabase();
        }

        [Fact]
        public void DbIsStillSingleUser_WhenSingleUserBeforeExecution()
        {
            string connectionString = string.Empty;
            string dbName = string.Empty;
            DbConnection singleUserModeConnection = SqlSession.Current.OpenConnection();

            using (IDbConnection connection = SqlSession.Current.OpenConnection())
            {
                connectionString = connection.ConnectionString;
                dbName = connection.Database;

                SqlUtility.SetSingleUser(singleUserModeConnection, dbName);

                string sql = "select 1/0;";

                DbCommand cmd = DbCommandProvider.Create(connection as DbConnection);
                cmd.CommandText = SqlUtility.WrapSqlInSingleUserMode(sql, dbName);
                cmd.CommandTimeout = 1800;

                try
                {
                    DbCommandProvider.ExecuteScalar(cmd);
                }
                catch (Exception e) when (e.Message.Contains("The connection is broken and recovery is not possible"))
                {
                }
            }

            Assert.True(SqlUtility.IsSingleUser(connectionString, dbName));
            SqlUtility.SetMultiUser(singleUserModeConnection);
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
