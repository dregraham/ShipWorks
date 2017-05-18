using System;
using System.Collections.Generic;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using Autofac.Features.Indexed;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups.LocalRating;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.LocalRating
{
    public class UpsLocalWithApiFailOverRatingClientTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly ShipmentEntity shipment;
        private readonly Mock<IIndex<UpsRatingMethod, IUpsRateClient>> rateClientFactory;

        public UpsLocalWithApiFailOverRatingClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            shipment = new ShipmentEntity();
            rateClientFactory = new Mock<IIndex<UpsRatingMethod, IUpsRateClient>>();
        }

        [Fact]
        public void GetRates_ReturnsLocalRates_WhenLocalRatesSuccessful()
        {
            var localResult =
                GenericResult.FromSuccess(new List<UpsServiceRate>
                {
                    new UpsServiceRate(UpsServiceType.Ups2DayAir, 0, false, null)
                });
            SetupRateIndex(() => localResult, UpsRatingMethod.LocalOnly);
            
            var testObject = mock.Create<UpsLocalWithApiFailOverRatingClient>();
            var result = testObject.GetRates(shipment);
            Assert.Equal(localResult, result);
        }

        [Fact]
        public void GetRates_ReturnsApiRates_WhenLocalRatesReturnsUnsuccessful()
        {
            var apiResult =
                GenericResult.FromSuccess(new List<UpsServiceRate>
                {
                    new UpsServiceRate(UpsServiceType.Ups2DayAir, 0, false, null)
                });

            var localResult = GenericResult.FromError<List<UpsServiceRate>>("error");

            SetupRateIndex(() => localResult, UpsRatingMethod.LocalOnly);
            SetupRateIndex(() => apiResult, UpsRatingMethod.ApiOnly);

            var testObject = mock.Create<UpsLocalWithApiFailOverRatingClient>();
            var result = testObject.GetRates(shipment);
            Assert.Equal(apiResult, result);
        }

        [Fact]
        public void GetRates_ReturnsApiRates_WhenLocalRatesThrows()
        {
            var apiResult =
                GenericResult.FromSuccess(new List<UpsServiceRate>
                {
                    new UpsServiceRate(UpsServiceType.Ups2DayAir, 0, false, null)
                });


            SetupRateIndex(() => { throw new UpsLocalRatingException(); }, UpsRatingMethod.LocalOnly);
            SetupRateIndex(() => apiResult, UpsRatingMethod.ApiOnly);

            var testObject = mock.Create<UpsLocalWithApiFailOverRatingClient>();
            var result = testObject.GetRates(shipment);
            Assert.Equal(apiResult, result);
        }

        [Fact]
        public void GetRates_ReturnsApiRates_WhenLocalRatesReturnsSuccess_AndNoActualRates()
        {
            var localResult = GenericResult.FromSuccess(new List<UpsServiceRate>());

            var apiResult = GenericResult.FromSuccess(new List<UpsServiceRate>());

            SetupRateIndex(() => localResult, UpsRatingMethod.LocalOnly);
            SetupRateIndex(() => apiResult, UpsRatingMethod.ApiOnly);

            var testObject = mock.Create<UpsLocalWithApiFailOverRatingClient>();
            var result = testObject.GetRates(shipment);
            Assert.Equal(apiResult, result);
        }

        [Fact]
        public void GetRates_ReturnsApiRates_WhenLocalRatesReturnsSuccess_AndNullValue()
        {
            List<UpsServiceRate> nullRates = null;
            var localResult = GenericResult.FromSuccess(nullRates);
            var apiResult = GenericResult.FromSuccess(new List<UpsServiceRate>());

            SetupRateIndex(() => localResult, UpsRatingMethod.LocalOnly);
            SetupRateIndex(() => apiResult, UpsRatingMethod.ApiOnly);

            var testObject = mock.Create<UpsLocalWithApiFailOverRatingClient>();
            var result = testObject.GetRates(shipment);
            Assert.Equal(apiResult, result);
        }

        private void SetupRateIndex(Func<GenericResult<List<UpsServiceRate>>> getLocalResult, UpsRatingMethod ratingMethod)
        {
            var client = mock.CreateMock<IUpsRateClient>();
            client.Setup(l => l.GetRates(shipment)).Returns(getLocalResult);

            rateClientFactory.Setup(x => x[ratingMethod]).Returns(client.Object);
            mock.Provide(rateClientFactory.Object);
        }

        public void Dispose()
        {
            mock.Dispose();
        }

    }
}