using System;
using System.Data.SqlClient;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Filters.Grid
{
    /// <summary>
    /// Represents a single layout of grid columns, specific to a filter node and user
    /// </summary>
    public class FilterNodeColumnSettings
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(FilterNodeColumnSettings));

        GridColumnDefinitionSet gridDefinitionSet;

        FilterNodeEntity filterNode;
        UserEntity user;

        // The underlying entity we represent
        FilterNodeColumnSettingsEntity settingsEntity;

        // The column layout that holds the column layout information
        GridColumnLayout columnLayout;

        // This means this layout represents a FilterNode that has been deleted, and no save's should be attempted.
        bool isPhantom = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public FilterNodeColumnSettings(FilterNodeEntity filterNode, UserEntity user)
        {
            this.filterNode = filterNode;
            this.user = user;

            gridDefinitionSet = FilterHelper.ConvertToGridColumnDefinitionSet((FilterTarget) filterNode.Filter.FilterTarget);

            // Initialize our own layout entity
            EnsureDefaultSettings();
            LoadSettingsEntity();

            // Ensure all parent layouts have been created, for when Inherited is used.
            EnsureAncestorSettings();
        }

        /// <summary>
        /// The node for which we display columns
        /// </summary>
        public FilterNodeEntity FilterNodeEntity
        {
            get
            {
                return filterNode;
            }
        }

        /// <summary>
        /// The underlying data that this object represents.
        /// </summary>
        public FilterNodeColumnSettingsEntity FilterNodeColumnSettingsEntity
        {
            get
            {
                return settingsEntity;
            }
        }

        #region Loading

        /// <summary>
        /// Ensure every ancestor settings instance is created and ready.
        /// </summary>
        private void EnsureAncestorSettings()
        {
            FilterNodeEntity parent = GetParentNode(filterNode);
            if (parent != null)
            {
                // This parent will then recursively ensure its parent, and so on ensuring all ancestors.
                FilterNodeColumnSettings parentSettungs = GetNodeSettings(parent);
            }
        }

        /// <summary>
        /// Ensure that the default settings for this user settings (if it is one) has ben created.
        /// </summary>
        private void EnsureDefaultSettings()
        {
            // If its specific to a user, ensure that the default layout has already been creatd.
            if (user != null)
            {
                FilterNodeColumnManager.GetDefaultSettings(filterNode);
            }
        }

        /// <summary>
        /// Load the entity instance for the filter\user
        /// </summary>
        private void LoadSettingsEntity()
        {
            long? userID = (user != null) ? (long?) user.UserID : null;

            if (!LoadExistingFromDatabase(userID))
            {
                settingsEntity = CreateSettingsEntity(userID);

                // If its a new node, we can't save this right away
                if (!filterNode.IsNew)
                {
                    try
                    {
                        using (SqlAdapter adapter = new SqlAdapter(true))
                        {
                            // Save the column layout
                            columnLayout.Save(adapter);

                            // Update the reference to the layout
                            settingsEntity.GridColumnLayoutID = columnLayout.GridColumnLayoutID;

                            // Save the settings
                            adapter.SaveAndRefetch(settingsEntity);

                            adapter.Commit();
                        }
                    }
                    catch (ORMQueryExecutionException ex)
                    {
                        SqlException sqlEx = ex.InnerException as SqlException;

                        if (sqlEx != null)
                        {
                            // 2601: Cannot insert duplicate key
                            if (sqlEx.Number == 2601)
                            {
                                log.Info("Race condition creating new settings entity, already inserted since we looked.", ex);

                                LoadExistingFromDatabase(userID);

                                return;
                            }

                            // 547: statement conflicted with constraint
                            if (sqlEx.Number == 547)
                            {
                                log.Info(string.Format("Could not insert settings for filter node {0}, looks like it went away.", filterNode.FilterNodeID), ex);

                                // If the filter node has gone away, we dont even have to try to worry about saving this, so just ignore
                                // completely. The entity itself will still be marked new, which will cause the columns not to try to save
                                // themselves, which prevents a problem there.
                                isPhantom = true;

                                return;
                            }
                        }

                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Load an existing settings entity from the database. If it exists and was loaded, true is returned.  If it does not yet
        /// exist, false is returned.
        /// </summary>
        private bool LoadExistingFromDatabase(long? userID)
        {
            using (SqlAdapter adapter = new SqlAdapter())
            {
                // Try to load existing
                FilterNodeColumnSettingsCollection settings = FilterNodeColumnSettingsCollection.Fetch(adapter,
                    FilterNodeColumnSettingsFields.UserID == userID &
                    FilterNodeColumnSettingsFields.FilterNodeID == filterNode.FilterNodeID);

                // Layout already exists
                if (settings.Count == 1)
                {
                    settingsEntity = settings[0];
                    columnLayout = new GridColumnLayout(settingsEntity.GridColumnLayoutID, null, null);

                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Create a new settings entity, since one does not exist in the database
        /// </summary>
        private FilterNodeColumnSettingsEntity CreateSettingsEntity(long? userID)
        {
            FilterNodeColumnSettingsEntity settingsEntity = new FilterNodeColumnSettingsEntity();

            // The default - for defaults - is to just inherit.
            if (userID == null)
            {
                settingsEntity.FilterNodeID = filterNode.FilterNodeID;
                settingsEntity.UserID = userID;
                settingsEntity.Inherit = true;

                // Create the default column layout
                columnLayout = new GridColumnLayout(gridDefinitionSet, null, null);

                if (FilterHelper.IsBuiltin(filterNode) && !FilterHelper.IsMyFiltersRoot(filterNode))
                {
                    settingsEntity.Inherit = false;
                }
            }
            // For a user-instance, we pull the defaults from the default
            else
            {
                FilterNodeColumnSettings defaultSettings = FilterNodeColumnManager.GetDefaultSettings(filterNode);

                // Copy all the fields
                settingsEntity.Fields = defaultSettings.settingsEntity.Fields.CloneAsDirty();
                settingsEntity.Fields.State = EntityState.New;

                // It will be for this user
                settingsEntity.UserID = userID;

                columnLayout = new GridColumnLayout(gridDefinitionSet, null, null);
                columnLayout.CopyFrom(defaultSettings.columnLayout);
            }

            return settingsEntity;
        }

        /// <summary>
        /// Copy the settings from the given settings to settings layout
        /// </summary>
        public void CopyFrom(FilterNodeColumnSettings copyFromSettings)
        {
            if (copyFromSettings == null)
            {
                throw new ArgumentNullException("copyFromSettings");
            }

            // Copy the layout
            columnLayout.CopyFrom(copyFromSettings.columnLayout);

            // Copy the inheritance.
            settingsEntity.Inherit = copyFromSettings.settingsEntity.Inherit;

            // If inherited, to make it work as expected form a user standpoint, we have to copy from the inherited settings
            if (settingsEntity.Inherit)
            {
                FilterNodeColumnSettings inheritedSettings = GetInheritedSettings();
                if (inheritedSettings != null)
                {
                    inheritedSettings.CopyFrom(copyFromSettings.GetInheritedSettings());
                }
            }
        }

        #endregion

        #region Saving

        /// <summary>
        /// Save the complete state of the settings
        /// </summary>
        public void Save(SqlAdapter adapter)
        {
            if (adapter == null)
            {
                throw new ArgumentNullException("adapter");
            }

            if (!adapter.InSystemTransaction)
            {
                throw new InvalidOperationException("A transaction must be in progress on the adapter.");
            }

            // If we have a potential inhertance, we have to try to save it, even if we are not inherited. Its possible
            // the user made changes to inherited stuff before switching to non-inherited.
            FilterNodeColumnSettings inheritedSettings = GetInheritedSettings();
            if (inheritedSettings != null)
            {
                inheritedSettings.Save(adapter);
            }

            // There is no real underlying data row to save to.
            if (isPhantom)
            {
                return;
            }

            // Save all our direct stuff, even if we are inherited.  Its possible the user changed
            // non-inherited stuff before switching to inherited.
            try
            {
                // Save the layout
                columnLayout.Save(adapter);

                adapter.SaveAndRefetch(settingsEntity, false);
            }
            catch (ORMConcurrencyException ex)
            {
                // Since we are not doing concurrency tracking on FilterNodeColumnSettings, this must mean that the FilterNodeColumnSettings row
                // no longer exists.  So who cares, if its gone, we don't need to save it.  It would go away if the user deleted the node
                // that it used to be fore.
                log.Info(string.Format("FilterNodeColumnSettings {0} seems to have gone away.", settingsEntity.FilterNodeColumnSettingsID), ex);

                isPhantom = true;
            }
        }

        /// <summary>
        /// Used for saving the default settings for a new filter to all the nodes that get created when a filter is created
        /// </summary>
        public void SaveAs(FilterNodeEntity node, SqlAdapter adapter)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }

            if (adapter == null)
            {
                throw new ArgumentNullException("adapter");
            }

            if (!adapter.InSystemTransaction)
            {
                throw new InvalidOperationException("A transaction must be in progress on the adapter.");
            }

            FilterNodeColumnSettingsEntity newEntity = new FilterNodeColumnSettingsEntity();
            newEntity.Fields = settingsEntity.Fields.CloneAsDirty();
            newEntity.FilterNodeID = node.FilterNodeID;

            GridColumnLayout newLayout = new GridColumnLayout(gridDefinitionSet, null, null);
            newLayout.CopyFrom(columnLayout);

            // Save the grid layout
            newLayout.Save(adapter);

            // Set the pointer to the new layout
            newEntity.GridColumnLayoutID = newLayout.GridColumnLayoutID;

            // Save
            adapter.SaveEntity(newEntity, false, false);
        }

        /// <summary>
        /// Cancel any changes that have not yet been saved to the database
        /// </summary>
        public void CancelChanges()
        {
            // If we have a potential inhertance, we have to try to save it, even if we are not inherited. Its possible
            // the user made changes to inherited stuff before switching to non-inherited.
            FilterNodeColumnSettings inheritedSettings = GetInheritedSettings();
            if (inheritedSettings != null)
            {
                inheritedSettings.CancelChanges();
            }

            columnLayout.CancelChanges();
            settingsEntity.RollbackChanges();
        }

        #endregion

        #region Inherited Settings

        /// <summary>
        /// The settiongs entity that is being used for display, based on the inheritance setting
        /// </summary>
        public GridColumnLayout EffectiveLayout
        {
            get
            {
                if (Inherited)
                {
                    return GetInheritedSettings().columnLayout;
                }

                return columnLayout;
            }
        }

        /// <summary>
        /// Indiciates if the columns come from an ancestor folder
        /// </summary>
        public bool Inherited
        {
            get
            {
                return settingsEntity.Inherit;
            }
            set
            {
                if (InheritedColumnsNode == null && value)
                {
                    throw new InvalidOperationException("No parent node to inherit from.");
                }

                settingsEntity.Inherit = value;
            }
        }

        /// <summary>
        /// The node that the columns would get their settings from when inherited
        /// </summary>
        public FilterNodeEntity InheritedColumnsNode
        {
            get
            {
                FilterNodeEntity ancestor = GetParentNode(filterNode);
                while (ancestor != null)
                {
                    FilterNodeColumnSettings ancestorLayout = GetNodeSettings(ancestor);
                    if (!ancestorLayout.Inherited)
                    {
                        return ancestor;
                    }

                    ancestor = GetParentNode(ancestor);
                }

                return null;
            }
        }

        /// <summary>
        /// Get the settings we would inherit from if Inherited was true.  null if nothing to inherit.
        /// </summary>
        private FilterNodeColumnSettings GetInheritedSettings()
        {
            FilterNodeEntity inheritNode = InheritedColumnsNode;
            if (inheritNode == null)
            {
                return null;
            }

            return GetNodeSettings(inheritNode);
        }

        /// <summary>
        /// Get the settings for the specified node.
        /// </summary>
        private FilterNodeColumnSettings GetNodeSettings(FilterNodeEntity node)
        {
            if (user == null)
            {
                return FilterNodeColumnManager.GetDefaultSettings(node);
            }
            else
            {
                return FilterNodeColumnManager.GetUserSettings(node);
            }
        }

        /// <summary>
        /// Get the effective parent node of the given node.  This takes into account that to the user
        /// the root node looks like the parent of the My Filters node.
        /// </summary>
        private FilterNodeEntity GetParentNode(FilterNodeEntity node)
        {
            FilterNodeEntity parent;

            // My Filters and Search\Local both have a virtual parent of the root
            if (FilterHelper.IsMyFiltersRoot(node) || node.Purpose == (int) FilterNodePurpose.Search || node.Purpose == (int) FilterNodePurpose.Quick)
            {
                // FilterLayoutContext ensures that the Context will contain the root nodes.
                FilterNodeEntity root = (FilterNodeEntity) FilterLayoutContext.Current.FindNode(BuiltinFilter.GetTopLevelKey((FilterTarget) node.Filter.FilterTarget));
                if (root == null)
                {
                    throw new InvalidOperationException("No root node found in context.");
                }

                // It doesnt really have a parent, but it has a virtual parent of the root
                parent = root;
            }
            else
            {
                parent = node.ParentNode;
            }

            return parent;
        }

        #endregion
    }
}
