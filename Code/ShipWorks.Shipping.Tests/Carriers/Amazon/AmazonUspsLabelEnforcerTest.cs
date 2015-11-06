using ShipWorks.Data.Model.EntityClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Connection;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Stores;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Amazon
{
    public class AmazonUspsLabelEnforcerTest
    {
        ShipmentEntity shipment;
        private Mock<IStoreManager> storeManager;
        private Mock<IDateTimeProvider> dateTimeProvider; 
        private AmazonStoreEntity store;
        private AmazonUspsLabelEnforcer testObject;

        public AmazonUspsLabelEnforcerTest()
        {
            shipment = new ShipmentEntity()
            {
                ShipmentID = 1,
                TrackingNumber = "1111111111111111"
            };
            
            store = new AmazonStoreEntity()
            {
                AmazonShippingToken = "hlkH7XeEA5GJOefdipC2s6DY+ZF7GWI3nazovu5UYESp9FqfeIiKcfyOzL9Mdsy0",
            };

            storeManager = new Mock<IStoreManager>();
            storeManager.Setup(x => x.GetRelatedStore(shipment.ShipmentID)).Returns(store);
            dateTimeProvider.Setup(x => x.CurrentSqlServerDateTime).Returns(DateTime.Today);
            testObject = new AmazonUspsLabelEnforcer(storeManager.Object, dateTimeProvider.Object);
        }

        [Fact]
        public void CheckRestriction_ReturnsSuccess_WhenAmazonShippingTokenIsNotToday_Test()
        {
            Assert.Equal(EnforcementResult.Success, testObject.CheckRestriction(shipment));
        }

        [Fact]
        public void VerifyShipment_DoesNotSetAmazonShippingToken_WhenGivenStampsShipment_Test()
        {
            testObject.VerifyShipment(shipment);
            Assert.Equal("hlkH7XeEA5GJOefdipC2s6DY+ZF7GWI3nazovu5UYESp9FqfeIiKcfyOzL9Mdsy0", store.AmazonShippingToken);
        }

        [Fact]
        public void CheckRestriction_ReturnsEnforcementFailure_WhenAmazonShippingTokenIsToday_Test()
        {
            store.AmazonShippingToken =
                SecureText.Encrypt(
                    $"{{\"ErrorDate\":\"{dateTimeProvider.Object.CurrentSqlServerDateTime}\", \"ErrorReason\":\"ShipWorks experienced an error while trying to create your shipping label using the Amazon Shipping service. Please confirm your Stamps.com account is linked correctly in Amazon Seller Central.\"}}", "AmazonShippingToken");
        }
    }
}
