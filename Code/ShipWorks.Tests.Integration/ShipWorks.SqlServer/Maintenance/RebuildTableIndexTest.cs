using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ShipWorks.Data.Connection;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared.Database;
using Xunit;

namespace ShipWorks.Tests.Integration.MSTest.ShipWorks.SqlServer.Maintenance
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class RebuildTableIndexTest : IDisposable
    {
        private readonly DataContext context;

        public RebuildTableIndexTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));
            CreateTestTable();
        }

        [Fact]
        [Trait("Category", "SqlServer.Maintenance")]
        public void RebuildTableIndex_RebuildAllIndexes_Succeeds()
        {
            // This assumes it is being run against the "seeded" database (see SeedDatabase.sql script
            // in solution directory)
            foreach (Tuple<string, string> tablesAndIndexes in GetAllTablesAndIndexes())
            {
                RunRebuildIndex(tablesAndIndexes.Item1, tablesAndIndexes.Item2);
            }
        }

        /// <summary>
        /// Create a test table with data types to test with.
        /// </summary>
        private void CreateTestTable()
        {
            using (SqlConnection sqlConnection = new SqlConnection(SqlAdapter.Default.ConnectionString))
            {
                sqlConnection.Open();

                SqlCommand cmd = sqlConnection.CreateCommand();
                cmd.CommandText = CreateTestTableCommandText;

                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Runs the sql to rebuild an index
        /// </summary>
        private void RunRebuildIndex(string tableName, string indexName)
        {
            using (SqlConnection sqlConnection = new SqlConnection(SqlAdapter.Default.ConnectionString))
            {
                sqlConnection.Open();

                SqlCommand cmd = sqlConnection.CreateCommand();
                cmd.CommandText = string.Format("exec RebuildTableIndex {0}, {1}", tableName, indexName);

                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Gets a list of all user tables and their indexes
        /// </summary>
        /// <returns></returns>
        private List<Tuple<string, string>> GetAllTablesAndIndexes()
        {
            List<Tuple<string, string>> tablesAndIndexes = new List<Tuple<string, string>>();

            using (SqlConnection sqlConnection = new SqlConnection(SqlAdapter.Default.ConnectionString))
            {
                sqlConnection.Open();

                SqlCommand cmd = sqlConnection.CreateCommand();
                cmd.CommandText = @"
                    select '[' + t.name + ']' as 'TableName', i.name as 'IndexName'
                    from sys.indexes i, sys.tables t
                    where i.object_id = t.object_id
                      and t.type = 'U'
                      and i.name is not null
                    order by t.name, i.is_primary_key desc, i.name asc";

                using (SqlDataReader sqlDataReader = cmd.ExecuteReader())
                {
                    while (sqlDataReader.Read())
                    {
                        tablesAndIndexes.Add(new Tuple<string, string>(sqlDataReader["TableName"].ToString(), sqlDataReader["IndexName"].ToString()));
                    }
                }
            }

            return tablesAndIndexes;
        }

        public void Dispose() => context.Dispose();

        // Script to create the test table, add indexes for testing.
        private const string CreateTestTableCommandText = @"
            IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TestRebuildingIndexes]') AND type in (N'U'))
                DROP TABLE [dbo].[TestRebuildingIndexes]

            CREATE TABLE [dbo].[TestRebuildingIndexes](
                [TestRebuildingIndexesID] [bigint] IDENTITY(1,1) NOT NULL,
                [SmallNvarchar] nvarchar(500) NOT NULL,
                [LargeNvarchar] nvarchar(500) NOT NULL,
             CONSTRAINT [PK_TestRebuildingIndexes] PRIMARY KEY CLUSTERED
            (
                [TestRebuildingIndexesID] ASC
            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
            ) ON [PRIMARY]


            CREATE NONCLUSTERED INDEX [IX_TestRebuildingIndexes_SmallNvarchar] ON [dbo].[TestRebuildingIndexes]
            (
                [SmallNvarchar] ASC
            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

            CREATE NONCLUSTERED INDEX [IX_TestRebuildingIndexes_LargeNvarchar] ON [dbo].[TestRebuildingIndexes]
            (
                [LargeNvarchar] ASC
            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

            DECLARE @RowCount INT
            DECLARE @RowString VARCHAR(10)
            DECLARE @Random INT
            DECLARE @Upper INT
            DECLARE @Lower INT
            DECLARE @InsertDate DATETIME
            SET @Lower = -730
            SET @Upper = -1
            SET @RowCount = 0

            WHILE @RowCount < 1000
            BEGIN
                SET @RowString = CAST(@RowCount AS VARCHAR(10))
                SELECT @Random = ROUND(((@Upper - @Lower -1) * RAND() + @Lower), 0)
                SET @InsertDate = DATEADD(dd, @Random, GETDATE())

                INSERT INTO [TestRebuildingIndexes] ([SmallNvarchar], [LargeNvarchar])
                VALUES (REPLICATE('0', 10 - DATALENGTH(@RowString)) + @RowString , @InsertDate )

                SET @RowCount = @RowCount + 1
            END

            update [TestRebuildingIndexes] set SmallNvarchar = cast(TestRebuildingIndexesID as nvarchar(50)) + SmallNvarchar
            update [TestRebuildingIndexes] set [LargeNvarchar] = cast(TestRebuildingIndexesID as nvarchar(50)) + [LargeNvarchar]
            ";
    }
}
