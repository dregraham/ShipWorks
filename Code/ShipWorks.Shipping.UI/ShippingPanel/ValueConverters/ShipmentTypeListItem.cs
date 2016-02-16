using System.Reflection;

namespace ShipWorks.Shipping.UI.ShippingPanel.ValueConverters
{
    /// <summary>
    /// Item to be used in a shipment type list
    /// </summary>
    public struct ShipmentTypeListItem
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentTypeListItem(ShipmentTypeCode value, string description)
        {
            Value = value;
            Description = description;
        }

        /// <summary>
        /// Value of the item
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ShipmentTypeCode Value { get; }

        /// <summary>
        /// Description of the item
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Description { get; }
    }
}
