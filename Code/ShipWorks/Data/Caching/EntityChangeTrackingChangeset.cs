using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model;
using System.Data.SqlClient;

namespace ShipWorks.Data.Caching
{
    /// <summary>
    /// Represents the change set since the last time changes were checked on the entity with SQL Server Change Tracking
    /// </summary>
    public class EntityChangeTrackingChangeset
    {
        EntityType entityType;

        List<long> deletes;
        List<long> inserts;
        List<long> updates;

        /// <summary>
        /// Private constructor
        /// </summary>
        private EntityChangeTrackingChangeset(EntityType entityType)
        {
            this.entityType = entityType;
        }

        /// <summary>
        /// Create a new EntityChangeTrackingChangeset that is valid with no changes (current)
        /// </summary>
        public static EntityChangeTrackingChangeset LoadAsCurrent(EntityType entityType)
        {
            EntityChangeTrackingChangeset changeset = new EntityChangeTrackingChangeset(entityType);
            changeset.deletes = new List<long>();
            changeset.updates = new List<long>();
            changeset.inserts = new List<long>();

            return changeset;
        }

        /// <summary>
        /// Create a new EntityChangeTrackingChangeset that is invalid (changes are unkown, and all data should be considered stale)
        /// </summary>
        public static EntityChangeTrackingChangeset LoadAsInvalid(EntityType entityType)
        {
            return new EntityChangeTrackingChangeset(entityType);
        }

        /// <summary>
        /// Load the results of the given changeset based on the given open reader.  The schema of the reader is defined by the EntityChangeTrackingMonitor
        /// </summary>
        public static EntityChangeTrackingChangeset LoadFromChanges(EntityType entityType, DataTable table)
        {
            if (table == null)
            {
                throw new ArgumentNullException("table");
            }

            EntityChangeTrackingChangeset changeset = new EntityChangeTrackingChangeset(entityType);

            changeset.deletes = new List<long>();
            changeset.inserts = new List<long>();
            changeset.updates = new List<long>();

            foreach (DataRow dataRow in table.Rows)
            {
                long key = (long) dataRow[0];
                string operation = (string) dataRow[1];

                switch (operation)
                {
                    case "D": changeset.deletes.Add(key); break;
                    case "I": changeset.inserts.Add(key); break;
                    case "U": changeset.updates.Add(key); break;
                }
            }
            return changeset;
        }

        /// <summary>
        /// The EntityType the change set is for
        /// </summary>
        public EntityType EntityType
        {
            get { return entityType; }
        }

        /// <summary>
        /// Indicates if the the change set is valid - or if it's been too long due to the retention period and changes couldn't reliably detected, 
        /// then this is false and none of collections will be valid objects.
        /// </summary>
        public bool IsValid
        {
            get { return (deletes != null); }
        }

        /// <summary>
        /// Indicates if there are any changes in the change set.  Will always be false if IsValid is false.
        /// </summary>
        public bool HasChanges
        {
            get
            {
                return IsValid && (deletes.Count + inserts.Count + updates.Count) > 0;
            }
        }

        /// <summary>
        /// The PK's of all deleted entities.  null if IsValid is false.
        /// </summary>
        public List<long> Deletes
        {
            get { return deletes; }
        }

        /// <summary>
        /// The PK's of all inserted entities.  null if IsValid is false.
        /// </summary>
        public List<long> Inserts
        {
            get { return inserts; }
        }

        /// <summary>
        /// The PK's of all updated entities.  null if IsValid is false.
        /// </summary>
        public List<long> Updates
        {
            get { return updates; }
        }
    }
}
