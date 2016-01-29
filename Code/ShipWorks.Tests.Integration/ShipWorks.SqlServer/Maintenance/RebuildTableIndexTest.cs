using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using Xunit;
using Moq;
using ShipWorks.ApplicationCore.ExecutionMode;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Common.Threading;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;
using ShipWorks.Shipping.Settings.Defaults;
using ShipWorks.Shipping.ShipSense;
using ShipWorks.Shipping.ShipSense.Population;
using ShipWorks.Startup;
using ShipWorks.Stores;
using ShipWorks.Templates;
using ShipWorks.Users;
using ShipWorks.Users.Audit;

namespace ShipWorks.Tests.Integration.MSTest.ShipWorks.SqlServer.Maintenance
{
    public class RebuildTableIndexTest
    {
        private readonly Mock<ExecutionMode> executionMode;
        private Mock<IProgressReporter> progressReporter;

        public RebuildTableIndexTest()
        {
            executionMode = new Mock<ExecutionMode>();
            executionMode.Setup(m => m.IsUISupported).Returns(true);

            progressReporter = new Mock<IProgressReporter>();

            Guid swInstance = GetShipWorksInstance();

            if (ApplicationCore.ShipWorksSession.ComputerID == Guid.Empty)
            {
                ContainerInitializer.Initialize();

                ApplicationCore.ShipWorksSession.Initialize(swInstance);
                SqlSession.Initialize();

                Console.WriteLine(SqlSession.Current.Configuration.DatabaseName);
                Console.WriteLine(SqlSession.Current.Configuration.ServerInstance);

                DataProvider.InitializeForApplication(executionMode.Object);
                AuditProcessor.InitializeForApplication();
                
                ShippingSettings.InitializeForCurrentDatabase();
                ShippingProfileManager.InitializeForCurrentSession();
                ShippingDefaultsRuleManager.InitializeForCurrentSession();
                ShippingProviderRuleManager.InitializeForCurrentSession();

                StoreManager.InitializeForCurrentSession();

                UserManager.InitializeForCurrentUser();
                
                UserSession.InitializeForCurrentDatabase(executionMode.Object);

                if (!UserSession.Logon("shipworks", "shipworks", true))
                {
                    throw new Exception("A 'shipworks' account with password 'shipworks' needs to be created.");
                }

                ShippingManager.InitializeForCurrentDatabase();
                LogSession.Initialize();

                TemplateManager.InitializeForCurrentSession();

                CreateTestTable();
            }
        }

        private Guid GetShipWorksInstance()
        {
            Guid instance;

            string instanceFromConfig = System.Configuration.ConfigurationManager.AppSettings["ShipWorksInstanceGuid"];
            if (!string.IsNullOrWhiteSpace(instanceFromConfig))
            {
                instance = Guid.Parse(instanceFromConfig);
            }
            else
            {
                // Fall back to the hard-coded values in the case where the instance value is not found in the
                // configuration file
                switch (Environment.MachineName.ToLower())
                {
                    case "tim-pc":
                        instance = Guid.Parse("{2D64FF9F-527F-47EF-BA24-ECBF526431EE}");
                        break;
                    case "john3610-pc":
                        instance = Guid.Parse("{a721d9e4-fb3b-4a64-a612-8579b1251c95}");
                        break;
                    case "kevin-pc":
                        instance = Guid.Parse("{6db3aa02-32bb-430e-95d2-0c59b3b7417a}");
                        break;
                    case "MSTest-vm":
                        instance = Guid.Parse("{3BAE47D1-6903-428B-BD9D-31864E614709}");
                        break;
                    case "benz-pc":
                        instance = Guid.Parse("{a21e0f50-8eb6-469c-8d23-7632c5cdc652}");
                        break;
                    case "berger-pc":
                        instance = Guid.Parse("{AABB7285-a889-46af-87b8-69c10cdbAABB}");
                        break;
                    case "mirza-pc2":
                        instance = Guid.Parse("{1231F4A9-640C-4E08-A52A-AE3B2C2FB864}");
                        break;
                    default:
                        throw new ApplicationException("Enter your machine and ShipWorks instance guid in ShipSenseLoaderTest()");
                }
            }

            return instance;
        }

        [Fact]
        [Trait("Category", "SqlServer.Maintenance")]
        [Trait("Category", "ContinuousIntegration")]
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
