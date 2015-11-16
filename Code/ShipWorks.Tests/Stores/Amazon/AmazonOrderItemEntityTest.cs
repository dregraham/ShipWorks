using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Amazon;
using Xunit;

namespace ShipWorks.Tests.Stores.Amazon
{
    public class AmazonOrderItemEntityTest
    {

        [Fact]
        public void AmazonOrderItemEntity_Implements_IAmazonOrder_Test()
        {
            IAmazonOrderItem testObject = new AmazonOrderItemEntity() as IAmazonOrderItem;

            Assert.True(testObject != null);
        }
    }
}
