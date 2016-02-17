using System;
using System.Collections.ObjectModel;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Editions;
using ShipWorks.Stores;

namespace ShipWorks.UI.Controls.ChannelLimit.ChannelLimitBehavior
{
    /// <summary>
    /// Channel limit behavior for the GenericModule
    /// </summary>
    public class GenericModuleBehavior : IChannelLimitBehavior
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

            channels.Add(StoreTypeCode.GenericModule);
        }

        /// <summary>
        /// Gets the edition feature.
        /// </summary>
        public EditionFeature EditionFeature => EditionFeature.GenericModule;

        /// <summary>
        /// Title
        /// </summary>
        public string Title => "Generic Module not allowed";
    }
}