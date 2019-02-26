using System;
using System.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared.Database;
using Xunit;

namespace ShipWorks.Core.Tests.Integration.Data
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class SqlServerInfoTest : IDisposable
    {
        private readonly DataContext context;

        public SqlServerInfoTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));
        }

        [Fact]
        public void HasCustomTriggers_ReturnsCorrectValue()
        {
            IDbConnection connection = SqlSession.Current.OpenConnection();
            DropStoredProc(connection);

            Assert.False(SqlServerInfo.HasCustomTriggers());

            AddStoredProc(connection);
            Assert.True(SqlServerInfo.HasCustomTriggers());
        }

        private void AddStoredProc(IDbConnection connection)
        {
            string sql = $@"
                IF EXISTS (SELECT * FROM sys.objects WHERE [name] = N'ExtTrig' AND [type] = 'TR')
                BEGIN
                      DROP TRIGGER [dbo].[ExtTrig];
                END;";
            RunSqlNonQuery(connection, sql);

            sql = @"
                CREATE TRIGGER dbo.ExtTrig 
                   ON  dbo.[Product]
                   AFTER INSERT,DELETE,UPDATE
                AS 
                BEGIN
                    SET NOCOUNT ON;
                END";

            RunSqlNonQuery(connection, sql);
        }

        private void DropStoredProc(IDbConnection connection)
        {
            string sql = $@"
                IF EXISTS (SELECT * FROM sys.objects WHERE [name] = N'ExtTrig' AND [type] = 'TR')
                BEGIN
                      DROP TRIGGER [dbo].[ExtTrig];
                END;";

            RunSqlNonQuery(connection, sql);
        }

        private void RunSqlNonQuery(IDbConnection connection, string sql)
        {
            using (IDbCommand cmd = connection.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
