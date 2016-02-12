using System.Windows;
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
        IChannelConfirmDeleteDlg GetConfirmDeleteDlg(StoreTypeCode storeType, Window owner);
    }
}