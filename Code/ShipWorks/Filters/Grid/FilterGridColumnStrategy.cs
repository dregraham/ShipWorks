using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Grid;
using System.Windows.Forms;
using ShipWorks.Filters.Controls;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Data.Connection;
using ShipWorks.Users;
using System.ComponentModel;

namespace ShipWorks.Filters.Grid
{
    /// <summary>
    /// Column settings persistence strategy for the filter grid
    /// </summary>
    public class FilterGridColumnStrategy : EntityGridColumnStrategy
    {
        // Active column settings used for column generation
        FilterNodeColumnSettings activeSettings;

        // Remember the last sort to support the user settings
        Guid lastSortColumnID = Guid.Empty;
        ListSortDirection lastSortOrder = ListSortDirection.Descending;

        /// <summary>
        /// Load the full set of configured grid columns into the grid.
        /// </summary>
        public override void LoadColumns(EntityGrid grid)
        {
            if (((FilterEntityGrid) grid).ActiveFilterNode == null)
            {
                return;
            }

            // Fetch the latest cached grid layout
            activeSettings = FilterNodeColumnManager.GetUserSettings(((FilterEntityGrid) grid).ActiveFilterNode);

            GridColumnLayout layout = activeSettings.EffectiveLayout;

            // Add the columns
            grid.Columns.Clear();
            grid.Columns.AddRange(layout.CreateSandGridColumns().OrderByDescending(c=>c.Visible).ToArray());

            // Apply the detail settings
            grid.DetailViewSettings = layout.DetailViewSettings;
        }

        /// <summary>
        /// Persist the set of grid columns and settings from the grid.
        /// </summary>
        public override void SaveColumns(EntityGrid grid)
        {
            if (activeSettings == null)
            {
                return;
            }

            // This is to support the UserSetting for FilterInitialSortType.CurrentUsed
            if (grid.SortColumn != null)
            {
                lastSortColumnID = ((EntityGridColumn) grid.SortColumn).ColumnGuid;
                lastSortOrder = grid.SortDirection;
            }

            using (SqlAdapter adapter = new SqlAdapter(true))
            {
                activeSettings.EffectiveLayout.SetSandGridColumnState(grid.Columns.ToArray());
                activeSettings.Save(adapter);

                adapter.Commit();
            }
        }

        /// <summary>
        /// Clears the active settings so they aren't attempted to be saved on subsequent save requests.
        /// </summary>
        public void Clear()
        {
            activeSettings = null;
        }

        /// <summary>
        /// Apply the user-configured initial sort to the given grid.
        /// </summary>
        public override void ApplyInitialSort(EntityGrid grid)
        {
            if (((FilterEntityGrid) grid).ActiveFilterNode == null)
            {
                return;
            }

            FilterInitialSortType initialSort = (FilterInitialSortType) UserSession.User.Settings.FilterInitialSortType;

            ListSortDirection sortDirection = ListSortDirection.Descending;
            EntityGridColumn sortColumn = null;

            // If its the current sort
            if (initialSort == FilterInitialSortType.CurrentSort)
            {
                sortColumn = FindColumn(grid, lastSortColumnID, true);
                sortDirection = lastSortOrder;
            }

            GridColumnLayout columnLayout = activeSettings.EffectiveLayout;

            // See if the last active sort is visible
            if (initialSort == FilterInitialSortType.LastActiveSort ||
               (initialSort == FilterInitialSortType.CurrentSort && sortColumn == null))
            {
                sortColumn = FindColumn(grid, columnLayout.LastSortColumnGuid, true);
                sortDirection = columnLayout.LastSortOrder;
            }

            // If we don't have a sort yet, try the default
            if (sortColumn == null)
            {
                sortColumn = FindColumn(grid, columnLayout.DefaultSortColumnGuid, true);
                sortDirection = columnLayout.DefaultSortOrder;
            }

            // If we still don't have a sort yet, use the first visible column
            if (sortColumn == null)
            {
                if (grid.Columns.DisplayColumns.Length > 0)
                {
                    sortColumn = (EntityGridColumn) grid.Columns.DisplayColumns[0];
                    sortDirection = ListSortDirection.Descending;
                }
            }

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
            if (((FilterEntityGrid) grid).ActiveFilterNode == null)
            {
                return null;
            }
            
            FilterNodeColumnEditor editor = new FilterNodeColumnEditor();
            editor.LoadSettings(activeSettings);

            // Any time there is a change we have to call the callback
            editor.SettingsChanged += delegate { interactiveEditCallback(); };

            return editor;
        }

        /// <summary>
        /// Create Form that can be used for modal editing of the column settings.
        /// </summary>
        public override Form CreateModalEditor()
        {
            throw new NotSupportedException("The MainGrid is responsible for opening the modal editor.");
        }

        /// <summary>
        /// Find the column with the given ID.  Returns null if not found.
        /// </summary>
        private EntityGridColumn FindColumn(EntityGrid grid, Guid columnID, bool mustBeVisible)
        {
            foreach (EntityGridColumn column in grid.Columns)
            {
                if (column.ColumnGuid == columnID && (column.Visible || !mustBeVisible))
                {
                    return column;
                }
            }

            return null;
        }
    }
}
