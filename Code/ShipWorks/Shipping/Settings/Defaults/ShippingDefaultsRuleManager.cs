using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model;
using System.ComponentModel;
using ShipWorks.Data.Connection;
using ShipWorks.Data;

namespace ShipWorks.Shipping.Settings.Defaults
{
    /// <summary>
    /// Manager of ShippingDefaultsRule entities
    /// </summary>
    public static class ShippingDefaultsRuleManager
    {
        static TableSynchronizer<ShippingDefaultsRuleEntity> synchronizer;
        static bool needCheckForChanges = false;

        /// <summary>
        /// Initialize when a user logs in
        /// </summary>
        public static void InitializeForCurrentUser()
        {
            synchronizer = new TableSynchronizer<ShippingDefaultsRuleEntity>();
            InternalCheckForChanges();
        }

        /// <summary>
        /// Check for any changes made in the database since initialization or the last check
        /// </summary>
        public static void CheckForChangesNeeded()
        {
            lock (synchronizer)
            {
                needCheckForChanges = true;
            }
        }

        /// <summary>
        /// Do the actual trip to the database to check for changes
        /// </summary>
        private static void InternalCheckForChanges()
        {
            lock (synchronizer)
            {
                if (synchronizer.Synchronize())
                {
                    synchronizer.EntityCollection.Sort((int) ShippingDefaultsRuleFieldIndex.ShippingDefaultsRuleID, ListSortDirection.Ascending);
                }

                needCheckForChanges = false;
            }
        }

        /// <summary>
        /// Return the active list of all rules for the given shipment type
        /// </summary>
        public static List<ShippingDefaultsRuleEntity> GetRules(ShipmentTypeCode shipmentType)
        {
            lock (synchronizer)
            {
                if (needCheckForChanges)
                {
                    InternalCheckForChanges();
                }

                return EntityUtility.CloneEntityCollection(synchronizer.EntityCollection.Where(r => r.ShipmentType == (int) shipmentType));
            }
        }

        /// <summary>
        /// Apply the given profile to the given shipment
        /// </summary>
        public static void SaveRule(ShippingDefaultsRuleEntity rule)
        {
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                SaveRule(rule, adapter);

                adapter.Commit();
            }
        }

        /// <summary>
        /// Apply the given profile to the given shipment
        /// </summary>
        public static void SaveRule(ShippingDefaultsRuleEntity rule, SqlAdapter adapter)
        {
            // If there were changes, make sure that the object references are up-to-date
            if (adapter.SaveAndRefetch(rule))
            {
                ObjectReferenceManager.SetReference(
                    rule.ShippingDefaultsRuleID,
                    "FilterNodeID",
                    rule.FilterNodeID,
                    string.Format("Shipment defaults for '{0}'", ShipmentTypeManager.GetType((ShipmentTypeCode) rule.ShipmentType).ShipmentTypeName)
                    );

                ObjectReferenceManager.SetReference(
                    rule.ShippingDefaultsRuleID,
                    "ShippingProfile",
                    rule.ShippingProfileID,
                    string.Format("Shipment defaults for '{0}'", ShipmentTypeManager.GetType((ShipmentTypeCode) rule.ShipmentType).ShipmentTypeName)
                    );
            }

            CheckForChangesNeeded();
        }

        /// <summary>
        /// Delete the specified rule
        /// </summary>
        public static void DeleteRule(ShippingDefaultsRuleEntity rule)
        {
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                ObjectReferenceManager.ClearReference(rule.ShippingDefaultsRuleID, "FilterNodeID");
                ObjectReferenceManager.ClearReference(rule.ShippingDefaultsRuleID, "ShippingProfile");

                // Delete
                adapter.DeleteEntity(rule);

                adapter.Commit();
            }

            CheckForChangesNeeded();
        }
    }
}
