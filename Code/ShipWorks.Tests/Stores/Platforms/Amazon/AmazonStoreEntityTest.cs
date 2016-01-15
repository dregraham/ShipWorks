using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Amazon;
using Xunit;

namespace ShipWorks.Tests.Stores.Amazon
{
    public class AmazonStoreEntityTest
    {

        [Fact]
        public void AmazonStoreEntity_Implements_IAmazonCredentials_Test()
        {
            IAmazonCredentials testObject = new AmazonStoreEntity() as IAmazonCredentials;

            Assert.True(testObject != null);
        }
    }
}
