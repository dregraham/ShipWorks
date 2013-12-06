using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores
{
    /// <summary>
    /// Custom ImageResource attribute for icons for store types
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public sealed class StoreTypeIconAttribute : ImageResourceAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StoreTypeIconAttribute(string resourceKey)
            : base(resourceKey)
        {
            ResourceSet = "ShipWorks.Properties.StoreIcons";
        }
    }
}
