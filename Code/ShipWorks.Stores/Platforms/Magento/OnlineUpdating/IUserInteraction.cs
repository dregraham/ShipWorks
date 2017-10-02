using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Platforms.Magento.Enums;

namespace ShipWorks.Stores.Platforms.Magento.OnlineUpdating
{
    /// <summary>
    /// User interaction needed for uploading data to Magento
    /// </summary>
    public interface IUserInteraction
    {
        /// <summary>
        /// Get action comments for uploading to Magento
        /// </summary>
        GenericResult<ActionComments> GetActionComments(IWin32Window owner, MagentoVersion version);
    }
}