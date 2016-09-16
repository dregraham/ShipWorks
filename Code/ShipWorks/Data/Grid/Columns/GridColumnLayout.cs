using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using Divelements.SandGrid;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Grid.DetailView;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Data.Grid.Columns
{
    /// <summary>
    /// Represents a single layout of grid columns
    /// </summary>
    public class GridColumnLayout
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(GridColumnLayout));

        GridColumnDefinitionSet definitionSet;

        // The underlying data storage
        GridColumnLayoutEntity layoutEntity;

        // The collection of columns
        GridColumnPositionList columnPositions;

        // The view settings for the grid
        DetailViewSettings detailViewSettings;

        // The initializer to adjust the initial set of layout columns
        Action<GridColumnLayout> layoutInitializer;

        // Provides context data that is passed to the applicability test for determining what columns should be available to the user
        Func<object> applicabilityContextDataProvider;

        /// <summary>
        /// Create a new GridColumnLayout based on the given definition set.  It is not saved to the database
        /// </summary>
        public GridColumnLayout(GridColumnDefinitionSet definitionSet, Action<GridColumnLayout> layoutInitializer, Func<object> applicabilityContextDataProvider)
        {
            this.definitionSet = definitionSet;

            List<GridColumnDefinition> definitions = new List<GridColumnDefinition>(GridColumnDefinitionManager.GetColumnDefinitions(definitionSet));

            // Initialize our own layout entity
            layoutEntity = CreateLayoutEntity(definitions);
            layoutEntity.DefinitionSet = (int) definitionSet;

            // Create the detail view
            detailViewSettings = new DetailViewSettings(layoutEntity.DetailViewSettings);

            // Create the column list
            columnPositions = new GridColumnPositionList(layoutEntity, definitions);

            // Save the applicability provider
            this.applicabilityContextDataProvider = applicabilityContextDataProvider != null ? applicabilityContextDataProvider : () => null;

            // Apply initialization
            this.layoutInitializer = layoutInitializer != null ? layoutInitializer : (GridColumnLayout layout) => { };
            this.layoutInitializer(this);
        }

        /// <summary>
        /// Create an instance that maps to the specified ID
        /// </summary>
        public GridColumnLayout(long layoutID, Action<GridColumnLayout> layoutInitializer, Func<object> applicabilityContextDataProvider)
        {
            layoutEntity = new GridColumnLayoutEntity(layoutID);
            this.layoutInitializer = layoutInitializer != null ? layoutInitializer : (GridColumnLayout layout) => { };

            // Save the applicability provider
            this.applicabilityContextDataProvider = applicabilityContextDataProvider != null ? applicabilityContextDataProvider : () => null;

            using (SqlAdapter adapter = new SqlAdapter())
            {
                adapter.FetchEntity(layoutEntity);
            }

            if (layoutEntity.Fields.State != EntityState.Fetched)
            {
                throw new InvalidOperationException(string.Format("Could not found GridColumnLayout {0}.", layoutID));
            }

            this.definitionSet = (GridColumnDefinitionSet) layoutEntity.DefinitionSet;

            // Create the detail view
            detailViewSettings = new DetailViewSettings(layoutEntity.DetailViewSettings);

            columnPositions = new GridColumnPositionList(
                layoutEntity,
                new List<GridColumnDefinition>(GridColumnDefinitionManager.GetColumnDefinitions(definitionSet)));
        }

        /// <summary>
        /// Returns the target grid type for the layout columns
        /// </summary>
        public GridColumnDefinitionSet DefinitionSet
        {
            get { return definitionSet; }
        }

        /// <summary>
        /// The underlying ID of the GridColumnLayoutEntity
        /// </summary>
        public long GridColumnLayoutID
        {
            get
            {
                return layoutEntity.GridColumnLayoutID;
            }
        }

        /// <summary>
        /// The detail view settings for this grid layout
        /// </summary>
        public DetailViewSettings DetailViewSettings
        {
            get { return detailViewSettings; }
        }

        #region Loading

        /// <summary>
        /// Create a new layout entity, since one does not exist in the database
        /// </summary>
        private GridColumnLayoutEntity CreateLayoutEntity(List<GridColumnDefinition> definitions)
        {
            GridColumnLayoutEntity layoutEntity = new GridColumnLayoutEntity();

            layoutEntity.DefaultSortColumnGuid = definitions[0].ColumnGuid;
            layoutEntity.DefaultSortOrder = (int) System.Data.SqlClient.SortOrder.Descending;

            layoutEntity.LastSortColumnGuid = layoutEntity.DefaultSortColumnGuid;
            layoutEntity.LastSortOrder = layoutEntity.DefaultSortOrder;

            return layoutEntity;
        }

        /// <summary>
        /// Copy the settings from the given layout to this layout
        /// </summary>
        public void CopyFrom(GridColumnLayout copyFromLayout)
        {
            if (copyFromLayout == null)
            {
                throw new ArgumentNullException("copyFromLayout");
            }

            layoutEntity.DefaultSortColumnGuid = copyFromLayout.layoutEntity.DefaultSortColumnGuid;
            layoutEntity.DefaultSortOrder = copyFromLayout.layoutEntity.DefaultSortOrder;

            // Copy the column settings (Needs to be a lower case "c" here (Dont use the ColumnPositions property) - we want the non-inherited columns, in the case that
            // inheritance is on.
            columnPositions.CopyFrom(copyFromLayout.columnPositions);
        }

        /// <summary>
        /// Reset the layout to its default state
        /// </summary>
        public void ResetToDefault()
        {
            GridColumnLayout defaultLayout = new GridColumnLayout(definitionSet, layoutInitializer, applicabilityContextDataProvider);

            CopyFrom(defaultLayout);
        }

        #endregion

        #region Saving

        /// <summary>
        /// Save the complete state of the layout
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

            // Save the detail view settings
            layoutEntity.DetailViewSettings = detailViewSettings.SerializeSettings();

            adapter.SaveAndRefetch(layoutEntity, false);

            // Save the collection
            columnPositions.Save(adapter);
        }

        /// <summary>
        /// Cancel any changes that have not yet been saved to the database
        /// </summary>
        public void CancelChanges()
        {
            layoutEntity.RollbackChanges();
            columnPositions.CancelChanges();
        }

        #endregion

        #region Property Wrappers

        /// <summary>
        /// The column that sorts by default
        /// </summary>
        public Guid DefaultSortColumnGuid
        {
            get
            {
                return layoutEntity.DefaultSortColumnGuid;
            }
            set
            {
                layoutEntity.DefaultSortColumnGuid = value;
            }
        }

        /// <summary>
        /// The default sort order
        /// </summary>
        public ListSortDirection DefaultSortOrder
        {
            get
            {
                return (ListSortDirection) layoutEntity.DefaultSortOrder;
            }
            set
            {
                layoutEntity.DefaultSortOrder = (int) value;
            }
        }

        /// <summary>
        /// The column that was last sorted
        /// </summary>
        public Guid LastSortColumnGuid
        {
            get
            {
                return layoutEntity.LastSortColumnGuid;
            }
            set
            {
                layoutEntity.LastSortColumnGuid = value;
            }
        }

        /// <summary>
        /// The sort order last being used
        /// </summary>
        public ListSortDirection LastSortOrder
        {
            get
            {
                return (ListSortDirection) layoutEntity.LastSortOrder;
            }
            set
            {
                layoutEntity.LastSortOrder = (int) value;
            }
        }

        /// <summary>
        /// The ordered collection of columns in the layout.
        /// </summary>
        public GridColumnPositionList AllColumns
        {
            get
            {
                return columnPositions;
            }
        }

        /// <summary>
        /// The ordered collectino of columns in the layout that are applicable to the current user and data.
        /// </summary>
        public List<GridColumnPosition> ApplicableColumns
        {
            get
            {
                // Run the applicable provider to get the context data
                object contextData = applicabilityContextDataProvider();

                // Return the columns tat are applicable
                return columnPositions.Where(c => c.Definition.IsApplicable(contextData)).ToList();
            }
        }

        #endregion

        #region SandGrid Columns

        /// <summary>
        /// Create the columns that represent the settings for the specified filter node
        /// </summary>
        public GridColumn[] CreateSandGridColumns()
        {
            List<GridColumnPosition> applicableColumns = ApplicableColumns;

            List<GridColumn> gridColumns = new List<GridColumn>();

            // Go through each column in order and add it to the grid if applicable
            foreach (GridColumnPosition layoutColumn in AllColumns)
            {
                GridColumnDefinition definition = layoutColumn.Definition;

                // The definition knows how to create the column
                EntityGridColumn gridColumn = definition.CreateGridColumn();

                // Apply layout properties
                gridColumn.Visible = layoutColumn.Visible && applicableColumns.Contains(layoutColumn);
                gridColumn.Width = layoutColumn.Width;

                // Apply definition properties
                gridColumn.AutoSize = definition.AutoSizeMode;
                gridColumn.AllowWrap = definition.AutoWrap;

                // Add to the list
                gridColumns.Add(gridColumn);
            }

            return gridColumns.ToArray();
        }

        /// <summary>
        /// Save the state of the given columns
        /// </summary>
        public void SetSandGridColumnState(GridColumn[] gridColumns)
        {
            if (gridColumns == null)
            {
                throw new ArgumentNullException("gridColumns");
            }

            // Go through each column
            foreach (EntityGridColumn gridColumn in gridColumns)
            {
                GridColumnPosition columnPosition = AllColumns[gridColumn.ColumnGuid];

                if (columnPosition == null)
                {
                    throw new InvalidOperationException(string.Format("Column position not found for grid column. {0} ({1})", gridColumn.ColumnGuid, gridColumn.HeaderText));
                }

                columnPosition.Width = gridColumn.Width;
                columnPosition.Position = gridColumn.DisplayIndex;

                // See if this is sorted
                if (gridColumn.SortOrder != System.Windows.Forms.SortOrder.None)
                {
                    layoutEntity.LastSortColumnGuid = columnPosition.Definition.ColumnGuid;
                    layoutEntity.LastSortOrder = (int) ((gridColumn.SortOrder == System.Windows.Forms.SortOrder.Ascending) ? ListSortDirection.Ascending : ListSortDirection.Descending);
                }
            }
        }

        #endregion
    }
}
