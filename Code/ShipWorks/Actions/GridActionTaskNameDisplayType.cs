using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Editors;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Users;
using ShipWorks.Actions;
using ShipWorks.Data.Grid.Columns;
using ShipWorks.Actions.Tasks;

namespace ShipWorks.Actions
{
    /// <summary>
    /// Column DisplayType for showing the name of an Action Task baed on its identifier
    /// </summary>
    public class GridActionTaskNameDisplayType : GridColumnDisplayType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GridActionTaskNameDisplayType()
        {
            this.PreviewInputType = GridColumnPreviewInputType.LiteralString;
        }

        /// <summary>
        /// Format the identifier into the task name
        /// </summary>
        protected override string GetDisplayText(object value)
        {
            string identifier = (string) value;

            return ActionTaskManager.GetDescriptor(identifier).BaseName;
        }
    }
}
