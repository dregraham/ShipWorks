using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Stores;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Amazon.Mws;
using Xunit;
using Xunit.Sdk;

namespace ShipWorks.Shipping.Tests.Carriers.Amazon
{
    public class AmazonShipmentTypeTest
    {
        [Theory]
        [InlineData(StoreTypeCode.Amazon, AmazonMwsIsPrime.Yes, true)]
        [InlineData(StoreTypeCode.Ebay, AmazonMwsIsPrime.Yes, false)]
        [InlineData(StoreTypeCode.Amazon, AmazonMwsIsPrime.No, false)]
        [InlineData(StoreTypeCode.Amazon, AmazonMwsIsPrime.Unknown, false)]
        public void IsAllowedFor_ReturnsTrueWhenShipmentStoreIsAmazon(StoreTypeCode storeType, AmazonMwsIsPrime isPrime, bool expected)
        {
            using (var mock = AutoMock.GetLoose())
            {
                int storeId = 10;
                var orderManager = new Mock<IOrderManager>();
                orderManager.Setup(o => o.PopulateOrderDetails(It.IsAny<ShipmentEntity>()))
                    .Callback<ShipmentEntity>(shipment => shipment.Order = 
                    new AmazonOrderEntity
                    {
                        StoreID = storeId,
                        IsPrime = (int) isPrime
                    });

                var storeManager = new Mock<IStoreManager>();
                storeManager.Setup(m => m.GetStore(It.Is<long>(id => id == storeId)))
                    .Returns(new StoreEntity() { TypeCode = (int) storeType });

                mock.Provide(orderManager.Object);
                mock.Provide(storeManager.Object);
                AmazonShipmentType testObject = mock.Create<AmazonShipmentType>();

                Assert.Equal(testObject.IsAllowedFor(new ShipmentEntity()), expected);
            }
        }
    }
}