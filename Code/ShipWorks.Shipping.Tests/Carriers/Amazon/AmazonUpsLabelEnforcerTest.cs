using System;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Stores;
using Xunit;
using Autofac.Extras.Moq;

namespace ShipWorks.Shipping.Tests.Carriers.Amazon
{
    public class AmazonUpsLabelEnforcerTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly AmazonStoreEntity store;

        public AmazonUpsLabelEnforcerTest()
        {
            mock = AutoMock.GetFromRepository(new MockRepository(MockBehavior.Loose) { DefaultValue = DefaultValue.Mock });

            store = new AmazonStoreEntity();

            mock.Mock<IStoreManager>()
                .Setup(x => x.GetRelatedStore(It.IsAny<long>()))
                .Returns(store);

            mock.Mock<IDateTimeProvider>()
                .Setup(x => x.CurrentSqlServerDateTime)
                .Returns(new DateTime(2015, 1, 1));
        }

        [Fact]
        public void CheckRestriction_ReturnsSuccess_WhenAmazonShippingTokenIsNotTodayAndAccountsExist_Test()
        {
            mock.Mock<ICarrierAccountRepository<UpsAccountEntity>>()
                .Setup(x => x.Accounts)
                .Returns(new[] { new UpsAccountEntity() });

            AmazonUpsLabelEnforcer testObject = mock.Create<AmazonUpsLabelEnforcer>();

            Assert.Equal(EnforcementResult.Success, testObject.CheckRestriction(new ShipmentEntity()));
        }

        [Fact]
        public void CheckRestriction_DelegatesToStoreManager()
        {
            AmazonUpsLabelEnforcer testObject = mock.Create<AmazonUpsLabelEnforcer>();
            testObject.CheckRestriction(new ShipmentEntity { ShipmentID = 1234 });

            mock.Mock<IStoreManager>()
                .Verify(x => x.GetRelatedStore(1234));
        }

        [Fact]
        public void CheckRestriction_ReturnsEnforcementFailureWithMessage_WhenAmazonShippingTokenIsToday_Test()
        {
            mock.Mock<ICarrierAccountRepository<UpsAccountEntity>>()
                .Setup(x => x.Accounts)
                .Returns(new[] { new UpsAccountEntity() });

            store.SetShippingToken(new AmazonShippingToken
            {
                ErrorDate = new DateTime(2015,1,1),
                ErrorReason = "Foo Bar"
            });

            AmazonUpsLabelEnforcer testObject = mock.Create<AmazonUpsLabelEnforcer>();
            EnforcementResult result = testObject.CheckRestriction(new ShipmentEntity());

            Assert.Equal(false, result.IsValid);
            Assert.Equal("Foo Bar", result.FailureReason);
        }

        [Fact]
        public void CheckRestriction_ReturnsEnforcementFailureWithMessage_WhenNoUpsAccountExists_Test()
        {
            store.SetShippingToken(new AmazonShippingToken
            {
                ErrorDate = new DateTime(2012, 1, 1),
                ErrorReason = "Foo Bar"
            });

            AmazonUpsLabelEnforcer testObject = mock.Create<AmazonUpsLabelEnforcer>();
            EnforcementResult result = testObject.CheckRestriction(new ShipmentEntity());

            Assert.Equal(false, result.IsValid);
            Assert.Equal("Foo Bar", result.FailureReason);
        }

        [Fact]
        public void CheckRestriction_ThrowsShippingException_WhenGivenNonAmazonShipment_Test()
        {
            mock.Mock<IStoreManager>()
                .Setup(x => x.GetRelatedStore(It.IsAny<long>()))
                .Returns(new StoreEntity());

            AmazonUpsLabelEnforcer testObject = mock.Create<AmazonUpsLabelEnforcer>();

            Exception e = Assert.Throws<ShippingException>(() => testObject.CheckRestriction(new ShipmentEntity()));
            Assert.Equal("Amazon as shipping carrier can only be used on orders from an Amazon store", e.Message);
        }

        [Fact]
        public void VerifyShipment_DelegatesToStoreManager()
        {
            AmazonUpsLabelEnforcer testObject = mock.Create<AmazonUpsLabelEnforcer>();
            testObject.VerifyShipment(new ShipmentEntity { ShipmentID = 1234 });

            mock.Mock<IStoreManager>()
                .Verify(x => x.GetRelatedStore(1234));
        }

        [Fact]
        public void VerifyShipment_ThrowsShippingException_WhenGivenNonAmazonShipment_Test()
        {
            mock.Mock<IStoreManager>()
                .Setup(x => x.GetRelatedStore(It.IsAny<long>()))
                .Returns(new StoreEntity());

            AmazonUpsLabelEnforcer testObject = mock.Create<AmazonUpsLabelEnforcer>();
            Exception e = Assert.Throws<ShippingException>(() => testObject.VerifyShipment(new ShipmentEntity()));
            Assert.Equal("Amazon as shipping carrier can only be used on orders from an Amazon store", e.Message);
        }

        [Fact]
        public void VerifyShipment_DoesNotSetError_WhenUpsAccountsExist()
        {
            mock.Mock<ICarrierAccountRepository<UpsAccountEntity>>()
                .Setup(x => x.Accounts)
                .Returns(new[] { new UpsAccountEntity { AccountNumber = "9922" } });

            AmazonUpsLabelEnforcer testObject = mock.Create<AmazonUpsLabelEnforcer>();
            testObject.VerifyShipment(new ShipmentEntity { TrackingNumber = "12399223" });

            Assert.Null(store.AmazonShippingToken);
        }

        public void Dispose() => mock?.Dispose();
    }
}
