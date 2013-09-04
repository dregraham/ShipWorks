using ShipWorks.Users;
using System.Linq;

namespace ShipWorks.Data.Grid.Columns.DisplayTypes
{
    /// <summary>
    /// GridColumnDisplayType for showing a list of computer names an action is limited to running on based 
    /// on an array of computer IDs.
    /// </summary>
    public class GridComputerLimitedListDisplayType : GridColumnDisplayType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GridComputerLimitedListDisplayType"/> class.
        /// </summary>
        public GridComputerLimitedListDisplayType()
        {
            this.PreviewInputType = GridColumnPreviewInputType.LiteralString;
        }

        /// <summary>
        /// Format the computer IDs into the computer names. 
        /// </summary>
        protected override string GetDisplayText(object value)
        {
            long[] computerIDs = value as long[];

            if (computerIDs == null || computerIDs.Length == 0)
            {
                // Since this is currently only used by the ActionErrorColumnDefinitionFactory, so returning 
                // "Any computer" when the list is null or empty is the desired behavior in this context
                return "Any computer";
            }

            return string.Join(", ",
                computerIDs
                    .Select(ComputerManager.GetComputer)
                    .Where(x => x != null)
                    .Select(x => x.Name)
            );
        }
    }
}
