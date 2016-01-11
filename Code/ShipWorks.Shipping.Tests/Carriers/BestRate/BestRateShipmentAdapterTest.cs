using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Services;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.BestRate
{
    public class BestRateShipmentAdapterTest
    {
        private readonly ShipmentEntity shipment;
        private readonly AutoMock mock;
        private readonly Mock<IShipmentTypeManager> shipmentTypeManager;
        private readonly Mock<ICustomsManager> customsManager;
        private readonly Mock<BestRateShipmentType> shipmentTypeMock;

        public BestRateShipmentAdapterTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            shipment = new ShipmentEntity()
            {
                ShipmentTypeCode = ShipmentTypeCode.BestRate,
                ShipDate = new DateTime(2015, 11, 24, 10, 10, 10),
                ContentWeight = 1,
                TotalWeight = 1,
                Insurance = true,
                BestRate = new BestRateShipmentEntity()
            };

            customsManager = mock.CreateMock<ICustomsManager>();

            shipmentTypeMock = mock.CreateMock<BestRateShipmentType>();

            shipmentTypeManager = new Mock<IShipmentTypeManager>();
            shipmentTypeManager.Setup(x => x.Get(shipment)).Returns(shipmentTypeMock.Object);
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExcpetion_WhenParamsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new BestRateShipmentAdapter(null, shipmentTypeManager.Object, customsManager.Object));
            Assert.Throws<ArgumentNullException>(() => new BestRateShipmentAdapter(new ShipmentEntity(), null, customsManager.Object));
            Assert.Throws<ArgumentNullException>(() => new BestRateShipmentAdapter(new ShipmentEntity(), shipmentTypeManager.Object, null));
        }

        [Fact]
        public void AccountId_ReturnsNull()
        {
            var testObject = new BestRateShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            Assert.Null(testObject.AccountId);
        }

        [Fact]
        public void AccountId_DoesNotThrow_WhenValueIsValid()
        {
            var testObject = new BestRateShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            testObject.AccountId = 6;
        }

        [Fact]
        public void AccountId_DoesNotThrow_WhenValueIsNull()
        {
            var testObject = new BestRateShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            testObject.AccountId = null;
        }

        [Fact]
        public void Shipment_IsNotNull()
        {
            var testObject = new BestRateShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            Assert.NotNull(testObject.Shipment);
        }

        [Fact]
        public void ShipmentTypeCode_IsBestRate()
        {
            var testObject = new BestRateShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            Assert.Equal(ShipmentTypeCode.BestRate, testObject.ShipmentTypeCode);
        }

        [Fact]
        public void SupportsAccounts_IsFalse()
        {
            BestRateShipmentAdapter testObject = new BestRateShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);

            Assert.False(testObject.SupportsAccounts);
        }

        [Fact]
        public void SupportsMultiplePackages_IsFalse()
        {
            BestRateShipmentAdapter testObject = new BestRateShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            Assert.False(testObject.SupportsMultiplePackages);
        }

        [Fact]
        public void SupportsMultiplePackages_DomesticIsTrue_WhenShipCountryIsUs()
        {
            shipmentTypeMock.CallBase = true;

            shipment.OriginCountryCode = "US";
            shipment.ShipCountryCode = "US";

            BestRateShipmentAdapter testObject = new BestRateShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            Assert.True(testObject.IsDomestic);
        }

        [Fact]
        public void SupportsMultiplePackages_DomesticIsFalse_WhenShipCountryIsCa()
        {
            shipment.OriginCountryCode = "US";
            shipment.ShipCountryCode = "CA";

            BestRateShipmentAdapter testObject = new BestRateShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            Assert.False(testObject.IsDomestic);
        }

        [Fact]
        public void UpdateDynamicData_DelegatesToShipmentTypeAndCustomsManager()
        {
            BestRateShipmentAdapter testObject = new BestRateShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
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

            BestRateShipmentAdapter testObject = new BestRateShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);

            Assert.NotNull(testObject.UpdateDynamicData());
            Assert.Equal(1, testObject.UpdateDynamicData().Count);
        }

        [Fact]
        public void SupportsPackageTypes_IsFalse()
        {
            ICarrierShipmentAdapter testObject = new BestRateShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);

            Assert.False(testObject.SupportsPackageTypes);
        }

        [Fact]
        public void ShipDate_ReturnsShipmentValue()
        {
            ICarrierShipmentAdapter testObject = new BestRateShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            Assert.Equal(shipment.ShipDate, testObject.ShipDate);
        }

        [Fact]
        public void ShipDate_IsUpdated()
        {
            ICarrierShipmentAdapter testObject = new BestRateShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);

            testObject.ShipDate = testObject.ShipDate.AddDays(1);

            Assert.Equal(shipment.ShipDate, testObject.ShipDate);
        }

        [Fact]
        public void UsingInsurance_ReturnsShipmentValue()
        {
            ICarrierShipmentAdapter testObject = new BestRateShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            Assert.Equal(shipment.Insurance, testObject.UsingInsurance);
        }

        [Fact]
        public void UsingInsurance_IsUpdated()
        {
            ICarrierShipmentAdapter testObject = new BestRateShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);

            testObject.UsingInsurance = !testObject.UsingInsurance;

            Assert.Equal(shipment.Insurance, testObject.UsingInsurance);
        }
    }
}
