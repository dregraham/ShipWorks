using ShipWorks.Stores;

namespace ShipWorks.UI.Controls.ChannelConfirmDelete
{
    public interface IChannelConfirmDeleteFactory
    {
        IChannelConfirmDeleteDlg GetConfirmDeleteDlg(StoreTypeCode storeType);
    }
}