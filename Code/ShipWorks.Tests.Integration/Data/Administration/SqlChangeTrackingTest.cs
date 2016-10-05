﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Interapptive.Shared.Data;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Connection;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared.Database;
using Xunit;

namespace ShipWorks.Tests.Integration.MSTest.Data.Administration
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class SqlChangeTrackingTest : IDisposable
    {
        private SqlChangeTracking testObject;
        private readonly DataContext context;

        public SqlChangeTrackingTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));

            testObject = new SqlChangeTracking();
        }

        private string TablesWithChangeTrackingQuery =>
            @"SELECT SYS.TABLES.NAME
                FROM sys.change_tracking_tables
                INNER JOIN sys.tables
                    ON sys.tables.object_id = sys.change_tracking_tables.object_id
                INNER JOIN sys.schemas
                    ON sys.schemas.schema_id = sys.tables.schema_id";

        private string DisableChangeTrackingOnAllTablesQuery =>
            @"DECLARE @SQL NVARCHAR(MAX)='';
                SELECT @SQL = @SQL + 'ALTER TABLE [' + t.name + '] Disable Change_tracking;'
                FROM sys.change_tracking_tables ct
                    JOIN sys.tables t
                        ON ct.object_id= t.object_id
                    JOIN sys.schemas s
                        ON t.schema_id= s.schema_id;

                PRINT @SQL;
                EXEC sp_executesql @SQL;";

        [Fact]
        [Trait("Category", "ContinuousIntegration")]
        public void TablesRequiringChangeTracking_CountMatchesNumberOfTablesWithChangeTrackingInDatabase()
        {
            // This assumes a database is being used that has change tracking configured correctly on
            // the appropriate tables (i.e. seed database)

            // This test in addition to the test for matching actual table names, ensures that the exact
            // tables used in the SqlChangeTracking class match the database. No more. No less.
            Assert.Equal(GetTablesWithChangeTrackingEnabledFromDatabase().Count, testObject.TablesRequiringChangeTracking.Count);
        }

        [Fact]
        [Trait("Category", "ContinuousIntegration")]
        public void TablesRequiringChangeTracking_MatchesTableNamesWithChangeTrackingInDatabase()
        {
            // This assumes a database is being used that has change tracking configured correctly on
            // the appropriate tables (i.e. seed database)
            List<string> expectedTableNames = GetTablesWithChangeTrackingEnabledFromDatabase();

            foreach (string tableName in expectedTableNames)
            {
                Assert.True(testObject.TablesRequiringChangeTracking.Contains(tableName));
            }
        }

        [Fact]
        [Trait("Category", "ContinuousIntegration")]
        public void Enable_TurnsOnChangeTrackingForDatabase()
        {
            // Setup
            DisableChangeTrackingOnAllTables();
            DisableChangeTrackingOnDatabase();

            testObject.Enable();

            Assert.True(IsDatabaseChangeTrackingEnabled());
        }

        [Fact]
        [Trait("Category", "ContinuousIntegration")]
        public void Enable_TurnsOnChangeTrackingForAllTables()
        {
            // Setup
            DisableChangeTrackingOnAllTables();
            DisableChangeTrackingOnDatabase();

            testObject.Enable();

            Assert.Equal(GetTablesWithChangeTrackingEnabledFromDatabase().Count, testObject.TablesRequiringChangeTracking.Count);
        }

        /// <summary>
        /// Helper method to determine whether change tracking is enabled on the database
        /// </summary>
        private bool IsDatabaseChangeTrackingEnabled()
        {
            const string changeTrackingQueryFormat =
                @"SELECT COUNT(0)
                    FROM SYS.CHANGE_TRACKING_DATABASES
                    WHERE database_id = DB_ID('{0}')";

            using (DbConnection con = SqlSession.Current.OpenConnection())
            {
                using (DbCommand cmd = DbCommandProvider.Create(con))
                {
                    cmd.CommandText = string.Format(changeTrackingQueryFormat, cmd.Connection.Database);
                    int count = (int) cmd.ExecuteScalar();

                    // Consider change tracking disabled if the current database was not in the list of
                    // change tracking databases in SQL Server
                    return count > 0;
                }
            }
        }

        /// <summary>
        /// Helper method that issues a query to disable change tracking on all tables.
        /// </summary>
        private void DisableChangeTrackingOnAllTables()
        {
            using (DbConnection con = SqlSession.Current.OpenConnection())
            {
                using (DbCommand cmd = DbCommandProvider.Create(con))
                {
                    cmd.CommandText = string.Format(DisableChangeTrackingOnAllTablesQuery, cmd.Connection.Database);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Helper method that issues a query to disable change on the database.
        /// </summary>
        private void DisableChangeTrackingOnDatabase()
        {
            using (DbConnection con = SqlSession.Current.OpenConnection())
            {
                using (DbCommand cmd = DbCommandProvider.Create(con))
                {
                    cmd.CommandText = string.Format("ALTER DATABASE {0} SET CHANGE_TRACKING = OFF", cmd.Connection.Database);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Helper method that finds all the table names that have change tracking turned on.
        /// </summary>
        private List<string> GetTablesWithChangeTrackingEnabledFromDatabase()
        {
            List<string> tableNames = new List<string>();

            using (DbConnection con = SqlSession.Current.OpenConnection())
            {
                using (DbCommand cmd = DbCommandProvider.Create(con))
                {
                    cmd.CommandText = string.Format(TablesWithChangeTrackingQuery, cmd.Connection.Database);

                    using (IDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tableNames.Add(reader.GetString(0));
                        }
                    }
                }
            }

            return tableNames;
        }

        public void Dispose() => context.Dispose();
    }
}
