using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Divelements.SandGrid;
using ShipWorks.UI.Controls.SandGrid;

namespace ShipWorks.Data.Grid.Columns
{
    /// <summary>
    /// Customized GridRow that can have a GridColumnDefinition associated with it.  This enables specialized columns that display information about
    /// that defintion.  This is used in the windows that allow the user to organize and format the grid columns.
    /// 
    /// We derive from SandGridDragDropRow b\c the GridColumnLayoutEditor needs drag\drop.  If a grid does not support Drag\Drop, then the extra
    /// functionality is just ignored.
    /// 
    /// </summary>
    public class GridColumnDefinitionRow : SandGridDragDropRow
    {
        GridColumnDefinition definition;

        /// <summary>
        /// Constructor
        /// </summary>
        public GridColumnDefinitionRow(GridColumnDefinition definition)
            : base(definition.HeaderText)
        {
            this.definition = definition;
        }

        /// <summary>
        /// The column definition the row represents
        /// </summary>
        public GridColumnDefinition Definition
        {
            get { return definition; }
        }

    }
}
