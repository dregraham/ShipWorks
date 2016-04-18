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
    /// Over Channel Limit Behavior 
    /// </summary>
    public class OverChannelLimitBehavior : IChannelLimitBehavior
    {
        private readonly ICustomerLicense license;
        private readonly Func<string, IShipWorksLicense> shipWorksLicenseFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="OverChannelLimitBehavior"/> class.
        /// </summary>
        public OverChannelLimitBehavior(ILicenseService licenseService, 
            IStoreManager storeManager,
            Func<string, IShipWorksLicense> shipWorksLicenseFactory)
        {
            license = licenseService.GetLicenses().First() as ICustomerLicense;
            this.shipWorksLicenseFactory = shipWorksLicenseFactory;
        }

        /// <summary>
        /// Populates the channels.
        /// </summary>
        public void PopulateChannels(ObservableCollection<StoreTypeCode> channels,
            StoreTypeCode? channelToAdd)
        {
            channels.Clear();

            // Load the collection with the licenses active stores
            // If ChannelToAdd is set, filter out 
            IEnumerable<StoreTypeCode> activeTangoChannels =
                license.GetActiveStores()
                    .Select(s => shipWorksLicenseFactory(s.StoreLicenseKey).StoreTypeCode);

            activeTangoChannels
                .Distinct()
                .Where(s => !channelToAdd.HasValue || s != channelToAdd.Value) // exclude channelToAdd if it is set
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