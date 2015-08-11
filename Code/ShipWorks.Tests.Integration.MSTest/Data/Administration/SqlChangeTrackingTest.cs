using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Interapptive.Shared.Data;
using Xunit;
using Moq;
using ShipWorks.ApplicationCore.ExecutionMode;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Connection;

namespace ShipWorks.Tests.Integration.MSTest.Data.Administration
{
    public class SqlChangeTrackingTest
    {
        private SqlChangeTracking testObject;

        private ShipWorksInitializer initializer;
        private readonly Mock<ExecutionMode> executionMode;

        public SqlChangeTrackingTest()
        {
            executionMode = new Mock<ExecutionMode>();
            executionMode.Setup(m => m.IsUISupported).Returns(true);

            initializer = new ShipWorksInitializer(executionMode.Object);

            testObject = new SqlChangeTracking();
        }

        private string TablesWithChangeTrackingQuery
        {
            get 
            {
                return @"SELECT SYS.TABLES.NAME 
	
	                    FROM sys.change_tracking_tables

	                    INNER JOIN sys.tables 
		                    ON sys.tables.object_id = sys.change_tracking_tables.object_id
	                    INNER JOIN sys.schemas 
		                    ON sys.schemas.schema_id = sys.tables.schema_id"; 
            }
        }

        private string DisableChangeTrackingOnAllTablesQuery
        {
            get
            {
                return @"DECLARE @SQL NVARCHAR(MAX)='';
                        SELECT @SQL = @SQL + 'ALTER TABLE [' + t.name + '] Disable Change_tracking;'
                        FROM sys.change_tracking_tables ct
	                        JOIN sys.tables t
		                        ON ct.object_id= t.object_id
	                        JOIN sys.schemas s
		                        ON t.schema_id= s.schema_id;

                        PRINT @SQL;
                        EXEC sp_executesql @SQL;";
            }
        }
        
        [Fact]
        [TestCategory("ContinuousIntegration")]
        public void TablesRequiringChangeTracking_CountMatchesNumberOfTablesWithChangeTrackingInDatabase_Test()
        {
            // This assumes a database is being used that has change tracking configured correctly on 
            // the appropriate tables (i.e. seed database)

            // This test in addition to the test for matching actual table names, ensures that the exact
            // tables used in the SqlChangeTracking class match the database. No more. No less.
            Assert.Equal(GetTablesWithChangeTrackingEnabledFromDatabase().Count, testObject.TablesRequiringChangeTracking.Count);
        }

        [Fact]
        [TestCategory("ContinuousIntegration")]
        public void TablesRequiringChangeTracking_MatchesTableNamesWithChangeTrackingInDatabase_Test()
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
        [TestCategory("ContinuousIntegration")]
        public void Enable_TurnsOnChangeTrackingForDatabase_Test()
        {
            // Setup
            DisableChangeTrackingOnAllTables();
            DisableChangeTrackingOnDatabase();

            testObject.Enable();

            Assert.True(IsDatabaseChangeTrackingEnabled());
        }

        [Fact]
        [TestCategory("ContinuousIntegration")]
        public void Enable_TurnsOnChangeTrackingForAllTables_Test()
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

            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                using (SqlCommand cmd = SqlCommandProvider.Create(con))
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
            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                using (SqlCommand cmd = SqlCommandProvider.Create(con))
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
            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                using (SqlCommand cmd = SqlCommandProvider.Create(con))
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

            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                using (SqlCommand cmd = SqlCommandProvider.Create(con))
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
    }
}
