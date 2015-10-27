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
    public class AmazonShipmentTypeTest
    {
        [Fact]
        public void IsAllowedFor_DelegatesToOrderManager_ToPopulateOrderDetails()
        {
            using (var mock = AutoMock.GetLoose())
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
        }

        [Fact]
        public void IsAllowedFor_ReturnsFalse_WhenStoreTypeIsNotAmazon()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IStoreManager>()
                    .Setup(m => m.GetStore(12))
                    .Returns(new StoreEntity { TypeCode = (int)StoreTypeCode.Ebay });

                AmazonShipmentType testObject = mock.Create<AmazonShipmentType>();

                Assert.False(testObject.IsAllowedFor(new ShipmentEntity { Order = new OrderEntity { StoreID = 12 } }));
            }
        }

        [Theory]
        [InlineData(AmazonMwsIsPrime.Yes, true)]
        [InlineData(AmazonMwsIsPrime.No, false)]
        [InlineData(AmazonMwsIsPrime.Unknown, false)]
        public void IsAllowedFor_ReturnsTrue_OnlyWhenAmazonOrderIsPrime(AmazonMwsIsPrime isPrime, bool expected)
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IStoreManager>()
                    .Setup(m => m.GetStore(It.IsAny<long>()))
                    .Returns(new StoreEntity { TypeCode = (int) StoreTypeCode.Amazon });
                
                AmazonShipmentType testObject = mock.Create<AmazonShipmentType>();

                Assert.Equal(expected, testObject.IsAllowedFor(new ShipmentEntity {
                    Order = new AmazonOrderEntity { IsPrime = (int)isPrime }
                }));
            }
        }
    }
}