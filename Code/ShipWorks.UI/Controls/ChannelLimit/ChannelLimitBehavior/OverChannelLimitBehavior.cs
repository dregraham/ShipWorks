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
        public void PopulateChannels(ObservableCollection<StoreTypeCode> channels,
            ICustomerLicense license,
            IStoreManager storeManager,
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

        public EditionFeature EditionFeature => EditionFeature.ChannelCount;
    }
}