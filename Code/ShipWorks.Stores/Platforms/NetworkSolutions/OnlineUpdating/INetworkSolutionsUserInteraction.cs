using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.NetworkSolutions.OnlineUpdating
{
    /// <summary>
    /// User interaction for NetworkSolutions online updating
    /// </summary>
    public interface INetworkSolutionsUserInteraction
    {
        /// <summary>
        /// Get details for sending a comment with the order status
        /// </summary>
        GenericResult<NetworkSolutionsCommentDetails> GetMessagingDetails(IWin32Window owner, NetworkSolutionsStoreEntity store);
    }
}