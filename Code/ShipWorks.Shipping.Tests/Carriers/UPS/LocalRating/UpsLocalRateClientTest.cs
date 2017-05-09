using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using Autofac.Features.Indexed;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using ShipWorks.Shipping.Carriers.Ups.LocalRating.ServiceFilters;
using ShipWorks.Shipping.Carriers.Ups.LocalRating.Surcharges;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.LocalRating
{
    public class UpsLocalRateClientTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly ShipmentEntity shipment;

        public UpsLocalRateClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            shipment = Create.Shipment().AsUps().Build();
        }

        [Fact]
        public void GetRates_ReturnsRatesFromLocalRateTable()
        {
            var calculated = new[]
            {
                new UpsLocalServiceRate(UpsServiceType.Ups2DayAir, 1, true, 0),
                new UpsLocalServiceRate(UpsServiceType.UpsGround, 2, true, 0)
            };

            mock.Mock<IUpsLocalRateTable>()
                .Setup(r => r.CalculateRates(shipment))
                .Returns(() => GenericResult.FromSuccess<IEnumerable<UpsLocalServiceRate>>(calculated));

            var testObject = mock.Create<UpsLocalRateClient>();
            GenericResult<List<UpsServiceRate>> genericResult = testObject.GetRates(shipment);

            Assert.True(genericResult.Success);
            Assert.Equal(calculated, genericResult.Value);
        }
        

        //[Fact]
        //public void GetRates_ReturnsRatesFromRepository()
        //{
        //    var serviceRates = new[] {new UpsLocalServiceRate(UpsServiceType.Ups2DayAir, 3.50M, false, 1)};
        //    var testObject = mock.Create<UpsLocalRateClient>();
        //    mock.Mock<IUpsLocalRateTableRepository>()
        //        .Setup(r => r.GetServiceRates(shipment.Ups))
        //        .Returns(serviceRates);

        //    var result = testObject.GetRates(shipment);
        //    Assert.Equal(serviceRates[0], result.Value.Single());
        //}

        [Fact]
        public void GetRates_DelegatesToApiClient_WhenCannotCalculateLocalRates()
        {
            var serviceRates = new UpsLocalServiceRate[0];
            mock.Mock<IUpsLocalRateTableRepository>()
                .Setup(r => r.GetServiceRates(shipment.Ups))
                .Returns(serviceRates);

            var apiRateClient = mock.CreateMock<IUpsRateClient>();
            var indexMock = new Mock<IIndex<UpsRatingMethod, IUpsRateClient>>();
            indexMock.Setup(x => x[UpsRatingMethod.Api]).Returns(apiRateClient.Object);
            mock.Provide(indexMock.Object);

            var testObject = mock.Create<UpsLocalRateClient>();
            testObject.GetRates(shipment);

            apiRateClient.Verify(c => c.GetRates(shipment), Times.Once);
        }

        [Fact]
        public void GetRates_DelegatesToApiClient_WhenErrorIsThrownGettingLocalRates()
        {
            mock.Mock<IUpsLocalRateTableRepository>()
                .Setup(r => r.GetServiceRates(shipment.Ups))
                .Throws<UpsLocalRatingException>();

            var apiRateClient = mock.CreateMock<IUpsRateClient>();
            var indexMock = new Mock<IIndex<UpsRatingMethod, IUpsRateClient>>();
            indexMock.Setup(x => x[UpsRatingMethod.Api]).Returns(apiRateClient.Object);
            mock.Provide(indexMock.Object);

            var testObject = mock.Create<UpsLocalRateClient>();
            testObject.GetRates(shipment);

            apiRateClient.Verify(c => c.GetRates(shipment), Times.Once);
        }

        [Fact]
        public void GetRates_CallsLogForEachServiceRate()
        {
            IEnumerable<UpsServiceType> eligibleServieTypes = new[]
            {
                UpsServiceType.UpsGround,
                UpsServiceType.Ups3DaySelect
            };

            var filter = mock.CreateMock<IServiceFilter>();
            filter.Setup(f => f.GetEligibleServices(shipment.Ups, It.IsAny<IEnumerable<UpsServiceType>>()))
                .Returns(eligibleServieTypes);
            mock.Provide<IEnumerable<IServiceFilter>>(new[] { filter.Object });

            var serviceRate = new UpsLocalServiceRate(UpsServiceType.UpsGround, 3.50M, false, 1);
            var serviceRates = new[] { serviceRate };
            mock.Mock<IUpsLocalRateTableRepository>()
                .Setup(r => r.GetServiceRates(shipment.Ups))
                .Returns(serviceRates);

            var apiLogEntry = mock.Mock<IApiLogEntry>();

            var testObject = mock.Create<UpsLocalRateClient>();
            testObject.GetRates(shipment);

            apiLogEntry.Verify(l => l.LogResponse(It.Is<string>(s => s.Contains("3.50")), "txt"), Times.Once);
        }

        [Fact]
        public void GetRates_SurchargesAppliedToRatesFromRepository()
        {
            IEnumerable<UpsServiceType> eligibleServieTypes = new[]
            {
                UpsServiceType.UpsGround,
                UpsServiceType.Ups3DaySelect
            };

            var filter = mock.CreateMock<IServiceFilter>();
            filter.Setup(f => f.GetEligibleServices(shipment.Ups, It.IsAny<IEnumerable<UpsServiceType>>()))
                .Returns(eligibleServieTypes);
            mock.Provide<IEnumerable<IServiceFilter>>(new[] {filter.Object});

            var serviceRate = new UpsLocalServiceRate(UpsServiceType.Ups2DayAir, 3.50M, false, 1);
            var serviceRates = new[] {serviceRate};
            mock.Mock<IUpsLocalRateTableRepository>()
                .Setup(r => r.GetServiceRates(shipment.Ups))
                .Returns(serviceRates);

            Mock<IUpsSurcharge> surcharge = mock.CreateMock<IUpsSurcharge>();
            mock.Mock<IUpsSurchargeFactory>()
                .Setup(f => f.Get(It.IsAny<IDictionary<UpsSurchargeType, double>>(), It.IsAny<UpsLocalRatingZoneFileEntity>()))
                .Returns(new[] {surcharge.Object, surcharge.Object});

            var testObject = mock.Create<UpsLocalRateClient>();
            testObject.GetRates(shipment);

            surcharge.Verify(s => s.Apply(shipment.Ups, serviceRate), Times.Exactly(2));
        }

        //[Fact]
        //public void GetRates_EligibleServicesFromServiceFilterPassedToRepository()
        //{
        //    IEnumerable<UpsServiceType> eligibleServieTypes = new[]
        //    {
        //        UpsServiceType.UpsGround,
        //        UpsServiceType.Ups3DaySelect
        //    };

        //    var filter = mock.CreateMock<IServiceFilter>();
        //    filter.Setup(f => f.GetEligibleServices(shipment.Ups, It.IsAny<IEnumerable<UpsServiceType>>()))
        //        .Returns(eligibleServieTypes);
        //    mock.Provide<IEnumerable<IServiceFilter>>(new [] {filter.Object});

        //    var apiRateClient = mock.CreateMock<IUpsRateClient>();
        //    var indexMock = new Mock<IIndex<UpsRatingMethod, IUpsRateClient>>();
        //    indexMock.Setup(x => x[UpsRatingMethod.Api]).Returns(apiRateClient.Object);
        //    mock.Provide(indexMock.Object);

        //    var testObject = mock.Create<UpsLocalRateClient>();
        //    testObject.GetRates(shipment);

        //    filter.Verify(f => f.GetEligibleServices(shipment.Ups, It.IsAny<IEnumerable<UpsServiceType>>()));

        //    mock.Mock<IUpsLocalRateTableRepository>()
        //        .Verify(r => r.GetServiceRates(shipment.Ups, eligibleServieTypes));
        //}

        public void Dispose()
        {
            mock.Dispose();
        }

    }
}