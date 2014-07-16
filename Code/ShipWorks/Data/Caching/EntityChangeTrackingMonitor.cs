using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using log4net;
using ShipWorks.Data.Administration.Retry;
using ShipWorks.Data.Model;
using ShipWorks.Data.Connection;
using System.Data.SqlClient;
using Interapptive.Shared.Data;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.SqlServer.Common.Data;

namespace ShipWorks.Data.Caching
{
    /// <summary>
    /// Utilizes SQL Server CHANGE_TRACKING functionality to monitor changes to tables
    /// </summary>
    public class EntityChangeTrackingMonitor
    {
        long lastSyncVersion = -1;

        // The list of tables to monitor
        List<EntityType> tables = new List<EntityType>();

        // The query we use to do the change monitoring
        string syncQuery;

        static readonly ILog log = LogManager.GetLogger(typeof(EntityChangeTrackingMonitor));
        
        /// <summary>
        /// Constructor
        /// </summary>
        public EntityChangeTrackingMonitor()
        {

        }

        /// <summary>
        /// Initialize the change tracking
        /// </summary>
        public void Initialize(IEnumerable<EntityType> tables)
        {
            if (lastSyncVersion >= 0)
            {
                throw new InvalidOperationException("Already initialized.");
            }

            if (tables == null || !tables.Any())
            {
                throw new ArgumentException("No tables were specified for change tracking monitoring.");
            }

            this.tables = tables.ToList();

            this.lastSyncVersion = GetCurrentSyncVersion();
            this.syncQuery = GenerateSyncQuery(tables);
        }

        /// <summary>
        /// The change tracking sync version the database was at on the last sync
        /// </summary>
        public long LastSyncVersion
        {
            get { return lastSyncVersion; }
        }

        /// <summary>
        /// Get the current sync tracking version
        /// </summary>
        private static long GetCurrentSyncVersion()
        {
            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                object result = SqlCommandProvider.ExecuteScalar(con, "SELECT CHANGE_TRACKING_CURRENT_VERSION()");

                if (result is DBNull)
                {
                    throw new InvalidOperationException("Change tracking is not enabled.");
                }

                return (long) result;
            }
        }

        /// <summary>
        /// Generate the query to use for the sync based on the given table list
        /// </summary>
        private static string GenerateSyncQuery(IEnumerable<EntityType> tables)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("DECLARE @nsv bigint");
            sb.AppendLine("SET @nsv = CHANGE_TRACKING_CURRENT_VERSION()");
            sb.AppendLine("IF (@lsv != @nsv) BEGIN");

            // Add in a query for each table
            int count = 0;
            foreach (EntityType entityType in tables)
            {
                string tableName = SqlAdapter.GetTableName(entityType);

                EntityField2 primaryKeyField = (EntityField2) GeneralEntityFactory.Create(entityType).Fields[0];
                string primaryKey = SqlAdapter.GetPersistenceInfo(primaryKeyField).SourceColumnName;

                sb.AppendFormat(
                    "IF (@lsv < CHANGE_TRACKING_MIN_VALID_VERSION(OBJECT_ID('{0}'))) SELECT 'I' as S\n" +
                    "ELSE SELECT {1}, SYS_CHANGE_OPERATION FROM CHANGETABLE(CHANGES [{0}], @lsv) as t{2};\n",
                    tableName,
                    primaryKey,
                    count++);
            }

            sb.AppendLine("SELECT @nsv;");
            sb.AppendLine("END");

            return sb.ToString();
        }

        /// <summary>
        /// Get changes to all monitored entities
        /// </summary>
        public List<EntityChangeTrackingChangeset> CheckForChanges()
        {
            string lsvParameter = "@lsv";

            lock (tables)
            {
                List<EntityChangeTrackingChangeset> changes = new List<EntityChangeTrackingChangeset>();

                try
                {
                    using (SqlConnection con = SqlSession.Current.OpenConnection())
                    {
                        SqlCommand cmd = SqlCommandProvider.Create(con);
                        cmd.CommandText = syncQuery;
                        cmd.Parameters.AddWithValue(lsvParameter, lastSyncVersion);
                        DataSet dataSet = new DataSet();

                        using (DataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataSet);
                        }

                        // There should always be at least 2 tables because the initialize command requires tables to include 1 entitytype.
                        // This would result in 1 table and the last table would be the changeset number returned by the proc.
                        if (dataSet.Tables.Count > 1)
                        {
                            for (int index = 0; index < tables.Count; index++)
                            {
                                // The EntityType in tables and tables in dataSet.Tables are in the same order.
                                EntityType entityType = tables[index];
                                DataTable dataTable = dataSet.Tables[index];

                                EntityChangeTrackingChangeset changeset;

                                if (dataTable != null && dataTable.Columns.Count == 1)
                                {
                                    string status = (string) dataTable.Rows[0][0];

                                    // Invalid
                                    if (status == "I")
                                    {
                                        changeset = EntityChangeTrackingChangeset.LoadAsInvalid(entityType);
                                    }
                                    else
                                    {
                                        throw new InvalidOperationException(string.Format("Unexpected status code reading changes: '{0}'", status));
                                    }
                                }
                                else
                                {
                                    // Load the changes from the reader
                                    changeset = EntityChangeTrackingChangeset.LoadFromChanges(entityType, dataTable);
                                }
                                
                                // Add to the result list
                                changes.Add(changeset);
                            }

                            lastSyncVersion = (long) dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][0];
                        } 
                        else if (dataSet.Tables.Count == 1)
                        {
                            // No table changesets returned. This should never really happen, but mirrors the original code.
                            // Maybe this branch of the if statement should be removed.
                            return tables.Select(EntityChangeTrackingChangeset.LoadAsCurrent).ToList();
                        }
                        else
                        {
                            // this means dataSet.Tables.Count = 0. This should never happen as there should always be at least 2 tables.
                            return tables.Select(EntityChangeTrackingChangeset.LoadAsInvalid).ToList();
                        }
                    }
                }
                catch (SqlException ex)
                {
                    log.Error("Error in CheckForChanges", ex);
                    return tables.Select(EntityChangeTrackingChangeset.LoadAsInvalid).ToList();
                }

                return changes;
            }
        }
    }
}
