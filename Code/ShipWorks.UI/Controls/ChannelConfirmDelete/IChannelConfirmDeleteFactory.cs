using System.Windows;
using System.Windows.Forms;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Stores;

namespace ShipWorks.UI.Controls.ChannelConfirmDelete
{
    /// <summary>
    /// Factory for ChannelConfirmDelete
    /// </summary>
    public interface IChannelConfirmDeleteFactory
    {
        /// <summary>
        /// Gets ChannelConfirmDeleteDlg
        /// </summary>
        IDialog GetConfirmDeleteDlg(StoreTypeCode storeType, IWin32Window owner);
    }
}