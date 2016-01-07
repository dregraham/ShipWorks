using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS;
using System;
using System.Collections.Generic;
using Moq;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Services;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Ups
{
    public class UpsShipmentAdapterTest
    {
        readonly ShipmentEntity shipment;
        private readonly Mock<IShipmentTypeFactory> shipmentTypeFactory;
        private readonly Mock<ICustomsManager> customsManager;
        private readonly Mock<UpsShipmentType> shipmentTypeMock;
        private readonly ShipmentType shipmentType;

        public UpsShipmentAdapterTest()
        {
            shipmentType = new UpsOltShipmentType();
            shipment = new ShipmentEntity
            {
                ShipmentTypeCode = ShipmentTypeCode.UpsOnLineTools,
                ShipDate = new DateTime(2015, 11, 24, 10, 10, 10),
                ContentWeight = 1,
                TotalWeight = 1,
                Insurance = true,
                Ups = new UpsShipmentEntity()
                {
                    Service = (int)UpsServiceType.Ups2DayAirAM
                }
            };

            customsManager = new Mock<ICustomsManager>();
            customsManager.Setup(c => c.EnsureCustomsLoaded(It.IsAny<IEnumerable<ShipmentEntity>>())).Returns(new Dictionary<ShipmentEntity, Exception>());

            shipmentTypeMock = new Mock<UpsShipmentType>(MockBehavior.Strict);
            shipmentTypeMock.Setup(b => b.UpdateDynamicShipmentData(shipment)).Verifiable();
            shipmentTypeMock.Setup(b => b.UpdateTotalWeight(shipment)).Verifiable();
            shipmentTypeMock.Setup(b => b.SupportsMultiplePackages).Returns(() => shipmentType.SupportsMultiplePackages);
            shipmentTypeMock.Setup(b => b.IsDomestic(It.IsAny<ShipmentEntity>())).Returns(() => shipmentType.IsDomestic(shipment));

            shipmentTypeFactory = new Mock<IShipmentTypeFactory>();
            shipmentTypeFactory.Setup(x => x.Get(shipment)).Returns(shipmentTypeMock.Object);
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExcpetion_WhenShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new UpsShipmentAdapter(null, shipmentTypeFactory.Object, customsManager.Object));
            Assert.Throws<ArgumentNullException>(() => new UpsShipmentAdapter(new ShipmentEntity(), shipmentTypeFactory.Object, customsManager.Object));
            Assert.Throws<ArgumentNullException>(() => new UpsShipmentAdapter(shipment, null, customsManager.Object));
            Assert.Throws<ArgumentNullException>(() => new UpsShipmentAdapter(shipment, shipmentTypeFactory.Object, null));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExcpetion_WhenPostalShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new UpsShipmentAdapter(new ShipmentEntity(), shipmentTypeFactory.Object, customsManager.Object));
        }

        [Fact]
        public void AccountId_ReturnsShipmentValue()
        {
            shipment.Ups.UpsAccountID = 12;
            var testObject = new UpsShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            Assert.Equal(12, testObject.AccountId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10009238)]
        public void AccountId_StoresSpecifiedValue_WhenValueIsValid(long value)
        {
            var testObject = new UpsShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            testObject.AccountId = value;
            Assert.Equal(value, shipment.Ups.UpsAccountID);
        }

        [Fact]
        public void AccountId_StoresZero_WhenValueIsNull()
        {
            var testObject = new UpsShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            testObject.AccountId = null;
            Assert.Equal(0, shipment.Ups.UpsAccountID);
        }

        [Fact]
        public void Shipment_IsNotNull()
        {
            var testObject = new UpsShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            Assert.NotNull(testObject.Shipment);
        }

        [Fact]
        public void ShipmentTypeCode_IsUps()
        {
            var testObject = new UpsShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            Assert.Equal(ShipmentTypeCode.UpsOnLineTools, testObject.ShipmentTypeCode);
        }

        [Fact]
        public void SupportsAccounts_IsTrue()
        {
            UpsShipmentAdapter testObject = new UpsShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);

            Assert.True(testObject.SupportsAccounts);
        }

        [Fact]
        public void SupportsMultiplePackages_IsTrue()
        {
            UpsShipmentAdapter testObject = new UpsShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            Assert.True(testObject.SupportsMultiplePackages);
        }

        [Fact]
        public void SupportsMultiplePackages_DomesticIsTrue_WhenShipCountryIsUs()
        {
            shipment.OriginCountryCode = "US";
            shipment.ShipCountryCode = "US";

            UpsShipmentAdapter testObject = new UpsShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            Assert.True(testObject.IsDomestic);
        }

        [Fact]
        public void SupportsMultiplePackages_DomesticIsFalse_WhenShipCountryIsCa()
        {
            shipment.OriginCountryCode = "US";
            shipment.ShipCountryCode = "CA";

            UpsShipmentAdapter testObject = new UpsShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            Assert.False(testObject.IsDomestic);
        }

        [Fact]
        public void UpdateDynamicData_DelegatesToShipmentTypeAndCustomsManager()
        {
            UpsShipmentAdapter testObject = new UpsShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            testObject.UpdateDynamicData();

            shipmentTypeMock.Verify(b => b.UpdateDynamicShipmentData(It.IsAny<ShipmentEntity>()), Times.Once);
            shipmentTypeMock.Verify(b => b.UpdateTotalWeight(It.IsAny<ShipmentEntity>()), Times.Once);

            customsManager.Verify(b => b.EnsureCustomsLoaded(It.IsAny<IEnumerable<ShipmentEntity>>()), Times.Once);
        }

        [Fact]
        public void UpdateDynamicData_ErrorsReturned_AreCorrect()
        {
            Dictionary<ShipmentEntity, Exception> errors = new Dictionary<ShipmentEntity, Exception>();
            errors.Add(shipment, new Exception("test"));

            customsManager.Setup(c => c.EnsureCustomsLoaded(It.IsAny<IEnumerable<ShipmentEntity>>())).Returns(errors);

            UpsShipmentAdapter testObject = new UpsShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);

            Assert.NotNull(testObject.UpdateDynamicData());
            Assert.Equal(1, testObject.UpdateDynamicData().Count);
        }

        [Fact]
        public void SupportsPackageTypes_IsTrue()
        {
            ICarrierShipmentAdapter testObject = new UpsShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);

            Assert.True(testObject.SupportsPackageTypes);
        }

        [Fact]
        public void ShipDate_ReturnsShipmentValue()
        {
            ICarrierShipmentAdapter testObject = new UpsShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            Assert.Equal(shipment.ShipDate, testObject.ShipDate);
        }

        [Fact]
        public void ShipDate_IsUpdated()
        {
            ICarrierShipmentAdapter testObject = new UpsShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);

            testObject.ShipDate = testObject.ShipDate.AddDays(1);

            Assert.Equal(shipment.ShipDate, testObject.ShipDate);
        }

        [Fact]
        public void ServiceType_ReturnsShipmentValue()
        {
            ICarrierShipmentAdapter testObject = new UpsShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            Assert.Equal(shipment.Ups.Service, testObject.ServiceType);
        }

        [Fact]
        public void ServiceType_IsUpdated()
        {
            ICarrierShipmentAdapter testObject = new UpsShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);

            shipment.Ups.Service = (int)UpsServiceType.UpsGround;
            testObject.ServiceType = (int)UpsServiceType.Ups2DayAir;

            Assert.Equal(shipment.Ups.Service, testObject.ServiceType);
        }
    }
}
