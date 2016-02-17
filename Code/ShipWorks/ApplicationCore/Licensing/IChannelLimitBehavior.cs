using System.Collections.ObjectModel;
using ShipWorks.Editions;
using ShipWorks.Stores;

namespace ShipWorks.ApplicationCore.Licensing
{
    public interface IChannelLimitBehavior
    {
        void PopulateChannels(ObservableCollection<StoreTypeCode> channels,
            ICustomerLicense license,
            IStoreManager storeManager,
            StoreTypeCode? channelToAdd);

        EditionFeature EditionFeature { get; }
    }
}