﻿using System;
using Autofac.Extras.Moq;
using Interapptive.Shared.Enums;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Shipping.Carriers.Amazon.SFP;
using ShipWorks.Shipping.Policies;
using ShipWorks.Stores;
using ShipWorks.Stores.Platforms.Amazon;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Amazon
{
    public class AmazonSFPShipmentTypeTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly ShipmentEntity trackedShipment;
        private readonly Mock<IDataProvider> dataProvider;
        private readonly Mock<IStoreManager> storeManager;
        private readonly Mock<ILicenseService> licenseService;
        private readonly Mock<ILicense> license;

        private const long nonAmazonOrderID = 1;
        private const long amazonOrderID = 100;
        private const long nonAmazonStoreID = 1005;
        private const long amazonStoreID = 2005;
        private OrderEntity nonAmazonOrder = new OrderEntity(nonAmazonOrderID);
        private StoreEntity nonAmazonStore = new StoreEntity(nonAmazonStoreID);
        private AmazonOrderEntity amazonOrder = new AmazonOrderEntity(amazonOrderID);
        private AmazonStoreEntity amazonStore = new AmazonStoreEntity(amazonStoreID);
        private AmazonPrimeShippingPolicyTarget target;
        private ShipmentEntity shipment;
        private AmazonShipmentShippingPolicy amazonShipmentShippingPolicy;

        public AmazonSFPShipmentTypeTest()
        {
            mock = AutoMock.GetLoose();
            trackedShipment = new ShipmentEntity { TrackingNumber = "foo" };

            dataProvider = mock.Mock<IDataProvider>();
            dataProvider.Setup(d => d.GetEntity(nonAmazonOrderID, It.IsAny<bool>()))
                .Returns(nonAmazonOrder);
            dataProvider.Setup(d => d.GetEntity(amazonOrderID, It.IsAny<bool>()))
                .Returns(amazonOrder);

            storeManager = mock.Mock<IStoreManager>();
            storeManager.Setup(d => d.GetStore(nonAmazonStoreID))
                .Returns(new StoreEntity());
            storeManager.Setup(d => d.GetStore(amazonStoreID))
                .Returns(new AmazonStoreEntity());

            license = mock.Mock<ILicense>();
            licenseService = mock.Mock<ILicenseService>();
            licenseService.Setup(ls => ls.GetLicenses()).Returns(new[] {license.Object});

            amazonOrder.IsPrime = (int) AmazonIsPrime.No;
            shipment = new ShipmentEntity
            {
                Order = amazonOrder
            };

            target = new AmazonPrimeShippingPolicyTarget()
            {
                Shipment = shipment,
                Allowed = false,
                AmazonOrder = amazonOrder,
                AmazonCredentials = amazonStore as IAmazonCredentials
            };

            amazonShipmentShippingPolicy = new AmazonShipmentShippingPolicy();
            amazonShipmentShippingPolicy.Configure("1");
            amazonShipmentShippingPolicy.Apply(target);

            license
                .Setup(l => l.ApplyShippingPolicy(ShipmentTypeCode.AmazonSFP, It.IsAny<object>()))
                .Callback((ShipmentTypeCode s, object t) => ((AmazonPrimeShippingPolicyTarget) t).Allowed = target.Allowed);
        }

        [Fact]
        public void IsAllowedFor_ReturnsFalse_WhenStoreDoesNotImplementIAmazonCredentials()
        {
            mock.Mock<IStoreManager>()
                .Setup(m => m.GetStore(It.IsAny<long>()))
                .Returns(new EbayStoreEntity { TypeCode = (int) StoreTypeCode.Ebay });

            MockShipmentTypeRestriction(EditionRestrictionLevel.None);

            AmazonSFPShipmentType testObject = mock.Create<AmazonSFPShipmentType>();

            Assert.False(testObject.IsAllowedFor(new ShipmentEntity { Order = new EbayOrderEntity() }));
        }

        [Theory]
        [InlineData(AmazonIsPrime.Yes, true)]
        [InlineData(AmazonIsPrime.No, false)]
        [InlineData(AmazonIsPrime.Unknown, false)]
        public void IsAllowedFor_AmazonStoreAndOrders_ReturnsTrue_OnlyWhenAmazonOrderIsPrime(AmazonIsPrime isPrime, bool expected)
        {
            amazonOrder.IsPrime = (int) isPrime;
            target.AmazonOrder = amazonOrder;

            MockShipmentTypeRestriction(EditionRestrictionLevel.None);
            
            amazonShipmentShippingPolicy.Apply(target);
            AmazonSFPShipmentType testObject = mock.Create<AmazonSFPShipmentType>();

            bool actualValue = testObject.IsAllowedFor(shipment);
            Assert.Equal(expected, actualValue);
        }

        [Theory]
        [InlineData(AmazonIsPrime.Yes, true)]
        [InlineData(AmazonIsPrime.No, false)]
        [InlineData(AmazonIsPrime.Unknown, false)]
        public void IsAllowedFor_ChannelAdvisorStoreAndOrders_ReturnsExpectedValue_BasedOnAmazonOrderIsPrime(AmazonIsPrime isPrime, bool expected)
        {
            target.Shipment.Order = new ChannelAdvisorOrderEntity {IsPrime = (int) isPrime};
            target.AmazonOrder = target.Shipment.Order as IAmazonOrder;

            mock.Mock<IStoreManager>()
                .Setup(m => m.GetStore(It.IsAny<long>()))
                .Returns(new ChannelAdvisorStoreEntity { TypeCode = (int) StoreTypeCode.ChannelAdvisor });

            MockShipmentTypeRestriction(EditionRestrictionLevel.None);

            amazonShipmentShippingPolicy.Apply(target);
            AmazonSFPShipmentType testObject = mock.Create<AmazonSFPShipmentType>();

            bool actualValue = testObject.IsAllowedFor(shipment);
            Assert.Equal(expected, actualValue);
        }

        [Theory]
        [InlineData(EditionRestrictionLevel.Hidden, false)]
        [InlineData(EditionRestrictionLevel.None, true)]
        public void IsAllowedFor_ReturnsExpectedValue_WhenAmazonShipmentTypeIsRestricted(EditionRestrictionLevel restrictionLevel, bool expectedValue)
        {
            target.Shipment.Order = new ChannelAdvisorOrderEntity {IsPrime = (int) AmazonIsPrime.Yes};
            target.AmazonOrder = target.Shipment.Order as IAmazonOrder;

            var store = new ChannelAdvisorStoreEntity { TypeCode = (int) StoreTypeCode.ChannelAdvisor };

            mock.Mock<ILicenseService>()
                .Setup(l => l.CheckRestriction(It.IsAny<EditionFeature>(), It.IsAny<ShipmentTypeCode>()))
                .Returns(restrictionLevel);

            mock.Mock<IStoreManager>()
                .Setup(m => m.GetStore(It.IsAny<long>()))
                .Returns(store);

            MockShipmentTypeRestriction(restrictionLevel);

            amazonShipmentShippingPolicy.Apply(target);
            AmazonSFPShipmentType testObject = mock.Create<AmazonSFPShipmentType>();

            bool isAllowed = testObject.IsAllowedFor(shipment);
            Assert.Equal(expectedValue, isAllowed);
        }

        [Fact]
        public void TrackShipment_NoTrackingFound_NoTrackingNumber()
        {
            AmazonSFPShipmentType testObject = mock.Create<AmazonSFPShipmentType>();

            Assert.Contains("no tracking number found", testObject.TrackShipment(new ShipmentEntity()).Summary, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void TrackShipment_NoTrackingInfo_ServiceNotFound()
        {
            SetGetServiceUsedReturn("BAR");

            AmazonSFPShipmentType testObject = mock.Create<AmazonSFPShipmentType>();

            Assert.Contains("no tracking information available", testObject.TrackShipment(trackedShipment).Summary, StringComparison.OrdinalIgnoreCase);
        }

        [Theory]
        [InlineData("blah UPS blah", "wwwapps.ups.com")]
        [InlineData(" USPS DHL sm", "webtrack.dhlglobalmail.com")]
        [InlineData(" USPS ", "tools.usps.com")]
        [InlineData(" fims FEDEX ", "mailviewrecipient.fedex.com")]
        [InlineData(" fedex ", "www.fedex.com")]
        public void TrackShipment_TrackingLinkToUPS_ServiceUsedHasUPS(string service, string linkShouldContain)
        {
            SetGetServiceUsedReturn(service);

            AmazonSFPShipmentType testObject = mock.Create<AmazonSFPShipmentType>();

            Assert.Contains(linkShouldContain, testObject.TrackShipment(trackedShipment).Summary, StringComparison.OrdinalIgnoreCase);
        }

        private void SetGetServiceUsedReturn(string serviceToReturn)
        {
            mock.Mock<IShippingManager>()
                .Setup(s => s.GetOverriddenServiceUsed(It.IsAny<ShipmentEntity>()))
                .Returns(serviceToReturn);
        }

        /// <summary>
        /// Mocks the shipment type restriction.
        /// </summary>
        private void MockShipmentTypeRestriction(EditionRestrictionLevel restrictionLevel)
        {
            var mockedEditionRestriction = new Mock<EditionRestrictionIssue>();
            mockedEditionRestriction.Setup(er => er.Level)
                .Returns(restrictionLevel);

            var mockedEditionRestrictionSet = new Mock<EditionRestrictionSet>();
            mockedEditionRestrictionSet.Setup(er => er.CheckRestriction(It.IsAny<EditionFeature>(), It.IsAny<Object>()))
                .Returns(mockedEditionRestriction.Object);

            mock.Mock<IEditionManager>()
                .Setup(e => e.ActiveRestrictions)
                .Returns(mockedEditionRestrictionSet.Object);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}