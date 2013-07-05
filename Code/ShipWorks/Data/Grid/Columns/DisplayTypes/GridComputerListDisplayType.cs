using ShipWorks.Users;
using System.Linq;


namespace ShipWorks.Data.Grid.Columns.DisplayTypes
{
    /// <summary>
    /// Column DisplayType for showing a list of computer names based on an array of computer IDs.
    /// </summary>
    public class GridComputerListDisplayType : GridColumnDisplayType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GridComputerListDisplayType()
        {
            this.PreviewInputType = GridColumnPreviewInputType.LiteralString;
        }

        /// <summary>
        /// Format the computer IDs into the computer names
        /// </summary>
        protected override string GetDisplayText(object value)
        {
            if (value == null)
            {
                return "";
            }

            var computerIDs = (long[])value;

            return string.Join(", ",
                computerIDs
                    .Select(ComputerManager.GetComputer)
                    .Where(x => x != null)
                    .Select(x => x.Name)
            );
        }
    }
}
