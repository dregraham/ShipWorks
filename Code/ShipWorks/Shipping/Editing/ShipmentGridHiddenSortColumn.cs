using System;
using ShipWorks.Data.Grid;

namespace ShipWorks.Shipping.Editing
{
    /// <summary>
    /// Placeholder column used to provide secondary sorting on Shipment Number
    /// </summary>
    public class ShipmentGridHiddenSortColumn : GridHiddenSortColumn<ShipmentGridRow>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentGridHiddenSortColumn(Func<ShipmentGridRow, object> valueFunction) :
            base(valueFunction)
        {

        }
    }
}
