using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model;
using System.ComponentModel;
using ShipWorks.Data.Connection;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Data;

namespace ShipWorks.Shipping.Settings.Printing
{
    /// <summary>
    /// Manager of PrintOutput entities
    /// </summary>
    public static class ShippingPrintOutputManager
    {
        static TableSynchronizer<ShippingPrintOutputEntity> synchronizer;
        static bool needCheckForChanges = false;

        /// <summary>
        /// Initialize when a user logs in
        /// </summary>
        public static void InitializeForCurrentUser()
        {
            synchronizer = new TableSynchronizer<ShippingPrintOutputEntity>();
            InternalCheckForChanges();
        }

        /// <summary>
        /// The fake FilterNodeID that represents that we always run a rule
        /// </summary>
        public static long FilterNodeAlwaysID
        {
            get { return -999; }
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
                List<ShippingPrintOutputEntity> added = new List<ShippingPrintOutputEntity>();
                List<ShippingPrintOutputEntity> modified = new List<ShippingPrintOutputEntity>();

                if (synchronizer.Synchronize(modified, added))
                {
                    synchronizer.EntityCollection.Sort((int) ShippingPrintOutputFieldIndex.ShippingPrintOutputID, ListSortDirection.Ascending);

                    // Load all the detail records for everything that changed
                    foreach (ShippingPrintOutputEntity outputGroup in modified.Concat(added))
                    {
                        outputGroup.Rules.Clear();

                        using (SqlAdapter adapter = new SqlAdapter())
                        {
                            adapter.FetchEntityCollection(
                                outputGroup.Rules,
                                new RelationPredicateBucket(
                                    ShippingPrintOutputRuleFields.ShippingPrintOutputID == outputGroup.ShippingPrintOutputID));

                            outputGroup.Rules.Sort((int) ShippingPrintOutputRuleFieldIndex.ShippingPrintOutputRuleID, ListSortDirection.Ascending);
                        }
                    }
                }

                needCheckForChanges = false;
            }
        }

        /// <summary>
        /// Return the active list of all output groups for the given shipment type
        /// </summary>
        public static List<ShippingPrintOutputEntity> GetOutputGroups(ShipmentTypeCode shipmentType)
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
        /// Save the given output group
        /// </summary>
        public static void SaveOutputGroup(ShippingPrintOutputEntity outputGroup)
        {
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                SaveOutputGroup(outputGroup, adapter);

                adapter.Commit();
            }
        }
        /// <summary>
        /// Save the given output group
        /// </summary>
        public static void SaveOutputGroup(ShippingPrintOutputEntity outputGroup, SqlAdapter adapter)
        {
            bool dirtyChildren = outputGroup.Rules.Any(r => r.IsDirty);

            // If there are dirty children force the group to change, so that the change_tracking picks it up
            if (dirtyChildren && !outputGroup.IsDirty)
            {
                outputGroup.Fields[(int) ShippingPrintOutputFieldIndex.ShipmentType].IsChanged = true;
                outputGroup.IsDirty = true;
            }

            var dirtyRules = outputGroup.Rules.Where(r => r.IsDirty || r.IsNew).ToList();

            bool anyDirty = adapter.SaveAndRefetch(outputGroup);

            // Save all the references for each dirty child
            foreach (ShippingPrintOutputRuleEntity rule in dirtyRules)
            {
                ObjectReferenceManager.SetReference(
                    rule.ShippingPrintOutputRuleID,
                    "FilterNodeID",
                    rule.FilterNodeID,
                    string.Format("Print settings '{0}' for '{1}'", outputGroup.Name, ShipmentTypeManager.GetType((ShipmentTypeCode) outputGroup.ShipmentType).ShipmentTypeName)
                    );

                ObjectReferenceManager.SetReference(
                    rule.ShippingPrintOutputRuleID,
                    "TemplateID",
                    rule.TemplateID,
                    string.Format("Print settings '{0}' for '{1}'", outputGroup.Name, ShipmentTypeManager.GetType((ShipmentTypeCode) outputGroup.ShipmentType).ShipmentTypeName)
                    );
            }

            if (anyDirty)
            {
                needCheckForChanges = true;
            }
        }

        /// <summary>
        /// Delete the specified rule from the output group
        /// </summary>
        public static void DeleteRule(ShippingPrintOutputRuleEntity rule)
        {
            ShippingPrintOutputEntity outputGroup = rule.ShippingPrintOutput;

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                // Clear any references used by the rule
                ObjectReferenceManager.ClearReferences(rule.ShippingPrintOutputRuleID);

                adapter.DeleteEntity(rule);

                // Force the parent to look dirty so that other shipworks pick up the change
                outputGroup.Fields[(int) ShippingPrintOutputFieldIndex.ShipmentType].IsChanged = true;
                outputGroup.IsDirty = true;

                adapter.SaveAndRefetch(outputGroup);

                adapter.Commit();

                // Once we know it works, remove it from the rule list
                rule.ShippingPrintOutput = null;
            }

            CheckForChangesNeeded();
        }

        /// <summary>
        /// Delete the specified output group
        /// </summary>
        public static void DeleteOutputGroup(ShippingPrintOutputEntity outputGroup)
        {
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                // Delete all the references utilized by all the rules
                foreach (ShippingPrintOutputRuleEntity rule in outputGroup.Rules)
                {
                    ObjectReferenceManager.ClearReferences(rule.ShippingPrintOutputRuleID);
                }

                // Delete the group
                adapter.DeleteEntity(outputGroup);

                adapter.Commit();
            }

            CheckForChangesNeeded();
        }

    }
}
