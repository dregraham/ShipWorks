using Autofac.Extras.Moq;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    public class FedExRateRateTypeManipulatorTest
    {
        private FedExRateRateTypeManipulator testObject;
        private readonly AutoMock mock;

        public FedExRateRateTypeManipulatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            testObject = mock.Create<FedExRateRateTypeManipulator>();
        }

        [Fact]
        public void ShouldApply_ReturnsTrue()
        {
            var result = testObject.ShouldApply(null, FedExRateRequestOptions.None);

            Assert.True(result);
        }

        [Fact]
        public void Manipulate_AccountsForNullRequestedShipment()
        {
            var result = testObject.Manipulate(null, new RateRequest());

            Assert.NotNull(result.RequestedShipment);
        }

        [Theory]
        [InlineData(true, RateRequestType.LIST)]
        [InlineData(false, RateRequestType.NONE)]
        public void Manipulate_SetsRateRequestType_BasedOnSettings(bool useListRates, RateRequestType expected)
        {
            mock.Mock<IFedExSettingsRepository>()
                .SetupGet(x => x.UseListRates)
                .Returns(useListRates);

            var result = testObject.Manipulate(null, new RateRequest());

            Assert.Equal(1, result.RequestedShipment.RateRequestTypes.Length);
            Assert.Contains(expected, result.RequestedShipment.RateRequestTypes);
        }
    }
}
