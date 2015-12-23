using System;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Stores;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Amazon
{
    public class AmazonUpsLabelEnforcerTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly AmazonStoreEntity store;
        private readonly ShipmentEntity shipment;

        public AmazonUpsLabelEnforcerTest()
        {
            mock = AutoMock.GetFromRepository(new MockRepository(MockBehavior.Loose) { DefaultValue = DefaultValue.Mock });

            store = new AmazonStoreEntity();

            mock.Mock<IStoreManager>()
                .Setup(x => x.GetRelatedStore(It.IsAny<ShipmentEntity>()))
                .Returns(store);

            mock.Mock<ISqlDateTimeProvider>()
                .Setup(x => x.GetLocalDate())
                .Returns(new DateTime(2015, 1, 1));

            shipment = new ShipmentEntity
            {
                Amazon = new AmazonShipmentEntity
                {
                    CarrierName = "UPS"
                }
            };
        }

        [Fact]
        public void CheckRestriction_ReturnsSuccess_WhenCarrierIsNotUPSAndNoAccountsExist()
        {
            shipment.Amazon.CarrierName = "STAMPS_DOT_COM";

            AmazonUpsLabelEnforcer testObject = mock.Create<AmazonUpsLabelEnforcer>();

            Assert.Equal(EnforcementResult.Success, testObject.CheckRestriction(shipment));
        }

        [Fact]
        public void CheckRestriction_ReturnsSuccess_WhenAmazonShippingTokenIsNotTodayAndAccountsExist_Test()
        {
            mock.Mock<ICarrierAccountRepository<UpsAccountEntity>>()
                .Setup(x => x.Accounts)
                .Returns(new[] { new UpsAccountEntity() });

            AmazonUpsLabelEnforcer testObject = mock.Create<AmazonUpsLabelEnforcer>();

            Assert.Equal(EnforcementResult.Success, testObject.CheckRestriction(shipment));
        }

        [Fact]
        public void CheckRestriction_DelegatesToStoreManager()
        {
            AmazonUpsLabelEnforcer testObject = mock.Create<AmazonUpsLabelEnforcer>();
            testObject.CheckRestriction(shipment);

            mock.Mock<IStoreManager>()
                .Verify(x => x.GetRelatedStore(shipment));
        }

        [Fact]
        public void CheckRestriction_ReturnsEnforcementFailureWithMessage_WhenAmazonShippingTokenIsToday_Test()
        {
            mock.Mock<ICarrierAccountRepository<UpsAccountEntity>>()
                .Setup(x => x.Accounts)
                .Returns(new[] { new UpsAccountEntity() });

            store.SetShippingToken(new AmazonShippingToken
            {
                ErrorDate = new DateTime(2015, 1, 1),
                ErrorReason = "Foo Bar"
            });

            AmazonUpsLabelEnforcer testObject = mock.Create<AmazonUpsLabelEnforcer>();
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

            AmazonUpsLabelEnforcer testObject = mock.Create<AmazonUpsLabelEnforcer>();

            Exception e = Assert.Throws<ShippingException>(() => testObject.CheckRestriction(shipment));
            Assert.Equal("Amazon as shipping carrier can only be used on orders from an Amazon store", e.Message);
        }

        [Fact]
        public void VerifyShipment_DelegatesToStoreManager()
        {
            AmazonUpsLabelEnforcer testObject = mock.Create<AmazonUpsLabelEnforcer>();
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

            AmazonUpsLabelEnforcer testObject = mock.Create<AmazonUpsLabelEnforcer>();
            Exception e = Assert.Throws<ShippingException>(() => testObject.VerifyShipment(shipment));
            Assert.Equal("Amazon as shipping carrier can only be used on orders from an Amazon store", e.Message);
        }

        [Fact]
        public void VerifyShipment_DoesNotSetError_WhenCasesDoNotMatchUpsAccountsExist()
        {
            shipment.TrackingNumber = "123Tt99223";

            mock.Mock<ICarrierAccountRepository<UpsAccountEntity>>()
                .Setup(x => x.Accounts)
                .Returns(new[] { new UpsAccountEntity { AccountNumber = "TT9922" } });

            AmazonUpsLabelEnforcer testObject = mock.Create<AmazonUpsLabelEnforcer>();
            testObject.VerifyShipment(shipment);

            Assert.Null(store.AmazonShippingToken);
        }

        [Fact]
        public void VerifyShipment_DoesNotSetError_WhenCasesDMatchUpsAccountsExist()
        {
            shipment.TrackingNumber = "123TT99223";

            mock.Mock<ICarrierAccountRepository<UpsAccountEntity>>()
                .Setup(x => x.Accounts)
                .Returns(new[] { new UpsAccountEntity { AccountNumber = "TT9922" } });

            AmazonUpsLabelEnforcer testObject = mock.Create<AmazonUpsLabelEnforcer>();
            testObject.VerifyShipment(shipment);

            Assert.Null(store.AmazonShippingToken);
        }

        [Fact]
        public void VerifyShipment_DoesSetError_WhenUpsAccountDoesNotExist()
        {
            shipment.TrackingNumber = "123TT99223";

            mock.Mock<ICarrierAccountRepository<UpsAccountEntity>>()
                .Setup(x => x.Accounts)
                .Returns(new[] { new UpsAccountEntity { AccountNumber = "5T9922" } });

            AmazonUpsLabelEnforcer testObject = mock.Create<AmazonUpsLabelEnforcer>();
            testObject.VerifyShipment(shipment);

            Assert.NotNull(store.AmazonShippingToken);
        }

        [Fact]
        public void VerifyShipment_DoesNotSetError_WhenCarrierIsNotUPS()
        {
            shipment.Amazon.CarrierName = "STAMPS_DOT_COM";

            AmazonUpsLabelEnforcer testObject = mock.Create<AmazonUpsLabelEnforcer>();

            testObject.VerifyShipment(shipment);

            Assert.Null(store.AmazonShippingToken);
        }

        public void Dispose() => mock?.Dispose();
    }
}
