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

        [Theory]
        [InlineData("2016-08-15T08:30:00")]
        [InlineData(null)]
        public void IsSameDay_ReturnsTrue_WhenReqeusted(string deliveryDateValue)
        {
            var order = new AmazonOrderEntity { RequestedShipping = "SameDay" };

            var isSameDay = order.IsSameDay();

            Assert.True(isSameDay);
        }
    }
}
