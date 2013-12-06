using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Utility;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Custom ImageResource attribute for icons for shipment types
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public sealed class ShipmentTypeIconAttribute : ImageResourceAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentTypeIconAttribute(string resourceKey)
            : base(resourceKey)
        {
            ResourceSet = "ShipWorks.Properties.ShippingIcons";
        }
    }
}
