using ShipWorks.Stores;

namespace ShipWorks.UI.Controls.ChannelConfirmDelete
{
    /// <summary>
    /// View model to confirm deleting a channel
    /// </summary>
    public interface IConfirmChannelDeleteViewModel
    {
        /// <summary>
        /// Load the storetype into the view model
        /// </summary>
        /// <param name="storeType"></param>
        void Load(StoreTypeCode storeType);
    }
}