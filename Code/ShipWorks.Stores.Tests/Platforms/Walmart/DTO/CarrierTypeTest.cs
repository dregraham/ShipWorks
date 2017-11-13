using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Stores.Platforms.Walmart.DTO;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Walmart.DTO
{
    public class CarrierTypeTest
    {
        [Fact]
        public void DhlExists()
        {
            // DHL was added manually. Make sure it is still there.
            Assert.Equal(carrierType.DHL, carrierType.DHL);
        }
    }
}
