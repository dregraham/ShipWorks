using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Stores;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Amazon
{
    public class AmazonShipmentAdapterTest
    {
        readonly ShipmentEntity shipment;
        private readonly AutoMock mock;

        public AmazonShipmentAdapterTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

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
            Assert.Throws<ArgumentNullException>(() => new AmazonShipmentAdapter(null, mock.Create<IShipmentTypeManager>(), mock.Create<IStoreManager>()));
            Assert.Throws<ArgumentNullException>(() => new AmazonShipmentAdapter(new ShipmentEntity(), mock.Create<IShipmentTypeManager>(), mock.Create<IStoreManager>()));
            Assert.Throws<ArgumentNullException>(() => new AmazonShipmentAdapter(shipment, null, mock.Create<IStoreManager>()));
        }

        [Fact]
        public void AccountId_ReturnsShipmentValue()
        {
            var testObject = mock.Create<AmazonShipmentAdapter>(TypedParameter.From(shipment));
            Assert.Null(testObject.AccountId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10009238)]
        public void AccountId_StoresSpecifiedValue_WhenValueIsValid(long value)
        {
            var testObject = mock.Create<AmazonShipmentAdapter>(TypedParameter.From(shipment));
            testObject.AccountId = value;
            Assert.Equal(null, testObject.AccountId);
        }

        [Fact]
        public void Shipment_IsNotNull()
        {
            var testObject = mock.Create<AmazonShipmentAdapter>(TypedParameter.From(shipment));
            Assert.NotNull(testObject.Shipment);
        }

        [Fact]
        public void ShipmentTypeCode_IsAmazon()
        {
            var testObject = mock.Create<AmazonShipmentAdapter>(TypedParameter.From(shipment));
            Assert.Equal(ShipmentTypeCode.Amazon, testObject.ShipmentTypeCode);
        }

        [Fact]
        public void SupportsAccounts_IsFalse()
        {
            var testObject = mock.Create<AmazonShipmentAdapter>(TypedParameter.From(shipment));

            Assert.False(testObject.SupportsAccounts);
        }

        [Fact]
        public void SupportsMultiplePackages_IsFalse()
        {
            var testObject = mock.Create<AmazonShipmentAdapter>(TypedParameter.From(shipment));
            Assert.False(testObject.SupportsMultiplePackages);
        }

        [Fact]
        public void SupportsMultiplePackages_DomesticIsTrue_WhenShipCountryIsUs()
        {
            shipment.OriginCountryCode = "US";
            shipment.ShipCountryCode = "US";

            var testObject = mock.Create<AmazonShipmentAdapter>(TypedParameter.From(shipment));
            Assert.False(testObject.IsDomestic);
        }

        [Fact]
        public void SupportsMultiplePackages_DomesticIsFalse_WhenShipCountryIsCa()
        {
            shipment.OriginCountryCode = "US";
            shipment.ShipCountryCode = "CA";

            var testObject = mock.Create<AmazonShipmentAdapter>(TypedParameter.From(shipment));
            Assert.False(testObject.IsDomestic);
        }

        [Fact]
        public void UpdateDynamicData_DelegatesToShipmentType()
        {
            var testObject = mock.Create<AmazonShipmentAdapter>(TypedParameter.From(shipment));
            testObject.UpdateDynamicData();

            mock.VerifyAll = true;
        }

        [Fact]
        public void UpdateDynamicData_ErrorsReturned_AreCorrect()
        {
            Dictionary<ShipmentEntity, Exception> errors = new Dictionary<ShipmentEntity, Exception>();
            errors.Add(shipment, new Exception("test"));

            var testObject = mock.Create<AmazonShipmentAdapter>(TypedParameter.From(shipment));

            Assert.NotNull(testObject.UpdateDynamicData());
            Assert.Equal(0, testObject.UpdateDynamicData().Count);
        }

        [Fact]
        public void SupportsPackageTypes_IsFalse()
        {
            var testObject = mock.Create<AmazonShipmentAdapter>(TypedParameter.From(shipment));

            Assert.False(testObject.SupportsPackageTypes);
        }

        [Fact]
        public void ShipDate_ReturnsShipmentValue()
        {
            var testObject = mock.Create<AmazonShipmentAdapter>(TypedParameter.From(shipment));
            Assert.Equal(shipment.ShipDate, testObject.ShipDate);
        }

        [Fact]
        public void ShipDate_IsUpdated()
        {
            var testObject = mock.Create<AmazonShipmentAdapter>(TypedParameter.From(shipment));

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
                ShippingServiceOfferId = "Baz",
                CarrierName = "Quux"
            };

            var testObject = mock.Create<AmazonShipmentAdapter>(TypedParameter.From(shipment));
            testObject.SelectServiceFromRate(new RateResult("Foo", "1", 1M, rateTag)
            {
                Selectable = true,
                ShipmentType = ShipmentTypeCode.Amazon
            });

            Assert.Equal("Foo", shipment.Amazon.ShippingServiceName);
            Assert.Equal("Bar", shipment.Amazon.ShippingServiceID);
            Assert.Equal("Baz", shipment.Amazon.ShippingServiceOfferID);
            Assert.Equal("Quux", shipment.Amazon.CarrierName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("Foo")]
        public void UpdateServiceFromRate_DoesNotSetService_WhenTagIsNotValid(string value)
        {
            shipment.Amazon.ShippingServiceName = "A";
            shipment.Amazon.ShippingServiceID = "B";
            shipment.Amazon.ShippingServiceOfferID = "C";
            shipment.Amazon.CarrierName = "D";

            var testObject = mock.Create<AmazonShipmentAdapter>(TypedParameter.From(shipment));
            testObject.SelectServiceFromRate(new RateResult("Foo", "1", 1M, value)
            {
                Selectable = true,
                ShipmentType = ShipmentTypeCode.Amazon
            });

            Assert.Equal("A", shipment.Amazon.ShippingServiceName);
            Assert.Equal("B", shipment.Amazon.ShippingServiceID);
            Assert.Equal("C", shipment.Amazon.ShippingServiceOfferID);
            Assert.Equal("D", shipment.Amazon.CarrierName);
        }
    }
}
