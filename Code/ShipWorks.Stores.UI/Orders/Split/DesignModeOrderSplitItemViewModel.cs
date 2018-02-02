using System.Collections.Generic;

namespace ShipWorks.Stores.UI.Orders.Split
{
    /// <summary>
    /// SplitItemViewModel for design mode
    /// </summary>
    public class DesignModeOrderSplitItemViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DesignModeOrderSplitItemViewModel()
        {
            Name = "Collegiate Cardigan";
            OriginalQuantity = 3;
            SplitQuantity = 1;
            Attributes = new Dictionary<string, string> { { "Size", "Small" }, { "Color", "Red" } };
        }

        /// <summary>
        /// Name of the item
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Quantity of the item on the original order
        /// </summary>
        public double OriginalQuantity { get; set; }

        /// <summary>
        /// Quantity of the item on the split order
        /// </summary>
        public double SplitQuantity { get; set; }

        /// <summary>
        /// List of attributes for the item
        /// </summary>
        public IEnumerable<KeyValuePair<string, string>> Attributes { get; }
    }
}
