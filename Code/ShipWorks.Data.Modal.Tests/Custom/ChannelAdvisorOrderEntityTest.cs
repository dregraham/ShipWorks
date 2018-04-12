using Interapptive.Shared.Enums;
using ShipWorks.Data.Model.EntityClasses;
using Xunit;

namespace ShipWorks.Data.Modal.Tests.Custom
{
    public class ChannelAdvisorOrderEntityTest
    {
        [Theory]
        [InlineData(AmazonIsPrime.Yes, "service Same Day", true)]
        [InlineData(AmazonIsPrime.Yes, "service same day", true)]
        [InlineData(AmazonIsPrime.Yes, "service SameDay", true)]
        [InlineData(AmazonIsPrime.Yes, "service sameday", true)]
        [InlineData(AmazonIsPrime.Yes, "service another day", false)]
        [InlineData(AmazonIsPrime.No, "service sameday", false)]
        [InlineData(AmazonIsPrime.Unknown, "service sameday", false)]
        public void IsSameDay_ReturnsExpectedResult(AmazonIsPrime isPrime, string requestedShipping, bool expectedIsSameDay)
        {
            var order = new ChannelAdvisorOrderEntity
            {
                IsPrime = (int) isPrime,
                RequestedShipping = requestedShipping
            };
            
            Assert.Equal(expectedIsSameDay, order.IsSameDay());
        }
    }
}
