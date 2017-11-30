using ShipWorks.Data.Model.EntityClasses;
using Xunit;

namespace ShipWorks.Data.Modal.Tests.Custom
{
    public class AmazonOrderEntityTest
    {
        [Theory]
        [InlineData("USPS")]
        [InlineData(null)]
        public void IsSameDay_ReturnsFalse_WhenNotReqeusted(string requestedShipping)
        {
            var order = new AmazonOrderEntity { RequestedShipping = requestedShipping };

            var isSameDay = order.IsSameDay();

            Assert.False(isSameDay);
        }

        [Fact]
        public void IsSameDay_ReturnsTrue_WhenReqeusted()
        {
            var order = new AmazonOrderEntity { RequestedShipping = "SameDay" };

            var isSameDay = order.IsSameDay();

            Assert.True(isSameDay);
        }
    }
}
