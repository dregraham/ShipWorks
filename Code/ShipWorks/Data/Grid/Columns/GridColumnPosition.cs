using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.FactoryClasses;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Model.HelperClasses;
using SD.LLBLGen.Pro.QuerySpec.Adapter;

namespace ShipWorks.Data.Grid.Columns
{
    /// <summary>
    /// Represents a column within an ordered column collection for the column layout of a filter node.
    /// </summary>
    public class GridColumnPosition
    {
        GridColumnPositionEntity positionEntity;
        GridColumnDefinition definition;

        // Raised when the position property of the column changes
        public event EventHandler PositionChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        public GridColumnPosition(GridColumnPositionEntity positionEntity, GridColumnDefinition definition)
        {
            this.positionEntity = positionEntity;
            this.definition = definition;
        }

        /// <summary>
        /// Save the state using the given adapter.  Returns true if there were any changes that had to be saved,
        /// and false if nothing was saved.
        /// </summary>
        public bool Save(SqlAdapter adapter)
        {
            return Save(adapter, true);
        }

        /// <summary>
        /// Save the changes to the column.  If onlyIfDirty is true, the values will only be written
        /// if considered dirty by LLBLGen.
        /// </summary>
        public bool Save(SqlAdapter adapter, bool onlyIfDirty)
        {
            if (adapter == null)
            {
                throw new ArgumentNullException("adapter");
            }

            if (!onlyIfDirty)
            {
                positionEntity.Fields = positionEntity.Fields.CloneAsDirty();
            }

            bool isDirty = positionEntity.Fields.IsDirty;

            adapter.SaveAndRefetch(positionEntity);

            return isDirty;
        }

        /// <summary>
        /// Refetch the entity from the database.  This is useful for race conditions where the
        /// position has already been created by another instance.
        /// </summary>
        /// <param name="adapter"></param>
        public void Refetch(SqlAdapter adapter)
        {
            QueryFactory factory = new QueryFactory();
            EntityQuery<GridColumnPositionEntity> query = factory.Create<GridColumnPositionEntity>()
                .Where(GridColumnPositionFields.GridColumnLayoutID == positionEntity.GridColumnLayoutID)
                .AndWhere(GridColumnPositionFields.ColumnGuid == positionEntity.ColumnGuid);

            positionEntity = adapter.FetchFirst(query);

            adapter.FetchEntity(positionEntity);
        }

        /// <summary>
        /// Rollback any changes since the last save
        /// </summary>
        public void CancelChanges()
        {
            positionEntity.RollbackChanges();
        }

        /// <summary>
        /// Width of the column
        /// </summary>
        public int Width
        {
            get
            {
                return positionEntity.Width;
            }
            set
            {
                positionEntity.Width = value;
            }
        }

        /// <summary>
        /// If the column is displayed in the grid
        /// </summary>
        public bool Visible
        {
            get
            {
                return positionEntity.Visible;
            }
            set
            {
                positionEntity.Visible = value;
            }
        }

        /// <summary>
        /// Position of the column in the layout
        /// </summary>
        public int Position
        {
            get
            {
                return positionEntity.Position;
            }
            set
            {
                if (positionEntity.Position != value)
                {
                    positionEntity.Position = value;

                    if (PositionChanged != null)
                    {
                        PositionChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// The ShipWorks definition of the column
        /// </summary>
        public GridColumnDefinition Definition
        {
            get
            {
                return definition;
            }
        }
    }
}
