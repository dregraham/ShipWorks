using Xunit;
using ShipWorks.Shipping.Carriers.Postal.Usps;

namespace ShipWorks.Tests.Shipping.Carriers.Postal.Usps
{
    public class UspsResellerTypeTest
    {
        [Fact]
        public void Express1_HasOneAsValue_Test()
        {
            // The upgrade script depends on Express1 value being 1
            Assert.AreEqual(1, (int) UspsResellerType.Express1);
        }

        [Fact]
        public void None_HasZeroAsValue_Test()
        {
            // The upgrade script depends on None value being 0
            Assert.AreEqual(0, (int)UspsResellerType.None);
        }
    }
}
