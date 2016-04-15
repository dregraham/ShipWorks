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
    public class OverChannelLimitBehaviorTest
    {
        [Fact]
        public void PopulateChannels_ChannelsIncludeActiveChannelFromTango()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var license = mock.Mock<ICustomerLicense>();
                license.Setup(l => l.GetActiveStores())
                    .Returns(new List<ActiveStore> {new ActiveStore()});

                mock.Mock<ILicenseService>()
                    .Setup(s => s.GetLicenses())
                    .Returns(new List<ILicense> {license.Object});

                mock.Mock<IShipWorksLicense>()
                    .Setup(l => l.StoreTypeCode)
                    .Returns(StoreTypeCode.GenericFile);

                mock.Mock<IStoreManager>()
                    .Setup(s => s.GetAllStores())
                    .Returns(new List<StoreEntity>());

                var testObject = mock.Create<OverChannelLimitBehavior>();

                var channels = new ObservableCollection<StoreTypeCode>();

                testObject.PopulateChannels(channels, null);

                Assert.Equal(StoreTypeCode.GenericFile, channels.Single());
            }
        }

        [Fact]
        public void PopulateChannels_ChannelsDoNotIncludeInstalledStores()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var license = mock.Mock<ICustomerLicense>();
                license.Setup(l => l.GetActiveStores())
                    .Returns(new List<ActiveStore>());

                mock.Mock<ILicenseService>()
                    .Setup(s => s.GetLicenses())
                    .Returns(new List<ILicense> {license.Object});

                mock.Mock<IStoreManager>()
                    .Setup(s => s.GetAllStores())
                    .Returns(new List<StoreEntity>
                    {
                        new StoreEntity
                        {
                            TypeCode = (int) StoreTypeCode.BuyDotCom
                        }
                    });

                var testObject = mock.Create<OverChannelLimitBehavior>();

                var channels = new ObservableCollection<StoreTypeCode>();

                testObject.PopulateChannels(channels, null);

                Assert.Empty(channels);
            }
        }

        [Fact]
        public void PopulateChannels_ChannelToAddNotAddedToChannels_WhenChannelToAddEqualsAChannelInShipWorks()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var license = mock.Mock<ICustomerLicense>();
                license.Setup(l => l.GetActiveStores())
                    .Returns(new List<ActiveStore>());

                mock.Mock<ILicenseService>()
                    .Setup(s => s.GetLicenses())
                    .Returns(new List<ILicense> {license.Object});

                mock.Mock<IStoreManager>()
                    .Setup(s => s.GetAllStores())
                    .Returns(new List<StoreEntity>
                    {
                        new StoreEntity
                        {
                            TypeCode = (int) StoreTypeCode.BuyDotCom
                        }
                    });

                var testObject = mock.Create<OverChannelLimitBehavior>();

                var channels = new ObservableCollection<StoreTypeCode>();

                testObject.PopulateChannels(channels, StoreTypeCode.BuyDotCom);

                Assert.DoesNotContain(StoreTypeCode.BuyDotCom, channels);
            }
        }

        [Fact]
        public void EditionFeature_IsChannelCount()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var license = mock.Mock<ICustomerLicense>();
                license.Setup(l => l.GetActiveStores())
                    .Returns(new List<ActiveStore>());

                mock.Mock<ILicenseService>()
                    .Setup(s => s.GetLicenses())
                    .Returns(new List<ILicense> { license.Object });

                var testObject = mock.Create<OverChannelLimitBehavior>();
                Assert.Equal(EditionFeature.ChannelCount, testObject.EditionFeature);
            }
        }

        [Fact]
        public void Title_ContainsChannelLimit()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var license = mock.Mock<ICustomerLicense>();
                license.Setup(l => l.GetActiveStores())
                    .Returns(new List<ActiveStore>());

                mock.Mock<ILicenseService>()
                    .Setup(s => s.GetLicenses())
                    .Returns(new List<ILicense> { license.Object });

                var testObject = mock.Create<OverChannelLimitBehavior>();
                Assert.Contains("Channel Limit", testObject.Title);
            }
        }
    }
}
