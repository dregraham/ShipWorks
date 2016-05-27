using System.Collections.Generic;
using System.ComponentModel;
using ShipWorks.ApplicationCore;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Utility;

namespace ShipWorks.Shipping.Settings
{
    /// <summary>
    /// Manages and provides database access to the shipping provider rules.
    /// </summary>
    public class ShippingProviderRuleManager : IShippingProviderRuleManager, IInitializeForCurrentSession
    {
        TableSynchronizer<ShippingProviderRuleEntity> synchronizer;
        bool needCheckForChanges = false;

        /// <summary>
        /// Initialize ShippingProviderRuleManager
        /// </summary>
        public void InitializeForCurrentSession()
        {
            synchronizer = new TableSynchronizer<ShippingProviderRuleEntity>();
            InternalCheckForChanges();
        }

        /// <summary>
        /// Check for any changes made in the database since initialization or the last check
        /// </summary>
        public void CheckForChangesNeeded()
        {
            lock (synchronizer)
            {
                needCheckForChanges = true;
            }
        }

        /// <summary>
        /// Do the actual trip to the database to check for changes
        /// </summary>
        private void InternalCheckForChanges()
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
        public IEnumerable<ShippingProviderRuleEntity> GetRules()
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
        /// Apply the given provider rule
        /// </summary>
        public void SaveRule(ShippingProviderRuleEntity rule)
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
        public void SaveRule(ShippingProviderRuleEntity rule, SqlAdapter adapter)
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
        public void DeleteRule(ShippingProviderRuleEntity rule)
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
