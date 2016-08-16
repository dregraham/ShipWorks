using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Autofac;
using Interapptive.Shared.Data;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.FactoryClasses;

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
                    new SqlChangeTracking().Enable();

                    result = SqlCommandProvider.ExecuteScalar(con, "SELECT CHANGE_TRACKING_CURRENT_VERSION()");

                    if (result is DBNull)
                    {
                        throw new InvalidOperationException("Change tracking is not enabled and attempt to enable failed.");
                    }
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
            lock (tables)
            {
                List<EntityChangeTrackingChangeset> changes = new List<EntityChangeTrackingChangeset>();
                DataSet dataSet;

                try
                {
                    dataSet = GetChangesFromDatabase();
                }
                catch (SqlException ex)
                {
                    ProcessCheckForChangesSqlException(ex);
                    return tables.Select(EntityChangeTrackingChangeset.LoadAsInvalid).ToList();
                }

                // No tables means we didnt return any results at all - which will only happen if the lastSyncVersion has not changed
                if (dataSet.Tables.Count > 0)
                {
                    for (int index = 0; index < tables.Count; index++)
                    {
                        // The EntityType in tables and tables in dataSet. Tables are in the same order.
                        EntityType entityType = tables[index];
                        DataTable dataTable = dataSet.Tables[index];

                        EntityChangeTrackingChangeset changeset;

                        if (dataTable.Columns.Count == 1)
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
                            // Load the changes from the table
                            changeset = EntityChangeTrackingChangeset.LoadFromChanges(entityType, dataTable);
                        }

                        // Add to the result list
                        changes.Add(changeset);
                    }

                    // The last table returned contains one column and is the lastSyncVersion
                    lastSyncVersion = (long) dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][0];
                }
                else
                {
                    // lastSyncVersion hasn't changed
                    return tables.Select(EntityChangeTrackingChangeset.LoadAsCurrent).ToList();
                }

                return changes;
            }
        }

        /// <summary>
        /// Processes the check for changes SQL exception - Add ChangeTracking to the table if possible.
        /// </summary>
        private static void ProcessCheckForChangesSqlException(SqlException sqlException)
        {
            log.Error("Error in CheckForChanges", sqlException);
            log.Info("Attempting to enable change tracking.");

            if (sqlException.Errors.Cast<SqlError>().Any(error => error.Number == 22105))
            {
                new SqlChangeTracking().Enable();
            }
        }

        /// <summary>
        /// Gets the changes from database.
        /// </summary>
        private DataSet GetChangesFromDatabase()
        {
            DataSet dataSet;
            dataSet = new DataSet();
            string lsvParameter = "@lsv";

            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                using (SqlCommand cmd = SqlCommandProvider.Create(con))
                {
                    cmd.CommandText = syncQuery;
                    cmd.Parameters.AddWithValue(lsvParameter, lastSyncVersion);

                    using (DataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataSet);
                    }
                }
            }

            return dataSet;
        }
    }
}
