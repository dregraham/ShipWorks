using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Editions;
using ShipWorks.Stores;

namespace ShipWorks.UI.Controls.ChannelLimit.ChannelLimitBehavior
{
    public class OverChannelLimitBehavior : IChannelLimitBehavior
    {
        private readonly ICustomerLicense license;
        private readonly IStoreManager storeManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="OverChannelLimitBehavior"/> class.
        /// </summary>
        public OverChannelLimitBehavior(ICustomerLicense license, IStoreManager storeManager)
        {
            this.license = license;
            this.storeManager = storeManager;
        }

        /// <summary>
        /// Populates the channels.
        /// </summary>
        public void PopulateChannels(ObservableCollection<StoreTypeCode> channels,
            StoreTypeCode? channelToAdd)
        {
            channels.Clear();

            // Load the collection with the licenses active stores
            // If ChannelToAdd is set, fitler out 
            IEnumerable<StoreTypeCode> activeTangoChannels =
                license.GetActiveStores()
                    .Select(s => new ShipWorksLicense(s.StoreLicenseKey).StoreTypeCode);

            IEnumerable<StoreTypeCode> activeStoresInShipWorks =
                storeManager.GetAllStores()
                    .Select(s => (StoreTypeCode) s.TypeCode);

            activeTangoChannels.Union(activeStoresInShipWorks)
                .Distinct()
                // exclude channelToAdd if it is set
                .Where(s => !channelToAdd.HasValue || s != channelToAdd.Value)
                .ToList()
                .ForEach(channels.Add);
        }

        /// <summary>
        /// Gets the edition feature.
        /// </summary>
        public EditionFeature EditionFeature => EditionFeature.ChannelCount;

        /// <summary>
        /// Gets the title.
        /// </summary>
        public string Title => "Channel Limit Exceeded";
    }
}