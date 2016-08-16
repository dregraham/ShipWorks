using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;

using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Stores;
using ShipWorks.UI.Controls.ChannelLimit.ChannelLimitBehavior;
using Xunit;

namespace ShipWorks.UI.Tests.Controls.ChannelLimit.ChannelLimitBehavior
{
    public class ChannelsOutOfSyncBehaviorTest
    {
        private const string AmazonLicense = "E6N7A-EAMRZ-AMXI2-1IHNL-AMAZON-BRIAN@INTERAPPTIVE.COM";
        private const string AmeriCommerceLicense = "MO45A-ASRMC-MVX7G-901PU-AMERICOMMERCE-BRIAN@INTERAPPTIVE.COM";

        [Fact]
        public void EditionFeature_IsClientStoresAccountedFor()
        {
            using (AutoMock mock = AutoMock.GetLoose())
            {
                SetupCustomerLicense(mock, AmeriCommerceLicense);

                var testObject = mock.Create<ChannelsOutOfSyncBehavior>();

                Assert.Equal(EditionFeature.ClientChannelsAccountedFor, testObject.EditionFeature);
            }
        }

        [Fact]
        public void PopulateChannels_ChannelsAreCleared()
        {
            using (AutoMock mock = AutoMock.GetLoose())
            {
                var storeManager = mock.Mock<IStoreManager>();
                storeManager.Setup(m => m.GetAllStores()).Returns(new List<StoreEntity>());

                SetupCustomerLicense(mock, AmeriCommerceLicense);
                
                var testObject = mock.Create<ChannelsOutOfSyncBehavior>();

                var channels = new ObservableCollection<StoreTypeCode> {StoreTypeCode.Amazon};

                testObject.PopulateChannels(channels, null);

                Assert.Empty(channels);
            }
        }

        [Fact]
        public void PopulateChannels_ChannelsAreEmpty_WhenAllClientStoresAreInTango()
        {
            using (AutoMock mock = AutoMock.GetLoose())
            {
                var storeManager = mock.Mock<IStoreManager>();
                storeManager.Setup(m => m.GetAllStores())
                    .Returns(new List<StoreEntity> {new StoreEntity {License = AmazonLicense}});

                SetupCustomerLicense(mock, AmazonLicense);

                var testObject = mock.Create<ChannelsOutOfSyncBehavior>();

                var channels = new ObservableCollection<StoreTypeCode>();

                testObject.PopulateChannels(channels, null);

                Assert.Empty(channels);
            }
        }

        [Fact]
        public void PopulateChannels_ChannelContainsAmazon_WhenStoreManagerReturnsAmazonLicense_AndTangoDoesNotKnowAboutAmazon()
        {
            using (AutoMock mock = AutoMock.GetLoose())
            {
                // store in client
                var storeManager = mock.Mock<IStoreManager>();
                storeManager.Setup(m => m.GetAllStores())
                    .Returns(new List<StoreEntity> { new StoreEntity { License = AmazonLicense, TypeCode = (int) StoreTypeCode.Amazon } });

                // store in Tango
                SetupCustomerLicense(mock, AmeriCommerceLicense);

                var testObject = mock.Create<ChannelsOutOfSyncBehavior>();

                var channels = new ObservableCollection<StoreTypeCode>();

                testObject.PopulateChannels(channels, null);

                Assert.Equal(StoreTypeCode.Amazon, channels.Single());
            }
        }

        private static void SetupCustomerLicense(AutoMock mock, string licenseKey)
        {
            var customerLicense = mock.Mock<ICustomerLicense>();
            customerLicense.Setup(l => l.GetActiveStores())
                .Returns(new List<IActiveStore> { new ActiveStore() { StoreLicenseKey = licenseKey } });

            var customerLicenseService = mock.Mock<ILicenseService>();
            customerLicenseService.Setup(l => l.GetLicenses())
                .Returns(new List<ILicense> { customerLicense.Object });
        }
    }
}