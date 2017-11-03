using ShipWorks.Shipping.Carriers.FedEx.Api.Rate;
using ShipWorks.Shipping.Carriers.FedEx.Api.Rate.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Rate;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Rate.Manipulators.Request
{
    public class FedExRateReturnTransitManipulatorTest
    {
        private FedExRateReturnTransitManipulator testObject;

        public FedExRateReturnTransitManipulatorTest()
        {
            testObject = new FedExRateReturnTransitManipulator();
        }

        [Fact]
        public void ShouldApply_ReturnsTrue()
        {
            var result = testObject.ShouldApply(null, FedExRateRequestOptions.None);
            Assert.True(result);
        }

        [Fact]
        public void Manipulate_ReturnTransitAndCommitIsTrue()
        {
            var result = testObject.Manipulate(null, new RateRequest());
            Assert.True(result.ReturnTransitAndCommit);
        }

        [Fact]
        public void Manipulate_ReturnTransitAndCommitSpecifiedIsTrue()
        {
            var result = testObject.Manipulate(null, new RateRequest());
            Assert.True(result.ReturnTransitAndCommitSpecified);
        }
    }
}
