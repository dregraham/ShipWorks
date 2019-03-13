using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.SFP;
using ShipWorks.Shipping.Carriers.Amazon.SFP.Api.DTOs;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Stores;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Amazon
{
    public class AmazonSFPShipmentAdapterTest
    {
        readonly ShipmentEntity shipment;
        private readonly AutoMock mock;

        public AmazonSFPShipmentAdapterTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            shipment = new ShipmentEntity
            {
                ShipmentTypeCode = ShipmentTypeCode.AmazonSFP,
                ShipDate = new DateTime(2015, 11, 24, 10, 10, 10),
                ContentWeight = 1,
                TotalWeight = 1,
                Insurance = true,
                AmazonSFP = new AmazonSFPShipmentEntity()
                {
                    ShippingServiceID = "FEDEX_PTP_PRIORITY_OVERNIGHT"
                }
            };

            mock.WithShipmentTypeFromShipmentManager(x =>
            {
                x.Setup(b => b.SupportsGetRates).Returns(true);
                x.Setup(b => b.UpdateDynamicShipmentData(shipment)).Verifiable();
                x.Setup(b => b.UpdateTotalWeight(shipment)).Verifiable();
                x.Setup(b => b.SupportsMultiplePackages).Returns(() => false);
                x.Setup(b => b.IsDomestic(It.IsAny<ShipmentEntity>())).Returns(() => false);
            });
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullExcpetion_WhenShipmentIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new AmazonSFPShipmentAdapter(null, mock.Build<IShipmentTypeManager>(), mock.Build<IStoreManager>(), mock.Build<IAmazonSFPServiceTypeRepository>()));
            Assert.Throws<ArgumentNullException>(() => new AmazonSFPShipmentAdapter(new ShipmentEntity(), mock.Build<IShipmentTypeManager>(), mock.Build<IStoreManager>(), mock.Build<IAmazonSFPServiceTypeRepository>()));
            Assert.Throws<ArgumentNullException>(() => new AmazonSFPShipmentAdapter(shipment, null, mock.Build<IStoreManager>(), mock.Build<IAmazonSFPServiceTypeRepository>()));
        }

        [Fact]
        public void AccountId_ReturnsShipmentValue()
        {
            var testObject = mock.Create<AmazonSFPShipmentAdapter>(TypedParameter.From(shipment));
            Assert.Null(testObject.AccountId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10009238)]
        public void AccountId_StoresSpecifiedValue_WhenValueIsValid(long value)
        {
            var testObject = mock.Create<AmazonSFPShipmentAdapter>(TypedParameter.From(shipment));
            testObject.AccountId = value;
            Assert.Equal(null, testObject.AccountId);
        }

        [Fact]
        public void Shipment_IsNotNull()
        {
            var testObject = mock.Create<AmazonSFPShipmentAdapter>(TypedParameter.From(shipment));
            Assert.NotNull(testObject.Shipment);
        }

        [Fact]
        public void ShipmentTypeCode_IsAmazon()
        {
            var testObject = mock.Create<AmazonSFPShipmentAdapter>(TypedParameter.From(shipment));
            Assert.Equal(ShipmentTypeCode.AmazonSFP, testObject.ShipmentTypeCode);
        }

        [Fact]
        public void SupportsAccounts_IsFalse()
        {
            var testObject = mock.Create<AmazonSFPShipmentAdapter>(TypedParameter.From(shipment));

            Assert.False(testObject.SupportsAccounts);
        }

        [Fact]
        public void SupportsRateShopping_IsFalse()
        {
            var testObject = mock.Create<AmazonSFPShipmentAdapter>(TypedParameter.From(shipment));

            Assert.False(testObject.SupportsRateShopping);
        }

        [Fact]
        public void SupportsMultiplePackages_IsFalse()
        {
            var testObject = mock.Create<AmazonSFPShipmentAdapter>(TypedParameter.From(shipment));
            Assert.False(testObject.SupportsMultiplePackages);
        }

        [Fact]
        public void SupportsMultiplePackages_DomesticIsTrue_WhenShipCountryIsUs()
        {
            shipment.OriginCountryCode = "US";
            shipment.ShipCountryCode = "US";

            var testObject = mock.Create<AmazonSFPShipmentAdapter>(TypedParameter.From(shipment));
            Assert.False(testObject.IsDomestic);
        }

        [Fact]
        public void SupportsMultiplePackages_DomesticIsFalse_WhenShipCountryIsCa()
        {
            shipment.OriginCountryCode = "US";
            shipment.ShipCountryCode = "CA";

            var testObject = mock.Create<AmazonSFPShipmentAdapter>(TypedParameter.From(shipment));
            Assert.False(testObject.IsDomestic);
        }

        [Fact]
        public void UpdateDynamicData_DelegatesToShipmentType()
        {
            var testObject = mock.Create<AmazonSFPShipmentAdapter>(TypedParameter.From(shipment));
            testObject.UpdateDynamicData();

            mock.VerifyAll = true;
        }

        [Fact]
        public void UpdateDynamicData_ErrorsReturned_AreCorrect()
        {
            Dictionary<ShipmentEntity, Exception> errors = new Dictionary<ShipmentEntity, Exception>();
            errors.Add(shipment, new Exception("test"));

            var testObject = mock.Create<AmazonSFPShipmentAdapter>(TypedParameter.From(shipment));

            Assert.NotNull(testObject.UpdateDynamicData());
            Assert.Equal(0, testObject.UpdateDynamicData().Count);
        }

        [Fact]
        public void SupportsPackageTypes_IsFalse()
        {
            var testObject = mock.Create<AmazonSFPShipmentAdapter>(TypedParameter.From(shipment));

            Assert.False(testObject.SupportsPackageTypes);
        }

        [Fact]
        public void ShipDate_ReturnsShipmentValue()
        {
            var testObject = mock.Create<AmazonSFPShipmentAdapter>(TypedParameter.From(shipment));
            Assert.Equal(shipment.ShipDate, testObject.ShipDate);
        }

        [Fact]
        public void ShipDate_IsUpdated()
        {
            var testObject = mock.Create<AmazonSFPShipmentAdapter>(TypedParameter.From(shipment));

            testObject.ShipDate = testObject.ShipDate.AddDays(1);

            Assert.Equal(shipment.ShipDate, testObject.ShipDate);
        }

        [Fact]
        public void UpdateServiceFromRate_SetsService_WhenTagIsValid()
        {
            AmazonRateTag rateTag = new AmazonRateTag
            {
                Description = "Foo",
                ShippingServiceId = "Bar",
                CarrierName = "Quux",
                ServiceTypeID = 2
            };

            mock.Mock<IAmazonSFPServiceTypeRepository>().Setup(r => r.Get())
                .Returns(new List<AmazonSFPServiceTypeEntity>() { new AmazonSFPServiceTypeEntity() { AmazonSFPServiceTypeID = 2, Description = "Foo", ApiValue = "Bar" } });

            var testObject = mock.Create<AmazonSFPShipmentAdapter>(TypedParameter.From(shipment));
            testObject.SelectServiceFromRate(new RateResult("Foo", "1", 1M, rateTag)
            {
                Selectable = true,
                ShipmentType = ShipmentTypeCode.AmazonSFP
            });

            Assert.Equal("Foo", shipment.AmazonSFP.ShippingServiceName);
            Assert.Equal("Bar", shipment.AmazonSFP.ShippingServiceID);
            Assert.Equal("Quux", shipment.AmazonSFP.CarrierName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("Foo")]
        public void UpdateServiceFromRate_DoesNotSetService_WhenTagIsNotValid(string value)
        {
            shipment.AmazonSFP.ShippingServiceName = "A";
            shipment.AmazonSFP.ShippingServiceID = "B";
            shipment.AmazonSFP.CarrierName = "D";

            var testObject = mock.Create<AmazonSFPShipmentAdapter>(TypedParameter.From(shipment));
            testObject.SelectServiceFromRate(new RateResult("Foo", "1", 1M, value)
            {
                Selectable = true,
                ShipmentType = ShipmentTypeCode.AmazonSFP
            });

            Assert.Equal("A", shipment.AmazonSFP.ShippingServiceName);
            Assert.Equal("B", shipment.AmazonSFP.ShippingServiceID);
            Assert.Equal("D", shipment.AmazonSFP.CarrierName);
        }
    }
}
