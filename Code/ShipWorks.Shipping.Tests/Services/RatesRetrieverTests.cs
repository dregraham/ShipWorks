using System;
using System.Linq;
using Autofac.Extras.Moq;
using Autofac.Features.Indexed;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;
using ShipWorks.Stores;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Services
{
    public class RatesRetrieverTests : IDisposable
    {
        readonly AutoMock mock;
        readonly Mock<ShipmentType> shipmentType;
        readonly Mock<IRatingService> ratingService;

        public RatesRetrieverTests()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            shipmentType = mock.CreateMock<ShipmentType>();
            shipmentType.Setup(x => x.SupportsGetRates).Returns(true);
            ratingService = mock.CreateMock<IRatingService>();

            mock.Mock<IStoreTypeManager>()
                .Setup(x => x.GetType(It.IsAny<ShipmentEntity>()))
                .Returns(new TestStoreType());
            mock.Override<IIndex<ShipmentTypeCode, ShipmentType>>()
                .Setup(x => x[It.IsAny<ShipmentTypeCode>()])
                .Returns(shipmentType.Object);
            mock.Override<IIndex<ShipmentTypeCode, IRatingService>>()
                .Setup(x => x[It.IsAny<ShipmentTypeCode>()])
                .Returns(ratingService.Object);
        }

        [Fact]
        public void GetRates_DelegatesToShipmentType_ToUpdateDynamicData()
        {
            var shipment = new ShipmentEntity();
            var testObject = mock.Create<RatesRetriever>();

            testObject.GetRates(shipment, shipmentType.Object);

            shipmentType.Verify(x => x.UpdateDynamicShipmentData(shipment));
        }

        [Fact]
        public void GetRates_DelegatesToShipmentType_ToDetermineResidentialStatus()
        {
            var shipment = new ShipmentEntity { ShipmentID = 123 };
            var testObject = mock.Create<RatesRetriever>();

            testObject.GetRates(shipment, shipmentType.Object);

            shipmentType.Verify(x => x.IsResidentialStatusRequired(It.Is<ShipmentEntity>(s => s.ShipmentID == 123)));
        }

        [Fact]
        public void GetRates_DelegatesToStoreTypeManager_ToGetStoreTypeForShipment()
        {
            var shipment = new ShipmentEntity { ShipmentID = 123 };
            var testObject = mock.Create<RatesRetriever>();

            testObject.GetRates(shipment, shipmentType.Object);

            mock.Mock<IStoreTypeManager>().Verify(x => x.GetType(It.Is<ShipmentEntity>(s => s.ShipmentID == 123)));
        }

        [Fact]
        public void GetRates_DelegatesToStoreType_ToOverrideShipmentDetails()
        {
            var storeType = mock.CreateMock<TestStoreType>();

            mock.Mock<IStoreTypeManager>()
                .Setup(x => x.GetType(It.IsAny<ShipmentEntity>()))
                .Returns(storeType.Object);
            var shipment = new ShipmentEntity { ShipmentID = 123 };
            var testObject = mock.Create<RatesRetriever>();

            testObject.GetRates(shipment, shipmentType.Object);

            storeType.Verify(x => x.OverrideShipmentDetails(It.Is<ShipmentEntity>(s => s.ShipmentID == 123)));
        }

        [Fact]
        public void GetRates_DelegatesToCachedRatesService_ToGetRates()
        {
            var shipment = new ShipmentEntity { ShipmentID = 123 };
            var testObject = mock.Create<RatesRetriever>();

            testObject.GetRates(shipment, shipmentType.Object);

            mock.Mock<ICachedRatesService>()
                .Verify(x => x.GetCachedRates<ShippingException>(
                    It.Is<ShipmentEntity>(s => s.ShipmentID == 123),
                    ratingService.Object.GetRates));
        }

        [Fact]
        public void GetRates_ReturnsRates_FromCachedService()
        {
            RateGroup rateGroup = new RateGroup(Enumerable.Empty<RateResult>());
            mock.Mock<ICachedRatesService>()
                .Setup(x => x.GetCachedRates<ShippingException>(It.IsAny<ShipmentEntity>(), It.IsAny<Func<ShipmentEntity, RateGroup>>()))
                .Returns(rateGroup);

            var shipment = new ShipmentEntity { ShipmentID = 123 };
            var testObject = mock.Create<RatesRetriever>();

            var results = testObject.GetRates(shipment, shipmentType.Object);

            Assert.Equal(rateGroup, results.Value);
        }

        [Fact]
        public void GetRates_ReturnsMessage_WhenShipmentIsProcessed()
        {
            var shipment = new ShipmentEntity { ShipmentID = 123, Processed = true };
            var testObject = mock.Create<RatesRetriever>();

            var results = testObject.GetRates(shipment, shipmentType.Object);

            Assert.Contains("processed", results.Message);
        }

        [Fact]
        public void GetRates_ReturnsMessage_WhenShipmentTypeDoesNotSupportRating()
        {
            var shipment = new ShipmentEntity { ShipmentID = 123 };
            var testObject = mock.Create<RatesRetriever>();

            shipmentType.Setup(x => x.SupportsGetRates).Returns(false);

            var results = testObject.GetRates(shipment, shipmentType.Object);

            Assert.Contains("does not support", results.Message);
        }

        [Fact]
        public void GetRates_ReturnsEmptyRateResult_WhenNotSuccessful()
        {
            var shipment = new ShipmentEntity { ShipmentID = 123 };
            var testObject = mock.Create<RatesRetriever>();

            shipmentType.Setup(x => x.SupportsGetRates).Returns(false);

            var results = testObject.GetRates(shipment, shipmentType.Object);

            Assert.Empty(results.Value.Rates);
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}
