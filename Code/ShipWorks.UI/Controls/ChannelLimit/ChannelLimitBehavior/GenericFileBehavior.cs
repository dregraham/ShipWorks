using System.Collections.ObjectModel;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Editions;
using ShipWorks.Stores;

namespace ShipWorks.UI.Controls.ChannelLimit.ChannelLimitBehavior
{
    /// <summary>
    /// ChannelLimitBehavior for the GenericFile
    /// </summary>
    public class GenericFileBehavior : IChannelLimitBehavior
    {
        /// <summary>
        /// Populates the channels.
        /// </summary>
        public void PopulateChannels(ObservableCollection<StoreTypeCode> channels,
            ICustomerLicense license,
            IStoreManager storeManager,
            StoreTypeCode? channelToAdd)
        {
            channels.Clear();

            channels.Add(StoreTypeCode.GenericFile);
        }

        /// <summary>
        /// Gets EditionFeature.GenericFile
        /// </summary>
        public EditionFeature EditionFeature => EditionFeature.GenericFile;
    }
}