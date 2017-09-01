using System.Windows.Forms;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Platforms.Etsy.Dialog;

namespace ShipWorks.Stores.Platforms.Etsy.OnlineUpdating
{
    /// <summary>
    /// User interaction for Etsy online updating
    /// </summary>
    public class EtsyUserInteraction : IEtsyUserInteraction
    {
        /// <summary>
        /// Get a comment from the user
        /// </summary>
        public GenericResult<string> GetComment(IWin32Window owner)
        {
            using (GetCommentTokenDlg dialog = new GetCommentTokenDlg())
            {
                return dialog.ShowDialog(owner) == DialogResult.OK ?
                    GenericResult.FromSuccess(dialog.Comment) :
                    GenericResult.FromError<string>("canceled");
            }
        }
    }
}
