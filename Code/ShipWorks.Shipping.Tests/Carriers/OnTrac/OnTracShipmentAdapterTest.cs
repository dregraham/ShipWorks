using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.OnTrac;
using ShipWorks.Shipping.Carriers.OnTrac.Enums;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Stores;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.OnTrac
{
    public class OnTracShipmentAdapterTest : IDisposable
    {
        readonly AutoMock mock;
        readonly ShipmentEntity shipment;

        public OnTracShipmentAdapterTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            shipment = new ShipmentEntity
            {
                ShipmentTypeCode = ShipmentTypeCode.OnTrac,
                ShipDate = new DateTime(2015, 11, 24, 10, 10, 10),
                ContentWeight = 1,
                TotalWeight = 1,
                Insurance = true,
                OnTrac = new OnTracShipmentEntity()
                {
                    Service = (int) OnTracServiceType.Ground
                }
            };
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExcpetion_WhenShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new OnTracShipmentAdapter(null, mock.Create<IShipmentTypeManager>(),
                    mock.Create<ICustomsManager>(), mock.Create<IStoreManager>()));
            Assert.Throws<ArgumentNullException>(() =>
                new OnTracShipmentAdapter(new ShipmentEntity(), mock.Create<IShipmentTypeManager>(),
                    mock.Create<ICustomsManager>(), mock.Create<IStoreManager>()));
            Assert.Throws<ArgumentNullException>(() =>
                new OnTracShipmentAdapter(shipment, null,
                    mock.Create<ICustomsManager>(), mock.Create<IStoreManager>()));
            Assert.Throws<ArgumentNullException>(() =>
                new OnTracShipmentAdapter(shipment, mock.Create<IShipmentTypeManager>(), null,
                    mock.Create<IStoreManager>()));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExcpetion_WhenOnTracShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new OnTracShipmentAdapter(new ShipmentEntity(), mock.Create<IShipmentTypeManager>(),
                    mock.Create<ICustomsManager>(), mock.Create<IStoreManager>()));
        }

        [Fact]
        public void AccountId_ReturnsShipmentValue()
        {
            shipment.OnTrac.OnTracAccountID = 12;
            var testObject = mock.Create<OnTracShipmentAdapter>(TypedParameter.From(shipment));
            Assert.Equal(12, testObject.AccountId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10009238)]
        public void AccountId_StoresSpecifiedValue_WhenValueIsValid(long value)
        {
            var testObject = mock.Create<OnTracShipmentAdapter>(TypedParameter.From(shipment));
            testObject.AccountId = value;
            Assert.Equal(value, shipment.OnTrac.OnTracAccountID);
        }

        [Fact]
        public void AccountId_StoresZero_WhenValueIsNull()
        {
            var testObject = mock.Create<OnTracShipmentAdapter>(TypedParameter.From(shipment));
            testObject.AccountId = null;
            Assert.Equal(0, shipment.OnTrac.OnTracAccountID);
        }

        [Fact]
        public void Shipment_IsNotNull()
        {
            var testObject = mock.Create<OnTracShipmentAdapter>(TypedParameter.From(shipment));
            Assert.NotNull(testObject.Shipment);
        }

        [Fact]
        public void ShipmentTypeCode_IsOnTrac()
        {
            var testObject = mock.Create<OnTracShipmentAdapter>(TypedParameter.From(shipment));
            Assert.Equal(ShipmentTypeCode.OnTrac, testObject.ShipmentTypeCode);
        }

        [Fact]
        public void SupportsAccounts_IsTrue()
        {
            var testObject = mock.Create<OnTracShipmentAdapter>(TypedParameter.From(shipment));

            Assert.True(testObject.SupportsAccounts);
        }

        [Fact]
        public void SupportsMultiplePackages_IsFalse()
        {
            var testObject = mock.Create<OnTracShipmentAdapter>(TypedParameter.From(shipment));
            Assert.False(testObject.SupportsMultiplePackages);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, true)]
        public void IsDomestic_DomesticIsTrue_WhenShipCountryIsUs(bool isDomestic, bool expected)
        {
            mock.WithShipmentTypeFromShipmentManager(x =>
                x.Setup(b => b.IsDomestic(It.IsAny<ShipmentEntity>())).Returns(isDomestic));

            var testObject = mock.Create<OnTracShipmentAdapter>(TypedParameter.From(shipment));

            Assert.Equal(expected, testObject.IsDomestic);
        }

        [Fact]
        public void UpdateDynamicData_DelegatesToShipmentTypeAndCustomsManager()
        {
            var shipmentType = mock.WithShipmentTypeFromShipmentManager();

            var testObject = mock.Create<OnTracShipmentAdapter>(TypedParameter.From(shipment));
            testObject.UpdateDynamicData();

            shipmentType.Verify(b => b.UpdateDynamicShipmentData(It.IsAny<ShipmentEntity>()));
            shipmentType.Verify(b => b.UpdateTotalWeight(It.IsAny<ShipmentEntity>()));

            mock.Mock<ICustomsManager>()
                .Verify(b => b.EnsureCustomsLoaded(It.IsAny<IEnumerable<ShipmentEntity>>()));
        }

        [Fact]
        public void UpdateDynamicData_ErrorsReturned_AreCorrect()
        {
            Dictionary<ShipmentEntity, Exception> errors = new Dictionary<ShipmentEntity, Exception>();
            errors.Add(shipment, new Exception("test"));

            mock.Mock<ICustomsManager>()
                .Setup(c => c.EnsureCustomsLoaded(It.IsAny<IEnumerable<ShipmentEntity>>())).Returns(errors);

            var testObject = mock.Create<OnTracShipmentAdapter>(TypedParameter.From(shipment));

            Assert.NotNull(testObject.UpdateDynamicData());
            Assert.Equal(1, testObject.UpdateDynamicData().Count);
        }

        [Fact]
        public void SupportsPackageTypes_IsTrue()
        {
            var testObject = mock.Create<OnTracShipmentAdapter>(TypedParameter.From(shipment));

            Assert.True(testObject.SupportsPackageTypes);
        }

        [Fact]
        public void ShipDate_ReturnsShipmentValue()
        {
            var testObject = mock.Create<OnTracShipmentAdapter>(TypedParameter.From(shipment));
            Assert.Equal(shipment.ShipDate, testObject.ShipDate);
        }

        [Fact]
        public void ShipDate_IsUpdated()
        {
            var testObject = mock.Create<OnTracShipmentAdapter>(TypedParameter.From(shipment));

            testObject.ShipDate = testObject.ShipDate.AddDays(1);

            Assert.Equal(shipment.ShipDate, testObject.ShipDate);
        }

        [Fact]
        public void ServiceType_ReturnsShipmentValue()
        {
            var testObject = mock.Create<OnTracShipmentAdapter>(TypedParameter.From(shipment));
            Assert.Equal(shipment.OnTrac.Service, testObject.ServiceType);
        }

        [Fact]
        public void ServiceType_IsUpdated()
        {
            var testObject = mock.Create<OnTracShipmentAdapter>(TypedParameter.From(shipment));

            shipment.OnTrac.Service = (int) OnTracServiceType.Sunrise;
            testObject.ServiceType = (int) OnTracServiceType.SunriseGold;

            Assert.Equal(shipment.OnTrac.Service, testObject.ServiceType);
        }

        [Theory]
        [InlineData(OnTracServiceType.None)]
        [InlineData(OnTracServiceType.PalletizedFreight)]
        [InlineData(OnTracServiceType.SunriseGold)]
        public void UpdateServiceFromRate_SetsService_WhenTagIsValid(OnTracServiceType serviceType)
        {
            mock.WithShipmentTypeFromShipmentManager(x => x.Setup(b => b.SupportsGetRates).Returns(true));
            var testObject = mock.Create<OnTracShipmentAdapter>(TypedParameter.From(shipment));

            testObject.SelectServiceFromRate(new RateResult("Foo", "1", 1M, (int) serviceType)
            {
                Selectable = true,
                ShipmentType = ShipmentTypeCode.OnTrac
            });

            Assert.Equal((int) serviceType, shipment.OnTrac.Service);
        }

        [Theory]
        [InlineData(OnTracServiceType.None)]
        [InlineData(OnTracServiceType.PalletizedFreight)]
        [InlineData(OnTracServiceType.SunriseGold)]
        public void UpdateServiceFromRate_SetsService_WhenTagIsValidServiceType(OnTracServiceType serviceType)
        {
            mock.WithShipmentTypeFromShipmentManager(x => x.Setup(b => b.SupportsGetRates).Returns(true));
            var testObject = mock.Create<OnTracShipmentAdapter>(TypedParameter.From(shipment));
            testObject.SelectServiceFromRate(new RateResult("Foo", "1", 1M, serviceType)
            {
                Selectable = true,
                ShipmentType = ShipmentTypeCode.OnTrac
            });
            Assert.Equal((int) serviceType, shipment.OnTrac.Service);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("Foo")]
        public void UpdateServiceFromRate_DoesNotSetService_WhenTagIsNotValid(string value)
        {
            mock.WithShipmentTypeFromShipmentManager(x => x.Setup(b => b.SupportsGetRates).Returns(true));
            shipment.OnTrac.Service = (int) OnTracServiceType.PalletizedFreight;
            var testObject = mock.Create<OnTracShipmentAdapter>(TypedParameter.From(shipment));
            testObject.SelectServiceFromRate(new RateResult("Foo", "1", 1M, value)
            {
                Selectable = true,
                ShipmentType = ShipmentTypeCode.OnTrac
            });
            Assert.Equal((int) OnTracServiceType.PalletizedFreight, shipment.OnTrac.Service);
        }

        [Fact]
        public void DoesRateMatchSelectedService_ReturnsFalse_WhenRateShipmentTypeDoesNotMatch()
        {
            var testObject = mock.Create<OnTracShipmentAdapter>(new TypedParameter(typeof(ShipmentEntity), shipment));
            var rate = new RateResult("Foo", "1", 0, 1) { ShipmentType = ShipmentTypeCode.None };

            var result = testObject.DoesRateMatchSelectedService(rate);

            Assert.False(result);
        }

        [Fact]
        public void DoesRateMatchSelectedService_ReturnsFalse_WhenRateServiceAsIntDoesNotMatch()
        {
            var testObject = mock.Create<OnTracShipmentAdapter>(new TypedParameter(typeof(ShipmentEntity), shipment));
            var rate = new RateResult("Foo", "1", 0, (int) OnTracServiceType.PalletizedFreight)
            {
                ShipmentType = ShipmentTypeCode.OnTrac
            };

            var result = testObject.DoesRateMatchSelectedService(rate);

            Assert.False(result);
        }

        [Fact]
        public void DoesRateMatchSelectedService_ReturnsFalse_WhenRateServiceAsServiceTypeDoesNotMatch()
        {
            var testObject = mock.Create<OnTracShipmentAdapter>(new TypedParameter(typeof(ShipmentEntity), shipment));
            var rate = new RateResult("Foo", "1", 0, OnTracServiceType.PalletizedFreight)
            {
                ShipmentType = ShipmentTypeCode.OnTrac
            };

            var result = testObject.DoesRateMatchSelectedService(rate);

            Assert.False(result);
        }

        [Fact]
        public void DoesRateMatchSelectedService_ReturnsFalse_WhenRateTagIsOtherObject()
        {
            var testObject = mock.Create<OnTracShipmentAdapter>(new TypedParameter(typeof(ShipmentEntity), shipment));
            var rate = new RateResult("Foo", "1", 0, "NOT A RATE")
            {
                ShipmentType = ShipmentTypeCode.OnTrac
            };

            var result = testObject.DoesRateMatchSelectedService(rate);

            Assert.False(result);
        }

        [Fact]
        public void DoesRateMatchSelectedService_ReturnsTrue_WhenRateServiceAsIntMatches()
        {
            var testObject = mock.Create<OnTracShipmentAdapter>(new TypedParameter(typeof(ShipmentEntity), shipment));
            var rate = new RateResult("Foo", "1", 0, (int) OnTracServiceType.Ground)
            {
                ShipmentType = ShipmentTypeCode.OnTrac
            };

            var result = testObject.DoesRateMatchSelectedService(rate);

            Assert.True(result);
        }

        [Fact]
        public void DoesRateMatchSelectedService_ReturnsTrue_WhenRateServiceAsServiceTypeMatches()
        {
            var testObject = mock.Create<OnTracShipmentAdapter>(new TypedParameter(typeof(ShipmentEntity), shipment));
            var rate = new RateResult("Foo", "1", 0, OnTracServiceType.Ground)
            {
                ShipmentType = ShipmentTypeCode.OnTrac
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
