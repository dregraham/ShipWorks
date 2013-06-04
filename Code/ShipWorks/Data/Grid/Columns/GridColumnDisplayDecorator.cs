using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Decorators.Editors;
using Interapptive.Shared.Utility;
using System.Xml.Serialization;

namespace ShipWorks.Data.Grid.Columns
{
    /// <summary>
    /// Base class for decorators that can be used to addorn GridColumnDisplayType instances with additional functionality.
    /// </summary>
    public abstract class GridColumnDisplayDecorator : SerializableObject
    {
        // Must be set to a unique value within a DisplayType so that serialization can work correctly.
        string identifier;

        /// <summary>
        /// Default constructor
        /// </summary>
        protected GridColumnDisplayDecorator()
        {

        }

        /// <summary>
        /// Gets \ sets a unique value (within a DisplayType) so that value serialization works correctly
        /// </summary>
        [XmlIgnore]
        public string Identifier
        {
            get { return identifier; }
            set { identifier = value; }
        }

        /// <summary>
        /// Get the editor used to edit the decorator settings.  Can return null if there are no settings to edit.
        /// </summary>
        public virtual GridColumnDecoratorEditor CreateEditor()
        {
            return null;
        }

        /// <summary>
        /// Decorate the formatted value with the attributes of this decorator
        /// </summary>
        public virtual void ApplyDecoration(GridColumnFormattedValue formattedValue)
        {

        }

        /// <summary>
        /// The mouse is moving over a cell defined by this column for the given row.
        /// </summary>
        internal virtual void OnCellMouseMove(EntityGridRow row, EntityGridColumn column, MouseEventArgs e)
        {

        }

        /// <summary>
        /// The mouse is being pressed in a cell defined by this column.  Return false to cancel the press.
        /// </summary>
        internal virtual bool OnCellMouseDown(EntityGridRow row, EntityGridColumn column, MouseEventArgs e)
        {
            return true;
        }

        /// <summary>
        /// The mouse is being released in a cell defined by this column.
        /// </summary>
        internal virtual void OnCellMouseUp(EntityGridRow row, EntityGridColumn column, MouseEventArgs e)
        {

        }

        /// <summary>
        /// The mouse has left the area of the cell for the given row
        /// </summary>
        internal virtual void OnCellMouseLeave(EntityGridRow row, EntityGridColumn column)
        {

        }
    }
}
