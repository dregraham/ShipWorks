using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Amazon;
using Xunit;

namespace ShipWorks.Tests.Stores.Amazon
{
    public class ChannelAdvisorStoreEntityTest
    {

        [Fact]
        public void ChannelAdvisorStoreEntity_Implements_IAmazonCredentials()
        {
            IAmazonCredentials testObject = new ChannelAdvisorStoreEntity() as IAmazonCredentials;

            Assert.True(testObject != null);
        }
    }
}
