using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ShipWorks.Shipping.ShipSense.Hashing
{
    /// <summary>
    /// A class intended to uniquely identify an order item based on the quantity and a set of
    /// property and attribute values.
    /// </summary>
    public class ShipSenseOrderItemKey
    {
        private readonly Dictionary<string, string> itemPropertyAttributeKeys;
        private readonly double quantity;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipSenseOrderItemKey" /> class.
        /// </summary>
        /// <param name="quantity">The quantity of a particular order item.</param>
        public ShipSenseOrderItemKey(double quantity)
        {
            itemPropertyAttributeKeys = new Dictionary<string, string>();
            this.quantity = quantity;
        }
        
        /// <summary>
        /// Gets a value that uniquely represents this instance of an order item based on the
        /// identifier values and the quantity.
        /// </summary>
        public string Value
        {
            get
            {
                // Build a pipe delimited string from the identifier values and the quantity
                string identiferSection = string.Join("|", itemPropertyAttributeKeys.Select(pair => string.Format("[{0},{1}]", pair.Key, pair.Value)));
                return string.Join("|", identiferSection, quantity.ToString(CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Adds a name/value pair that should be used to when constructing the key value for this instance.
        /// </summary>
        public void Add(string key, string value)
        {
            itemPropertyAttributeKeys[key] = value;
        }

        /// <summary>
        /// Determines whether this instance is valid.
        /// </summary>
        public bool IsValid()
        {
            // For a key to be valid, at least one entry in the dictionary needs to have a value
            return itemPropertyAttributeKeys.Any(pair => !string.IsNullOrWhiteSpace(pair.Value));
        }
    }
}
