using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Ebay.WebServices;

namespace ShipWorks.Stores.Platforms.Ebay.Requests.Download
{
    /// <summary>
    /// An interface for downloading order item data.
    /// </summary>
    public interface IItemRequest
    {
        /// <summary>
        /// Gets the item details.
        /// </summary>
        /// <param name="itemId">The item ID.</param>
        /// <returns>A GetItemResponseType object.</returns>
        GetItemResponseType GetItemDetails(string itemId);
    }
}
