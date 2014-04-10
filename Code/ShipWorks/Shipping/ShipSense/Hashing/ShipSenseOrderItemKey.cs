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
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ShipSenseOrderItemKey" /> class.
        /// </summary>
        public ShipSenseOrderItemKey()
        {
            itemPropertyAttributeKeys = new Dictionary<string, string>();
        }

        /// <summary>
        /// A copy constructor to initialize a new instance of the <see cref="ShipSenseOrderItemKey"/> class
        /// based on an existing instance.
        /// </summary>
        /// <param name="orderItemKey">The order item key.</param>
        public ShipSenseOrderItemKey(ShipSenseOrderItemKey orderItemKey)
            : this()
        {
            // Copy over the data from the given key into this
            foreach (KeyValuePair<string, string> pair in orderItemKey.itemPropertyAttributeKeys)
            {
                itemPropertyAttributeKeys[pair.Key] = pair.Value;
            }

            Quantity = orderItemKey.Quantity;
        }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        public double Quantity { get; set; }

        /// <summary>
        /// Gets a value that uniquely represents this instance of an order item based on the
        /// identifier values and the quantity.
        /// </summary>
        public string KeyValue
        {
            get
            {
                // Build a pipe delimited string from the identifier values
                return string.Join("|", itemPropertyAttributeKeys.Select(pair => string.Format("[{0},{1}]", pair.Key, pair.Value)));
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
