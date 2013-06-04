using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.Custom;
using System.Threading;
using log4net;

namespace ShipWorks.Data.Caching
{
    /// <summary>
    /// Provides versioning for changes to an EntityType.  Any time an update\insert\delete is detected for any entity of a given type, 
    /// the version is incremented.
    /// </summary>
    public class EntityTypeChangeVersion
    {
        static readonly ILog log = LogManager.GetLogger(typeof(EntityTypeChangeVersion));

        EntityType entityType;

        int insertVersion = 0;
        int updateVersion = 0;
        int deleteVersion = 0;

        object locker = new object();

        /// <summary>
        /// Constructor
        /// </summary>
        public EntityTypeChangeVersion(EntityType entityType)
        {
            this.entityType = entityType;
        }

        /// <summary>
        /// The EntityType the versioning applies to
        /// </summary>
        public EntityType EntityType
        {
            get { return entityType; }
        }

        /// <summary>
        /// Incrememnts every time an 'Insert' of the EntityType is detected
        /// </summary>
        public int InsertVersion
        {
            get { lock (locker) return insertVersion; }
        }

        /// <summary>
        /// Incrememnts every time an 'Update' of the EntityType is detected
        /// </summary>
        public int UpdateVersion
        {
            get { lock (locker) return updateVersion; }
        }

        /// <summary>
        /// Incrememnts every time a 'Delete' of the EntityType is detected
        /// </summary>
        public int DeleteVersion
        {
            get { lock (locker) return deleteVersion; }
        }

        /// <summary>
        /// Increment all versions
        /// </summary>
        public void Increment()
        {
            Increment(EntityPersistedAction.Insert);
            Increment(EntityPersistedAction.Update);
            Increment(EntityPersistedAction.Delete);
        }

        /// <summary>
        /// Increment the version represented by the given action
        /// </summary>
        public void Increment(EntityPersistedAction action)
        {
            lock (locker)
            {
                switch (action)
                {
                    case EntityPersistedAction.Insert: insertVersion++; break;
                    case EntityPersistedAction.Update: updateVersion++; break;
                    case EntityPersistedAction.Delete: deleteVersion++; break;
                }
            }
        }
    }
}
