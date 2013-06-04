using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model;
using ShipWorks.Data.Connection;
using System.Data.SqlClient;
using Interapptive.Shared.Data;
using SD.LLBLGen.Pro.ORMSupportClasses;
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
            lock (tables)
            {
                List<EntityChangeTrackingChangeset> changes = new List<EntityChangeTrackingChangeset>();

                using (SqlConnection con = SqlSession.Current.OpenConnection())
                {
                    SqlCommand cmd = SqlCommandProvider.Create(con);
                    cmd.CommandText = syncQuery;
                    cmd.Parameters.AddWithValue("@lsv", lastSyncVersion);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // No fields means we didnt return any results at all - which will only happen if the lastSyncVersion has not changed
                        if (reader.FieldCount > 0)
                        {
                            // Read the results of each table we are monitoring
                            foreach (EntityType entityType in tables)
                            {
                                EntityChangeTrackingChangeset changeset;

                                // Status code
                                if (reader.FieldCount == 1)
                                {
                                    reader.Read();
                                    string status = reader.GetString(0);

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
                                    changeset = EntityChangeTrackingChangeset.LoadFromChanges(entityType, reader);
                                }

                                // Add to the result list
                                changes.Add(changeset);

                                // Move to the next SQL result set
                                reader.NextResult();
                            }

                            // We should now be positioned on the last SQL result set - which should be the next sync version
                            reader.Read();
                            lastSyncVersion = reader.GetInt64(0);
                        }
                        else
                        {
                            return tables.Select(entityType => EntityChangeTrackingChangeset.LoadAsCurrent(entityType)).ToList();
                        }
                    }
                }

                return changes;
            }
        }
    }
}
