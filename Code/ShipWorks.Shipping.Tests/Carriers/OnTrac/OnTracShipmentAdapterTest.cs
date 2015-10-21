using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.OnTrac;
using System;
using System.Collections.Generic;
using Moq;
using ShipWorks.AddressValidation;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.OnTrac
{
    public class OnTracShipmentAdapterTest
    {
        readonly ShipmentEntity shipment;
        private Mock<IShipmentTypeFactory> shipmentTypeFactory;
        private Mock<ICustomsManager> customsManager;
        private Mock<OnTracShipmentType> shipmentTypeMock;
        private ShipmentType shipmentType;

        public OnTracShipmentAdapterTest()
        {
            shipmentType = new OnTracShipmentType();
            shipment = new ShipmentEntity
            {
                ShipmentTypeCode = ShipmentTypeCode.OnTrac,
                OnTrac = new OnTracShipmentEntity()
            };

            customsManager = new Mock<ICustomsManager>();
            customsManager.Setup(c => c.EnsureCustomsLoaded(It.IsAny<IEnumerable<ShipmentEntity>>(), It.IsAny<ValidatedAddressScope>())).Returns(new Dictionary<ShipmentEntity, Exception>());

            shipmentTypeMock = new Mock<OnTracShipmentType>(MockBehavior.Strict);
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
            Assert.Throws<ArgumentNullException>(() => new OnTracShipmentAdapter(null, shipmentTypeFactory.Object, customsManager.Object));
            Assert.Throws<ArgumentNullException>(() => new OnTracShipmentAdapter(new ShipmentEntity(), shipmentTypeFactory.Object, customsManager.Object));
            Assert.Throws<ArgumentNullException>(() => new OnTracShipmentAdapter(shipment, null, customsManager.Object));
            Assert.Throws<ArgumentNullException>(() => new OnTracShipmentAdapter(shipment, shipmentTypeFactory.Object, null));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExcpetion_WhenOnTracShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new OnTracShipmentAdapter(new ShipmentEntity(), shipmentTypeFactory.Object, customsManager.Object));
        }

        [Fact]
        public void AccountId_ReturnsShipmentValue()
        {
            shipment.OnTrac.OnTracAccountID = 12;
            var testObject = new OnTracShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            Assert.Equal(12, testObject.AccountId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10009238)]
        public void AccountId_StoresSpecifiedValue_WhenValueIsValid(long value)
        {
            var testObject = new OnTracShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            testObject.AccountId = value;
            Assert.Equal(value, shipment.OnTrac.OnTracAccountID);
        }

        [Fact]
        public void AccountId_StoresZero_WhenValueIsNull()
        {
            var testObject = new OnTracShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            testObject.AccountId = null;
            Assert.Equal(0, shipment.OnTrac.OnTracAccountID);
        }

        [Fact]
        public void Shipment_IsNotNull()
        {
            var testObject = new OnTracShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            Assert.NotNull(testObject.Shipment);
        }

        [Fact]
        public void ShipmentTypeCode_IsOnTrac()
        {
            var testObject = new OnTracShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            Assert.Equal(ShipmentTypeCode.OnTrac, testObject.ShipmentTypeCode);
        }

        [Fact]
        public void SupportsAccounts_IsTrue()
        {
            OnTracShipmentAdapter testObject = new OnTracShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);

            Assert.True(testObject.SupportsAccounts);
        }

        [Fact]
        public void SupportsMultiplePackages_IsFalse()
        {
            OnTracShipmentAdapter testObject = new OnTracShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            Assert.False(testObject.SupportsMultiplePackages);
        }

        [Fact]
        public void SupportsMultiplePackages_DomesticIsTrue_WhenShipCountryIsUs()
        {
            shipment.OriginCountryCode = "US";
            shipment.ShipCountryCode = "US";

            OnTracShipmentAdapter testObject = new OnTracShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            Assert.True(testObject.IsDomestic);
        }

        [Fact]
        public void SupportsMultiplePackages_DomesticIsFalse_WhenShipCountryIsCa()
        {
            shipment.OriginCountryCode = "US";
            shipment.ShipCountryCode = "CA";

            OnTracShipmentAdapter testObject = new OnTracShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            Assert.False(testObject.IsDomestic);
        }

        [Fact]
        public void UpdateDynamicData_DelegatesToShipmentTypeAndCustomsManager()
        {
            using (ValidatedAddressScope validatedAddressScope = new ValidatedAddressScope())
            {
                OnTracShipmentAdapter testObject = new OnTracShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
                testObject.UpdateDynamicData(validatedAddressScope);

                shipmentTypeMock.Verify(b => b.UpdateDynamicShipmentData(It.IsAny<ShipmentEntity>()), Times.Once);
                shipmentTypeMock.Verify(b => b.UpdateTotalWeight(It.IsAny<ShipmentEntity>()), Times.Once);

                customsManager.Verify(b => b.EnsureCustomsLoaded(It.IsAny<IEnumerable<ShipmentEntity>>(), It.IsAny<ValidatedAddressScope>()), Times.Once);
            }
        }

        [Fact]
        public void UpdateDynamicData_ErrorsReturned_AreCorrect()
        {
            using (ValidatedAddressScope validatedAddressScope = new ValidatedAddressScope())
            {
                Dictionary<ShipmentEntity, Exception> errors = new Dictionary<ShipmentEntity, Exception>();
                errors.Add(shipment, new Exception("test"));

                customsManager.Setup(c => c.EnsureCustomsLoaded(It.IsAny<IEnumerable<ShipmentEntity>>(), It.IsAny<ValidatedAddressScope>())).Returns(errors);

                OnTracShipmentAdapter testObject = new OnTracShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);

                Assert.NotNull(testObject.UpdateDynamicData(validatedAddressScope));
                Assert.Equal(1, testObject.UpdateDynamicData(validatedAddressScope).Count);
            }
        }
    }
}
