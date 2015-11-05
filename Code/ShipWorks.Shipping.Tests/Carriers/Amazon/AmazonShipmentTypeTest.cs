using System;
using Autofac.Core;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Stores;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Amazon.Mws;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Amazon
{
    public class AmazonShipmentTypeTest : IDisposable
    {
        AutoMock mock;
        private ShipmentEntity trackedShipment;
        
        public AmazonShipmentTypeTest()
        {
            mock = AutoMock.GetLoose();
            trackedShipment = new ShipmentEntity {TrackingNumber = "foo"};
        }

        [Fact]
        public void IsAllowedFor_DelegatesToOrderManager_ToPopulateOrderDetails()
        {
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
        public void IsAllowedFor_ReturnsFalse_WhenStoreTypeIsNotAmazon()
        {
            mock.Mock<IStoreManager>()
                .Setup(m => m.GetStore(12))
                .Returns(new StoreEntity { TypeCode = (int) StoreTypeCode.Ebay });

            AmazonShipmentType testObject = mock.Create<AmazonShipmentType>();

            Assert.False(testObject.IsAllowedFor(new ShipmentEntity { Order = new OrderEntity { StoreID = 12 } }));
        }

        [Theory]
        [InlineData(AmazonMwsIsPrime.Yes, true)]
        [InlineData(AmazonMwsIsPrime.No, false)]
        [InlineData(AmazonMwsIsPrime.Unknown, false)]
        public void IsAllowedFor_ReturnsTrue_OnlyWhenAmazonOrderIsPrime(AmazonMwsIsPrime isPrime, bool expected)
        {
            mock.Mock<IStoreManager>()
                .Setup(m => m.GetStore(It.IsAny<long>()))
                .Returns(new AmazonStoreEntity { TypeCode = (int) StoreTypeCode.Amazon });

            AmazonShipmentType testObject = mock.Create<AmazonShipmentType>();

            Assert.Equal(expected, testObject.IsAllowedFor(new ShipmentEntity
            {
                Order = new AmazonOrderEntity { IsPrime = (int) isPrime }
            }));
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
        public void Dispose()
        {
            mock.Dispose();
        }
    }
}