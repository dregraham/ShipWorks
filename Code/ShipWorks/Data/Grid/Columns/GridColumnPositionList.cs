using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using System.Collections;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Model.HelperClasses;
using log4net;
using System.Diagnostics;
using System.Data;
using ShipWorks.Data.Adapter;
using System.Data.SqlClient;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Grid.Columns.ValueProviders;

namespace ShipWorks.Data.Grid.Columns
{
    /// <summary>
    /// Ordered collection of grid columns positions
    /// </summary>
    public class GridColumnPositionList : IEnumerable<GridColumnPosition>
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(GridColumnPositionList));

        // The node and layout the columns belong to
        GridColumnLayoutEntity layoutEntity;

        // The list of columns
        List<GridColumnPosition> columnPositions = new List<GridColumnPosition>();

        // Don't listen to position changed events while true
        bool suspendPositionChanged = false;

        /// <summary>
        /// Instantiate the column collection that belongs to 
        /// </summary>
        public GridColumnPositionList(GridColumnLayoutEntity layoutEntity, ICollection<GridColumnDefinition> definitions)
        {
            this.layoutEntity = layoutEntity;

            // Make a copy of the list so we don't affect the original
            LoadPositions(new List<GridColumnDefinition>(definitions));
        }

        #region Loading

        /// <summary>
        /// Load the column collection
        /// </summary>
        private void LoadPositions(List<GridColumnDefinition> definitions)
        {
            List<GridColumnDefinition> leftToProcess = new List<GridColumnDefinition>(definitions);
            int definitionCount = leftToProcess.Count;

            LoadExistingPositions(leftToProcess);

            // Create any columns we dont have in our layout
            if (CreateMissingPositions(leftToProcess, definitions))
            {
                SetPositionsFromIndex();
            }

            // Save the current state
            if (!layoutEntity.IsNew)
            {
                using (SqlAdapter adapter = new SqlAdapter())
                {
                    foreach (GridColumnPosition columnPosition in columnPositions)
                    {
                        try
                        {
                            columnPosition.Save(adapter);
                        }
                        catch (ORMQueryExecutionException ex)
                        {
                            SqlException sqlEx = ex.InnerException as SqlException;

                            // 2601: Cannot insert duplicate key
                            if (sqlEx != null && sqlEx.Number == 2601)
                            {
                                log.InfoFormat("Race condition loading layout column, created since we just loaded.");
                            }

                            // This should be so rare it will never happen.  If we see it happen from a crash report, we can deal with it then by
                            // loading the existing one from the database when this is detected.
                            throw;
                        }
                    }
                }
            }


            // Should have used up all the definitions exactly
            Debug.Assert(columnPositions.Count == definitionCount);

            // Start listening for position changes
            foreach (GridColumnPosition columnPosition in columnPositions)
            {
                columnPosition.PositionChanged += new EventHandler(OnPositionChanged);
            }
        }

        /// <summary>
        /// Load the GridColumnPosition instances that currently exist in the database
        /// </summary>
        private void LoadExistingPositions(List<GridColumnDefinition> definitionsToUse)
        {
            // If the layout entity is new, then there is no need to try to go to the db
            if (layoutEntity.IsNew)
            {
                return;
            }

            GridColumnPositionCollection entities = GridColumnPositionCollection.Fetch(SqlAdapter.Default,
                GridColumnPositionFields.GridColumnLayoutID == layoutEntity.GridColumnLayoutID);

            GridColumnPositionCollection toDelete = new GridColumnPositionCollection();

            // Load all the ones we know about
            foreach (GridColumnPositionEntity positionEntity in new List<GridColumnPositionEntity>(entities))
            { 
                // Find the definition that represents this column
                GridColumnDefinition definition = FindDefinition(positionEntity.ColumnGuid, definitionsToUse);

                // Column must have been removed from the ShipWorks schema
                if (definition == null)
                {
                    log.InfoFormat("Deleting GridColumnPosition for missing column {0}.", positionEntity.ColumnGuid);
                    entities.Remove(positionEntity);

                    toDelete.Add(positionEntity);
                }
                else
                {
                    columnPositions.Add(new GridColumnPosition(positionEntity, definition));

                    definitionsToUse.Remove(definition);
                }
            }

            SortByPosition();

            if (toDelete.Count > 0)
            {
                SetPositionsFromIndex();

                using (SqlAdapter adapter = new SqlAdapter(true))
                {
                    adapter.DeleteEntityCollection(toDelete);

                    Save(adapter);

                    adapter.Commit();
                }
            }
        }

        /// <summary>
        /// Create a GridColumnPositionEntity for each definition that we don't already have one for
        /// </summary>
        private bool CreateMissingPositions(List<GridColumnDefinition> missingDefinitions, List<GridColumnDefinition> allDefinitions)
        {
            if (missingDefinitions.Count == 0)
            {
                return false;
            }

            bool logMissing = missingDefinitions.Count != allDefinitions.Count;

            // Each remaining definition needs an entity created for it
            foreach (GridColumnDefinition definition in missingDefinitions)
            {
                if (logMissing)
                {
                    log.InfoFormat("Creating missing column {0} ({1})", definition.HeaderText, definition.ColumnGuid);
                }

                GridColumnPositionEntity positionEntity = new GridColumnPositionEntity();
                positionEntity.GridColumnLayout = layoutEntity;
                positionEntity.ColumnGuid = definition.ColumnGuid;
                positionEntity.Visible = definition.DefaultVisible;
                positionEntity.Width = definition.DefaultWidth;

                // Don't know the position just yet
                positionEntity.Position = -1;

                GridColumnPosition position = new GridColumnPosition(positionEntity, definition);

                // It has to be inserted into the order right after the previous definition, so it maintains
                // its position relative to its previous sibling it was defined after.
                int insertIndex = columnPositions.Count;

                int allIndex = allDefinitions.IndexOf(definition);
                if (allIndex == 0)
                {
                    insertIndex = 0;
                }
                else
                {
                    for (int i = 0; i < columnPositions.Count; i++)
                    {
                        if (columnPositions[i].Definition == allDefinitions[allIndex - 1])
                        {
                            insertIndex = i + 1;
                        }
                    }
                }

                columnPositions.Insert(insertIndex, position);
            }

            missingDefinitions.Clear();

            return true;
        }

        /// <summary>
        /// Find the definition with the given id
        /// </summary>
        private GridColumnDefinition FindDefinition(Guid columnGuid, List<GridColumnDefinition> definitionsToUse)
        {
            foreach (GridColumnDefinition definition in definitionsToUse)
            {
                if (definition.ColumnGuid == columnGuid)
                {
                    return definition;
                }
            }

            return null;
        }

        /// <summary>
        /// Copy the settings of the given columns into our columns
        /// </summary>
        public void CopyFrom(GridColumnPositionList copyFromPositions)
        {
            foreach (GridColumnPosition copyFromPosition in copyFromPositions)
            {
                // Find out corresponding column
                GridColumnPosition ourPosition = this[copyFromPosition.Definition.ColumnGuid];

                ourPosition.PositionChanged -= new EventHandler(this.OnPositionChanged);

                // Copy the settings
                ourPosition.Visible = copyFromPosition.Visible;
                ourPosition.Width = copyFromPosition.Width;
                ourPosition.Position = copyFromPosition.Position;

                ourPosition.PositionChanged += new EventHandler(this.OnPositionChanged);
            }

            SortByPosition();
        }

        #endregion

        #region Saving

        /// <summary>
        /// Save any pending changes to all columns in the collection
        /// </summary>
        public void Save(SqlAdapter adapter)
        {
            bool hadChanges = false;

            foreach (GridColumnPosition columnPosition in columnPositions)
            {
                hadChanges |= columnPosition.Save(adapter);
            }

            // If there were changes, we have to validate the layout
            if (hadChanges)
            {
                try
                {
                    ActionProcedures.ValidateGridLayouts(adapter);
                }
                catch (SqlException ex)
                {
                    // 'corrupt' is in our error message thrown when the layout is corrupt.
                    if (ex.Message.Contains("corrupt"))
                    {
                        // Its possible its corrupt because two people saved the state of the layout with different dirty columns.  So what we
                        // can do to try to fix this is make our whole set dirty and try the save again.  If it fails this time, we just
                        // have to let it fail, b\c there is a bug.
                        foreach (GridColumnPosition columnPosition in columnPositions)
                        {
                            columnPosition.Save(adapter, false);
                        }

                        ActionProcedures.ValidateGridLayouts(adapter);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Rollback any pending changes to the collection
        /// </summary>
        public void CancelChanges()
        {
            foreach (GridColumnPosition position in columnPositions)
            {
                position.CancelChanges();
            }

            SortByPosition();
        }

        #endregion

        /// <summary>
        /// Get the GridColumnPosition with the specified ID
        /// </summary>
        public GridColumnPosition this[Guid columnGuid]
        {
            get
            {
                foreach (GridColumnPosition position in columnPositions)
                {
                    if (position.Definition.ColumnGuid == columnGuid)
                    {
                        return position;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Get the GridColumnPosition that represents the display of the given entity field.
        /// </summary>
        public GridColumnPosition this[EntityField2 field]
        {
            get
            {
                foreach (GridColumnPosition position in columnPositions)
                {
                    if (EntityUtility.IsSameField(position.Definition.DisplayValueProvider.PrimaryField, field))
                    {
                        return position;
                    }

                    // Its possible the display value provider is a FunctionValueProvider.  If the sort value provider is a field value provider, use that
                    if (EntityUtility.IsSameField(position.Definition.SortProvider.SortFields.FirstOrDefault(), field))
                    {
                        return position;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Get the generic enumerator
        /// </summary>
        public IEnumerator<GridColumnPosition> GetEnumerator()
        {
            return columnPositions.GetEnumerator();
        }

        /// <summary>
        /// Get the non-generic enumerator
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return columnPositions.GetEnumerator();
        }

        /// <summary>
        /// Sort our column collection based on their defined Positions
        /// </summary>
        private void SortByPosition()
        {
            columnPositions.Sort(new Comparison<GridColumnPosition>(delegate(GridColumnPosition left, GridColumnPosition right)
            {
                return left.Position.CompareTo(right.Position);
            }));
        }

        /// <summary>
        /// The the positions of all the columns to be their index in the collection.
        /// </summary>
        private void SetPositionsFromIndex()
        {
            suspendPositionChanged = true;

            for (int i = 0; i < columnPositions.Count; i++)
            {
                columnPositions[i].Position = i;
            }

            suspendPositionChanged = false;
        }

        /// <summary>
        /// The Position property of one of the layouts in the collection has changed.
        /// </summary>
        private void OnPositionChanged(object sender, EventArgs e)
        {
            if (suspendPositionChanged)
            {
                return;
            }

            GridColumnPosition columnPosition = (GridColumnPosition) sender;

            // Move the column to its newly set position
            columnPositions.Remove(columnPosition);
            columnPositions.Insert(columnPosition.Position, columnPosition);

            // Now update the rest of the column's positions to reflect the collection reordering
            SetPositionsFromIndex();
        }
    }
}
