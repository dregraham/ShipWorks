using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Editors;
using ShipWorks.UI.Controls;
using ShipWorks.Users;

namespace ShipWorks.Data.Grid.Columns.DisplayTypes
{
    /// <summary>
    /// Display type for displaying weight information
    /// </summary>
    public class GridWeightDisplayType : GridColumnDisplayType
    {
        /// <summary>
        /// Get formatted weight value
        /// </summary>
        protected override string GetDisplayText(object value)
        {
            if (!(value is double))
            {
                return string.Empty;
            }

            return WeightControl.FormatWeight((double) value);
        }
    }
}
