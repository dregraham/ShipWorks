using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Data.Grid.Columns
{
    /// <summary>
    /// Holds display values for a cell of a column
    /// </summary>
    public class GridColumnFormattedValue
    {
        object value;
        EntityBase2 entity;
        EntityField2 primaryField;

        string text = "";
        Image image = null;
        Color? foreColor = null;
        FontStyle? fontStyle = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public GridColumnFormattedValue(object value, EntityBase2 entity, EntityField2 primaryField)
        {
            this.value = value;
            this.entity = entity;
            this.primaryField = primaryField;
        }

        /// <summary>
        /// The value that is to be formatted for display
        /// </summary>
        public object Value
        {
            get { return value; }
        }

        /// <summary>
        /// The entity that the value came from.  Can be null in a preview scenario.
        /// </summary>
        public EntityBase2 Entity
        {
            get { return entity; }
        }

        /// <summary>
        /// The primary field that was used to derive the Value from the Entity.  Can be null in a preview scenario or when the value
        /// is calculated from an function with no field specified as primary.
        /// </summary>
        public EntityField2 PrimaryField
        {
            get { return primaryField; }
        }

        /// <summary>
        /// The text to display in the column
        /// </summary>
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        /// <summary>
        /// The image\icon to display in the column.
        /// </summary>
        public Image Image
        {
            get { return image; }
            set { image = value; }
        }

        /// <summary>
        /// The color of the text
        /// </summary>
        public Color? ForeColor
        {
            get { return foreColor; }
            set { foreColor = value; }
        }

        /// <summary>
        /// The font style to apply
        /// </summary>
        public FontStyle? FontStyle
        {
            get { return fontStyle; }
            set { fontStyle = value; }
        }

        /// <summary>
        /// Gets or sets the tool tip text.
        /// </summary>
        public string ToolTipText { get; set; }
    }
}
