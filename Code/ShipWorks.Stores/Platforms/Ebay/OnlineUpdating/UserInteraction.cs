using System.Collections.Generic;
using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.Ebay.OnlineUpdating
{
    /// <summary>
    /// User interaction for Ebay online updating
    /// </summary>
    [Component]
    public class UserInteraction : IUserInteraction
    {
        /// <summary>
        /// Get details for sending a message to ebay
        /// </summary>
        public GenericResult<EbayMessagingDetails> GetMessagingDetails(IWin32Window owner, IEnumerable<long> selectedIDs)
        {
            using (EbayMessagingDlg dlg = new EbayMessagingDlg(selectedIDs))
            {
                return dlg.ShowDialog(owner) == DialogResult.OK ?
                    GenericResult.FromSuccess(dlg.Details) :
                    GenericResult.FromError<EbayMessagingDetails>("Canceled");
            }
        }

        /// <summary>
        /// Get details for sending a message to ebay
        /// </summary>
        public GenericResult<EbayFeedbackDetails> GetFeedbackDetails(IWin32Window owner, IEnumerable<long> selectedIDs)
        {
            using (LeaveFeedbackDlg dlg = new LeaveFeedbackDlg(selectedIDs))
            {
                return dlg.ShowDialog(owner) == DialogResult.OK ?
                    GenericResult.FromSuccess(dlg.Details) :
                    GenericResult.FromError<EbayFeedbackDetails>("Canceled");
            }
        }
    }
}
