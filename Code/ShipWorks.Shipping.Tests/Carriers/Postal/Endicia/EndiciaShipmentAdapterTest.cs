using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Postal.Endicia
{
    public class EndiciaShipmentAdapterTest : IDisposable
    {
        readonly AutoMock mock;
        readonly ShipmentEntity shipment;
        private readonly Mock<IShipmentTypeManager> shipmentTypeManager;
        private readonly Mock<ICustomsManager> customsManager;
        private readonly Mock<EndiciaShipmentType> shipmentTypeMock;
        private readonly ShipmentType shipmentType;

        public EndiciaShipmentAdapterTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
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
                    Service = (int) PostalServiceType.FirstClass,
                    Confirmation = (int) PostalConfirmationType.Delivery,
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

            shipmentTypeManager = new Mock<IShipmentTypeManager>();
            shipmentTypeManager.Setup(x => x.Get(shipment)).Returns(shipmentTypeMock.Object);
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExcpetion_WhenShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new EndiciaShipmentAdapter(null, shipmentTypeManager.Object, customsManager.Object));
            Assert.Throws<ArgumentNullException>(() => new EndiciaShipmentAdapter(new ShipmentEntity(), shipmentTypeManager.Object, customsManager.Object));

            shipment.Postal.Endicia = null;
            Assert.Throws<ArgumentNullException>(() => new EndiciaShipmentAdapter(new ShipmentEntity(), shipmentTypeManager.Object, customsManager.Object));

            Assert.Throws<ArgumentNullException>(() => new EndiciaShipmentAdapter(shipment, null, customsManager.Object));
            Assert.Throws<ArgumentNullException>(() => new EndiciaShipmentAdapter(shipment, shipmentTypeManager.Object, null));
        }

        [Fact]
        public void AccountId_ReturnsShipmentValue()
        {
            shipment.Postal.Endicia.EndiciaAccountID = 12;
            var testObject = new EndiciaShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            Assert.Equal(12, testObject.AccountId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10009238)]
        public void AccountId_StoresSpecifiedValue_WhenValueIsValid(long value)
        {
            var testObject = new EndiciaShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            testObject.AccountId = value;
            Assert.Equal(value, shipment.Postal.Endicia.EndiciaAccountID);
        }

        [Fact]
        public void AccountId_StoresZero_WhenValueIsNull()
        {
            var testObject = new EndiciaShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            testObject.AccountId = null;
            Assert.Equal(0, shipment.Postal.Endicia.EndiciaAccountID);
        }

        [Fact]
        public void Shipment_IsNotNull()
        {
            var testObject = new EndiciaShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            Assert.NotNull(testObject.Shipment);
        }

        [Fact]
        public void ShipmentTypeCode_IsEndicia()
        {
            var testObject = new EndiciaShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            Assert.Equal(ShipmentTypeCode.Endicia, testObject.ShipmentTypeCode);
        }

        [Fact]
        public void ShipmentTypeCode_IsExpress1Endicia()
        {
            shipment.ShipmentTypeCode = ShipmentTypeCode.Express1Endicia;
            var testObject = new EndiciaShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            Assert.Equal(ShipmentTypeCode.Express1Endicia, testObject.ShipmentTypeCode);
        }

        [Fact]
        public void SupportsAccounts_IsTrue()
        {
            EndiciaShipmentAdapter testObject = new EndiciaShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);

            Assert.True(testObject.SupportsAccounts);
        }

        [Fact]
        public void SupportsMultiplePackages_IsFalse()
        {
            EndiciaShipmentAdapter testObject = new EndiciaShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            Assert.False(testObject.SupportsMultiplePackages);
        }

        [Fact]
        public void SupportsMultiplePackages_DomesticIsTrue_WhenShipCountryIsUs()
        {
            shipment.OriginCountryCode = "US";
            shipment.ShipCountryCode = "US";

            EndiciaShipmentAdapter testObject = new EndiciaShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            Assert.True(testObject.IsDomestic);
        }

        [Fact]
        public void SupportsMultiplePackages_DomesticIsFalse_WhenShipCountryIsCa()
        {
            shipment.OriginCountryCode = "US";
            shipment.ShipCountryCode = "CA";

            EndiciaShipmentAdapter testObject = new EndiciaShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            Assert.False(testObject.IsDomestic);
        }

        [Fact]
        public void UpdateDynamicData_DelegatesToShipmentTypeAndCustomsManager()
        {
            using (AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                Mock<EndiciaShipmentType> shipmentTypeMock2 = mock.WithShipmentTypeFromShipmentManager<EndiciaShipmentType>(x => { });
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

            EndiciaShipmentAdapter testObject = new EndiciaShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);

            Assert.NotNull(testObject.UpdateDynamicData());
            Assert.Equal(1, testObject.UpdateDynamicData().Count);
        }

        [Fact]
        public void SupportsPackageTypes_IsTrue()
        {
            ICarrierShipmentAdapter testObject = new EndiciaShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);

            Assert.True(testObject.SupportsPackageTypes);
        }

        [Fact]
        public void ShipDate_ReturnsShipmentValue()
        {
            ICarrierShipmentAdapter testObject = new EndiciaShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            Assert.Equal(shipment.ShipDate, testObject.ShipDate);
        }

        [Fact]
        public void ShipDate_IsUpdated()
        {
            ICarrierShipmentAdapter testObject = new EndiciaShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);

            testObject.ShipDate = testObject.ShipDate.AddDays(1);

            Assert.Equal(shipment.ShipDate, testObject.ShipDate);
        }

        [Fact]
        public void ServiceType_ReturnsShipmentValue()
        {
            ICarrierShipmentAdapter testObject = new EndiciaShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            Assert.Equal(shipment.Postal.Service, testObject.ServiceType);
        }

        [Fact]
        public void ServiceType_IsUpdated()
        {
            ICarrierShipmentAdapter testObject = new EndiciaShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);

            shipment.Postal.Service = (int) PostalServiceType.FirstClass;
            testObject.ServiceType = (int) PostalServiceType.ParcelSelect;

            Assert.Equal(shipment.Postal.Service, testObject.ServiceType);
        }

        [Theory]
        [InlineData(PostalServiceType.AsendiaIpa, PostalConfirmationType.AdultSignatureRequired)]
        [InlineData(PostalServiceType.PriorityMail, PostalConfirmationType.Delivery)]
        [InlineData(PostalServiceType.ExpressMail, PostalConfirmationType.None)]
        public void UpdateServiceFromRate_SetsService_WhenTagIsValid(PostalServiceType serviceType, PostalConfirmationType confirmationType)
        {
            shipmentTypeManager.Setup(x => x.Get(shipment)).Returns(shipmentType);
            var testObject = new EndiciaShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            testObject.SelectServiceFromRate(new RateResult("Foo", "1", 1M, new PostalRateSelection(serviceType, confirmationType))
            {
                Selectable = true,
                ShipmentType = ShipmentTypeCode.Endicia
            });
            Assert.Equal((int) serviceType, shipment.Postal.Service);
            Assert.Equal((int) confirmationType, shipment.Postal.Confirmation);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("Foo")]
        public void UpdateServiceFromRate_DoesNotSetService_WhenTagIsNotValid(string value)
        {
            shipmentTypeManager.Setup(x => x.Get(shipment)).Returns(shipmentType);
            shipment.Postal.Service = (int) PostalServiceType.PriorityMail;
            shipment.Postal.Confirmation = (int) PostalConfirmationType.AdultSignatureRestricted;

            var testObject = new EndiciaShipmentAdapter(shipment, shipmentTypeManager.Object, customsManager.Object);
            testObject.SelectServiceFromRate(new RateResult("Foo", "1", 1M, value)
            {
                Selectable = true,
                ShipmentType = ShipmentTypeCode.Endicia
            });
            Assert.Equal((int) PostalServiceType.PriorityMail, shipment.Postal.Service);
            Assert.Equal((int) PostalConfirmationType.AdultSignatureRestricted, shipment.Postal.Confirmation);
        }

        [Fact]
        public void DoesRateMatchSelectedService_ReturnsFalse_WhenRateShipmentTypeDoesNotMatch()
        {
            var testObject = mock.Create<EndiciaShipmentAdapter>(new TypedParameter(typeof(ShipmentEntity), shipment));
            var rate = new RateResult("Foo", "1", 0, 1) { ShipmentType = ShipmentTypeCode.None };

            var result = testObject.DoesRateMatchSelectedService(rate);

            Assert.False(result);
        }

        [Fact]
        public void DoesRateMatchSelectedService_ReturnsFalse_WhenServiceAndConfirmationDoNotMatch()
        {
            var testObject = mock.Create<EndiciaShipmentAdapter>(new TypedParameter(typeof(ShipmentEntity), shipment));
            var rate = new RateResult("Foo", "1", 0, new PostalRateSelection(PostalServiceType.AsendiaGeneric, PostalConfirmationType.Signature))
            {
                ShipmentType = ShipmentTypeCode.Endicia
            };

            var result = testObject.DoesRateMatchSelectedService(rate);

            Assert.False(result);
        }

        [Fact]
        public void DoesRateMatchSelectedService_ReturnsFalse_WhenServiceDoesNotMatch()
        {
            var testObject = mock.Create<EndiciaShipmentAdapter>(new TypedParameter(typeof(ShipmentEntity), shipment));
            var rate = new RateResult("Foo", "1", 0, new PostalRateSelection(PostalServiceType.AsendiaGeneric, PostalConfirmationType.Delivery))
            {
                ShipmentType = ShipmentTypeCode.Endicia
            };

            var result = testObject.DoesRateMatchSelectedService(rate);

            Assert.False(result);
        }

        [Fact]
        public void DoesRateMatchSelectedService_ReturnsFalse_WhenConfirmationDoesNotMatch()
        {
            var testObject = mock.Create<EndiciaShipmentAdapter>(new TypedParameter(typeof(ShipmentEntity), shipment));
            var rate = new RateResult("Foo", "1", 0, new PostalRateSelection(PostalServiceType.FirstClass, PostalConfirmationType.Signature))
            {
                ShipmentType = ShipmentTypeCode.Endicia
            };

            var result = testObject.DoesRateMatchSelectedService(rate);

            Assert.False(result);
        }

        [Fact]
        public void DoesRateMatchSelectedService_ReturnsFalse_WhenRateTagIsOtherObject()
        {
            var testObject = mock.Create<EndiciaShipmentAdapter>(new TypedParameter(typeof(ShipmentEntity), shipment));
            var rate = new RateResult("Foo", "1", 0, "NOT A RATE")
            {
                ShipmentType = ShipmentTypeCode.Endicia
            };

            var result = testObject.DoesRateMatchSelectedService(rate);

            Assert.False(result);
        }

        [Fact]
        public void DoesRateMatchSelectedService_ReturnsTrue_WhenServiceAndConfirmationMatches()
        {
            var testObject = mock.Create<EndiciaShipmentAdapter>(new TypedParameter(typeof(ShipmentEntity), shipment));
            var rate = new RateResult("Foo", "1", 0, new PostalRateSelection(PostalServiceType.FirstClass, PostalConfirmationType.Delivery))
            {
                ShipmentType = ShipmentTypeCode.Endicia
            };

            var result = testObject.DoesRateMatchSelectedService(rate);

            Assert.True(result);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
