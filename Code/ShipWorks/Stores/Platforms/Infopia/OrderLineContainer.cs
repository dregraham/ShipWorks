using System.Collections.Generic;
using ShipWorks.Stores.Platforms.Infopia.WebServices;

namespace ShipWorks.Stores.Platforms.Infopia
{
    /// <summary>
    /// Container class for grouping the Lines returned from Infopia by order
    /// </summary>
    /// <remarks>
    /// THIS STORE IS DEAD
    /// This store is scheduled for removal as it no longer exists. Do not update this store when making
    /// all-platform changes.
    /// </remarks>
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
