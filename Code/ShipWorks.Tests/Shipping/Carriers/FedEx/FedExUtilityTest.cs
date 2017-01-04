using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx
{
    public class FedExUtilityTest
    {
        [Theory]
        [InlineData(FedExServiceType.FedExFimsMailView)]
        [InlineData(FedExServiceType.FedExFimsMailViewLite)]
        [InlineData(FedExServiceType.FedExFimsPremium)]
        [InlineData(FedExServiceType.FedExFimsStandard)]
        public void IsFimsService_ReturnsTrue_WhenPassedFimsService(FedExServiceType fimsService)
        {
            Assert.True(FedExUtility.IsFimsService(fimsService));
        }

        [Fact]
        public void IsFimsService_ReturnsFalse_WhenPassedNonFimsService()
        {
            Assert.False(FedExUtility.IsFimsService(FedExServiceType.FedExGround));
        }
    }
}
