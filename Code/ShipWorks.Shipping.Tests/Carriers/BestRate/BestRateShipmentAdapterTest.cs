using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Stores;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.BestRate
{
    public class BestRateShipmentAdapterTest
    {
        private readonly ShipmentEntity shipment;
        private readonly AutoMock mock;

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
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExcpetion_WhenParamsNull()
        {
            Assert.Throws<ArgumentNullException>(() =>
                new BestRateShipmentAdapter(null, mock.Create<IShipmentTypeManager>(), mock.Create<ICustomsManager>(),
                mock.Create<IStoreManager>()));
            Assert.Throws<ArgumentNullException>(() =>
                new BestRateShipmentAdapter(new ShipmentEntity(), null, mock.Create<ICustomsManager>(),
                mock.Create<IStoreManager>()));
            Assert.Throws<ArgumentNullException>(() =>
                new BestRateShipmentAdapter(new ShipmentEntity(), mock.Create<IShipmentTypeManager>(), null,
                mock.Create<IStoreManager>()));
        }

        [Fact]
        public void AccountId_ReturnsNull()
        {
            var testObject = mock.Create<BestRateShipmentAdapter>(TypedParameter.From(shipment));
            Assert.Null(testObject.AccountId);
        }

        [Fact]
        public void AccountId_DoesNotThrow_WhenValueIsValid()
        {
            var testObject = mock.Create<BestRateShipmentAdapter>(TypedParameter.From(shipment));
            testObject.AccountId = 6;
        }

        [Fact]
        public void AccountId_DoesNotThrow_WhenValueIsNull()
        {
            var testObject = mock.Create<BestRateShipmentAdapter>(TypedParameter.From(shipment));
            testObject.AccountId = null;
        }

        [Fact]
        public void Shipment_IsNotNull()
        {
            var testObject = mock.Create<BestRateShipmentAdapter>(TypedParameter.From(shipment));
            Assert.NotNull(testObject.Shipment);
        }

        [Fact]
        public void ShipmentTypeCode_IsBestRate()
        {
            var testObject = mock.Create<BestRateShipmentAdapter>(TypedParameter.From(shipment));
            Assert.Equal(ShipmentTypeCode.BestRate, testObject.ShipmentTypeCode);
        }

        [Fact]
        public void SupportsAccounts_IsFalse()
        {
            var testObject = mock.Create<BestRateShipmentAdapter>(TypedParameter.From(shipment));

            Assert.False(testObject.SupportsAccounts);
        }

        [Fact]
        public void SupportsMultiplePackages_IsFalse()
        {
            var testObject = mock.Create<BestRateShipmentAdapter>(TypedParameter.From(shipment));
            Assert.False(testObject.SupportsMultiplePackages);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(false, false)]
        public void SupportsMultiplePackages_DomesticIsTrue_WhenShipCountryIsUs(bool isDomestic, bool expected)
        {
            mock.WithShipmentTypeFromShipmentManager(x => x.Setup(s => s.IsDomestic(shipment)).Returns(isDomestic));

            var testObject = mock.Create<BestRateShipmentAdapter>(TypedParameter.From(shipment));
            Assert.Equal(expected, testObject.IsDomestic);
        }

        [Fact]
        public void UpdateDynamicData_DelegatesToShipmentTypeAndCustomsManager()
        {
            mock.WithShipmentTypeFromShipmentManager(x =>
            {
                x.Setup(b => b.UpdateDynamicShipmentData(shipment)).Verifiable();
                x.Setup(b => b.UpdateTotalWeight(shipment)).Verifiable();
            });

            mock.Mock<ICustomsManager>().Setup(b => b.EnsureCustomsLoaded(new[] { shipment })).Verifiable();

            var testObject = mock.Create<BestRateShipmentAdapter>(TypedParameter.From(shipment));
            testObject.UpdateDynamicData();

            mock.VerifyAll = true;
        }

        [Fact]
        public void UpdateDynamicData_ErrorsReturned_AreCorrect()
        {
            Dictionary<ShipmentEntity, Exception> errors = new Dictionary<ShipmentEntity, Exception>();
            errors.Add(shipment, new Exception("test"));

            mock.Mock<ICustomsManager>().Setup(c => c.EnsureCustomsLoaded(It.IsAny<IEnumerable<ShipmentEntity>>())).Returns(errors);

            var testObject = mock.Create<BestRateShipmentAdapter>(TypedParameter.From(shipment));

            Assert.NotNull(testObject.UpdateDynamicData());
            Assert.Equal(1, testObject.UpdateDynamicData().Count);
        }

        [Fact]
        public void SupportsPackageTypes_IsFalse()
        {
            var testObject = mock.Create<BestRateShipmentAdapter>(TypedParameter.From(shipment));

            Assert.False(testObject.SupportsPackageTypes);
        }

        [Fact]
        public void ShipDate_ReturnsShipmentValue()
        {
            var testObject = mock.Create<BestRateShipmentAdapter>(TypedParameter.From(shipment));
            Assert.Equal(shipment.ShipDate, testObject.ShipDate);
        }

        [Fact]
        public void ShipDate_IsUpdated()
        {
            var testObject = mock.Create<BestRateShipmentAdapter>(TypedParameter.From(shipment));

            testObject.ShipDate = testObject.ShipDate.AddDays(1);

            Assert.Equal(shipment.ShipDate, testObject.ShipDate);
        }
    }
}
