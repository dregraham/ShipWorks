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
        public void PopulateChannels(ObservableCollection<StoreTypeCode> channels, StoreTypeCode? channelToAdd)
        {
            channels.Clear();

            channels.Add(StoreTypeCode.GenericFile);
        }

        /// <summary>
        /// Gets EditionFeature.GenericFile
        /// </summary>
        public EditionFeature EditionFeature => EditionFeature.GenericFile;

        /// <summary>
        /// Gets the title.
        /// </summary>
        public string Title => "Generic File not allowed";
    }
}