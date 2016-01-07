using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.iParcel;
using System;
using System.Collections.Generic;
using Moq;
using Xunit;
using System.Diagnostics.CodeAnalysis;
using ShipWorks.Shipping.Carriers.iParcel.Enums;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Tests.Carriers.iParcel
{
    [SuppressMessage("SonarLint", "S101:Class names should comply with a naming convention",
        Justification = "Class is names to match iParcel's naming convention")]
    public class iParcelShipmentAdapterTest
    {
        readonly ShipmentEntity shipment;
        private readonly Mock<IShipmentTypeFactory> shipmentTypeFactory;
        private readonly Mock<ICustomsManager> customsManager;
        private readonly Mock<iParcelShipmentType> shipmentTypeMock;
        private readonly ShipmentType shipmentType;

        public iParcelShipmentAdapterTest()
        {
            shipmentType = new iParcelShipmentType();
            shipment = new ShipmentEntity
            {
                ShipmentTypeCode = ShipmentTypeCode.iParcel,
                ShipDate = new DateTime(2015, 11, 24, 10, 10, 10),
                ContentWeight = 1,
                TotalWeight = 1,
                Insurance = true,
                IParcel = new IParcelShipmentEntity()
                {
                    Service = (int)iParcelServiceType.Immediate
                }
            };

            customsManager = new Mock<ICustomsManager>();
            customsManager.Setup(c => c.EnsureCustomsLoaded(It.IsAny<IEnumerable<ShipmentEntity>>())).Returns(new Dictionary<ShipmentEntity, Exception>());

            shipmentTypeMock = new Mock<iParcelShipmentType>(MockBehavior.Strict);
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
            Assert.Throws<ArgumentNullException>(() => new iParcelShipmentAdapter(null, shipmentTypeFactory.Object, customsManager.Object));
            Assert.Throws<ArgumentNullException>(() => new iParcelShipmentAdapter(new ShipmentEntity(), shipmentTypeFactory.Object, customsManager.Object));
            Assert.Throws<ArgumentNullException>(() => new iParcelShipmentAdapter(shipment, null, customsManager.Object));
            Assert.Throws<ArgumentNullException>(() => new iParcelShipmentAdapter(shipment, shipmentTypeFactory.Object, null));
        }

        [Fact]
        public void AccountId_ReturnsShipmentValue()
        {
            shipment.IParcel.IParcelAccountID = 12;
            var testObject = new iParcelShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            Assert.Equal(12, testObject.AccountId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10009238)]
        public void AccountId_StoresSpecifiedValue_WhenValueIsValid(long value)
        {
            var testObject = new iParcelShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            testObject.AccountId = value;
            Assert.Equal(value, shipment.IParcel.IParcelAccountID);
        }

        [Fact]
        public void AccountId_StoresZero_WhenValueIsNull()
        {
            var testObject = new iParcelShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            testObject.AccountId = null;
            Assert.Equal(0, shipment.IParcel.IParcelAccountID);
        }

        [Fact]
        public void Shipment_IsNotNull()
        {
            var testObject = new iParcelShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            Assert.NotNull(testObject.Shipment);
        }

        [Fact]
        public void ShipmentTypeCode_IsIParcel()
        {
            var testObject = new iParcelShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            Assert.Equal(ShipmentTypeCode.iParcel, testObject.ShipmentTypeCode);
        }

        [Fact]
        public void SupportsAccounts_IsTrue()
        {
            iParcelShipmentAdapter testObject = new iParcelShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);

            Assert.True(testObject.SupportsAccounts);
        }

        [Fact]
        public void SupportsMultiplePackages_IsTrue()
        {
            iParcelShipmentAdapter testObject = new iParcelShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            Assert.True(testObject.SupportsMultiplePackages);
        }

        [Fact]
        public void SupportsMultiplePackages_DomesticIsTrue_WhenShipCountryIsUs()
        {
            shipment.OriginCountryCode = "US";
            shipment.ShipCountryCode = "US";

            iParcelShipmentAdapter testObject = new iParcelShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            Assert.True(testObject.IsDomestic);
        }

        [Fact]
        public void SupportsMultiplePackages_DomesticIsFalse_WhenShipCountryIsCa()
        {
            shipment.OriginCountryCode = "US";
            shipment.ShipCountryCode = "CA";

            iParcelShipmentAdapter testObject = new iParcelShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            Assert.False(testObject.IsDomestic);
        }

        [Fact]
        public void UpdateDynamicData_DelegatesToShipmentTypeAndCustomsManager()
        {
            iParcelShipmentAdapter testObject = new iParcelShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
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

            iParcelShipmentAdapter testObject = new iParcelShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);

            Assert.NotNull(testObject.UpdateDynamicData());
            Assert.Equal(1, testObject.UpdateDynamicData().Count);
        }

        [Fact]
        public void SupportsPackageTypes_IsFalse()
        {
            ICarrierShipmentAdapter testObject = new iParcelShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);

            Assert.False(testObject.SupportsPackageTypes);
        }

        [Fact]
        public void ShipDate_ReturnsShipmentValue()
        {
            ICarrierShipmentAdapter testObject = new iParcelShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            Assert.Equal(shipment.ShipDate, testObject.ShipDate);
        }

        [Fact]
        public void ShipDate_IsUpdated()
        {
            ICarrierShipmentAdapter testObject = new iParcelShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);

            testObject.ShipDate = testObject.ShipDate.AddDays(1);

            Assert.Equal(shipment.ShipDate, testObject.ShipDate);
        }

        [Fact]
        public void ServiceType_ReturnsShipmentValue()
        {
            ICarrierShipmentAdapter testObject = new iParcelShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            Assert.Equal(shipment.IParcel.Service, testObject.ServiceType);
        }

        [Fact]
        public void ServiceType_IsUpdated()
        {
            ICarrierShipmentAdapter testObject = new iParcelShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);

            shipment.IParcel.Service = (int)iParcelServiceType.Immediate;
            testObject.ServiceType = (int)iParcelServiceType.Preferred;

            Assert.Equal(shipment.IParcel.Service, testObject.ServiceType);
        }
    }
}
