using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS;
using System;
using System.Collections.Generic;
using Moq;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Services;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Amazon
{
    public class AmazonShipmentAdapterTest
    {
        readonly ShipmentEntity shipment;
        private readonly Mock<IShipmentTypeManager> shipmentTypeManager;
        private readonly Mock<AmazonShipmentType> shipmentTypeMock;

        public AmazonShipmentAdapterTest()
        {
            shipment = new ShipmentEntity
            {
                ShipmentTypeCode = ShipmentTypeCode.Amazon,
                ShipDate = new DateTime(2015, 11, 24, 10, 10, 10),
                ContentWeight = 1,
                TotalWeight = 1,
                Insurance = true,
                Amazon = new AmazonShipmentEntity()
                {
                    ShippingServiceID = "FEDEX_PTP_PRIORITY_OVERNIGHT"
                }
            };

            shipmentTypeMock = new Mock<AmazonShipmentType>(MockBehavior.Strict);
            shipmentTypeMock.Setup(b => b.UpdateDynamicShipmentData(shipment)).Verifiable();
            shipmentTypeMock.Setup(b => b.UpdateTotalWeight(shipment)).Verifiable();
            shipmentTypeMock.Setup(b => b.SupportsMultiplePackages).Returns(() => false);
            shipmentTypeMock.Setup(b => b.IsDomestic(It.IsAny<ShipmentEntity>())).Returns(() => false);

            shipmentTypeManager = new Mock<IShipmentTypeManager>();
            shipmentTypeManager.Setup(x => x.Get(shipment)).Returns(shipmentTypeMock.Object);
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExcpetion_WhenShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new AmazonShipmentAdapter(null, shipmentTypeManager.Object));
            Assert.Throws<ArgumentNullException>(() => new AmazonShipmentAdapter(new ShipmentEntity(), shipmentTypeManager.Object));
            Assert.Throws<ArgumentNullException>(() => new AmazonShipmentAdapter(shipment, null));
        }

        [Fact]
        public void AccountId_ReturnsShipmentValue()
        {
            AmazonShipmentAdapter testObject = new AmazonShipmentAdapter(shipment, shipmentTypeManager.Object);
            Assert.Null(testObject.AccountId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10009238)]
        public void AccountId_StoresSpecifiedValue_WhenValueIsValid(long value)
        {
            AmazonShipmentAdapter testObject = new AmazonShipmentAdapter(shipment, shipmentTypeManager.Object);
            testObject.AccountId = value;
            Assert.Equal(null, testObject.AccountId);
        }

        [Fact]
        public void Shipment_IsNotNull()
        {
            var testObject = new AmazonShipmentAdapter(shipment, shipmentTypeManager.Object);
            Assert.NotNull(testObject.Shipment);
        }

        [Fact]
        public void ShipmentTypeCode_IsAmazon()
        {
            var testObject = new AmazonShipmentAdapter(shipment, shipmentTypeManager.Object);
            Assert.Equal(ShipmentTypeCode.Amazon, testObject.ShipmentTypeCode);
        }

        [Fact]
        public void SupportsAccounts_IsFalse()
        {
            AmazonShipmentAdapter testObject = new AmazonShipmentAdapter(shipment, shipmentTypeManager.Object);

            Assert.False(testObject.SupportsAccounts);
        }

        [Fact]
        public void SupportsMultiplePackages_IsFalse()
        {
            AmazonShipmentAdapter testObject = new AmazonShipmentAdapter(shipment, shipmentTypeManager.Object);
            Assert.False(testObject.SupportsMultiplePackages);
        }

        [Fact]
        public void SupportsMultiplePackages_DomesticIsTrue_WhenShipCountryIsUs()
        {
            shipment.OriginCountryCode = "US";
            shipment.ShipCountryCode = "US";

            AmazonShipmentAdapter testObject = new AmazonShipmentAdapter(shipment, shipmentTypeManager.Object);
            Assert.False(testObject.IsDomestic);
        }

        [Fact]
        public void SupportsMultiplePackages_DomesticIsFalse_WhenShipCountryIsCa()
        {
            shipment.OriginCountryCode = "US";
            shipment.ShipCountryCode = "CA";

            AmazonShipmentAdapter testObject = new AmazonShipmentAdapter(shipment, shipmentTypeManager.Object);
            Assert.False(testObject.IsDomestic);
        }

        [Fact]
        public void UpdateDynamicData_DelegatesToShipmentType()
        {
            AmazonShipmentAdapter testObject = new AmazonShipmentAdapter(shipment, shipmentTypeManager.Object);
            testObject.UpdateDynamicData();

            shipmentTypeMock.Verify(b => b.UpdateDynamicShipmentData(It.IsAny<ShipmentEntity>()), Times.Once);
            shipmentTypeMock.Verify(b => b.UpdateTotalWeight(It.IsAny<ShipmentEntity>()), Times.Once);
        }

        [Fact]
        public void UpdateDynamicData_ErrorsReturned_AreCorrect()
        {
            Dictionary<ShipmentEntity, Exception> errors = new Dictionary<ShipmentEntity, Exception>();
            errors.Add(shipment, new Exception("test"));

            AmazonShipmentAdapter testObject = new AmazonShipmentAdapter(shipment, shipmentTypeManager.Object);

            Assert.NotNull(testObject.UpdateDynamicData());
            Assert.Equal(0, testObject.UpdateDynamicData().Count);
        }

        [Fact]
        public void SupportsPackageTypes_IsFalse()
        {
            ICarrierShipmentAdapter testObject = new AmazonShipmentAdapter(shipment, shipmentTypeManager.Object);

            Assert.False(testObject.SupportsPackageTypes);
        }

        [Fact]
        public void ShipDate_ReturnsShipmentValue()
        {
            ICarrierShipmentAdapter testObject = new AmazonShipmentAdapter(shipment, shipmentTypeManager.Object);
            Assert.Equal(shipment.ShipDate, testObject.ShipDate);
        }

        [Fact]
        public void ShipDate_IsUpdated()
        {
            ICarrierShipmentAdapter testObject = new AmazonShipmentAdapter(shipment, shipmentTypeManager.Object);

            testObject.ShipDate = testObject.ShipDate.AddDays(1);

            Assert.Equal(shipment.ShipDate, testObject.ShipDate);
        }

        [Fact]
        public void UsingInsurance_ReturnsShipmentValue()
        {
            ICarrierShipmentAdapter testObject = new AmazonShipmentAdapter(shipment, shipmentTypeManager.Object);
            Assert.Equal(shipment.Insurance, testObject.UsingInsurance);
        }

        [Fact]
        public void UsingInsurance_IsUpdated()
        {
            ICarrierShipmentAdapter testObject = new AmazonShipmentAdapter(shipment, shipmentTypeManager.Object);

            testObject.UsingInsurance = !testObject.UsingInsurance;

            Assert.Equal(shipment.Insurance, testObject.UsingInsurance);
        }
    }
}
