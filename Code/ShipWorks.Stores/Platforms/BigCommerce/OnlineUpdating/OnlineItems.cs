using System.Collections.Generic;
using Interapptive.Shared.Collections;
using ShipWorks.Stores.Platforms.BigCommerce.DTO;

namespace ShipWorks.Stores.Platforms.BigCommerce.OnlineUpdating
{
    /// <summary>
    /// Detail of a BigCommerce online update
    /// </summary>
    public class OnlineItems
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OnlineItems(long orderAddressID, IEnumerable<BigCommerceItem> items)
        {
            OrderAddressID = orderAddressID;
            Items = items.ToReadOnly();
        }

        /// <summary>
        /// Order address id
        /// </summary>
        public long OrderAddressID { get; }

        /// <summary>
        /// Items for upload
        /// </summary>
        public IEnumerable<BigCommerceItem> Items { get; }
    }
}