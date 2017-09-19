using System.Windows.Forms;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.Etsy.OnlineUpdating
{
    /// <summary>
    /// User interaction for Etsy online updating
    /// </summary>
    public interface IEtsyUserInteraction
    {
        /// <summary>
        /// Get a comment from the user
        /// </summary>
        GenericResult<string> GetComment(IWin32Window owner);
    }
}