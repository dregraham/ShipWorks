using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Platforms.Magento.Enums;

namespace ShipWorks.Stores.Platforms.Magento.OnlineUpdating
{
    /// <summary>
    /// User interaction needed for uploading data to Magento
    /// </summary>
    [Component]
    public class UserInteraction : IUserInteraction
    {
        /// <summary>
        /// Get action comments for uploading to Magento
        /// </summary>
        public GenericResult<ActionComments> GetActionComments(IWin32Window owner, MagentoVersion version)
        {
            using (MagentoActionCommentsDlg dlg = new MagentoActionCommentsDlg(version))
            {
                return dlg.ShowDialog(owner) == DialogResult.OK ?
                    GenericResult.FromSuccess(new ActionComments { Action = dlg.Action, Comments = dlg.Comments }) :
                    GenericResult.FromError<ActionComments>("Canceled");
            }
        }
    }
}
