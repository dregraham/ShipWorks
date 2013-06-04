using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Connection;
using System.Reflection;
using ShipWorks.Filters.Content.SqlGeneration;
using System.Threading;

namespace ShipWorks.Data
{
    /// <summary>
    /// Wraps the global SystemDataEntity object
    /// </summary>
    public static class SystemData
    {
        static SystemDataEntity systemData;
        static bool needCheckForChanges = false;

        /// <summary>
        /// Initialize for the currently logged on user
        /// </summary>
        public static void InitializeForCurrentDatabase()
        {
            systemData = new SystemDataEntity(true);
            SqlAdapter.Default.FetchEntity(systemData);

            needCheckForChanges = false;
        }

        /// <summary>
        /// Check the database for the latest SystemData
        /// </summary>
        public static void CheckForChangesNeeded()
        {
            needCheckForChanges = true;
        }

        /// <summary>
        /// Get the current value of SystemData
        /// </summary>
        public static SystemDataEntity Fetch()
        {
            if (needCheckForChanges)
            {
                SqlAdapter.Default.FetchEntity(systemData);
                needCheckForChanges = false;
            }

            return EntityUtility.CloneEntity(systemData);
        }

        /// <summary>
        /// Save the given entity as the current system data
        /// </summary>
        public static void Save(SystemDataEntity entity)
        {
            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.SaveAndRefetch(entity);

                Interlocked.Exchange(ref systemData, EntityUtility.CloneEntity(entity));
            }

            needCheckForChanges = false;
        }

        /// <summary>
        /// Create a single instance of the data row for a new shipworks database instance
        /// </summary>
        public static void CreateInstance(SqlAdapter adapter)
        {
            SystemDataEntity system = new SystemDataEntity();
            system.SystemDataID = true;

            // Unique identifier for the database. Will never change.
            system.DatabaseID = Guid.NewGuid();

            // Stored as the local, date only UTC portion
            system.DateFiltersLastUpdate = DateTime.Now.Date;

            // No templates installed yet
            system.TemplateVersion = "0.0.0.0";

            adapter.SaveEntity(system);
        }
    }
}
