using Autofac.Extras.Moq;
using Autofac.Features.Indexed;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Stores.Platforms.SparkPay.DTO;
using ShipWorks.Stores.Platforms.SparkPay.Factories;
using System;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.SparkPay
{
    public class SparkPayShipmentFactoryTest
    {
        [Fact]
        public void Create_SetsShipmentNameFromShipment()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IIndex<ShipmentTypeCode, ShipmentType>> repo = mock.MockRepository.Create<IIndex<ShipmentTypeCode, ShipmentType>>();
                repo.Setup(x => x[It.IsAny<ShipmentTypeCode>()])
                    .Returns(new UspsShipmentType());
                mock.Provide(repo.Object);

                Mock<IShippingManager> shippingManager = mock.Mock<IShippingManager>();
                shippingManager.Setup(s => s.GetOverriddenServiceUsed(It.IsAny<ShipmentEntity>())).Returns("foo");

                SparkPayShipmentFactory testObject = mock.Create<SparkPayShipmentFactory>();

                ShipmentEntity shipment = new ShipmentEntity
                {
                    ShipmentType = (int)ShipmentTypeCode.Usps,
                    ProcessedDate = DateTime.UtcNow,
                    TrackingNumber = "abcdefg",
                    Order = new OrderEntity { OrderNumber = 123 }
                };

                Shipment result = testObject.Create(shipment);

                Assert.Equal("USPS foo", result.ShipmentName);
            }
        }

        [Fact]
        public void Create_SetsShippingMethodFromShipment()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IIndex<ShipmentTypeCode, ShipmentType>> repo = mock.MockRepository.Create<IIndex<ShipmentTypeCode, ShipmentType>>();
                repo.Setup(x => x[It.IsAny<ShipmentTypeCode>()])
                    .Returns(new UspsShipmentType());
                mock.Provide(repo.Object);

                Mock<IShippingManager> shippingManager = mock.Mock<IShippingManager>();
                shippingManager.Setup(s => s.GetOverriddenServiceUsed(It.IsAny<ShipmentEntity>())).Returns("foo");

                SparkPayShipmentFactory testObject = mock.Create<SparkPayShipmentFactory>();

                ShipmentEntity shipment = new ShipmentEntity
                {
                    ShipmentType = (int)ShipmentTypeCode.Usps,
                    ProcessedDate = DateTime.UtcNow,
                    TrackingNumber = "abcdefg",
                    Order = new OrderEntity { OrderNumber = 123 }
                };

                Shipment result = testObject.Create(shipment);

                Assert.Equal("USPS foo", result.ShippingMethod);
            }
        }

        [Fact]
        public void Create_SetsTrackingNumberFromShipment()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IIndex<ShipmentTypeCode, ShipmentType>> repo = mock.MockRepository.Create<IIndex<ShipmentTypeCode, ShipmentType>>();
                repo.Setup(x => x[It.IsAny<ShipmentTypeCode>()])
                    .Returns(new UspsShipmentType());
                mock.Provide(repo.Object);

                Mock<IShippingManager> shippingManager = mock.Mock<IShippingManager>();
                shippingManager.Setup(s => s.GetOverriddenServiceUsed(It.IsAny<ShipmentEntity>())).Returns("foo");

                SparkPayShipmentFactory testObject = mock.Create<SparkPayShipmentFactory>();

                ShipmentEntity shipment = new ShipmentEntity
                {
                    ShipmentType = (int)ShipmentTypeCode.Usps,
                    ProcessedDate = DateTime.UtcNow,
                    TrackingNumber = "abcdefg",
                    Order = new OrderEntity { OrderNumber = 123 }
                };

                Shipment result = testObject.Create(shipment);

                Assert.Equal(shipment.TrackingNumber, result.TrackingNumbers);
            }
        }

        [Fact]
        public void Create_DelegatesToShippingManagerForServiceUsed()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IIndex<ShipmentTypeCode, ShipmentType>> repo = mock.MockRepository.Create<IIndex<ShipmentTypeCode, ShipmentType>>();
                repo.Setup(x => x[It.IsAny<ShipmentTypeCode>()])
                    .Returns(new UspsShipmentType());
                mock.Provide(repo.Object);

                Mock<IShippingManager> shippingManager = mock.Mock<IShippingManager>();
                shippingManager.Setup(s => s.GetOverriddenServiceUsed(It.IsAny<ShipmentEntity>())).Returns("foo");

                SparkPayShipmentFactory testObject = mock.Create<SparkPayShipmentFactory>();

                ShipmentEntity shipment = new ShipmentEntity
                {
                    ShipmentType = (int)ShipmentTypeCode.Usps,
                    ProcessedDate = DateTime.UtcNow,
                    TrackingNumber = "abcdefg",
                    Order = new OrderEntity { OrderNumber = 123 }
                };

                testObject.Create(shipment);

                shippingManager.Verify(s => s.GetOverriddenServiceUsed(shipment), Times.Once);
            }
        }

        [Fact]
        public void Create_SetsShippingMethodWithoutSymbols()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IIndex<ShipmentTypeCode, ShipmentType>> repo = mock.MockRepository.Create<IIndex<ShipmentTypeCode, ShipmentType>>();
                repo.Setup(x => x[It.IsAny<ShipmentTypeCode>()])
                    .Returns(new UspsShipmentType());
                mock.Provide(repo.Object);

                Mock<IShippingManager> shippingManager = mock.Mock<IShippingManager>();
                shippingManager.Setup(s => s.GetOverriddenServiceUsed(It.IsAny<ShipmentEntity>())).Returns("FedEx Priority Overnight®");

                SparkPayShipmentFactory testObject = mock.Create<SparkPayShipmentFactory>();

                ShipmentEntity shipment = new ShipmentEntity
                {
                    ShipmentType = (int)ShipmentTypeCode.FedEx,
                    ProcessedDate = DateTime.UtcNow,
                    TrackingNumber = "abcdefg",
                    Order = new OrderEntity { OrderNumber = 123 }
                };

                Shipment result = testObject.Create(shipment);

                Assert.Equal("FedEx FedEx Priority Overnight", result.ShippingMethod);
            }
        }
    }
}
