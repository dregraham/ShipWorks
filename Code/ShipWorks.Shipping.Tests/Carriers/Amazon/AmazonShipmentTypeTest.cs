using System;
using System.Collections.Generic;
using Autofac.Core;
using Autofac.Extras.Moq;
using Moq;
using Moq.Language.Flow;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Stores;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Amazon.Mws;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Amazon
{
    public class AmazonShipmentTypeTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly ShipmentEntity trackedShipment;
        
        public AmazonShipmentTypeTest()
        {
            mock = AutoMock.GetLoose();
            trackedShipment = new ShipmentEntity {TrackingNumber = "foo"};
        }

        [Fact]
        public void IsAllowedFor_DelegatesToOrderManager_ToPopulateOrderDetails()
        {
            MockShipmentTypeRestriction(EditionRestrictionLevel.None);

            ShipmentEntity shipment = new ShipmentEntity();
            mock.Mock<IOrderManager>()
                .Setup(o => o.PopulateOrderDetails(shipment))
                .Callback<ShipmentEntity>(s => s.Order = new AmazonOrderEntity())
                .Verifiable();

            AmazonShipmentType testObject = mock.Create<AmazonShipmentType>();

            testObject.IsAllowedFor(shipment);

            mock.VerifyAll = true;
        }

        [Fact]
        public void IsAllowedFor_ReturnsFalse_WhenStoreDoesNotImplementIAmazonCredentials()
        {
            mock.Mock<IStoreManager>()
                .Setup(m => m.GetStore(It.IsAny<long>()))
                .Returns(new EbayStoreEntity { TypeCode = (int)StoreTypeCode.Ebay });

            MockShipmentTypeRestriction(EditionRestrictionLevel.None);

            AmazonShipmentType testObject = mock.Create<AmazonShipmentType>();

            Assert.False(testObject.IsAllowedFor(new ShipmentEntity { Order = new EbayOrderEntity() }));
        }

        [Theory]
        [InlineData(AmazonMwsIsPrime.Yes, true)]
        [InlineData(AmazonMwsIsPrime.No, false)]
        [InlineData(AmazonMwsIsPrime.Unknown, false)]
        public void IsAllowedFor_AmazonStoreAndOrders_ReturnsTrue_OnlyWhenAmazonOrderIsPrime(AmazonMwsIsPrime isPrime, bool expected)
        {
            ShipmentEntity shipment = new ShipmentEntity
            {
                Order = new AmazonOrderEntity {IsPrime = (int) isPrime}
            };

            MockShipmentTypeRestriction(EditionRestrictionLevel.None);
            
            mock.Mock<IStoreManager>()
                .Setup(m => m.GetStore(It.IsAny<long>()))
                .Returns(new AmazonStoreEntity { TypeCode = (int) StoreTypeCode.Amazon });

            AmazonShipmentType testObject = mock.Create<AmazonShipmentType>();

            bool actualValue = testObject.IsAllowedFor(shipment);
            Assert.Equal(expected, actualValue);
        }

        [Theory]
        [InlineData(AmazonMwsIsPrime.Yes, true)]
        [InlineData(AmazonMwsIsPrime.No, false)]
        [InlineData(AmazonMwsIsPrime.Unknown, false)]
        public void IsAllowedFor_ChannelAdvisorStoreAndOrders_ReturnsExpectedValue_BasedOnAmazonOrderIsPrime(AmazonMwsIsPrime isPrime, bool expected)
        {
            ShipmentEntity shipment = new ShipmentEntity
            {
                Order = new ChannelAdvisorOrderEntity { IsPrime = (int)isPrime }
            };

            mock.Mock<IStoreManager>()
                .Setup(m => m.GetStore(It.IsAny<long>()))
                .Returns(new ChannelAdvisorStoreEntity { TypeCode = (int)StoreTypeCode.ChannelAdvisor });

            MockShipmentTypeRestriction(EditionRestrictionLevel.None);

            AmazonShipmentType testObject = mock.Create<AmazonShipmentType>();

            bool actualValue = testObject.IsAllowedFor(shipment);
            Assert.Equal(expected, actualValue);
        }

        [Theory]
        [InlineData(EditionRestrictionLevel.Hidden, false)]
        [InlineData(EditionRestrictionLevel.None, true)]
        public void IsAllowedFor_ReturnsExpectedValue_WhenAmazonShipmentTypeIsRestricted(EditionRestrictionLevel restrictionLevel, bool expectedValue)
        {
            ShipmentEntity shipment = new ShipmentEntity
            {
                Order = new ChannelAdvisorOrderEntity { IsPrime = (int)AmazonMwsIsPrime.Yes }
            };

            var store = new ChannelAdvisorStoreEntity { TypeCode = (int)StoreTypeCode.ChannelAdvisor };

            mock.Mock<IStoreManager>()
                .Setup(m => m.GetStore(It.IsAny<long>()))
                .Returns(store);

            MockShipmentTypeRestriction(restrictionLevel);

            AmazonShipmentType testObject = mock.Create<AmazonShipmentType>();

            bool isAllowed = testObject.IsAllowedFor(shipment);
            Assert.Equal(expectedValue, isAllowed);
        }

        [Fact]
        public void TrackShipment_NoTrackingFound_NoTrackingNumber()
        {
            AmazonShipmentType testObject = mock.Create<AmazonShipmentType>();
            
            Assert.Contains("no tracking number found", testObject.TrackShipment(new ShipmentEntity()).Summary, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void TrackShipment_NoTrackingInfo_ServiceNotFound()
        {
            SetGetServiceUsedReturn("BAR");

            AmazonShipmentType testObject = mock.Create<AmazonShipmentType>();

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

            AmazonShipmentType testObject = mock.Create<AmazonShipmentType>();

            Assert.Contains(linkShouldContain, testObject.TrackShipment(trackedShipment).Summary, StringComparison.OrdinalIgnoreCase);
        }

        public void SetGetServiceUsedReturn(string serviceToReturn)
        {
            mock.Mock<IShippingManager>()
                .Setup(s => s.GetServiceUsed(It.IsAny<ShipmentEntity>()))
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