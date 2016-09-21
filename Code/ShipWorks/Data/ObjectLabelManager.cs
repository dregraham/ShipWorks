using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Connection;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model;
using Interapptive.Shared.Collections;

namespace ShipWorks.Data
{
    /// <summary>
    /// Provides access to the object labels for each object that needs one.
    /// </summary>
    public static class ObjectLabelManager
    {
        static LruCache<long, ObjectLabel> labelCache;

        /// <summary>
        /// Initialize the cache and reset it for the currently logged in user
        /// </summary>
        public static void InitializeForCurrentSession()
        {
            labelCache = new LruCache<long, ObjectLabel>(1000, TimeSpan.FromSeconds(15));
        }

        /// <summary>
        /// Get the label for the given object
        /// </summary>
        public static ObjectLabel GetLabel(long objectID)
        {
            return GetLabel(objectID, false);
        }

        /// <summary>
        /// Get the label for the given object
        /// </summary>
        public static ObjectLabel GetLabel(long objectID, bool nullOnMissing)
        {
            lock (labelCache)
            {
                ObjectLabel label = labelCache[objectID];
                if (label == null)
                {
                    if (objectID > 0)
                    {
                        ObjectLabelEntity entity = new ObjectLabelEntity(objectID);

                        using (SqlAdapter adapter = new SqlAdapter())
                        {
                            adapter.FetchEntity(entity);
                        }

                        if (entity.Fields.State != EntityState.Fetched)
                        {
                            label = GetMissingLabelPlaceholder(objectID, nullOnMissing);
                        }
                        else
                        {
                            label = new ObjectLabel(entity);
                        }
                    }
                    else if (objectID < 0)
                    {
                        label = new ObjectLabel(new ObjectLabelEntity { EntityID = objectID, Label = GetNewEntityLabelText(EntityUtility.GetEntityType(objectID)) });
                    }
                    else
                    {
                        label = GetMissingLabelPlaceholder(objectID, nullOnMissing);
                    }

                    labelCache[objectID] = label;
                }

                return label;
            }
        }

        /// <summary>
        /// Get an ObjectLabel instance to use as a placeholder for when then given objectID is not in our ObjectLabel table.  This should only
        /// happen if the ID given is a completely bogus ID pulled out of nowhere - like the value zero as a starting value for something
        /// that hasn't been configured yet.
        /// </summary>
        private static ObjectLabel GetMissingLabelPlaceholder(long objectID, bool nullOnMissing)
        {
            if (nullOnMissing)
            {
                return null;
            }
            else
            {
                // When we release ShipWorks 3.0 i think this should throw the exception, since when its released, we shouldnt
                // have any databases where object labels don't exist
                //
                // throw new InvalidOperationException("No label exists for object " + objectID);
                //

                // Can't use EntityUtility.GetEntityType here, b\c we have no idea what type of object this may be - if its even one we know about or not.
                return new ObjectLabel(new ObjectLabelEntity { EntityID = objectID, Label = "Unknown", IsDeleted = true });
            }
        }

        /// <summary>
        /// Get the text to use to describe a new entity of the given type
        /// </summary>
        private static string GetNewEntityLabelText(EntityType entityType)
        {
            switch (entityType)
            {
                case EntityType.CustomerEntity:
                    return "New Customer";

                case EntityType.OrderEntity:
                    return "New Order";

                case EntityType.ShipmentEntity:
                    return "New Shipment";

                case EntityType.OrderItemEntity:
                    return "New Item";
            }

            throw new InvalidOperationException("Unhandled EntityType in GetNewEntityLabelText");
        }
    }
}
