using System.Collections.ObjectModel;
using ShipWorks.Editions;
using ShipWorks.Stores;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Provides behavior to the ChannelLimitViewModel specific to the current operation
    /// </summary>
    public interface IChannelLimitBehavior
    {
        /// <summary>
        /// Populates the channels.
        /// </summary>
        void PopulateChannels(ObservableCollection<StoreTypeCode> channels, StoreTypeCode? channelToAdd);

        /// <summary>
        /// Gets the edition feature.
        /// </summary>
        EditionFeature EditionFeature { get; }

        /// <summary>
        /// Gets the title.
        /// </summary>
        string Title { get; }
    }
}