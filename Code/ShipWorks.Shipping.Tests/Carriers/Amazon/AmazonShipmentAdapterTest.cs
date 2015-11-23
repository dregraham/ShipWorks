using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS;
using System;
using System.Collections.Generic;
using Moq;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Amazon
{
    public class AmazonShipmentAdapterTest
    {
        readonly ShipmentEntity shipment;
        private readonly Mock<IShipmentTypeFactory> shipmentTypeFactory;
        private readonly Mock<AmazonShipmentType> shipmentTypeMock;

        public AmazonShipmentAdapterTest()
        {
            shipment = new ShipmentEntity
            {
                ShipmentTypeCode = ShipmentTypeCode.Amazon,
                Amazon = new AmazonShipmentEntity()
            };

            shipmentTypeMock = new Mock<AmazonShipmentType>(MockBehavior.Strict);
            shipmentTypeMock.Setup(b => b.UpdateDynamicShipmentData(shipment)).Verifiable();
            shipmentTypeMock.Setup(b => b.UpdateTotalWeight(shipment)).Verifiable();
            shipmentTypeMock.Setup(b => b.SupportsMultiplePackages).Returns(() => false);
            shipmentTypeMock.Setup(b => b.IsDomestic(It.IsAny<ShipmentEntity>())).Returns(() => false);

            shipmentTypeFactory = new Mock<IShipmentTypeFactory>();
            shipmentTypeFactory.Setup(x => x.Get(shipment)).Returns(shipmentTypeMock.Object);
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExcpetion_WhenShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new AmazonShipmentAdapter(null, shipmentTypeFactory.Object));
            Assert.Throws<ArgumentNullException>(() => new AmazonShipmentAdapter(new ShipmentEntity(), shipmentTypeFactory.Object));
            Assert.Throws<ArgumentNullException>(() => new AmazonShipmentAdapter(shipment, null));
        }

        [Fact]
        public void AccountId_ReturnsShipmentValue()
        {
            AmazonShipmentAdapter testObject = new AmazonShipmentAdapter(shipment, shipmentTypeFactory.Object);
            Assert.Null(testObject.AccountId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10009238)]
        public void AccountId_StoresSpecifiedValue_WhenValueIsValid(long value)
        {
            AmazonShipmentAdapter testObject = new AmazonShipmentAdapter(shipment, shipmentTypeFactory.Object);
            testObject.AccountId = value;
            Assert.Equal(null, testObject.AccountId);
        }

        [Fact]
        public void Shipment_IsNotNull()
        {
            var testObject = new AmazonShipmentAdapter(shipment, shipmentTypeFactory.Object);
            Assert.NotNull(testObject.Shipment);
        }

        [Fact]
        public void ShipmentTypeCode_IsAmazon()
        {
            var testObject = new AmazonShipmentAdapter(shipment, shipmentTypeFactory.Object);
            Assert.Equal(ShipmentTypeCode.Amazon, testObject.ShipmentTypeCode);
        }

        [Fact]
        public void SupportsAccounts_IsFalse()
        {
            AmazonShipmentAdapter testObject = new AmazonShipmentAdapter(shipment, shipmentTypeFactory.Object);

            Assert.False(testObject.SupportsAccounts);
        }

        [Fact]
        public void SupportsMultiplePackages_IsFalse()
        {
            AmazonShipmentAdapter testObject = new AmazonShipmentAdapter(shipment, shipmentTypeFactory.Object);
            Assert.False(testObject.SupportsMultiplePackages);
        }

        [Fact]
        public void SupportsMultiplePackages_DomesticIsTrue_WhenShipCountryIsUs()
        {
            shipment.OriginCountryCode = "US";
            shipment.ShipCountryCode = "US";

            AmazonShipmentAdapter testObject = new AmazonShipmentAdapter(shipment, shipmentTypeFactory.Object);
            Assert.False(testObject.IsDomestic);
        }

        [Fact]
        public void SupportsMultiplePackages_DomesticIsFalse_WhenShipCountryIsCa()
        {
            shipment.OriginCountryCode = "US";
            shipment.ShipCountryCode = "CA";

            AmazonShipmentAdapter testObject = new AmazonShipmentAdapter(shipment, shipmentTypeFactory.Object);
            Assert.False(testObject.IsDomestic);
        }

        [Fact]
        public void UpdateDynamicData_DelegatesToShipmentType()
        {
            AmazonShipmentAdapter testObject = new AmazonShipmentAdapter(shipment, shipmentTypeFactory.Object);
            testObject.UpdateDynamicData();

            shipmentTypeMock.Verify(b => b.UpdateDynamicShipmentData(It.IsAny<ShipmentEntity>()), Times.Once);
            shipmentTypeMock.Verify(b => b.UpdateTotalWeight(It.IsAny<ShipmentEntity>()), Times.Once);
        }

        [Fact]
        public void UpdateDynamicData_ErrorsReturned_AreCorrect()
        {
            Dictionary<ShipmentEntity, Exception> errors = new Dictionary<ShipmentEntity, Exception>();
            errors.Add(shipment, new Exception("test"));

            AmazonShipmentAdapter testObject = new AmazonShipmentAdapter(shipment, shipmentTypeFactory.Object);

            Assert.NotNull(testObject.UpdateDynamicData());
            Assert.Equal(0, testObject.UpdateDynamicData().Count);
        }
    }
}
