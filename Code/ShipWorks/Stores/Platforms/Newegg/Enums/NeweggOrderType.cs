using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.Newegg.Enums
{
    /// <summary>
    /// An enumeration for use in the Newegg download request criteria that will determine what types of orders
    /// get downloaded.
    /// </summary>
    public enum NeweggOrderType
    {
        /// <summary>
        /// All
        /// </summary>
        All = 0,

        /// <summary>
        /// Order to be shipped by Newegg
        /// </summary>
        ShipByNewegg = 1,

        /// <summary>
        /// Orders to be shipped by seller
        /// </summary>
        ShipBySeller = 2,

        /// <summary>
        /// Multi channel orders
        /// </summary>
        MultiChannel = 3
    }
}
