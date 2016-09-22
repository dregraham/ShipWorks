using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Divelements.SandGrid;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Users;

namespace ShipWorks.Data.Grid.Columns
{
    /// <summary>
    /// Simple and standard grid column stategegy where each grid has a set of columns that are configurable
    /// and persisted per user.
    /// </summary>
    public class StandardGridColumnStrategy : EntityGridColumnStrategy
    {
        UserColumnSettingsEntity settings;
        GridColumnLayout layout;

        bool detailViewEnabled = false;

        List<GridColumn> unboundColumns;

        /// <summary>
        /// Creates a new instance of the settings for the given key.  If the settings don't exist yet the specified
        /// callback is used to create the default layout settings.
        /// </summary>
        public StandardGridColumnStrategy(Guid settingsKey, GridColumnDefinitionSet definitionSet, Action<GridColumnLayout> layoutInitializer)
            : this(settingsKey, definitionSet, layoutInitializer, new List<GridColumn>())
        {

        }

        /// <summary>
        /// Creates a new instance of the settings for the given key.  If the settings don't exist yet the specified
        /// callback is used to create the default layout settings.
        /// </summary>
        public StandardGridColumnStrategy(Guid settingsKey, GridColumnDefinitionSet definitionSet, Action<GridColumnLayout> layoutInitializer, List<GridColumn> unboundColumns)
        {
            this.unboundColumns = unboundColumns;

            UserColumnSettingsCollection results = UserColumnSettingsCollection.Fetch(SqlAdapter.Default,
                UserColumnSettingsFields.SettingsKey == settingsKey & UserColumnSettingsFields.UserID == UserSession.User.UserID);

            // We don't pass on the context data provider function as is.  Instead, we wrap it, to give indirection, so that if it changes, the wrapper picks up the change.
            Func<object> contextDataProvider = () => GridColumnApplicabilityContextDataProvider != null ? GridColumnApplicabilityContextDataProvider() : null;

            // Load the existing grid settings if already created
            if (results.Count == 1)
            {
                settings = results[0];
                layout = new GridColumnLayout(settings.GridColumnLayoutID, layoutInitializer, contextDataProvider);
            }
            else
            {
                // Create the settings entity
                settings = new UserColumnSettingsEntity();
                settings.UserID = UserSession.User.UserID;
                settings.SettingsKey = settingsKey;
                settings.InitialSortType = (int) GridInitialSortMethod.DefaultSort;

                // Create a new layout
                layout = new GridColumnLayout(definitionSet, layoutInitializer, contextDataProvider);

                // Whatever got applied for defaults count as the last too, when its first creatd
                layout.LastSortColumnGuid = layout.DefaultSortColumnGuid;
                layout.LastSortOrder = layout.DefaultSortOrder;

                using (SqlAdapter adapter = new SqlAdapter(true))
                {
                    layout.Save(adapter);

                    settings.GridColumnLayoutID = layout.GridColumnLayoutID;
                    adapter.SaveAndRefetch(settings);

                    adapter.Commit();
                }
            }
        }

        /// <summary>
        /// Indicates if the user should be presented with options for configuring the Detail View of the grid.
        /// </summary>
        public bool DetailViewEnabled
        {
            get { return detailViewEnabled; }
            set { detailViewEnabled = value; }
        }

        /// <summary>
        /// Load the full set of configured grid columns into the grid.
        /// </summary>
        public override void LoadColumns(EntityGrid grid)
        {
            grid.Columns.Clear();
            grid.Columns.AddRange(layout.CreateSandGridColumns());

            grid.Columns.AddRange(unboundColumns.ToArray());

            if (detailViewEnabled)
            {
                grid.DetailViewSettings = layout.DetailViewSettings;
            }
        }

        /// <summary>
        /// Persist the set of grid columns and settings from the grid .
        /// </summary>
        public override void SaveColumns(EntityGrid grid)
        {
            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                // Save the column state.  The OfType is to filter out any unbound columns that have been added
                layout.SetSandGridColumnState(grid.Columns.OfType<EntityGridColumn>().ToArray());
                layout.Save(adapter);

                adapter.Commit();
            }
        }

        /// <summary>
        /// Apply the user-configured initial sort to the given grid.
        /// </summary>
        public override void ApplyInitialSort(EntityGrid grid)
        {
            bool useDefault = ((GridInitialSortMethod) settings.InitialSortType) == GridInitialSortMethod.DefaultSort;

            Guid sortColumnGuid = useDefault ? layout.DefaultSortColumnGuid : layout.LastSortColumnGuid;
            ListSortDirection sortDirection = useDefault ? layout.DefaultSortOrder : layout.LastSortOrder;

            // Find the column to sort by
            EntityGridColumn sortColumn = grid.Columns.OfType<EntityGridColumn>().SingleOrDefault(c => c.ColumnGuid == sortColumnGuid);

            // Apply sort
            grid.SortColumn = sortColumn;
            grid.SortDirection = sortDirection;
        }

        /// <summary>
        /// Create a UserControl for editing grid settings in a modeless interactive manner.  Any changes
        /// should be cause immediate invocation of the given callback.
        /// </summary>
        public override UserControl CreatePopupEditor(EntityGrid grid, MethodInvoker interactiveEditCallback)
        {
            GridColumnLayoutEditor editor = new GridColumnLayoutEditor();
            editor.LoadGridColumnLayout(layout);

            // Any time there is a change we have to call the callback
            editor.GridColumnLayoutChanged += delegate { interactiveEditCallback(); };

            return editor;
        }

        /// <summary>
        /// Create Form that can be used for modal editing of the column settings.
        /// </summary>
        public override Form CreateModalEditor()
        {
            return new GridColumnSettingsDlg(settings, layout, detailViewEnabled);
        }
    }
}
