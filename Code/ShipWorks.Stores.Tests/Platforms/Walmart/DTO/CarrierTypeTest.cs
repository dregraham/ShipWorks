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
        public void ManuallyAlteredCarrierNameExist()
        {
            Assert.True(typeof(string).Equals(typeof(carrierNameType).GetProperty("Item").PropertyType));
        }
    }
}
