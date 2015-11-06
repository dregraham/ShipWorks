using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using Moq;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Stores;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Amazon
{
    public class AmazonUpsLabelEnforcerTest
    {
        private ShipmentEntity amazonShipment;
        private ShipmentEntity nonAmazonShipment;
        private Mock<IStoreManager> storeManager;
        private Mock<IDateTimeProvider> dateTimeProvider;
        private Mock<ICarrierAccountRepository<IEntity2>> accountRepository;
        private AmazonStoreEntity store;
        private AmazonUpsLabelEnforcer testObject;

        public AmazonUpsLabelEnforcerTest()
        {
            amazonShipment = new ShipmentEntity()
            {
                ShipmentID = 1,
                TrackingNumber = "1111111111111111"
            };

            nonAmazonShipment = new ShipmentEntity()
            {
                ShipmentID = 2,
                TrackingNumber = "2222222222222222"
            };

            store = new AmazonStoreEntity()
            {
                AmazonShippingToken = "hlkH7XeEA5GJOefdipC2s6DY+ZF7GWI3nazovu5UYESp9FqfeIiKcfyOzL9Mdsy0",
            };

            List<UpsAccountEntity> accounts = new List<UpsAccountEntity>
            {
                new UpsAccountEntity() {AccountNumber = "12345"}
            };

            storeManager = new Mock<IStoreManager>();
            storeManager.Setup(x => x.GetRelatedStore(amazonShipment.ShipmentID)).Returns(store);
            storeManager.Setup(x => x.GetRelatedStore(nonAmazonShipment.ShipmentID)).Returns(new StoreEntity());

            dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(x => x.CurrentSqlServerDateTime).Returns(DateTime.Today);

            accountRepository = new Mock<ICarrierAccountRepository<IEntity2>>();
            accountRepository.Setup(x => x.Accounts).Returns(accounts);

            testObject = new AmazonUpsLabelEnforcer(accountRepository.Object, storeManager.Object, dateTimeProvider.Object);
        }

        [Fact]
        public void CheckRestriction_ReturnsSuccess_WhenAmazonShippingTokenIsNotToday_Test()
        {
            Assert.Equal(EnforcementResult.Success, testObject.CheckRestriction(amazonShipment));
        }

        [Fact]
        public void CheckRestriction_ReturnsEnforcementFailureWithMessage_WhenAmazonShippingTokenIsToday_Test()
        {
            store.AmazonShippingToken =
                SecureText.Encrypt(
                    $"{{\"ErrorDate\":\"{dateTimeProvider.Object.CurrentSqlServerDateTime.Date}\", \"ErrorReason\":\"ShipWorks experienced an error while trying to create your shipping label using the Amazon Shipping service. Please confirm your UPS account is linked correctly in Amazon Seller Central.\"}}", "AmazonShippingToken");

            EnforcementResult result = testObject.CheckRestriction(amazonShipment);

            Assert.Equal(false, result.IsValid);
            Assert.Equal("ShipWorks experienced an error while trying to create your shipping label using the Amazon Shipping service. Please confirm your UPS account is linked correctly in Amazon Seller Central.", result.FailureReason);
        }

        [Fact]
        public void CheckRestriction_ThrowsShippingException_WhenGivenNonAmazonShipment_Test()
        {
            Exception e = Assert.Throws<ShippingException>(() => testObject.CheckRestriction(nonAmazonShipment));
            Assert.Equal("Amazon as shipping carrier can only be used on orders from an Amazon store", e.Message);
        }

        [Fact]
        public void VerifyShipment_ThrowsShippingException_WhenGivenNonAmazonShipment_Test()
        {
            Exception e = Assert.Throws<ShippingException>(() => testObject.VerifyShipment(nonAmazonShipment));
            Assert.Equal("Amazon as shipping carrier can only be used on orders from an Amazon store", e.Message);
        }
    }
}
