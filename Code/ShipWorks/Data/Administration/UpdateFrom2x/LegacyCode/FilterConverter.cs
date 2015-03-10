using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Common.Threading;
using ShipWorks.Users.Audit;
using System.Transactions;
using System.Data.SqlClient;
using ShipWorks.Data.Connection;
using Interapptive.Shared.Data;
using ShipWorks.Filters;
using ShipWorks.Data.Model.EntityClasses;
using log4net;
using ShipWorks.Stores;
using System.Data;
using System.Xml.Linq;
using ShipWorks.Filters.Content.Conditions;
using ShipWorks.Templates;

namespace ShipWorks.Data.Administration.UpdateFrom2x.LegacyCode
{
    /// <summary>
    /// Responsible for converting 2x filters to 3x filters
    /// </summary>
    public static class FilterConverter
    {
        static readonly ILog log = LogManager.GetLogger(typeof(FilterConverter));

        /// <summary>
        /// Convert all the 2x filters to 3x filters
        /// </summary>
        public static void ConvertFilters(ProgressItem progress)
        {
            progress.Starting();
            progress.Detail = "Preparing...";

            // make sure we get the templates up-to-date before entering the transaction.  Touching Tree gets this done.
            TemplateTree currentTemplates = TemplateManager.Tree;

            // To use FilterLayout there needs to be a user in scope.  Since there is really no active user right now, we just use the SuperUser
            using (AuditBehaviorScope auditScope = new AuditBehaviorScope(AuditBehaviorUser.SuperUser, new AuditReason(AuditReasonType.Default), AuditState.Disabled))
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, SqlCommandProvider.DefaultTimeout))
                {
                    // Update the top-level order and customer nodes
                    UpdateTopLevelCounts();

                    try
                    {
                        // We need to push a new scope for the layout context, b\c if the user ends up cancelling the wizard, it needs to be restored to the
                        // way it was.  And if it doesnt, the layout context gets reloaded anyway.
                        FilterLayoutContext.PushScope();

                        // Load the flat list of all v2 filters
                        progress.Detail = "Converting filters...";
                        List<FilterV2> v2Filters = LoadOldFilters();

                        // Create all the filters with the appropriate layout
                        progress.PercentComplete = 50;
                        CreateFilters(LoadV2Layout(FilterTarget.Orders, v2Filters), FilterLayoutContext.Current.FindNode(BuiltinFilter.GetTopLevelKey(FilterTarget.Orders)), progress);
                        CreateFilters(LoadV2Layout(FilterTarget.Customers, v2Filters), FilterLayoutContext.Current.FindNode(BuiltinFilter.GetTopLevelKey(FilterTarget.Customers)), progress);
                    }
                    finally
                    {
                        FilterLayoutContext.PopScope();
                    }

                    using (SqlConnection con = SqlSession.Current.OpenConnection())
                    {
                        using (SqlCommand cmd = SqlCommandProvider.Create(con))
                        {
                            cmd.CommandText = "DELETE v2m_Filters";
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // done with transaction
                    scope.Complete();
                }
            }

            progress.Detail = "Done";
            progress.PercentComplete = 100;
            progress.Completed();
        }

        /// <summary>
        /// Update the top-level order and customer counts
        /// </summary>
        private static void UpdateTopLevelCounts()
        {
            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                using (SqlCommand cmd = SqlCommandProvider.Create(con))
                {
                    cmd.CommandText = @"UPDATE FilterNodeContent SET COUNT = (SELECT COUNT(*) FROM [Order]) WHERE FilterNodeContentID = @id";
                    cmd.Parameters.AddWithValue("@id", (long) BuiltinFilter.GetTopLevelKey(FilterTarget.Orders));
                    cmd.ExecuteNonQuery();
                }

                using (SqlCommand cmd = SqlCommandProvider.Create(con))
                {
                    cmd.CommandText = @"UPDATE FilterNodeContent SET COUNT = (SELECT COUNT(*) FROM [Customer]) WHERE FilterNodeContentID = @id";
                    cmd.Parameters.AddWithValue("@id", (long) BuiltinFilter.GetTopLevelKey(FilterTarget.Customers));
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Create all the filters and folders for the given set of nodes using the specified parent
        /// </summary>
        private static void CreateFilters(List<NodeV2> nodes, FilterNodeEntity parentNode, ProgressItem progress)
        {
            // Go through each order filter
            foreach (NodeV2 node in nodes.ToList())
            {
                log.InfoFormat("Migrating filter '{0}' from v2 to v3...", node.Name);

                // Remove from the list to process
                FolderV2 folder = node as FolderV2;
                if (folder != null)
                {
                    // Create the v3 folder and put it where it belongs in the tree
                    FilterEntity v3Folder = FilterHelper.CreateFilterFolderEntity(folder.Name, (FilterTarget) parentNode.Filter.FilterTarget);
                    FilterNodeEntity folderNode = FilterLayoutContext.Current.AddFilter(v3Folder, parentNode, nodes.IndexOf(node))[0];

                    // Now add all it's children
                    CreateFilters(folder.Children, folderNode, progress);
                }

                FilterV2 filter = node as FilterV2;
                if (filter != null)
                {
                    // Create the v3 filter and add it where it belongs to the tree
                    FilterEntity v3Filter = FilterHelper.CreateFilterEntity(filter.Name, filter.Definition);
                    FilterLayoutContext.Current.AddFilter(v3Filter, parentNode, nodes.IndexOf(node));
                }
            }
        }

        /// <summary>
        /// Create the filter layout tree materialization for the given target, built from the specified list of filters
        /// </summary>
        private static List<NodeV2> LoadV2Layout(FilterTarget filterTarget, List<FilterV2> v2Filters)
        {
            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                // If there's more than one pick the biggest one
                SqlCommand cmd = SqlCommandProvider.Create(con);
                cmd.CommandText = @"SELECT TOP(1) FilterLayout FROM [v2m_StoreFilterLayoutXml] ORDER BY LEN(FilterLayout) DESC";
                string layoutXml = (string) cmd.ExecuteScalar();

                return FilterTreeLayoutLoader.LoadLayout(layoutXml, filterTarget, v2Filters);
            }
        }

        /// <summary>
        /// Load all the old filters
        /// </summary>
        private static List<FilterV2> LoadOldFilters()
        {
            // We need the list of storetypes, so we know what filters we can get rid of
            List<StoreTypeCode> storeTypes = GetPresentStoreTypes();

            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                FilterDefinitionLoader.Initialize(con);

                List<FilterV2> filters = new List<FilterV2>();

                SqlDataAdapter adapter = new SqlDataAdapter("SELECT FilterID, FilterName, FilterXML FROM v2m_Filters", con);
                DataTable filterTable = new DataTable();
                adapter.Fill(filterTable);

                // Create a new filter object for each loaded row
                foreach (DataRow row in filterTable.Rows)
                {
                    string v2Filterxml = (string) row[2];

                    FilterV2 filter = new FilterV2();
                    filter.Name = (string) row[1];
                    filter.Definition = FilterDefinitionLoader.CreateDefinition(v2Filterxml);

                    if (!filter.Definition.IsRelevantToStoreTypes(storeTypes))
                    {
                        log.WarnFormat("Not converting filter '{0}' since it contains a condition for a non-present store type.", filter.Name);
                    }
                    else if (!CheckV2Compatibility(v2Filterxml, storeTypes))
                    {
                        log.WarnFormat("Not converting filter '{0}' since it specified a compatibilty for a store that doesn't exist.", filter.Name);
                    }
                    else if (IsRestoredFromArchiveFilter(filter))
                    {
                        log.WarnFormat("Not converting filter '{0}' since it is the 'Restored from Archive' filter.", filter.Name);
                    }
                    else
                    {
                        filters.Add(filter);
                    }
                }

                return filters;
            }
        }

        /// <summary>
        /// V2 filters have an editing option to specify what store types they are or are not compatible with.  This checks to see if what is specified is value
        /// for the given storeTypes list.
        /// </summary>
        private static bool CheckV2Compatibility(string v2Filterxml, List<StoreTypeCode> storeTypes)
        {
            XElement v2Xml = XElement.Parse(v2Filterxml);

            string storeTypeName = (string) v2Xml.Attribute("StoreType");
            bool compatible = (string) v2Xml.Attribute("CompatibleType") != "Not";

            StoreTypeCode typeCode = StoreTypeCode.Invalid;
            foreach (StoreType storeType in StoreTypeManager.StoreTypes)
            {
                if (storeType.StoreSafeName == storeTypeName)
                {
                    typeCode = storeType.TypeCode;
                    break;
                }
            }

            if (typeCode == StoreTypeCode.Invalid)
            {
                return true;
            }

            if (compatible)
            {
                return storeTypes.Contains(typeCode);
            }
            else
            {
                return !storeTypes.Contains(typeCode) || storeTypes.Count > 1;
            }
        }

        /// <summary>
        /// Indicate true if the given filter represents the default Restored from Arhcive filter
        /// </summary>
        private static bool IsRestoredFromArchiveFilter(FilterV2 filter)
        {
            if (filter.Definition.RootContainer.SecondGroup != null)
            {
                return false;
            }

            if (filter.Definition.RootContainer.FirstGroup.Conditions.Count != 1)
            {
                return false;
            }

            NotSupportedV2Condition notSupported = filter.Definition.RootContainer.FirstGroup.Conditions[0] as NotSupportedV2Condition;
            if (notSupported != null)
            {
                if (notSupported.Detail.Contains("Restored from Archive"))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Get all the StoreType's present in the current database
        /// </summary>
        private static List<StoreTypeCode> GetPresentStoreTypes()
        {
            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT DISTINCT(TypeCode) FROM [Store]", con);
                DataTable table = new DataTable();
                adapter.Fill(table);

                return table.Rows.Cast<DataRow>().Select(r => (StoreTypeCode) (int) r[0]).ToList();
            }
        }
    }
}
