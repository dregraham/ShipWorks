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

namespace ShipWorks.Shipping.Settings
{
    /// <summary>
    /// Manages and provides database access to the shipping provider rules.
    /// </summary>
    public static class ShippingProviderRuleManager
    {
        static TableSynchronizer<ShippingProviderRuleEntity> synchronizer;
        static bool needCheckForChanges = false;

        /// <summary>
        /// Initialize when a user logs in
        /// </summary>
        public static void InitializeForCurrentUser()
        {
            synchronizer = new TableSynchronizer<ShippingProviderRuleEntity>();
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
                    synchronizer.EntityCollection.Sort((int) ShippingProviderRuleFieldIndex.ShippingProviderRuleID, ListSortDirection.Ascending);
                }

                needCheckForChanges = false;
            }
        }

        /// <summary>
        /// Return the active list of all rules for the given shipment type
        /// </summary>
        public static List<ShippingProviderRuleEntity> GetRules()
        {
            lock (synchronizer)
            {
                if (needCheckForChanges)
                {
                    InternalCheckForChanges();
                }

                return EntityUtility.CloneEntityCollection(synchronizer.EntityCollection);
            }
        }

        /// <summary>
        /// Apply the given privder rule
        /// </summary>
        public static void SaveRule(ShippingProviderRuleEntity rule)
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
        public static void SaveRule(ShippingProviderRuleEntity rule, SqlAdapter adapter)
        {
            // If there were changes, make sure that the object references are up-to-date
            if (adapter.SaveAndRefetch(rule))
            {
                ObjectReferenceManager.SetReference(
                    rule.ShippingProviderRuleID,
                    "FilterNodeID",
                    rule.FilterNodeID,
                    "Default shipping provider rules"
                    );
            }

            CheckForChangesNeeded();
        }

        /// <summary>
        /// Delete the specified rule
        /// </summary>
        public static void DeleteRule(ShippingProviderRuleEntity rule)
        {
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                ObjectReferenceManager.ClearReference(rule.ShippingProviderRuleID, "FilterNodeID");

                // Delete
                adapter.DeleteEntity(rule);

                adapter.Commit();
            }

            CheckForChangesNeeded();
        }
    }
}
