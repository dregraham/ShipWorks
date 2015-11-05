using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Amazon;
using Xunit;

namespace ShipWorks.Tests.Stores.ChannelAdvisor
{
    public class ChannelAdvisorOrderEntityTest
    {

        [Fact]
        public void ChannelAdvisorEntity_Implements_IAmazonOrder_Test()
        {
            IAmazonOrder testObject = new ChannelAdvisorOrderEntity() as IAmazonOrder;

            Assert.True(testObject != null);
        }
    }
}
