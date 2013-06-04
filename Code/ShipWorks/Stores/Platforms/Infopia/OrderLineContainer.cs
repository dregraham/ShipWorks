using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Infopia.WebServices;

namespace ShipWorks.Stores.Platforms.Infopia
{
    /// <summary>
    /// Container class for grouping the Lines returned from Infopia by order
    /// </summary>
    public class OrderLineContainer
    {
        // The parent of all child and attribute lines for an order
        public Line OrderLine { get; private set; }

        // order description lines from Infopia
        public List<Line> ChildLines { get; private set; }

        // product attributes for order items 
        public List<Line> AttributeLines { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public OrderLineContainer(Line orderLine)
        {
            OrderLine = orderLine;
            ChildLines = new List<Line>();
            AttributeLines = new List<Line>();
        }
    }
}
