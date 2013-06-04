using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Divelements.SandGrid;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Editing
{
    /// <summary>
    /// Placeholder column used to provide secondary sorting on Shipment Number
    /// </summary>
    public class ShipmentGridHiddenSortColumn : GridColumn
    {
        Func<ShipmentGridRow, object> valueFunction;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentGridHiddenSortColumn(Func<ShipmentGridRow, object> valueFunction)
        {
            Visible = false;

            this.valueFunction = valueFunction;
        }

        /// <summary>
        /// Get the sort value for the given shipment
        /// </summary>
        public object GetSortValue(ShipmentGridRow shipment)
        {
            return valueFunction(shipment);
        }
    }
}
