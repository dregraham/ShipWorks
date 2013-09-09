using System;
using Divelements.SandGrid;

namespace ShipWorks.Data.Grid
{
    /// <summary>
    /// Placeholder column used to provide secondary sorting on arbitrary data
    /// </summary>
    public class GridHiddenSortColumn<T> : GridColumn where T : GridRow
    {
        readonly Func<T, object> valueFunction;

        /// <summary>
        /// Constructor
        /// </summary>
        public GridHiddenSortColumn(Func<T, object> valueFunction)
        {
            Visible = false;

            this.valueFunction = valueFunction;
        }

        /// <summary>
        /// Get the sort value for the given row
        /// </summary>
        public object GetSortValue(T shipment)
        {
            return valueFunction(shipment);
        }
    }
}
