using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using System;
using System.Collections.Generic;
using Moq;
using ShipWorks.AddressValidation;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Services;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Postal.Usps
{
    public class UspsShipmentAdapterTest
    {
        readonly ShipmentEntity shipment;
        private readonly Mock<IShipmentTypeFactory> shipmentTypeFactory;
        private readonly Mock<ICustomsManager> customsManager;
        private readonly Mock<UspsShipmentType> shipmentTypeMock;
        private readonly ShipmentType shipmentType;

        public UspsShipmentAdapterTest()
        {
            shipmentType = new UspsShipmentType();
            shipment = new ShipmentEntity
            {
                ShipmentTypeCode = ShipmentTypeCode.Usps,
                ShipDate = new DateTime(2015, 11, 24, 10, 10, 10),
                ContentWeight = 1,
                TotalWeight = 1,
                Insurance = true,
                Postal = new PostalShipmentEntity
                {
                    Service = (int)PostalServiceType.FirstClass,
                    Usps = new UspsShipmentEntity()
                }
            };

            customsManager = new Mock<ICustomsManager>();
            customsManager.Setup(c => c.EnsureCustomsLoaded(It.IsAny<IEnumerable<ShipmentEntity>>())).Returns(new Dictionary<ShipmentEntity, Exception>());

            shipmentTypeMock = new Mock<UspsShipmentType>(MockBehavior.Strict);
            shipmentTypeMock.Setup(b => b.UpdateDynamicShipmentData(shipment)).Verifiable();
            shipmentTypeMock.Setup(b => b.UpdateTotalWeight(shipment)).Verifiable();
            shipmentTypeMock.Setup(b => b.SupportsMultiplePackages).Returns(() => shipmentType.SupportsMultiplePackages);
            shipmentTypeMock.Setup(b => b.IsDomestic(It.IsAny<ShipmentEntity>())).Returns(() => shipmentType.IsDomestic(shipment));
            shipmentTypeMock.Setup(b => b.ShipmentTypeCode).Returns(() => shipmentType.ShipmentTypeCode);

            shipmentTypeFactory = new Mock<IShipmentTypeFactory>();
            shipmentTypeFactory.Setup(x => x.Get(shipment)).Returns(shipmentTypeMock.Object);
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExcpetion_WhenShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new UspsShipmentAdapter(null, shipmentTypeFactory.Object, customsManager.Object));
            Assert.Throws<ArgumentNullException>(() => new UspsShipmentAdapter(new ShipmentEntity(), shipmentTypeFactory.Object, customsManager.Object));
            Assert.Throws<ArgumentNullException>(() => new UspsShipmentAdapter(new ShipmentEntity { Postal = new PostalShipmentEntity() }, shipmentTypeFactory.Object, customsManager.Object));

            Assert.Throws<ArgumentNullException>(() => new UspsShipmentAdapter(shipment, null, customsManager.Object));
            Assert.Throws<ArgumentNullException>(() => new UspsShipmentAdapter(shipment, shipmentTypeFactory.Object, null));

        }

        [Fact]
        public void AccountId_ReturnsShipmentValue()
        {
            shipment.Postal.Usps.UspsAccountID = 12;
            var testObject = new UspsShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            Assert.Equal(12, testObject.AccountId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10009238)]
        public void AccountId_StoresSpecifiedValue_WhenValueIsValid(long value)
        {
            var testObject = new UspsShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            testObject.AccountId = value;
            Assert.Equal(value, shipment.Postal.Usps.UspsAccountID);
        }

        [Fact]
        public void AccountId_StoresZero_WhenValueIsNull()
        {
            var testObject = new UspsShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            testObject.AccountId = null;
            Assert.Equal(0, shipment.Postal.Usps.UspsAccountID);
        }

        [Fact]
        public void Shipment_IsNotNull()
        {
            var testObject = new UspsShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            Assert.NotNull(testObject.Shipment);
        }

        [Fact]
        public void ShipmentTypeCode_IsUsps()
        {
            var testObject = new UspsShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            Assert.Equal(ShipmentTypeCode.Usps, testObject.ShipmentTypeCode);
        }

        [Fact]
        public void ShipmentTypeCode_IsExpress1Usps()
        {
            shipment.ShipmentTypeCode = ShipmentTypeCode.Express1Usps;
            var testObject = new UspsShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            Assert.Equal(ShipmentTypeCode.Express1Usps, testObject.ShipmentTypeCode);
        }

        [Fact]
        public void SupportsAccounts_IsTrue()
        {
            UspsShipmentAdapter testObject = new UspsShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);

            Assert.True(testObject.SupportsAccounts);
        }

        [Fact]
        public void SupportsMultiplePackages_IsFalse()
        {
            UspsShipmentAdapter testObject = new UspsShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            Assert.False(testObject.SupportsMultiplePackages);
        }

        [Fact]
        public void SupportsMultiplePackages_DomesticIsTrue_WhenShipCountryIsUs()
        {
            shipment.OriginCountryCode = "US";
            shipment.ShipCountryCode = "US";

            UspsShipmentAdapter testObject = new UspsShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            Assert.True(testObject.IsDomestic);
        }

        [Fact]
        public void SupportsMultiplePackages_DomesticIsFalse_WhenShipCountryIsCa()
        {
            shipment.OriginCountryCode = "US";
            shipment.ShipCountryCode = "CA";

            UspsShipmentAdapter testObject = new UspsShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            Assert.False(testObject.IsDomestic);
        }

        [Fact]
        public void UpdateDynamicData_DelegatesToShipmentTypeAndCustomsManager()
        {
            UspsShipmentAdapter testObject = new UspsShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
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

            UspsShipmentAdapter testObject = new UspsShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);

            Assert.NotNull(testObject.UpdateDynamicData());
            Assert.Equal(1, testObject.UpdateDynamicData().Count);
        }

        [Fact]
        public void SupportsPackageTypes_IsTrue()
        {
            ICarrierShipmentAdapter testObject = new UspsShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);

            Assert.True(testObject.SupportsPackageTypes);
        }

        [Fact]
        public void ShipDate_ReturnsShipmentValue()
        {
            ICarrierShipmentAdapter testObject = new UspsShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            Assert.Equal(shipment.ShipDate, testObject.ShipDate);
        }

        [Fact]
        public void ShipDate_IsUpdated()
        {
            ICarrierShipmentAdapter testObject = new UspsShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);

            testObject.ShipDate = testObject.ShipDate.AddDays(1);

            Assert.Equal(shipment.ShipDate, testObject.ShipDate);
        }

        [Fact]
        public void ServiceType_ReturnsShipmentValue()
        {
            ICarrierShipmentAdapter testObject = new UspsShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            Assert.Equal(shipment.Postal.Service, testObject.ServiceType);
        }

        [Fact]
        public void ServiceType_IsUpdated()
        {
            ICarrierShipmentAdapter testObject = new UspsShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);

            shipment.Postal.Service = (int)PostalServiceType.FirstClass;
            testObject.ServiceType = (int)PostalServiceType.ParcelSelect;

            Assert.Equal(shipment.Postal.Service, testObject.ServiceType);
        }
    }
}
