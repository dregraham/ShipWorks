using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Editions;
using ShipWorks.Stores;

namespace ShipWorks.UI.Controls.ChannelLimit.ChannelLimitBehavior
{
    /// <summary>
    /// ChannelLimitBehavior for when there are stores in Shipworks that
    /// Tango doesn't know about.
    /// </summary>
    public class ChannelsOutOfSyncBehavior : IChannelLimitBehavior
    {
        private readonly ICustomerLicense license;
        private readonly IStoreManager storeManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelsOutOfSyncBehavior"/> class.
        /// </summary>
        public ChannelsOutOfSyncBehavior(ILicenseService licenseService,
            IStoreManager storeManager,
            Func<string, IShipWorksLicense> shipWorksLicenseFactory)
        {
            license = licenseService.GetLicenses().First() as ICustomerLicense;
            this.storeManager = storeManager;
        }

        /// <summary>
        /// Populates the channels missing from Tango.
        /// </summary>
        public void PopulateChannels(ObservableCollection<StoreTypeCode> channels, StoreTypeCode? channelToAdd)
        {
            channels.Clear();

            // Load the collection with the licenses active stores
            // If ChannelToAdd is set, filter out 
            IEnumerable<IActiveStore> activeTangoStores =
                license.GetActiveStores();

            // Get all the stores in ShipWorks, filter out stores tango knows about, get the channel and add them to channels.
            storeManager.GetAllStores()
                .Where(clientStore => !(activeTangoStores.Any(tangoStore => tangoStore.StoreLicenseKey == clientStore.License)))
                .Select(missingStore => (StoreTypeCode) missingStore.TypeCode)
                .Distinct()
                .ToList()
                .ForEach(channels.Add);
        }

        /// <summary>
        /// Gets the edition feature.
        /// </summary>
        public EditionFeature EditionFeature => EditionFeature.ClientChannelsAccountedFor;

        /// <summary>
        /// Gets the title.
        /// </summary>
        public string Title => "Unknown Channel Found";
    }
}