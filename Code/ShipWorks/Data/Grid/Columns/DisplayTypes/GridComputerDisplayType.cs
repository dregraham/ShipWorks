using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Grid.Columns.DisplayTypes.Editors;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Users;

namespace ShipWorks.Data.Grid.Columns.DisplayTypes
{
    /// <summary>
    /// Column DisplayType for showing a computer name based on a computerID
    /// </summary>
    public class GridComputerDisplayType : GridColumnDisplayType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GridComputerDisplayType()
        {
            this.PreviewInputType = GridColumnPreviewInputType.LiteralString;
        }

        /// <summary>
        /// Format the ComputerID into the computer name
        /// </summary>
        protected override string GetDisplayText(object value)
        {
            if (value == null)
            {
                return "";
            }

            long computerID = (long) value;
            ComputerEntity computer = ComputerManager.GetComputer(computerID);

            // Has been deleted, or just created and we havnt loaded it yet
            if (computer == null)
            {
                return "";
            }

            return computer.Name;
        }
    }
}
