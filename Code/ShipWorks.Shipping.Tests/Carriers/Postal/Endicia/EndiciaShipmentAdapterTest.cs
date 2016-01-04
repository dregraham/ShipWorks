using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using System;
using System.Collections.Generic;
using Moq;
using ShipWorks.AddressValidation;
using ShipWorks.Tests.Shared;
using Xunit;
using Autofac;
using Autofac.Extras.Moq;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Tests.Carriers.Postal.Endicia
{
    public class EndiciaShipmentAdapterTest
    {
        readonly ShipmentEntity shipment;
        private readonly Mock<IShipmentTypeFactory> shipmentTypeFactory;
        private readonly Mock<ICustomsManager> customsManager;
        private readonly Mock<EndiciaShipmentType> shipmentTypeMock;
        private readonly ShipmentType shipmentType;

        public EndiciaShipmentAdapterTest()
        {
            shipmentType = new EndiciaShipmentType();
            shipment = new ShipmentEntity
            {
                ShipmentTypeCode = ShipmentTypeCode.Endicia,
                ShipDate = new DateTime(2015, 11, 24, 10, 10, 10),
                ContentWeight = 1,
                TotalWeight = 1,
                Insurance = true,
                Postal = new PostalShipmentEntity
                {
                    Service = (int)PostalServiceType.FirstClass,
                    Endicia = new EndiciaShipmentEntity()
                }
            };

            customsManager = new Mock<ICustomsManager>();
            customsManager.Setup(c => c.EnsureCustomsLoaded(It.IsAny<IEnumerable<ShipmentEntity>>())).Returns(new Dictionary<ShipmentEntity, Exception>());

            shipmentTypeMock = new Mock<EndiciaShipmentType>(MockBehavior.Strict);
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
            Assert.Throws<ArgumentNullException>(() => new EndiciaShipmentAdapter(null, shipmentTypeFactory.Object, customsManager.Object));
            Assert.Throws<ArgumentNullException>(() => new EndiciaShipmentAdapter(new ShipmentEntity(), shipmentTypeFactory.Object, customsManager.Object));

            shipment.Postal.Endicia = null;
            Assert.Throws<ArgumentNullException>(() => new EndiciaShipmentAdapter(new ShipmentEntity(), shipmentTypeFactory.Object, customsManager.Object));

            Assert.Throws<ArgumentNullException>(() => new EndiciaShipmentAdapter(shipment, null, customsManager.Object));
            Assert.Throws<ArgumentNullException>(() => new EndiciaShipmentAdapter(shipment, shipmentTypeFactory.Object, null));
        }

        [Fact]
        public void AccountId_ReturnsShipmentValue()
        {
            shipment.Postal.Endicia.EndiciaAccountID = 12;
            var testObject = new EndiciaShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            Assert.Equal(12, testObject.AccountId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10009238)]
        public void AccountId_StoresSpecifiedValue_WhenValueIsValid(long value)
        {
            var testObject = new EndiciaShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            testObject.AccountId = value;
            Assert.Equal(value, shipment.Postal.Endicia.EndiciaAccountID);
        }

        [Fact]
        public void AccountId_StoresZero_WhenValueIsNull()
        {
            var testObject = new EndiciaShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            testObject.AccountId = null;
            Assert.Equal(0, shipment.Postal.Endicia.EndiciaAccountID);
        }

        [Fact]
        public void Shipment_IsNotNull()
        {
            var testObject = new EndiciaShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            Assert.NotNull(testObject.Shipment);
        }

        [Fact]
        public void ShipmentTypeCode_IsEndicia()
        {
            var testObject = new EndiciaShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            Assert.Equal(ShipmentTypeCode.Endicia, testObject.ShipmentTypeCode);
        }

        [Fact]
        public void ShipmentTypeCode_IsExpress1Endicia()
        {
            shipment.ShipmentTypeCode = ShipmentTypeCode.Express1Endicia;
            var testObject = new EndiciaShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            Assert.Equal(ShipmentTypeCode.Express1Endicia, testObject.ShipmentTypeCode);
        }

        [Fact]
        public void SupportsAccounts_IsTrue()
        {
            EndiciaShipmentAdapter testObject = new EndiciaShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);

            Assert.True(testObject.SupportsAccounts);
        }

        [Fact]
        public void SupportsMultiplePackages_IsFalse()
        {
            EndiciaShipmentAdapter testObject = new EndiciaShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            Assert.False(testObject.SupportsMultiplePackages);
        }

        [Fact]
        public void SupportsMultiplePackages_DomesticIsTrue_WhenShipCountryIsUs()
        {
            shipment.OriginCountryCode = "US";
            shipment.ShipCountryCode = "US";

            EndiciaShipmentAdapter testObject = new EndiciaShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            Assert.True(testObject.IsDomestic);
        }

        [Fact]
        public void SupportsMultiplePackages_DomesticIsFalse_WhenShipCountryIsCa()
        {
            shipment.OriginCountryCode = "US";
            shipment.ShipCountryCode = "CA";

            EndiciaShipmentAdapter testObject = new EndiciaShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            Assert.False(testObject.IsDomestic);
        }

        [Fact]
        public void UpdateDynamicData_DelegatesToShipmentTypeAndCustomsManager()
        {
            using (AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                Mock<EndiciaShipmentType> shipmentTypeMock2 = mock.WithShipmentTypeFromFactory<EndiciaShipmentType>(x => { });
                EndiciaShipmentAdapter testObject = mock.Create<EndiciaShipmentAdapter>(new TypedParameter(typeof(ShipmentEntity), shipment));
                testObject.UpdateDynamicData();

                shipmentTypeMock2.Verify(b => b.UpdateDynamicShipmentData(It.IsAny<ShipmentEntity>()));
                shipmentTypeMock2.Verify(b => b.UpdateTotalWeight(It.IsAny<ShipmentEntity>()));

                mock.Mock<ICustomsManager>()
                    .Verify(b => b.EnsureCustomsLoaded(It.IsAny<IEnumerable<ShipmentEntity>>()));
            }
        }

        [Fact]
        public void UpdateDynamicData_ErrorsReturned_AreCorrect()
        {
            Dictionary<ShipmentEntity, Exception> errors = new Dictionary<ShipmentEntity, Exception>();
            errors.Add(shipment, new Exception("test"));

            customsManager.Setup(c => c.EnsureCustomsLoaded(It.IsAny<IEnumerable<ShipmentEntity>>())).Returns(errors);

            EndiciaShipmentAdapter testObject = new EndiciaShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);

            Assert.NotNull(testObject.UpdateDynamicData());
            Assert.Equal(1, testObject.UpdateDynamicData().Count);
        }

        [Fact]
        public void SupportsPackageTypes_IsTrue()
        {
            ICarrierShipmentAdapter testObject = new EndiciaShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);

            Assert.True(testObject.SupportsPackageTypes);
        }

        [Fact]
        public void ShipDate_ReturnsShipmentValue()
        {
            ICarrierShipmentAdapter testObject = new EndiciaShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            Assert.Equal(shipment.ShipDate, testObject.ShipDate);
        }

        [Fact]
        public void ShipDate_IsUpdated()
        {
            ICarrierShipmentAdapter testObject = new EndiciaShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);

            testObject.ShipDate = testObject.ShipDate.AddDays(1);

            Assert.Equal(shipment.ShipDate, testObject.ShipDate);
        }

        [Fact]
        public void UsingInsurance_ReturnsShipmentValue()
        {
            ICarrierShipmentAdapter testObject = new EndiciaShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            Assert.Equal(shipment.Insurance, testObject.UsingInsurance);
        }

        [Fact]
        public void UsingInsurance_IsUpdated()
        {
            ICarrierShipmentAdapter testObject = new EndiciaShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);

            testObject.UsingInsurance = !testObject.UsingInsurance;

            Assert.Equal(shipment.Insurance, testObject.UsingInsurance);
        }

        [Fact]
        public void ServiceType_ReturnsShipmentValue()
        {
            ICarrierShipmentAdapter testObject = new EndiciaShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);
            Assert.Equal(shipment.Postal.Service, testObject.ServiceType);
        }

        [Fact]
        public void ServiceType_IsUpdated()
        {
            ICarrierShipmentAdapter testObject = new EndiciaShipmentAdapter(shipment, shipmentTypeFactory.Object, customsManager.Object);

            shipment.Postal.Service = (int)PostalServiceType.FirstClass;
            testObject.ServiceType = (int)PostalServiceType.ParcelSelect;

            Assert.Equal(shipment.Postal.Service, testObject.ServiceType);
        }
    }
}
