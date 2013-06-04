using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing;

namespace ShipWorks.Shipping.Editing
{
    /// <summary>
    /// EventHandler for the SelectionChanged event of the shipments control
    /// </summary>
    public delegate void ShipmentSelectionChangedEventHandler(object sender, ShipmentSelectionChangedEventArgs e);

    /// <summary>
    /// EventArgs for the SelectionChanged event of the shipment control
    /// </summary>
    public class ShipmentSelectionChangedEventArgs : EventArgs
    {
        List<ShipmentGridRow> previous;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentSelectionChangedEventArgs(List<ShipmentGridRow> previous)
        {
            this.previous = previous;
        }

        /// <summary>
        /// The previous selection before it changed
        /// </summary>
        public List<ShipmentGridRow> Previous
        {
            get { return previous; }
        }
    }
}
