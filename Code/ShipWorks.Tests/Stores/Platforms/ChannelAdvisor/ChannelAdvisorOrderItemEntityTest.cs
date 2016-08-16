using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Amazon;
using Xunit;

namespace ShipWorks.Tests.Stores.Amazon
{
    public class ChannelAdvisorOrderItemEntityTest
    {

        [Fact]
        public void ChannelAdvisorOrderItemEntity_Implements_IAmazonOrder()
        {
            IAmazonOrderItem testObject = new ChannelAdvisorOrderItemEntity() as IAmazonOrderItem;

            Assert.True(testObject != null);
        }
    }
}
