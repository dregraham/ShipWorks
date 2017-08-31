using System.Collections.Generic;
using System.Windows.Forms;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.Ebay.OnlineUpdating
{
    /// <summary>
    /// User interaction for Ebay online updating
    /// </summary>
    public interface IUserInteraction
    {
        /// <summary>
        /// Get details for sending a message to ebay
        /// </summary>
        GenericResult<EbayMessagingDetails> GetMessagingDetails(IWin32Window owner, IEnumerable<long> selectedIDs);

        /// <summary>
        /// Get details for sending a message to ebay
        /// </summary>
        GenericResult<EbayFeedbackDetails> GetFeedbackDetails(IWin32Window owner, IEnumerable<long> selectedIDs);
    }
}