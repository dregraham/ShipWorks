using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;

namespace ShipWorks.Stores.Orders.Archive
{
    /// <summary>
    /// Class used for generating SQL for order archiving
    /// </summary>
    [Component]
    public class OrderArchiveSqlGenerator : IOrderArchiveSqlGenerator
    {
        /// <summary>
        /// Generate and return SQL for copying a database
        /// </summary>
        public string CopyDatabaseSql(string newDatabasename, DateTime selectedArchivalDate, string sourceDatabasename)
        {
            string archivalSettingsXml = $@"
                <ArchivalSettings>
                  <ArchivalSetting>
                    <DateArchived>{DateTime.UtcNow}</DateArchived>
                    <SelectedArchivalDate>{selectedArchivalDate}</SelectedArchivalDate>
                    <NeedsFilterRegeneration>true</NeedsFilterRegeneration>
                    <SourceShipWorksVersion>{Application.ProductVersion}</SourceShipWorksVersion>
                    <SourceDatabaseName>{sourceDatabasename}</SourceDatabaseName>
                  </ArchivalSetting>
                </ArchivalSettings>
                ";

            return ResourceUtility.ReadString("ShipWorks.Stores.Orders.Archive.CopyDatabase.sql")
                .Replace("%destinationDatabaseName%", newDatabasename)
                .Replace("%archivalSettingsXml%", archivalSettingsXml);
        }

        /// <summary>
        /// Generate and return SQL for archiving order data
        /// </summary>
        public string ArchiveOrderDataSql(string databasename, DateTime maxOrderDate, OrderArchiverOrderDataComparisonType comparisonType)
        {
            string sqlToFormat = ResourceUtility.ReadString("ShipWorks.Stores.Orders.Archive.ArchiveOrderData.sql");

            string finalSql = sqlToFormat.Replace("%databaseName%", databasename)
                .Replace("%orderDate%", maxOrderDate.Date.ToString("yyyy-MM-dd HH:mm:ss"))
                .Replace("%orderDateComparer%", comparisonType == OrderArchiverOrderDataComparisonType.LessThan ? "<" : ">=");

            return finalSql;
        }

        /// <summary>
        /// Generate and return SQL for synching order data in the live database.
        /// </summary>
        public string SyncOrderDataSql(string databasename, DateTime maxOrderDate, OrderArchiverOrderDataComparisonType comparisonType)
        {
            string sqlToFormat = ResourceUtility.ReadString("ShipWorks.Stores.Orders.Archive.ArchiveOrderData.sql");

            return sqlToFormat.Replace("%databaseName%", databasename)
                .Replace("%orderDate%", maxOrderDate.Date.ToString("yyyy-MM-dd HH:mm:ss"))
                .Replace("%orderDateComparer%", comparisonType == OrderArchiverOrderDataComparisonType.LessThan ? "<" : ">=");
        }

        /// <summary>
        /// Generate SQL for generating archive triggers
        /// </summary>
        private string ArchiveTriggersSql()
        {
            return ResourceUtility.ReadString("ShipWorks.Stores.Orders.Archive.GetArchiveTriggers.sql")
                .Replace("%readonlyTableNames%", string.Join(",", OrderArchiveTableWritability.ReadonlyTableNames));
        }

        /// <summary>
        /// Generate sql to enable archive triggers, making the database "readonly"
        /// </summary>
        public async Task<string> EnableArchiveTriggersSql(ISqlAdapter adapter)
        {
            List<string> enableTriggerSqls = new List<string>();
            string sql = ArchiveTriggersSql();

            using (IRetrievalQuery query = new RetrievalQuery(new SqlCommand(sql)))
            {
                using (IDataReader dataReader = await adapter.FetchDataReaderAsync(query, CommandBehavior.Default, CancellationToken.None).ConfigureAwait(false))
                {
                    while (dataReader.Read())
                    {
                        enableTriggerSqls.Add(dataReader[0].ToString());
                    }
                }
            }

            return string.Join(Environment.NewLine, enableTriggerSqls);
        }

        /// <summary>
        /// Generate sql to disable archive triggers, making the database "writable"
        /// </summary>
        public async Task<string> DisableArchiveTriggersSql(ISqlAdapter adapter)
        {
            List<string> disableTriggerSqls = new List<string>();
            string sql = ArchiveTriggersSql();

            using (IRetrievalQuery query = new RetrievalQuery(new SqlCommand(sql)))
            {
                using (IDataReader dataReader = await adapter.FetchDataReaderAsync(query, CommandBehavior.Default, CancellationToken.None).ConfigureAwait(false))
                {
                    while (dataReader.Read())
                    {
                        disableTriggerSqls.Add(dataReader[1].ToString());
                    }
                }
            }

            return string.Join(Environment.NewLine, disableTriggerSqls);
        }
    }
}
