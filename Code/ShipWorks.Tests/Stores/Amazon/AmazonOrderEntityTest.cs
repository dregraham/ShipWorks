using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Amazon;
using Xunit;

namespace ShipWorks.Tests.Stores.Amazon
{
    public class AmazonOrderEntityTest
    {

        [Fact]
        public void AmazonOrderEntity_Implements_IAmazonOrder_Test()
        {
            IAmazonOrder testObject = new AmazonOrderEntity() as IAmazonOrder;

            Assert.True(testObject != null);
        }
    }
}
