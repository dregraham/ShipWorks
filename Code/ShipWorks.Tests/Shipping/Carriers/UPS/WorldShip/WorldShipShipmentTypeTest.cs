using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.UPS.WorldShip;
using ShipWorks.Shipping.Carriers.UPS.WorldShip.BestRate;
using ShipWorks.Tests.Shared;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.WorldShip
{
    public class WorldShipShipmentTypeTest
    {

        [Fact]
        public void GetShippingBroker_ReturnsWorldShipShippingBroker()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                WorldShipShipmentType testObject = mock.Create<WorldShipShipmentType>();

                var bestRateExcludedAccountRepository = mock.Mock<IBestRateExcludedAccountRepository>();
                bestRateExcludedAccountRepository.Setup(r => r.GetAll()).Returns(new List<long>());

                IBestRateShippingBroker broker = testObject.GetShippingBroker(new ShipmentEntity(), bestRateExcludedAccountRepository.Object);

                Assert.IsAssignableFrom<WorldShipBestRateBroker>(broker);
            }
        }
    }
}
