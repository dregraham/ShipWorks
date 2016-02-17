using System.Collections.ObjectModel;
using System.Windows.Input;
using ShipWorks.Stores;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Interface for the ChannelLimitviewModel
    /// </summary>
    public interface IChannelLimitViewModel
    {
        /// <summary>
        /// Collection of Store Channels
        /// </summary>
        ObservableCollection<StoreTypeCode> ChannelCollection { get; set; }

        /// <summary>
        /// Command for deleting a channel
        /// </summary>
        ICommand DeleteStoreClickCommand { get; }

        /// <summary>
        /// Error message
        /// </summary>
        string ErrorMessage { get; set; }

        /// <summary>
        /// Status indicator for when we are in the process of deleting a channel
        /// </summary>
        bool IsDeleting { get; set; }

        /// <summary>
        /// The selected store type
        /// </summary>
        StoreTypeCode SelectedStoreType { get; set; }

        /// <summary>
        /// Command for upgrading an account
        /// </summary>
        ICommand UpgradeClickCommand { get; }

        /// <summary>
        /// Gets or sets the channel to add.
        /// </summary>
        StoreTypeCode? ChannelToAdd { get; set; }

        /// <summary>
        /// Loads the given customer license 
        /// </summary>
        void Load(ICustomerLicense customerLicense, IChannelLimitBehavior channelLimitBehavior);
    }
}