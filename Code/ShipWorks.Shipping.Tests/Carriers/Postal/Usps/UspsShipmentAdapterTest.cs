using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Stores;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Postal.Usps
{
    public class UspsShipmentAdapterTest : IDisposable
    {
        readonly AutoMock mock;
        readonly ShipmentEntity shipment;

        public UspsShipmentAdapterTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            shipment = new ShipmentEntity
            {
                ShipmentTypeCode = ShipmentTypeCode.Usps,
                ShipDate = new DateTime(2015, 11, 24, 10, 10, 10),
                ContentWeight = 1,
                TotalWeight = 1,
                Insurance = true,
                Postal = new PostalShipmentEntity
                {
                    Service = (int) PostalServiceType.FirstClass,
                    Confirmation = (int) PostalConfirmationType.Delivery,
                    Usps = new UspsShipmentEntity()
                }
            };
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExcpetion_WhenShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new UspsShipmentAdapter(null, mock.Create<IShipmentTypeManager>(),
                    mock.Create<ICustomsManager>(), mock.Create<IStoreManager>()));
            Assert.Throws<ArgumentNullException>(() =>
                new UspsShipmentAdapter(new ShipmentEntity(), mock.Create<IShipmentTypeManager>(),
                    mock.Create<ICustomsManager>(), mock.Create<IStoreManager>()));
            Assert.Throws<ArgumentNullException>(() =>
                new UspsShipmentAdapter(new ShipmentEntity { Postal = new PostalShipmentEntity() }, mock.Create<IShipmentTypeManager>(),
                    mock.Create<ICustomsManager>(), mock.Create<IStoreManager>()));

            Assert.Throws<ArgumentNullException>(() =>
                new UspsShipmentAdapter(shipment, null,
                    mock.Create<ICustomsManager>(), mock.Create<IStoreManager>()));
            Assert.Throws<ArgumentNullException>(() => new UspsShipmentAdapter(shipment, mock.Create<IShipmentTypeManager>(), null, mock.Create<IStoreManager>()));

        }

        [Fact]
        public void AccountId_ReturnsShipmentValue()
        {
            shipment.Postal.Usps.UspsAccountID = 12;
            var testObject = mock.Create<UspsShipmentAdapter>(TypedParameter.From(shipment));
            Assert.Equal(12, testObject.AccountId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10009238)]
        public void AccountId_StoresSpecifiedValue_WhenValueIsValid(long value)
        {
            var testObject = mock.Create<UspsShipmentAdapter>(TypedParameter.From(shipment));
            testObject.AccountId = value;
            Assert.Equal(value, shipment.Postal.Usps.UspsAccountID);
        }

        [Fact]
        public void AccountId_StoresZero_WhenValueIsNull()
        {
            var testObject = mock.Create<UspsShipmentAdapter>(TypedParameter.From(shipment));
            testObject.AccountId = null;
            Assert.Equal(0, shipment.Postal.Usps.UspsAccountID);
        }

        [Fact]
        public void Shipment_IsNotNull()
        {
            var testObject = mock.Create<UspsShipmentAdapter>(TypedParameter.From(shipment));
            Assert.NotNull(testObject.Shipment);
        }

        [Fact]
        public void ShipmentTypeCode_IsUsps()
        {
            var testObject = mock.Create<UspsShipmentAdapter>(TypedParameter.From(shipment));
            Assert.Equal(ShipmentTypeCode.Usps, testObject.ShipmentTypeCode);
        }

        [Fact]
        public void ShipmentTypeCode_IsExpress1Usps()
        {
            shipment.ShipmentTypeCode = ShipmentTypeCode.Express1Usps;
            var testObject = mock.Create<UspsShipmentAdapter>(TypedParameter.From(shipment));
            Assert.Equal(ShipmentTypeCode.Express1Usps, testObject.ShipmentTypeCode);
        }

        [Fact]
        public void SupportsAccounts_IsTrue()
        {
            var testObject = mock.Create<UspsShipmentAdapter>(TypedParameter.From(shipment));

            Assert.True(testObject.SupportsAccounts);
        }

        [Fact]
        public void SupportsMultiplePackages_IsFalse()
        {
            var testObject = mock.Create<UspsShipmentAdapter>(TypedParameter.From(shipment));
            Assert.False(testObject.SupportsMultiplePackages);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, true)]
        public void IsDomestic_DelegatesToIsDomestic_OnShipmentType(bool isDomestic, bool expected)
        {
            mock.WithShipmentTypeFromShipmentManager(x => x.Setup(b => b.IsDomestic(shipment)).Returns(isDomestic));

            var testObject = mock.Create<UspsShipmentAdapter>(TypedParameter.From(shipment));

            Assert.Equal(expected, testObject.IsDomestic);
        }

        [Fact]
        public void UpdateDynamicData_DelegatesToShipmentTypeAndCustomsManager()
        {
            var shipmentType = mock.WithShipmentTypeFromShipmentManager();

            var testObject = mock.Create<UspsShipmentAdapter>(TypedParameter.From(shipment));
            testObject.UpdateDynamicData();

            shipmentType.Verify(b => b.UpdateDynamicShipmentData(It.IsAny<ShipmentEntity>()), Times.Once);
            shipmentType.Verify(b => b.UpdateTotalWeight(It.IsAny<ShipmentEntity>()), Times.Once);

            mock.Mock<ICustomsManager>().Verify(b => b.EnsureCustomsLoaded(It.IsAny<IEnumerable<ShipmentEntity>>()));
        }

        [Fact]
        public void UpdateDynamicData_ErrorsReturned_AreCorrect()
        {
            Dictionary<ShipmentEntity, Exception> errors = new Dictionary<ShipmentEntity, Exception>();
            errors.Add(shipment, new Exception("test"));

            mock.Mock<ICustomsManager>()
                .Setup(c => c.EnsureCustomsLoaded(It.IsAny<IEnumerable<ShipmentEntity>>())).Returns(errors);

            var testObject = mock.Create<UspsShipmentAdapter>(TypedParameter.From(shipment));

            Assert.NotNull(testObject.UpdateDynamicData());
            Assert.Equal(1, testObject.UpdateDynamicData().Count);
        }

        [Fact]
        public void SupportsPackageTypes_IsTrue()
        {
            var testObject = mock.Create<UspsShipmentAdapter>(TypedParameter.From(shipment));

            Assert.True(testObject.SupportsPackageTypes);
        }

        [Fact]
        public void ShipDate_ReturnsShipmentValue()
        {
            var testObject = mock.Create<UspsShipmentAdapter>(TypedParameter.From(shipment));
            Assert.Equal(shipment.ShipDate, testObject.ShipDate);
        }

        [Fact]
        public void ShipDate_IsUpdated()
        {
            var testObject = mock.Create<UspsShipmentAdapter>(TypedParameter.From(shipment));

            testObject.ShipDate = testObject.ShipDate.AddDays(1);

            Assert.Equal(shipment.ShipDate, testObject.ShipDate);
        }

        [Fact]
        public void ServiceType_ReturnsShipmentValue()
        {
            var testObject = mock.Create<UspsShipmentAdapter>(TypedParameter.From(shipment));
            Assert.Equal(shipment.Postal.Service, testObject.ServiceType);
        }

        [Fact]
        public void ServiceType_IsUpdated()
        {
            var testObject = mock.Create<UspsShipmentAdapter>(TypedParameter.From(shipment));

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
            mock.WithShipmentTypeFromShipmentManager(x =>
            {
                x.Setup(b => b.SupportsGetRates).Returns(true);
                x.Setup(b => b.ShipmentTypeCode).Returns(ShipmentTypeCode.Usps);
            });
            var testObject = mock.Create<UspsShipmentAdapter>(TypedParameter.From(shipment));
            testObject.SelectServiceFromRate(new RateResult("Foo", "1", 1M, new PostalRateSelection(serviceType, confirmationType))
            {
                Selectable = true,
                ShipmentType = ShipmentTypeCode.Usps
            });
            Assert.Equal((int) serviceType, shipment.Postal.Service);
            Assert.Equal((int) confirmationType, shipment.Postal.Confirmation);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("Foo")]
        public void UpdateServiceFromRate_DoesNotSetService_WhenTagIsNotValid(string value)
        {
            mock.WithShipmentTypeFromShipmentManager(x =>
            {
                x.Setup(b => b.SupportsGetRates).Returns(true);
                x.Setup(b => b.ShipmentTypeCode).Returns(ShipmentTypeCode.Usps);
            });
            shipment.Postal.Service = (int) PostalServiceType.PriorityMail;
            shipment.Postal.Confirmation = (int) PostalConfirmationType.AdultSignatureRestricted;

            var testObject = mock.Create<UspsShipmentAdapter>(TypedParameter.From(shipment));
            testObject.SelectServiceFromRate(new RateResult("Foo", "1", 1M, value)
            {
                Selectable = true,
                ShipmentType = ShipmentTypeCode.Usps
            });
            Assert.Equal((int) PostalServiceType.PriorityMail, shipment.Postal.Service);
            Assert.Equal((int) PostalConfirmationType.AdultSignatureRestricted, shipment.Postal.Confirmation);
        }

        [Fact]
        public void DoesRateMatchSelectedService_ReturnsFalse_WhenRateShipmentTypeDoesNotMatch()
        {
            mock.WithShipmentTypeFromShipmentManager(x =>
            {
                x.Setup(b => b.SupportsGetRates).Returns(true);
                x.Setup(b => b.ShipmentTypeCode).Returns(ShipmentTypeCode.Usps);
            });
            var testObject = mock.Create<UspsShipmentAdapter>(TypedParameter.From(shipment));
            var rate = new RateResult("Foo", "1", 0, 1) { ShipmentType = ShipmentTypeCode.None };

            var result = testObject.DoesRateMatchSelectedService(rate);

            Assert.False(result);
        }

        [Theory]
        [InlineData(PostalServiceType.AsendiaGeneric, PostalConfirmationType.Signature)]
        [InlineData(PostalServiceType.AsendiaGeneric, PostalConfirmationType.Delivery)]
        [InlineData(PostalServiceType.FirstClass, PostalConfirmationType.Signature)]
        public void DoesRateMatchSelectedService_ReturnsFalse_WhenServiceAndConfirmationDoNotMatch(PostalServiceType serviceType, PostalConfirmationType confirmation)
        {
            mock.WithShipmentTypeFromShipmentManager(x =>
            {
                x.Setup(b => b.SupportsGetRates).Returns(true);
                x.Setup(b => b.ShipmentTypeCode).Returns(ShipmentTypeCode.Usps);
            });

            var testObject = mock.Create<UspsShipmentAdapter>(TypedParameter.From(shipment));
            var rate = new RateResult("Foo", "1", 0, new PostalRateSelection(serviceType, confirmation))
            {
                ShipmentType = ShipmentTypeCode.Usps
            };

            var result = testObject.DoesRateMatchSelectedService(rate);

            Assert.False(result);
        }

        [Fact]
        public void DoesRateMatchSelectedService_ReturnsFalse_WhenRateTagIsOtherObject()
        {
            mock.WithShipmentTypeFromShipmentManager(x =>
            {
                x.Setup(b => b.SupportsGetRates).Returns(true);
                x.Setup(b => b.ShipmentTypeCode).Returns(ShipmentTypeCode.Usps);
            });

            var testObject = mock.Create<UspsShipmentAdapter>(TypedParameter.From(shipment));
            var rate = new RateResult("Foo", "1", 0, "NOT A RATE")
            {
                ShipmentType = ShipmentTypeCode.Usps
            };

            var result = testObject.DoesRateMatchSelectedService(rate);

            Assert.False(result);
        }

        [Fact]
        public void DoesRateMatchSelectedService_ReturnsTrue_WhenServiceAndConfirmationMatches()
        {
            mock.WithShipmentTypeFromShipmentManager(x =>
            {
                x.Setup(b => b.SupportsGetRates).Returns(true);
                x.Setup(b => b.ShipmentTypeCode).Returns(ShipmentTypeCode.Usps);
            });

            var testObject = mock.Create<UspsShipmentAdapter>(TypedParameter.From(shipment));
            var rate = new RateResult("Foo", "1", 0, new PostalRateSelection(PostalServiceType.FirstClass, PostalConfirmationType.Delivery))
            {
                ShipmentType = ShipmentTypeCode.Usps
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
