using ShipWorks.Data.Model.EntityClasses;
using System;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Stores;
using Xunit;
using Autofac.Extras.Moq;

namespace ShipWorks.Shipping.Tests.Carriers.Amazon
{
    public class AmazonUspsLabelEnforcerTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly AmazonStoreEntity store;
        private readonly ShipmentEntity shipment;

        public AmazonUspsLabelEnforcerTest()
        {
            mock = AutoMock.GetFromRepository(new MockRepository(MockBehavior.Loose) { DefaultValue = DefaultValue.Mock });

            store = new AmazonStoreEntity();

            mock.Mock<IStoreManager>()
                .Setup(x => x.GetRelatedStore(It.IsAny<ShipmentEntity>()))
                .Returns(store);

            mock.Mock<IDateTimeProvider>()
                .Setup(x => x.CurrentSqlServerDateTime)
                .Returns(new DateTime(2015, 1, 1));

            shipment = new ShipmentEntity
            {
                TrackingNumber = "00000110000",
                Amazon = new AmazonShipmentEntity
                {
                    CarrierName = "STAMPS_DOT_COM"
                }
            };
        }

        [Fact]
        public void CheckRestriction_DelegatesToStoreManager()
        {
            AmazonUspsLabelEnforcer testObject = mock.Create<AmazonUspsLabelEnforcer>();
            testObject.CheckRestriction(shipment);

            mock.Mock<IStoreManager>()
                .Verify(x => x.GetRelatedStore(shipment));
        }

        [Fact]
        public void CheckRestriction_ReturnsEnforcementFailureWithMessage_WhenAmazonShippingTokenIsToday_Test()
        {
            store.SetShippingToken(new AmazonShippingToken
            {
                ErrorDate = new DateTime(2015, 1, 1),
                ErrorReason = "Foo Bar"
            });

            AmazonUspsLabelEnforcer testObject = mock.Create<AmazonUspsLabelEnforcer>();
            EnforcementResult result = testObject.CheckRestriction(shipment);

            Assert.Equal(false, result.IsValid);
            Assert.Equal("Foo Bar", result.FailureReason);
        }

        [Fact]
        public void CheckRestriction_ThrowsShippingException_WhenGivenNonAmazonShipment_Test()
        {
            mock.Mock<IStoreManager>()
                .Setup(x => x.GetRelatedStore(It.IsAny<ShipmentEntity>()))
                .Returns(new StoreEntity());

            AmazonUspsLabelEnforcer testObject = mock.Create<AmazonUspsLabelEnforcer>();

            Exception e = Assert.Throws<ShippingException>(() => testObject.CheckRestriction(shipment));
            Assert.Equal("Amazon as shipping carrier can only be used on orders from an Amazon store", e.Message);
        }

        [Fact]
        public void VerifyShipment_DoesNotSetError_WhenCarrierIsNotUsps()
        {
            shipment.TrackingNumber = "0000011000";
            shipment.Amazon.CarrierName = "UPS";

            AmazonUspsLabelEnforcer testObject = mock.Create<AmazonUspsLabelEnforcer>();

            testObject.VerifyShipment(shipment);

            Assert.Null(store.AmazonShippingToken);
        }

        [Fact]
        public void VerifyShipment_DelegatesToStoreManager()
        {
            AmazonUspsLabelEnforcer testObject = mock.Create<AmazonUspsLabelEnforcer>();
            testObject.VerifyShipment(shipment);

            mock.Mock<IStoreManager>()
                .Verify(x => x.GetRelatedStore(shipment));
        }

        [Fact]
        public void VerifyShipment_ThrowsShippingException_WhenGivenNonAmazonShipment_Test()
        {
            mock.Mock<IStoreManager>()
                .Setup(x => x.GetRelatedStore(It.IsAny<ShipmentEntity>()))
                .Returns(new StoreEntity());

            AmazonUspsLabelEnforcer testObject = mock.Create<AmazonUspsLabelEnforcer>();
            Exception e = Assert.Throws<ShippingException>(() => testObject.VerifyShipment(shipment));
            Assert.Equal("Amazon as shipping carrier can only be used on orders from an Amazon store", e.Message);
        }

        [Theory]
        [InlineData("00000120000")]
        [InlineData("00000100000")]
        [InlineData("00000234")]
        [InlineData("000009932")]
        [InlineData("FooBarTracking")]
        public void VerifyShipment_SetsShipmentToken_WhenTrackingNumberIsNotStamps(string tracking)
        {
            shipment.TrackingNumber = tracking;
            shipment.Amazon.CarrierName = "STAMPS_DOT_COM";

            AmazonUspsLabelEnforcer testObject = mock.Create<AmazonUspsLabelEnforcer>();

            testObject.VerifyShipment(shipment);

            Assert.NotNull(store.AmazonShippingToken);
            Assert.Equal(new DateTime(2015, 1, 1), store.GetShippingToken().ErrorDate);
        }

        [Theory]
        [InlineData("00000110000")]
        [InlineData("00000160000")]
        public void VerifyShipment_DoesNotSetShipmentToken_WhenTrackingNumberIsStamps(string tracking)
        {
            shipment.TrackingNumber = tracking;
            shipment.Amazon.CarrierName = "STAMPS_DOT_COM";

            AmazonUspsLabelEnforcer testObject = mock.Create<AmazonUspsLabelEnforcer>();

            testObject.VerifyShipment(shipment);

            Assert.Null(store.AmazonShippingToken);
        }

        public void Dispose() => mock?.Dispose();
    }
}
