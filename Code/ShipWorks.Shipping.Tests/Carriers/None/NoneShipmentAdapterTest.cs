using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.None;
using System;
using System.Collections.Generic;
using Moq;
using ShipWorks.AddressValidation;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.None
{
    public class NoneShipmentAdapterTest
    {
        readonly ShipmentEntity shipment;
        private Mock<IShipmentTypeFactory> shipmentTypeFactory;
        private Mock<ICustomsManager> customsManager;
        private Mock<NoneShipmentType> shipmentTypeMock;
        private ShipmentType shipmentType;

        public NoneShipmentAdapterTest()
        {
            shipmentType = new NoneShipmentType();
            shipment = new ShipmentEntity()
            {
                ShipmentTypeCode = ShipmentTypeCode.None,
            };

            customsManager = new Mock<ICustomsManager>();
            customsManager.Setup(c => c.EnsureCustomsLoaded(It.IsAny<IEnumerable<ShipmentEntity>>(), It.IsAny<ValidatedAddressScope>())).Returns(new Dictionary<ShipmentEntity, Exception>());

            shipmentTypeMock = new Mock<NoneShipmentType>(MockBehavior.Strict);
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
            Assert.Throws<ArgumentNullException>(() => new NoneShipmentAdapter(null, shipmentTypeFactory.Object, customsManager.Object));
        }

        [Fact]
        public void AccountId_ReturnsNull()
        {
            var testObject = new NoneShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            Assert.Null(testObject.AccountId);
        }

        [Fact]
        public void AccountId_DoesNotThrow_WhenValueIsValid()
        {
            var testObject = new NoneShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            testObject.AccountId = 6;
        }

        [Fact]
        public void AccountId_DoesNotThrow_WhenValueIsNull()
        {
            var testObject = new NoneShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            testObject.AccountId = null;
        }

        [Fact]
        public void Shipment_IsNotNull()
        {
            var testObject = new NoneShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            Assert.NotNull(testObject.Shipment);
        }

        [Fact]
        public void ShipmentTypeCode_IsNone()
        {
            var testObject = new NoneShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            Assert.Equal(ShipmentTypeCode.None, testObject.ShipmentTypeCode);
        }

        [Fact]
        public void SupportsAccounts_IsFalse()
        {
            NoneShipmentAdapter testObject = new NoneShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);

            Assert.False(testObject.SupportsAccounts);
        }

        [Fact]
        public void SupportsMultiplePackages_IsFalse()
        {
            NoneShipmentAdapter testObject = new NoneShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            Assert.False(testObject.SupportsMultiplePackages);
        }

        [Fact]
        public void SupportsMultiplePackages_DomesticIsTrue_WhenShipCountryIsUs()
        {
            shipment.OriginCountryCode = "US";
            shipment.ShipCountryCode = "US";

            NoneShipmentAdapter testObject = new NoneShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            Assert.True(testObject.IsDomestic);
        }

        [Fact]
        public void SupportsMultiplePackages_DomesticIsFalse_WhenShipCountryIsCa()
        {
            shipment.OriginCountryCode = "US";
            shipment.ShipCountryCode = "CA";

            NoneShipmentAdapter testObject = new NoneShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            Assert.False(testObject.IsDomestic);
        }

        [Fact]
        public void UpdateDynamicData_DelegatesToShipmentTypeAndCustomsManager()
        {
            using (ValidatedAddressScope validatedAddressScope = new ValidatedAddressScope())
            {
                NoneShipmentAdapter testObject = new NoneShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
                testObject.UpdateDynamicData(validatedAddressScope);

                shipmentTypeMock.Verify(b => b.UpdateDynamicShipmentData(It.IsAny<ShipmentEntity>()), Times.Never);
                shipmentTypeMock.Verify(b => b.UpdateTotalWeight(It.IsAny<ShipmentEntity>()), Times.Never);

                customsManager.Verify(b => b.EnsureCustomsLoaded(It.IsAny<IEnumerable<ShipmentEntity>>(), It.IsAny<ValidatedAddressScope>()), Times.Never);
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

                NoneShipmentAdapter testObject = new NoneShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);

                Assert.NotNull(testObject.UpdateDynamicData(validatedAddressScope));
                Assert.Equal(0, testObject.UpdateDynamicData(validatedAddressScope).Count);
            }
        }
    }
}
