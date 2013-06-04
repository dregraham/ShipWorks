using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Editors;
using System.Drawing;
using ShipWorks.Properties;

namespace ShipWorks.Data.Grid.Columns.DisplayTypes
{
    /// <summary>
    /// Display type for showing notes
    /// </summary>
    public class GridNoteDisplayType : GridColumnDisplayType
    {
        /// <summary>
        /// Get the text to display
        /// </summary>
        protected override string GetDisplayText(object value)
        {
            int noteCount = (int) value;

            return noteCount > 0 ? string.Format("{0}", value) : string.Empty;
        }

        /// <summary>
        /// Get the image for the note column
        /// </summary>
        protected override Image GetDisplayImage(object value)
        {
            int noteCount = (int) value;

            return noteCount > 0 ? Resources.note16 : null;
        }
    }
}
