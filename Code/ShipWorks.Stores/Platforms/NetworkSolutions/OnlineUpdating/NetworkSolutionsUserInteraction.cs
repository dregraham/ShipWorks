using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.NetworkSolutions.OnlineUpdating
{
    /// <summary>
    /// User interaction for NetworkSolutions online updating
    /// </summary>
    [Component]
    public class NetworkSolutionsUserInteraction : INetworkSolutionsUserInteraction
    {
        /// <summary>
        /// Get details for sending a comment with the order status
        /// </summary>
        public GenericResult<NetworkSolutionsCommentDetails> GetMessagingDetails(IWin32Window owner, NetworkSolutionsStoreEntity store)
        {
            NetworkSolutionsStatusCodeProvider codeProvider = new NetworkSolutionsStatusCodeProvider(store);

            // get user input on the status and comments
            using (NetworkSolutionsOnlineStatusCommentDlg dlg = new NetworkSolutionsOnlineStatusCommentDlg(codeProvider))
            {
                return dlg.ShowDialog() == DialogResult.OK ?
                    GenericResult.FromSuccess(new NetworkSolutionsCommentDetails(dlg.Code, dlg.Comments)) :
                    GenericResult.FromError<NetworkSolutionsCommentDetails>("Failed");
            }
        }
    }
}
