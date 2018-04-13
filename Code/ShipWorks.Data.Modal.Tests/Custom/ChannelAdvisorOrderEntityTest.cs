using ShipWorks.Data.Model.EntityClasses;
using Xunit;

namespace ShipWorks.Data.Modal.Tests.Custom
{
    public class ChannelAdvisorOrderEntityTest
    {
        [Fact]
        public void IsSameDay_ReturnsFalse_Always()
        {
            var order = new ChannelAdvisorOrderEntity();
            var result = order.IsSameDay();
            Assert.False(result);
        }
    }
}
