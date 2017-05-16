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
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
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
            var rate1 = new UpsLocalServiceRate(UpsServiceType.Ups2DayAir, "1", 10, "x");
            var rate2 = new UpsLocalServiceRate(UpsServiceType.UpsGround, "1", 10, "x");
            var calculated = new[] {rate1, rate2};

            mock.Mock<IUpsLocalRateTable>()
                .Setup(r => r.CalculateRates(shipment))
                .Returns(() => GenericResult.FromSuccess<IEnumerable<UpsLocalServiceRate>>(calculated));

            var testObject = mock.Create<UpsLocalRateClient>();
            GenericResult<List<UpsServiceRate>> rateResult = testObject.GetRates(shipment);

            Assert.True(rateResult.Success);
            Assert.Equal(calculated, rateResult.Value);
        }

        [Fact]
        public void GetRates_CallsLogResponse_WithLogsFromEachService()
        {
            var rate1 = new UpsLocalServiceRate(UpsServiceType.Ups2DayAir, "1", 10, "x");
            var rate2 = new UpsLocalServiceRate(UpsServiceType.UpsGround, "1", 10, "x");
            var calculated = new[] { rate1, rate2 };

            mock.Mock<IUpsLocalRateTable>()
                .Setup(r => r.CalculateRates(shipment))
                .Returns(() => GenericResult.FromSuccess<IEnumerable<UpsLocalServiceRate>>(calculated));

            string log = null;
            mock.Mock<IApiLogEntry>()
                .Setup(l => l.LogResponse(It.IsAny<string>(), "txt"))
                .Callback<string, string>((text, extension) =>
                {
                    Assert.Null(log);
                    log = text;
                });

            var testObject = mock.Create<UpsLocalRateClient>();
            testObject.GetRates(shipment);

            mock.Mock<IApiLogEntry>().Verify(l => l.LogResponse(It.IsAny<string>(), "txt"), Times.Once);
            Assert.Contains($"Rate Calculation for {UpsServiceType.Ups2DayAir}", log);
            Assert.Contains($"Rate Calculation for {UpsServiceType.UpsGround}", log);
        }

        [Fact]
        public void GetRates_ReturnedRatesSortedByAmount()
        {
            var calculated = new[]
            {
                new UpsLocalServiceRate(UpsServiceType.Ups2DayAir, "1", 2, "x"),
                new UpsLocalServiceRate(UpsServiceType.UpsGround, "1", 3, "x"),
                new UpsLocalServiceRate(UpsServiceType.Ups3DaySelect, "1", 1, "x")
            };

            mock.Mock<IUpsLocalRateTable>()
                .Setup(r => r.CalculateRates(shipment))
                .Returns(() => GenericResult.FromSuccess<IEnumerable<UpsLocalServiceRate>>(calculated));

            var testObject = mock.Create<UpsLocalRateClient>();
            GenericResult<List<UpsServiceRate>> rateResult = testObject.GetRates(shipment);

            Assert.Equal(calculated.OrderBy(r => r.Amount), rateResult.Value);
        }

        [Fact]
        public void GetRates_DelegatesToApiClient_WhenCannotCalculateLocalRates()
        {
            mock.Mock<IUpsLocalRateTable>()
                .Setup(r => r.CalculateRates(shipment))
                .Returns(() => GenericResult.FromError<IEnumerable<UpsLocalServiceRate>>("No rates."));

            var apiGetRatesResponse = GenericResult.FromError<List<UpsServiceRate>>("Api Error");

            var apiRateClient = mock.CreateMock<IUpsRateClient>();
            apiRateClient.Setup(c => c.GetRates(shipment)).Returns(apiGetRatesResponse);

            var indexMock = new Mock<IIndex<UpsRatingMethod, IUpsRateClient>>();
            indexMock.Setup(x => x[UpsRatingMethod.Api]).Returns(apiRateClient.Object);
            mock.Provide(indexMock.Object);

            var testObject = mock.Create<UpsLocalRateClient>();
            var rateResult = testObject.GetRates(shipment);

            apiRateClient.Verify(c => c.GetRates(shipment), Times.Once);
            Assert.Equal(apiGetRatesResponse, rateResult);
        }

        [Fact]
        public void GetRates_LogsMessageFromLocalRateTable_WhenCalculateRatesReturnsFailure()
        {
            mock.Mock<IUpsLocalRateTable>()
                .Setup(r => r.CalculateRates(shipment))
                .Returns(() => GenericResult.FromError<IEnumerable<UpsLocalServiceRate>>("No rates."));

            var apiRateClient = mock.CreateMock<IUpsRateClient>();
            var indexMock = new Mock<IIndex<UpsRatingMethod, IUpsRateClient>>();
            indexMock.Setup(x => x[UpsRatingMethod.Api]).Returns(apiRateClient.Object);
            mock.Provide(indexMock.Object);

            var testObject = mock.Create<UpsLocalRateClient>();
            testObject.GetRates(shipment);

            string expectedError = "Error when calculating rates:\n\nNo rates.\n\nDelegating to UPS API.";
            mock.Mock<IApiLogEntry>().Verify(l => l.LogResponse(expectedError, "txt"));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}