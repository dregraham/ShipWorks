using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Other;
using System;
using System.Collections.Generic;
using Moq;
using ShipWorks.AddressValidation;
using ShipWorks.Shipping.Carriers.Other;
using ShipWorks.Shipping.Services;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Other
{
    public class OtherShipmentAdapterTest
    {
        readonly ShipmentEntity shipment;
        private readonly Mock<IShipmentTypeManager> shipmentTypeManager;
        private readonly Mock<ICustomsManager> customsManager;
        private readonly Mock<OtherShipmentType> shipmentTypeMock;
        private readonly ShipmentType shipmentType;

        public OtherShipmentAdapterTest()
        {
            shipmentType = new OtherShipmentType();
            shipment = new ShipmentEntity()
            {
                ShipmentTypeCode = ShipmentTypeCode.Other,
                ShipDate = new DateTime(2015, 11, 24, 10, 10, 10),
                ContentWeight = 1,
                TotalWeight = 1,
                Insurance = true,
            };

            customsManager = new Mock<ICustomsManager>();
            customsManager.Setup(c => c.EnsureCustomsLoaded(It.IsAny<IEnumerable<ShipmentEntity>>())).Returns(new Dictionary<ShipmentEntity, Exception>());

            shipmentTypeMock = new Mock<OtherShipmentType>(MockBehavior.Strict);
            shipmentTypeMock.Setup(b => b.UpdateDynamicShipmentData(shipment)).Verifiable();
            shipmentTypeMock.Setup(b => b.UpdateTotalWeight(shipment)).Verifiable();
            shipmentTypeMock.Setup(b => b.SupportsMultiplePackages).Returns(() => shipmentType.SupportsMultiplePackages);
            shipmentTypeMock.Setup(b => b.IsDomestic(It.IsAny<ShipmentEntity>())).Returns(() => shipmentType.IsDomestic(shipment));

            shipmentTypeManager = new Mock<IShipmentTypeManager>();
            shipmentTypeManager.Setup(x => x.Get(shipment)).Returns(shipmentTypeMock.Object);
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExcpetion_WhenShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new OtherShipmentAdapter(null, shipmentTypeManager.Object, customsManager.Object));
            Assert.Throws<ArgumentNullException>(() => new OtherShipmentAdapter(shipment, null, customsManager.Object));
            Assert.Throws<ArgumentNullException>(() => new OtherShipmentAdapter(shipment, shipmentTypeManager.Object, null));
        }

        [Fact]
        public void AccountId_ReturnsNull()
        {
            var testObject = new OtherShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            Assert.Null(testObject.AccountId);
        }

        [Fact]
        public void AccountId_DoesNotThrow_WhenValueIsValid()
        {
            var testObject = new OtherShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            testObject.AccountId = 6;
        }

        [Fact]
        public void AccountId_DoesNotThrow_WhenValueIsNull()
        {
            var testObject = new OtherShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            testObject.AccountId = null;
        }

        [Fact]
        public void Shipment_IsNotNull()
        {
            var testObject = new OtherShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            Assert.NotNull(testObject.Shipment);
        }

        [Fact]
        public void ShipmentTypeCode_IsOther()
        {
            var testObject = new OtherShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            Assert.Equal(ShipmentTypeCode.Other, testObject.ShipmentTypeCode);
        }

        [Fact]
        public void SupportsAccounts_IsFalse()
        {
            OtherShipmentAdapter testObject = new OtherShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);

            Assert.False(testObject.SupportsAccounts);
        }

        [Fact]
        public void SupportsMultiplePackages_IsFalse()
        {
            OtherShipmentAdapter testObject = new OtherShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            Assert.False(testObject.SupportsMultiplePackages);
        }

        [Fact]
        public void SupportsMultiplePackages_DomesticIsTrue_WhenShipCountryIsUs()
        {
            shipment.OriginCountryCode = "US";
            shipment.ShipCountryCode = "US";

            OtherShipmentAdapter testObject = new OtherShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            Assert.True(testObject.IsDomestic);
        }

        [Fact]
        public void SupportsMultiplePackages_DomesticIsFalse_WhenShipCountryIsCa()
        {
            shipment.OriginCountryCode = "US";
            shipment.ShipCountryCode = "CA";

            OtherShipmentAdapter testObject = new OtherShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            Assert.False(testObject.IsDomestic);
        }

        [Fact]
        public void UpdateDynamicData_DelegatesToShipmentTypeAndCustomsManager()
        {
            OtherShipmentAdapter testObject = new OtherShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
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

            OtherShipmentAdapter testObject = new OtherShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);

            Assert.NotNull(testObject.UpdateDynamicData());
            Assert.Equal(1, testObject.UpdateDynamicData().Count);
        }

        [Fact]
        public void SupportsPackageTypes_IsFalse()
        {
            ICarrierShipmentAdapter testObject = new OtherShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);

            Assert.False(testObject.SupportsPackageTypes);
        }

        [Fact]
        public void ShipDate_ReturnsShipmentValue()
        {
            ICarrierShipmentAdapter testObject = new OtherShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            Assert.Equal(shipment.ShipDate, testObject.ShipDate);
        }

        [Fact]
        public void ShipDate_IsUpdated()
        {
            ICarrierShipmentAdapter testObject = new OtherShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);

            testObject.ShipDate = testObject.ShipDate.AddDays(1);

            Assert.Equal(shipment.ShipDate, testObject.ShipDate);
        }

        [Fact]
        public void UsingInsurance_ReturnsShipmentValue()
        {
            ICarrierShipmentAdapter testObject = new OtherShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            Assert.Equal(shipment.Insurance, testObject.UsingInsurance);
        }

        [Fact]
        public void UsingInsurance_IsUpdated()
        {
            ICarrierShipmentAdapter testObject = new OtherShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);

            testObject.UsingInsurance = !testObject.UsingInsurance;

            Assert.Equal(shipment.Insurance, testObject.UsingInsurance);
        }
    }
}
